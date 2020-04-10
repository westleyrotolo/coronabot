using System;
using System.Threading.Tasks;
using CoronaBot.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Cognitive.LUIS.Programmatic;
using Cognitive.LUIS.Programmatic.Models;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using MimeKit;
using System.Net.Http;
using System.Diagnostics;
using System.Xml;
using System.ServiceModel.Syndication;
using Microsoft.Toolkit.Parsers.Rss;

namespace CoronaBot.Controllers
{
    [Route("api/faq")]
    [ApiController]
    public class FAQIntentController : Controller
    {
        private readonly string SubscriptionKey = "69d6501f607c47718930376a980536bc";
        private readonly Regions Region = Regions.WestUS;
        private readonly string appVersion = "0.1";
        private readonly string appId = "35b61d64-1957-4240-80f5-4c59fd4f32b2";

        AppDbContext appDbContext;
        public FAQIntentController(AppDbContext _appDbContext)
        {
            appDbContext = _appDbContext;
        }

        [HttpGet("intents")]
        public async Task<IActionResult> GetIntents()
        {
            try
            {
                var intents = appDbContext.FAQIntents.Select(x => x.Intent).Distinct();
                return Ok(intents);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + " " + ex.InnerException?.Message ?? "");
            }
        }


        [HttpGet("")]
        public IActionResult Get()
        {
            return Ok("ping");
        }

        [HttpGet("{id}")]
        public IActionResult FindById(int id)
        {
            try
            {
                var x = appDbContext.FAQIntents.Include(x => x.FAQQuestions).Include(x => x.FAQAnswers)
                    .First(x => x.Id == id);
                var f = new ViewModels.IntentFaqViewModel
                {
                    Id = x.Id,
                    Intent = x.Intent,
                    Answers = string.Join(" \n", x.FAQAnswers.Select(y => y.Answer)),
                    Category = x.Category ?? "",
                    SubCategory = x.SubCategory ?? "",
                    Questions = string.Join(" \n", x.FAQQuestions.Select(y => y.Question))
                };
                return Ok(f);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + " " + ex.InnerException?.Message ?? "");
            }
        }

        [HttpPost("search")]
        public IActionResult Find([FromBody]IntentCriteriaFilter intentFaqViewModel)
        {
            try
            {
                var faqIntents = appDbContext.FAQIntents.Include(x => x.FAQAnswers).Include(x => x.FAQQuestions)
                    .Where(x => (!intentFaqViewModel.ByIntent || x.Intent.ToUpper().Contains(intentFaqViewModel.Intent.ToUpper()))
                            && (!intentFaqViewModel.ByAnswer || x.FAQAnswers.Select(x => x.Answer.ToLower()).Any(y => y.Contains(intentFaqViewModel.Question.ToLower())))
                            && (!intentFaqViewModel.ByCategory || x.SubCategory.ToLower().Contains(intentFaqViewModel.Category.ToLower()))
                            && (!intentFaqViewModel.BySubCategory || x.SubCategory.ToLower().Contains(intentFaqViewModel.SubCategory.ToLower()))
                            && (!intentFaqViewModel.ByQuestion || x.FAQQuestions.Select(x => x.Question.ToLower()).Any(y => y.Contains(intentFaqViewModel.Question.ToLower()))))
                    .OrderByDescending(x => x.Id)
                    .Select((x) => new ViewModels.IntentFaqViewModel
                    {
                        Id = x.Id,
                        Intent = x.Intent,
                        Answers = string.Join("\n", x.FAQAnswers.Select(y => y.Answer)),
                        Category = x.Category ?? "",
                        SubCategory = x.SubCategory ?? "",
                        Questions = string.Join("\n", x.FAQQuestions.Select(y => y.Question)),
                        Filename = x.FAQAnswers.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x.Filename)).Filename ?? "",
                        Filepath = (x.FAQAnswers.Any(x => !string.IsNullOrWhiteSpace(x.Filepath)) ? "/Download/" + x.Intent : "")

                    })
                    .ToList();
                return Ok(faqIntents);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var f = appDbContext.FAQIntents.Include(x => x.FAQQuestions).Include(x => x.FAQAnswers).First(x => x.Id == id);
                if (f.FAQQuestions != null)
                    foreach (var i in f.FAQQuestions)
                    {
                        appDbContext.FAQQuestions.Remove(i);
                    }
                if (f.FAQAnswers != null)
                    foreach (var i in f.FAQAnswers)
                    {
                        appDbContext.FAQAnswers.Remove(i);
                    }

                appDbContext.FAQIntents.Remove(f);
                var deleteOnLUIS = await DeleteIntent(f.Intent);
                if (deleteOnLUIS)
                {
                    await appDbContext.SaveChangesAsync();
                    return Ok(true);
                }
                return BadRequest("Delete failed on luis");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("user/questions/add")]
        public async Task<IActionResult> AddQuestionToIntent([FromBody]ViewModels.IntentQuestionViewModel intentQuestion)
        {
            try
            {
                if (appDbContext.FAQIntents.Count(x => x.Intent == intentQuestion.Intent) > 0)
                {
                    var faqIntent = appDbContext.FAQIntents.Include(x => x.FAQQuestions).Include(x => x.FAQAnswers)
                                    .FirstOrDefault(x => x.Intent.ToUpper().Equals(intentQuestion.Intent.ToUpper()));
                    var userQuestions = appDbContext.UserQuestions.Where(x => x.Question.Equals(intentQuestion.Question));
                    if (faqIntent.FAQQuestions == null)
                        faqIntent.FAQQuestions = new List<FAQQuestion>();
                    var q = new FAQQuestion
                    {
                        Question = intentQuestion.Question
                    };
                    var result = await ShoulAddBatchExample(intentQuestion.Intent, new List<string> { intentQuestion.Question });
                    if (result.success)
                    {
                        foreach (var uq in userQuestions)
                        {
                            uq.Intent = intentQuestion.Intent.ToUpper();
                            appDbContext.Update(uq);
                        }
                        await appDbContext.AddAsync(q);
                        await appDbContext.SaveChangesAsync();
                        return Ok(true);
                    }
                    else
                    {
                        return BadRequest(result.message);
                    }
                }


                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("user/questions")]
        public async Task<IActionResult> FindUserQuestions([FromBody]ViewModels.UserQuestionSearchViewModel searchUserQuestions)
        {
            try
            {
                var questions = appDbContext.UserQuestions.Where(x =>
                                 (!searchUserQuestions.ByIntent || x.Intent.ToUpper().Contains(searchUserQuestions.Intent.ToUpper()))
                                && (!searchUserQuestions.ByQuestion || x.Intent.ToLower().Contains(searchUserQuestions.Question.ToLower()))
                                && (!searchUserQuestions.ByUser || x.UserId.ToLower().Contains(searchUserQuestions.User.ToLower()))
                                ).ToList();

                var ret = new ViewModels.UserQuestionSearchResponseViewModel
                {
                    Count = questions.Count,
                    Questions = questions
                                .Skip(searchUserQuestions.ItemPerPage * searchUserQuestions.Page)
                                .Take(searchUserQuestions.ItemPerPage)
                                .ToList()
                };
                return Ok(ret);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + " " + ex.InnerException?.Message ?? "");
            }
        }
        public async Task<(bool success, string message)> ShoulAddBatchExample(string intent, List<string> questions)
        {
            using (var client = new LuisProgClient(SubscriptionKey, Region))
            {
                var intents = await client.Intents.GetAllAsync(appId, appVersion, 0, 1000);
                intents.ToList().ForEach(x => Debug.WriteLine(x.Name));
                var i = intents.Where(x => x.Name.ToLower().Equals(intent.Trim().ToLower())).ToList();
                if (i == null || i.Count == 0)
                    await client.Intents.AddAsync(intent, appId, appVersion);

                List<Example> examples = new List<Example>();
                foreach (var q in questions)
                {
                    examples.Add(new Example
                    {
                        Text = q,
                        IntentName = intent
                    });


                }
                var addExamples = await client.Examples.AddBatchAsync(appId, appVersion, examples.ToArray());
                return (!addExamples.Any(x => x.HasError), string.Join(',', addExamples.SelectMany(x => x.Error?.Message ?? "")));

            }

        }
        public async Task ShouldAddNewIntentTest(string IntentName)
        {
            using (LuisProgClient client = new LuisProgClient(SubscriptionKey, Region))
            {
                var intents = await client.Intents.GetAllAsync(appId, appVersion, 0, 1000);
                var i = intents.Where(x => x.Name.ToLower().Equals(IntentName.Trim().ToLower())).ToList();
                if (i != null && i.Count > 0)
                    await client.Intents.DeleteAsync(i.FirstOrDefault().Id, appId, appVersion);
                var newId = await client.Intents.AddAsync(IntentName, appId, appVersion);
            }
        }

        public async Task<bool> DeleteIntent(string intent)
        {
            try
            {
                using (var client = new LuisProgClient(SubscriptionKey, Region))
                {

                    var intents = await client.Intents.GetAllAsync(appId, appVersion, 0, 1000);
                    if (intents.Count(x => x.Name.ToLower().Equals(intent.Trim().ToLower())) == 0)
                        return true;
                    await client.Intents.DeleteAsync(intents.FirstOrDefault(x => x.Name.ToUpper().Equals(intent.Trim().ToUpper())).Id, appId, appVersion);
                    return true;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> ShouldSendTrainingRequest()
        {
            using (var client = new LuisProgClient(SubscriptionKey, Region))
            {
                var train = await client.Training.TrainAndGetFinalStatusAsync(appId, appVersion);
                if (train.Status.Equals("Success"))
                {
                    var publish = await client.Publishing.PublishAsync(appId, appVersion);
                    publish.DirectVersionPublish = true;
                    return true;
                }
                else if (train.Status.Equals("UpToDate"))
                {

                    var publish = await client.Publishing.PublishAsync(appId, appVersion);
                    publish.DirectVersionPublish = true;
                    return true;
                }

                return false;
            }
        }
        [HttpGet("training")]
        public async Task<IActionResult> GetTestAsync()
        {
            try
            {
                var result = await ShouldSendTrainingRequest();
                if (result)
                    return Ok(true);
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + " " + ex.InnerException?.Message ?? "");
            }
        }
        [HttpGet("attachment/{intent}/download")]
        public async Task<IActionResult> DownloadAttachment(string intent)
        {
            try
            {

                var fAQIntent = appDbContext.FAQIntents.Include(x => x.FAQAnswers)
                        .Where(x => x.Intent.ToUpper().Equals(intent.ToUpper()))
                        .First();
                var attachment = fAQIntent.FAQAnswers.Where(x => !string.IsNullOrWhiteSpace(x.Filepath)).First().Filepath;
                if (System.IO.File.Exists(attachment))
                {
                    var memory = new MemoryStream();
                    using (var stream = new FileStream(attachment, FileMode.Open))
                    {
                        await stream.CopyToAsync(memory);
                    }
                    memory.Position = 0;
                    return File(memory, MimeTypes.GetMimeType(attachment), Path.GetFileName(attachment));
                }
                else
                {
                    return NotFound("Allegato non trovato");
                }


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + " " + ex.InnerException?.Message ?? "");
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]ViewModels.IntentFaqViewModel faqIntent)
        {
            try
            {

                bool toUpdate = false;
                var f = new FAQIntent();
                var questionsToAdd = new List<string>();
                if (!string.IsNullOrEmpty(faqIntent.Intent)
                     && (!string.IsNullOrEmpty(faqIntent.Questions))
                    && (!string.IsNullOrWhiteSpace(faqIntent.Answers) || (!string.IsNullOrWhiteSpace(faqIntent.Filepath))))
                {
                    faqIntent.Intent = faqIntent.Intent.ToUpper();
                    if (appDbContext.FAQIntents.Count(x => x.Intent.ToUpper().Equals(faqIntent.Intent.ToUpper())) > 0)
                    {
                        f = appDbContext.FAQIntents
                            .Include(x => x.FAQAnswers)
                            .Include(x => x.FAQQuestions)
                            .FirstOrDefault(x => x.Intent.ToUpper().Equals(faqIntent.Intent.ToUpper()));
                        toUpdate = true;
                    }
                    faqIntent.Intent = faqIntent.Intent.Trim().Replace(" ", "_");
                    f.Intent = faqIntent.Intent;
                    f.Category = faqIntent.Category ?? "";
                    f.SubCategory = faqIntent.SubCategory ?? "";
                    f.ResponseType = ResponseType.SEQUENCE;
                    if (f.FAQAnswers == null)
                        f.FAQAnswers = new List<FAQAnswer>();
                    if (f.FAQQuestions == null)
                        f.FAQQuestions = new List<FAQQuestion>();
                    if (toUpdate)
                        appDbContext.Update(f);
                    else
                        await appDbContext.AddAsync(f);


                    if (faqIntent.Questions.IndexOf('?') == -1)
                        faqIntent.Questions += "?";

                    if (faqIntent.Answers.IndexOf("\n\n") == -1)
                        faqIntent.Answers += "\n\n";
                    var questions = faqIntent.Questions.Split("?")
                                    .Where(x => !string.IsNullOrWhiteSpace(x))
                                    .Select(x => x.TrimStart('\n').Trim() + "?")
                                    .ToList();
                    var answers = faqIntent.Answers.Split("\n\n")
                                    .Where(x => !string.IsNullOrWhiteSpace(x))
                                    .ToList();
                    bool first = false;
                    if ((answers == null || answers.Count == 0) && !string.IsNullOrEmpty(faqIntent.Filepath))
                    {
                        var fa = new FAQAnswer();
                        fa.Filename = faqIntent.Filename ?? "";
                        fa.Filepath = faqIntent.Filepath ?? "";
                        f.FAQAnswers.Add(fa);
                        await appDbContext.AddAsync(fa);
                    }
                    else
                        foreach (var a in answers)
                        {
                            if (!string.IsNullOrWhiteSpace(a))
                            {
                                var ans = a.TrimStart('\n');
                                var fa = new FAQAnswer();

                                if (!first)
                                {
                                    fa.Filename = faqIntent.Filename ?? "";
                                    fa.Filepath = faqIntent.Filepath ?? "";
                                    first = true;
                                }
                                fa.Answer = ans.Trim();
                                f.FAQAnswers.Add(fa);
                                await appDbContext.AddAsync(fa);
                            }
                        }
                    foreach (var q in questions)
                    {
                        var fq = new FAQQuestion();
                        fq.Question = q;
                        f.FAQQuestions.Add(fq);
                        await appDbContext.AddAsync(fq);
                    }
                    var added = await ShoulAddBatchExample(f.Intent, questions);
                    if (added.success)
                    {
                        await appDbContext.SaveChangesAsync();

                        return Ok(f.Id);
                    }

                    return BadRequest(added.message);
                }
                return BadRequest("intent not filled");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + " " + ex.StackTrace ?? "");
            }
        }


        [HttpPost("Upload"), DisableRequestSizeLimit]
        public IActionResult Upload()
        {
            try
            {
                var attachment = new AttachmentViewModel();
                var file = Request.Form.Files[0];
                var folderName = "/wwwroot/images";
                var path = Directory.GetCurrentDirectory() + folderName;
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(path, fileName);
                    var dbPath = Path.Combine(folderName, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    attachment.Filename = fileName;

                    attachment.Path = fullPath;
                    return Ok(attachment);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("region")]
        public async Task<IActionResult> DownloadStatsRegion()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var x = appDbContext.RegionCovidStats.ToList();
                    appDbContext.RegionCovidStats.RemoveRange(x);
                    var r = await httpClient.GetAsync("https://raw.githubusercontent.com/pcm-dpc/COVID-19/master/dati-json/dpc-covid19-ita-regioni.json");
                    if (r.IsSuccessStatusCode)
                    {
                        var content = await r.Content.ReadAsStringAsync();
                        var stats = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RegionCovidStats>>(content);
                        int i = 0;
                        foreach (var s in stats)
                        {

                            s.Id = ++i;
                            await appDbContext.AddAsync(s);
                            await appDbContext.SaveChangesAsync();
                        }

                        return Ok(stats.Count);
                    }
                    return StatusCode(500, await r.Content.ReadAsStringAsync());

                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message + " " + ex.InnerException?.Message ?? string.Empty);

            }
        }


        [HttpGet("province")]
        public async Task<IActionResult> DownloadStatsProvince()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var x = appDbContext.ProvinceCovidStats.ToList();
                    appDbContext.ProvinceCovidStats.RemoveRange(x);
                    var r = await httpClient.GetAsync("https://raw.githubusercontent.com/pcm-dpc/COVID-19/master/dati-json/dpc-covid19-ita-province.json");
                    if (r.IsSuccessStatusCode)
                    {
                        var content = await r.Content.ReadAsStringAsync();
                        var stats = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ProvinceCovidStats>>(content);
                        int i = 0;
                        foreach (var s in stats)
                        {

                            s.Id = ++i;
                            await appDbContext.AddAsync(s);
                            await appDbContext.SaveChangesAsync();
                        }
                        return Ok(stats.Count);
                    }
                    return StatusCode(500, await r.Content.ReadAsStringAsync());

                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message + " " + ex.InnerException?.Message ?? string.Empty);

            }
        }

        [HttpGet("rss")]
        public async Task<IActionResult> SeedFeedRSS()
        {
            try
            {
                var feeds = appDbContext.FeedRss.ToList();
                foreach (var feed in feeds)
                {
                    if (feed.FeedItems == null)
                        feed.FeedItems = new List<FeedItem>();
                    var fs = await ParseRSS(feed.Link);
                    fs.ForEach(x =>
                    {
                        x.Author = feed.Title;
                        if (appDbContext.FeedItems.Count(a=>a.Link == x.Link)>0)
                        {
                            appDbContext.Remove(x);
                            appDbContext.SaveChanges();
                        }

                        feed.FeedItems.Add(x);
                    });

                }
                await appDbContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message + " " + ex.InnerException?.Message ?? "");
            }
            
        }

        [HttpPost("rss")]
        public async Task<IActionResult> GetFeedRSS([FromBody]FeedSearch feedSearch)
        {
            try
            {
                var items = appDbContext.FeedRss.Include(x=>x.FeedItems).SelectMany(x => x.FeedItems).OrderByDescending(x=>x.PubDate).Take(50).ToList();
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message + " " + ex.InnerException?.Message ?? string.Empty);
            }
        }

        public async Task<List<FeedItem>> ParseRSS(string url)
        {
            string feed = null;
            var feeds = new List<FeedItem>();
            using (var client = new HttpClient())
            {
                try
                {
                    feed = await client.GetStringAsync(url);
                }
                catch
                {
                    return null;
                }
            }

            if (feed != null)
            {
                var parser = new RssParser();
                var rss = parser.Parse(feed);

                foreach (var element in rss)
                {
                    var item = new FeedItem
                    {
                        Link = element.FeedUrl,
                        Title = element.Title,
                        Media = element.MediaUrl,
                        Summary = element.Content,
                        PubDate = element.PublishDate
                    };
                    feeds.Add(item);
                }
            }
            return feeds;
        }
    }
}
