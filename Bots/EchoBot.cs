    // Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CoronaBot.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CovidBot.Luis;
using Microsoft.Bot.Builder.AI.Luis;
using System.IO;
using System;
using CovidBot.Helpers;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO;
using Syncfusion.DocIORenderer;
using Syncfusion.Pdf;
using MimeKit;
using System.Globalization;

namespace Microsoft.BotBuilderSamples.Bots
{
    public class EchoBot : ActivityHandler
    {
        private readonly AppDbContext appDbContext;
        private readonly IRecognizer luisRecognizer;
        public EchoBot(AppDbContext _appDbContext, IRecognizer _luisRecognizer)
        {
            appDbContext = _appDbContext;
            luisRecognizer = _luisRecognizer;
        }

        private static Attachment GetAttachment(FAQAnswer fAQAnswer)
        {
           // var fileData = Convert.ToBase64String(File.ReadAllBytes(x));
            var contentType = MimeTypes.GetMimeType(fAQAnswer.Filepath);
            var filename = string.Empty;
            if (!contentType.StartsWith("imag") && !contentType.StartsWith("video"))
            {
                filename = Path.GetFileName(fAQAnswer.Filepath);
            }
            return new Attachment
            {
                Name = string.IsNullOrWhiteSpace(fAQAnswer.Filename) ? filename : fAQAnswer.Filename,
                ContentType = contentType,
                //  ContentUrl = $"data:{contentType};base64,{fileData}",
                ContentUrl = "https://heybot-covid.azurewebsites.net//api/faq/attachment/" + fAQAnswer.Id
            };
        }
        private static Attachment GetInlineAttachment ()
        {
            var imagePath = Path.Combine(Environment.CurrentDirectory, @"Resources/de-luca-nervoso.jpg");
            var imageData = Convert.ToBase64String(File.ReadAllBytes(imagePath));
            return new Attachment
            {
                Name = @" ",
                ContentType = "image/jpg",
                ContentUrl = $"data:image/jpg;base64,{imageData}",
            };
        }
        private static Attachment GetDeLucaArrabbiato()
        {
            var imagePath = Path.Combine(Environment.CurrentDirectory, @"Resources/delucaarrabbiato.png");
            var imageData = Convert.ToBase64String(File.ReadAllBytes(imagePath));
            return new Attachment
            {
                Name = @" ",
                ContentType = "image/png",
                ContentUrl = $"data:image/png;base64,{imageData}",
            };
        }
        private static Attachment GetCustomDoc(SelfCertification selfCertification)
        {

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjMwNTM1QDMxMzgyZTMxMmUzME40VEMzVVZwOGRtTDFMMjBQY2k4b0lZWk1JaFBUbFJHeGhwTXFsL1k2blE9;MjMwNTM2QDMxMzgyZTMxMmUzMHBBamZWcjdCRnVsRktkbXk1RDgrZGFRbmszYlBHREpRWWZsdmdqb0xza3M9;MjMwNTM3QDMxMzgyZTMxMmUzMElCVW0zK3Brcm04NnFCUkdVZ0xUNHBKNk5mYVhDWlpzOUhDVFBkNzFTZFU9;MjMwNTM4QDMxMzgyZTMxMmUzMGNjVmRBNk1NQ0J5Vng4RUFYUDZIc1FWd3lDZjBEcDJtdzgwSFBObmxLck09;MjMwNTM5QDMxMzgyZTMxMmUzMFV6aXJRcnoyMGhZd2cwekZKMnFDOFNBNHZmL0VmYTRncTRodGJiNUtCQlU9;MjMwNTQwQDMxMzgyZTMxMmUzMGxlR1YwTjlIRVR2Q2NwOFo1bHg3UGI4KzZhWG1oWGVjaHA1cytPZnhFSk09;MjMwNTQxQDMxMzgyZTMxMmUzMFBiN2dDTW5sYzZTdEUrdnF4LzYvckpvWUdnSmxQWThxQVQyMjhBdkV1Ync9;MjMwNTQyQDMxMzgyZTMxMmUzMGxhYTZjS1VMS0JuZFhHY0ZHdURwQTIyVmp6UHJmMXhvR1pVcmVGZW5pWGs9;MjMwNTQzQDMxMzgyZTMxMmUzMG50WFJWcFpUM2M0MERUQThieW1RQXBINWJoN0Q2Q1Vham9uTWhKSGltUmM9;NT8mJyc2IWhia31ifWN9YGVoYmF8YGJ8ampqanNiYmlmamlmanMDHmgyISE8JDQhNjY9Ez86JTZ9Oic=;MjMwNTQ0QDMxMzgyZTMxMmUzMGx5RVJxUU12QkZvaWdZTW1KV1ZicTcrVVhabEhydng5NTBWUFFZM3g0NTg9");
            var path = Path.Combine(Environment.CurrentDirectory, @"Resources/autocertificazione.docx");
            FileStream fileStream1 = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            //Load template document
            WordDocument doc = new WordDocument(fileStream1, FormatType.Docx);
            doc.Replace("{Name}", selfCertification.Name ?? "", true, true);
            doc.Replace("{DateOfBorn}", selfCertification.DateOfBorn ?? "", true, true);
            doc.Replace("{PlaceOfBorn}", selfCertification.PlaceOfBorn ?? "", true, true);
            doc.Replace("{ResidenceCity}", selfCertification.ResidenceCity ?? "", true, true);
            doc.Replace("{ResidenceAddress}", selfCertification.ResidenceAddress ?? "", true, true);
            doc.Replace("{DomicileCity}", selfCertification.DomicileCity ?? "", true, true);
            doc.Replace("{DomicileAddress}", selfCertification.DomicileAddress ?? "", true, true);
            doc.Replace("{IdentificationType}", selfCertification.IdentificationType ?? "", true, true);
            doc.Replace("{IdentificationNumber}", selfCertification.IdentificationNumber ?? "", true, true);
            doc.Replace("{IdentificationDate}", selfCertification.IdentificationDate ?? "", true, true);
            doc.Replace("{IdentificationReleased}", selfCertification.IdentificationReleased ?? "", true, true);
            doc.Replace("{IdentificationDate}", selfCertification.IdentificationDate ?? "", true, true);
            doc.Replace("{PhoneNumber}", selfCertification.PhoneNumber ?? "", true, true);
            doc.Replace("{StartPlace}", selfCertification.StartPlace ?? "", true, true);
            doc.Replace("{EndPlace}", selfCertification.EndPlace ?? "", true, true);
            doc.Replace("{StartRegion}", selfCertification.StartRegion ?? "", true, true);
            doc.Replace("{EndRegion}", selfCertification.EndRegion ?? "", true, true);
            doc.Replace("{Reason}", selfCertification.Reason ?? "", true, true);
            doc.Replace("X1", selfCertification.X1Work ? "X" : "", true, true);
            doc.Replace("X2", selfCertification.X2Urgency ? "X" : "", true, true);
            doc.Replace("X3", selfCertification.X3Necessary ? "X" : "", true, true);
            doc.Replace("X4", selfCertification.X4Health ? "X" : "", true, true);
            string note1 = "";
            string note2 = "";

            if (selfCertification.Note != null && selfCertification.Note.Length > 100)
            {
                note1.Substring(0, 100);
                note2.Substring(100);
            }
            else
            {
                note1 = selfCertification.Note ?? "";
            }
            doc.Replace("{Note}", note1, true, true);
            doc.Replace("{Note2}", note2, true, true);

            DocIORenderer converter = new DocIORenderer();

            converter.Settings.AutoDetectComplexScript = true;
            converter.Settings.EmbedFonts = true;
            converter.Settings.EmbedCompleteFonts = true;

            PdfDocument pdfDocument = converter.ConvertToPDF(doc);

            FileStream pdfStream = File.Create("autocertificazione.pdf");
            pdfDocument.Save(pdfStream);

            var pdfData = Convert.ToBase64String(ReadToEnd(pdfStream));
            return new Attachment
            {
                Name = "Autocertificazione",
                ContentType = "application/pdf",
                ContentUrl = $"data:application/pdf;base64,{pdfData}",
            };

        }
        async Task SendMessage(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            try
            {
                await turnContext.SendActivityAsync(new Activity { Type = "typing" });


                if (turnContext.Activity.Value?.Equals("[compilazione]OK") ?? false)
                {
                    var heroCard = new HeroCard()
                    {
                        Title = "Compilare",
                        Text = "Iniziamo con l'autocertificazione \n\n"
                                                                            + "Qualora volessi tornare indietro di uno step per modificare un'informazione inserita, "
                                                                            + "ti basterà scrivere 'indietro' \n\n"
                                                                            + "Per interrompere la compilazione del modulo 'annulla'. \n\n"
                                                                            + "Oppure utilizzare i seguenti pulsanti",
                        Buttons = new List<CardAction>()
                                            {
                                                new CardAction
                                                {
                                                Title = "Salta questo passaggio",
                                                Text = "Salta questo passaggio",
                                                Value = "[compilazione]EMPTY",
                                                Type = ActionTypes.MessageBack
                                                },
                                                new CardAction
                                                {
                                                    Title = "Indietro",
                                                    Value = "Indietro",
                                                    Text = "Indietro",
                                                    Type = ActionTypes.MessageBack
                                                },
                                                new CardAction
                                                {
                                                    Title = "Annulla",
                                                    Value = "Annulla",
                                                    
                                                    Text = "Annulla",
                                                    Type = ActionTypes.MessageBack
                                                },

                                            }
                    };
                    await turnContext.SendActivityAsync(MessageFactory.Attachment(heroCard.ToAttachment()), cancellationToken);
                    await turnContext.SendActivityAsync(MessageFactory.Text(SelfCertificationManager.Steps[1]), cancellationToken);
                    return;
                }
                if (turnContext.Activity.Value?.Equals("[compilazione]KO") ?? false)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text("Grazie"), cancellationToken);
                    return;
                }
   
                var tour = await Tour(turnContext, cancellationToken);
                if (tour)
                    return;
                var user = appDbContext.Users.Include(x => x.SelfCertifications).Where(x => x.Id == turnContext.Activity.From.Id).FirstOrDefault();
                if (user == null)
                {
                    user = new User();
                    user.Id = turnContext.Activity.From.Id;
                    await appDbContext.Users.AddAsync(user);

                    await appDbContext.SaveChangesAsync();
                }

                if (user.SelfCertifications != null
                    && user.SelfCertifications.Count > 0
                    && user.SelfCertifications.Last().IsOpen)
                {
                    string text = turnContext.Activity.Text ?? (turnContext.Activity.Value?.ToString() ?? string.Empty);
                    SelfCertification selfCertification = null;
                    if (turnContext.Activity.Value?.Equals("[compilazione]EMPTY") ?? false)
                    {
                        selfCertification = SelfCertificationManager.SetData("", user.SelfCertifications.Last());
                     
                    }
                    else
                    {
                        selfCertification = SelfCertificationManager.SetData(text, user.SelfCertifications.Last());

                    }


                    if (text.Trim().ToLower().Equals("vai indietro")
                        || text.Trim().ToLower().Equals("indietro"))
                    {
                        selfCertification.Step = Math.Max(0, selfCertification.Step - 1);
                        selfCertification.NextStepType = selfCertification.StepType;
                        appDbContext.Update(selfCertification);
                        await appDbContext.SaveChangesAsync();
                        await turnContext.SendActivityAsync(MessageFactory.Text(SelfCertificationManager.Steps[selfCertification.Step]));
                    }
                    else if (text.Trim().ToLower().Equals("scarica")
                        || text.Trim().ToLower().Equals("scarica"))
                    {
                        selfCertification.Step--;
                        selfCertification.NextStepType = selfCertification.StepType;
                        selfCertification = SelfCertificationManager.SetData("", user.SelfCertifications.Last());
                        selfCertification.IsOpen = false;
                        await turnContext.SendActivityAsync(MessageFactory.Attachment(GetCustomDoc(selfCertification), "Puoi scaricare la tua autocertificazione cliccando qui:"), cancellationToken);
                        appDbContext.Remove(selfCertification);
                        await appDbContext.SaveChangesAsync();

                    }
                    else if (text.Trim().ToLower().Equals("annulla")
                        || text.Trim().ToLower().Equals("stop"))
                    {

                        selfCertification.IsOpen = false;
                        appDbContext.Remove(selfCertification);

                        await appDbContext.SaveChangesAsync();
                        await turnContext.SendActivityAsync(MessageFactory.Text("Compilazione annullata"));

                    }
                    else
                    {
                        if (selfCertification.NextStepType != StepType.Download)
                        {
                            appDbContext.Update(selfCertification);
                            await appDbContext.SaveChangesAsync();

                            if (selfCertification.NextStepType == StepType.DomicileAsk)
                            {
                                var heroCard = new HeroCard
                                {
                                    Title = string.Empty,
                                    Text = SelfCertificationManager.Steps[selfCertification.Step],
                                    Buttons = new List<CardAction>
                                    {
                                        new CardAction
                                        {
                                        Title = "Salta questo passaggio",
                                        Text = "Salta questo passaggio",
                                        Value = "[compilazione]EMPTY",
                                        Type = ActionTypes.MessageBack
                                        },
                                        new CardAction
                                        {
                                            Title = "Sì",
                                            Text = "Sì",
                                            Value = "Sì",
                                            Type = ActionTypes.MessageBack
                                        },
                                        new CardAction
                                        {
                                            Title = "No",
                                            Text = "No",
                                            Value = "No",
                                            Type = ActionTypes.MessageBack
                                        }
                                    }

                                };
                                await turnContext.SendActivityAsync(MessageFactory.Attachment(heroCard.ToAttachment()), cancellationToken);

                            }
                            else if (selfCertification.NextStepType == StepType.IdentificationType)
                            {
                                var heroCard = new HeroCard
                                {
                                    Title = string.Empty,
                                    Text = SelfCertificationManager.Steps[selfCertification.Step],
                                    Buttons = new List<CardAction>
                                    {
                                        new CardAction
                                        {
                                        Title = "Salta questo passaggio",
                                        Text = "Salta questo passaggio",
                                        Value = "[compilazione]EMPTY",
                                        Type = ActionTypes.MessageBack
                                        },
                                        new CardAction
                                        {
                                            Title = "Carta d'Identità",
                                            Text = "Carta d'Identità",
                                            Value = "Carta d'Identità",
                                            Type = ActionTypes.MessageBack
                                        },
                                        new CardAction
                                        {
                                            Title = "Passaporto",
                                            Text = "Passaporto",
                                            Value = "Passaporto",
                                            Type = ActionTypes.MessageBack
                                        },
                                        new CardAction
                                        {
                                            Title = "Patente",
                                            Text = "Patente",
                                            Value = "Patente",
                                            Type = ActionTypes.MessageBack
                                        }
                                    }

                                };
                           
                            await turnContext.SendActivityAsync(MessageFactory.Attachment(heroCard.ToAttachment()), cancellationToken);

                            }
                            else if (selfCertification.NextStepType == StepType.StartPlaceAsk)
                            {
                                var heroCard = new HeroCard
                                {
                                    Title = string.Empty,
                                    Text = SelfCertificationManager.Steps[selfCertification.Step],
                                    Buttons = new List<CardAction>()
                                };
                                var _cardAction = new CardAction
                                {
                                    Title = "Salta questo passaggio",
                                    Text = "Salta questo passaggio",
                                    Value = "[compilazione]EMPTY",
                                    Type = ActionTypes.MessageBack
                                };
                                heroCard.Buttons.Add(_cardAction);

                                heroCard.Buttons.Add(
                                        new CardAction
                                        {
                                            Title = "Sì",
                                            Text = "Sì",
                                            Value = "Sì",
                                            Type = ActionTypes.MessageBack
                                        });
                                heroCard.Buttons.Add(
                                        new CardAction
                                        {
                                            Title = "No",
                                            Text = "No",
                                            Value = "No",
                                            Type = ActionTypes.MessageBack
                                        });
                                await turnContext.SendActivityAsync(MessageFactory.Attachment(heroCard.ToAttachment()), cancellationToken);
                            }
                            else if (selfCertification.NextStepType == StepType.EndPlaceAsk)
                            {
                                var heroCard = new HeroCard
                                {
                                    Title = string.Empty,
                                    Text = SelfCertificationManager.Steps[selfCertification.Step],
                                    Buttons = new List<CardAction>()
                                };
                                var _cardAction = new CardAction
                                {
                                    Title = "Salta questo passaggio",
                                    Text = "Salta questo passaggio",
                                    Value = "[compilazione]EMPTY",
                                    Type = ActionTypes.MessageBack
                                };
                                heroCard.Buttons.Add(_cardAction);

                                heroCard.Buttons.Add(
                                        new CardAction
                                        {
                                            Title = "Sì",
                                            Text = "Sì",
                                            Value = "Sì",
                                            Type = ActionTypes.MessageBack
                                        });
                                heroCard.Buttons.Add(
                                        new CardAction
                                        {
                                            Title = "No",
                                            Text = "No",
                                            Value = "No",
                                            Type = ActionTypes.MessageBack
                                        });
                                await turnContext.SendActivityAsync(MessageFactory.Attachment(heroCard.ToAttachment()), cancellationToken);
                            }
                            else if (selfCertification.NextStepType == StepType.StartRegion)
                            {
                                var heroCard = new HeroCard
                                {
                                    Title = string.Empty,
                                    Text = SelfCertificationManager.Steps[selfCertification.Step],
                                    Buttons = new List<CardAction>()
                                };
                                var _cardAction = new CardAction
                                {
                                    Title = "Salta questo passaggio",
                                    Text = "Salta questo passaggio",
                                    Value = "[compilazione]EMPTY",
                                    Type = ActionTypes.MessageBack
                                };
                                heroCard.Buttons.Add(_cardAction);
                                foreach (var r in SelfCertificationManager.Regions)
                                {
                                    var cardAction = new CardAction
                                    {
                                        Title = r,
                                        Text = r,
                                        Value = r,
                                        Type = ActionTypes.MessageBack
                                    };
                                    heroCard.Buttons.Add(cardAction);
                                }
                                await turnContext.SendActivityAsync(MessageFactory.Attachment(heroCard.ToAttachment()), cancellationToken);
                            }
                            else if (selfCertification.NextStepType == StepType.EndRegion)
                            {
                                var heroCard = new HeroCard
                                {
                                    Title = string.Empty,
                                    Text = SelfCertificationManager.Steps[selfCertification.Step],
                                    Buttons = new List<CardAction>()
                                };
                                var _cardAction = new CardAction
                                {
                                    Title = "Salta questo passaggio",
                                    Text = "Salta questo passaggio",
                                    Value = "[compilazione]EMPTY",
                                    Type = ActionTypes.MessageBack
                                };
                                heroCard.Buttons.Add(_cardAction);
                                foreach (var r in SelfCertificationManager.Regions)
                                {
                                    var cardAction = new CardAction
                                    {
                                        Title = r,
                                        Text = r,
                                        Value = r,
                                        Type = ActionTypes.MessageBack
                                    };
                                    heroCard.Buttons.Add(cardAction);
                                }
                                await turnContext.SendActivityAsync(MessageFactory.Attachment(heroCard.ToAttachment()), cancellationToken);

                            }
                            else if (selfCertification.NextStepType == StepType.ChooseMotive)
                            {
                                var heroCard = new HeroCard
                                {
                                    Title = string.Empty,
                                    Text = SelfCertificationManager.Steps[selfCertification.Step],
                                    Buttons = new List<CardAction>()
                                };
                                var _cardAction = new CardAction
                                {
                                    Title = "Salta questo passaggio",
                                    Text = "Salta questo passaggio",
                                    Value = "[compilazione]EMPTY",
                                    Type = ActionTypes.MessageBack
                                };
                                heroCard.Buttons.Add(_cardAction);
                                foreach (var r in SelfCertificationManager.ChooseMotive)
                                {
                                    var cardAction = new CardAction
                                    {
                                        Title = r,
                                        Text = r,
                                        Value = r,
                                        Type = ActionTypes.MessageBack
                                    };
                                    heroCard.Buttons.Add(cardAction);
                                }
                                await turnContext.SendActivityAsync(MessageFactory.Attachment(heroCard.ToAttachment()), cancellationToken);
                            }
                            else
                            {
                                var heroCard = new HeroCard
                                {
                                    Title = string.Empty,
                                    Text = SelfCertificationManager.Steps[selfCertification.Step],
                                    Buttons = new List<CardAction>()
                                };
                                var cardAction = new CardAction
                                {
                                    Title = "Salta questo passaggio",
                                    Text = "Salta questo passaggio",
                                    Value = "[compilazione]EMPTY",
                                    Type = ActionTypes.MessageBack
                                };
                                heroCard.Buttons.Add(cardAction);
                                await turnContext.SendActivityAsync(MessageFactory.Attachment(heroCard.ToAttachment()));

                            }


                        }
                        else
                        {
                            selfCertification.IsOpen = false;

                            await turnContext.SendActivityAsync(MessageFactory.Text("Grazie. \n\nA momenti potrai scaricare la tua autocertificazione"));
                            await appDbContext.SaveChangesAsync();
                            await turnContext.SendActivityAsync(MessageFactory.Attachment(GetCustomDoc(selfCertification), "Puoi scaricare la tua autocertificazione cliccando qui:"), cancellationToken);


                            appDbContext.Remove(selfCertification);
                            await appDbContext.SaveChangesAsync();
                        }
                    }


                }
                else
                {
                    if (turnContext.Activity.Text.Equals("/start"))
                    {
                        var id = turnContext.Activity.From.Id;
                        if (user == null && appDbContext.Users.Count(x => x.Id == id) == 0)
                        {
                            var _user = new User();
                            _user.Id = id;
                            appDbContext.Add(_user);
                            await appDbContext.SaveChangesAsync();
                        }
                        await StartTour(turnContext, cancellationToken);
                    }
                    else
                    {

                        turnContext.Activity.Text = turnContext.Activity.Text?.ToLower() ?? string.Empty;
                        var luisResult = await luisRecognizer.RecognizeAsync(turnContext, new CancellationToken());
                        var score = luisResult.GetTopScoringIntent().score;
                        var _intent = luisResult.GetTopScoringIntent().intent;
                        if (_intent == IntentLUIS.NONE)
                        {
                            if (luisResult.Intents.Count>1)
                            {
                                var newIntent = luisResult.Intents.OrderByDescending(x => x.Value.Score).Skip(1).Take(1).FirstOrDefault();
                                if (newIntent.Value.Score>.5)
                                {
                                    _intent = newIntent.Key;
                                }
                            }
                        }

                        if (score < .1)
                        {
                            await StartTour(turnContext, cancellationToken);
                        }
                        else
                            switch (_intent)
                            {
                                case IntentLUIS.SCARICARE:
                                    await turnContext.SendActivityAsync(MessageFactory.Attachment(GetCustomDoc(new SelfCertification())), cancellationToken);
                                    break;
                                case IntentLUIS.COMPILARE:
                                    {

                                        var selfCertification = new SelfCertification();
                                        selfCertification.Step = 1;
                                        selfCertification.StepType = StepType.Name;
                                        selfCertification.NextStepType = StepType.Name;
                                        selfCertification.IsOpen = true;
                                        if (user.SelfCertifications == null)
                                            user.SelfCertifications = new List<SelfCertification>();
                                        user.SelfCertifications.Add(selfCertification);

                                        appDbContext.Add(selfCertification);
                                        appDbContext.SaveChanges();

                                        var heroCard = new HeroCard()
                                        {
                                            Title = "Compilare",
                                            Text = "I dati che inserirai saranno memorizzati per la sola durata della sessione, "
                                             + "una volta completata, annullata la compilazione del modulo "
                                             + "o qualsiasi altra eventualità i dati saranno eliminati "
                                             + "e non saranno più accessibili. "
                                             + "Potrai comunque sempre saltare la compilazione di qualsiasi campo tu voglia.",
                                  
                                            Buttons = new List<CardAction>()
                                            {
                                                new CardAction
                                                {
                                                    Title = "Acconsento",
                                                    Value = "[compilazione]OK",
                                                    Text = "Acconsento",
                                                    Type = ActionTypes.MessageBack
                                                },
                                                new CardAction
                                                {
                                                    Title = "Non Acconsento",
                                                    Value = "[compilazione]KO",
                                                    Text = "Non Acconsento",
                                                    Type = ActionTypes.MessageBack
                                                },
                                                new CardAction(
                                                    ActionTypes.OpenUrl,
                                                    "Consulta la Policy Privacy",
                                                    value: "https://www.covinf24.it/privacy.txt"),


                                            }
                                        };
                                        await turnContext.SendActivityAsync(MessageFactory.Attachment(heroCard.ToAttachment()), cancellationToken);
                                        break;
                                    }
                                case IntentLUIS.LAUREA:
                                    {
                                        await turnContext.SendActivityAsync(MessageFactory.Text("Ti mando i carabinieri"), cancellationToken);
                                        await turnContext.SendActivityAsync(MessageFactory.Text("Ma te li mando con i lanciafiamme"), cancellationToken);
                                        break;
                                    }
                                case IntentLUIS.USCIRE:
                                    {

                                        var attchment = new Attachment
                                        {
                                            Name = @"No.",
                                            ContentType = "image/png",
                                            ContentUrl = $"{CovidBot.Helpers.CovidImage.deluca}",
                                        };
                                        await turnContext.SendActivityAsync(MessageFactory.Attachment(attchment));
                                        
                                        break;
                                    }
                                case IntentLUIS.GIORNALI:
                                    {
                                        foreach (var pharse in CovidFAQ.giornali)
                                        {
                                            await turnContext.SendActivityAsync(MessageFactory.Text(pharse), cancellationToken);

                                        }
                                        break;
                                    }
                                case IntentLUIS.CORONAVIRUS:
                                    {
                                        foreach (var pharse in CovidFAQ.coronavirus)
                                        {
                                            await turnContext.SendActivityAsync(MessageFactory.Text(pharse), cancellationToken);

                                        }
                                        break;
                                    }

                                case IntentLUIS.LAVORO:
                                    {
                                        foreach (var pharse in CovidFAQ.lavoro)
                                        {
                                            await turnContext.SendActivityAsync(MessageFactory.Text(pharse), cancellationToken);

                                        }
                                        break;
                                    }

                                case IntentLUIS.UFFICIPUBBLICI:
                                    {
                                        foreach (var pharse in CovidFAQ.ufficipubblici)
                                        {
                                            await turnContext.SendActivityAsync(MessageFactory.Text(pharse), cancellationToken);

                                        }
                                        break;
                                    }

                                case IntentLUIS.SECONDACASA:
                                    {
                                        foreach (var pharse in CovidFAQ.secondacasa)
                                        {
                                            await turnContext.SendActivityAsync(MessageFactory.Text(pharse), cancellationToken);

                                        }
                                        break;
                                    }

                                case IntentLUIS.SPESA:
                                    {
                                        foreach (var pharse in CovidFAQ.spesa)
                                        {
                                            await turnContext.SendActivityAsync(MessageFactory.Text(pharse), cancellationToken);

                                        }
                                        break;
                                    }
                                case IntentLUIS.NEGOZI:
                                case IntentLUIS.COMMERCIALI:
                                    {
                                        foreach (var pharse in CovidFAQ.commerciali)
                                        {
                                            await turnContext.SendActivityAsync(MessageFactory.Text(pharse), cancellationToken);

                                        }
                                        break;
                                    }
                                case IntentLUIS.SCUOLA:
                                    {
                                        foreach (var pharse in CovidFAQ.scuola)
                                        {
                                            await turnContext.SendActivityAsync(MessageFactory.Text(pharse), cancellationToken);

                                        }
                                        break;
                                    }
                                case IntentLUIS.SALUTO:
                                    {
                                        var id = turnContext.Activity.From.Id;
                                        if (user == null && appDbContext.Users.Count(x => x.Id == id) == 0)
                                        {
                                            var _user = new User();
                                            _user.Id = id;
                                            appDbContext.Add(_user);
                                            await appDbContext.SaveChangesAsync();
                                        }
                                        await turnContext.SendActivityAsync(MessageFactory.Text("Ciao, qui puoi trovare informazioni utili riguardanti il COVID-19, compilare un nuovo modulo di autocertificazione o scaricarne uno vuoto"), cancellationToken);
                                        break;
                                    }
                                case IntentLUIS.APPROFONDIMENTI:
                                    {
                                        await StartTour(turnContext, cancellationToken, false);
                                        break;
                                    }
                                case IntentLUIS.COSACHIEDO:
                                    {
                                       
                                        var intent = appDbContext.FAQIntents.Include(x => x.FAQAnswers).Where(x => x.Intent == luisResult.GetTopScoringIntent().intent).FirstOrDefault();
                                        var intents = appDbContext.FAQIntents.Include(x => x.FAQQuestions).ToList();

                                        var answers = intent.FAQAnswers.Where(x => !string.IsNullOrWhiteSpace(x.Answer)).ToList();
                                        await turnContext.SendActivityAsync(MessageFactory.Text(string.Join(" \n\n", answers.Select(f => f.Answer))), cancellationToken);
                                        var rnd = new Random();
                                        var q = string.Empty;
                                        foreach (var i in Enumerable.Range(0, 4))
                                        {
                                            var r = rnd.Next(intents.Count - 1);
                                            var randomIntent = intents[r];
                                            var rq = rnd.Next(randomIntent.FAQQuestions.Count - 1);
                                            var question = randomIntent.FAQQuestions[rq].Question;
                                            q += question + " \n\n";
                                        }
                                        await turnContext.SendActivityAsync(MessageFactory.Text(q), cancellationToken);

                                        await StartTour(turnContext, cancellationToken, false);
                                        break;
                                    }
                                case IntentLUIS.ULTIMENOTIZIE:
                                    {
                                        var intent = appDbContext.FAQIntents.Include(x => x.FAQAnswers).Where(x => x.Intent == luisResult.GetTopScoringIntent().intent).FirstOrDefault();
                                        var answers = intent.FAQAnswers.Where(x => !string.IsNullOrWhiteSpace(x.Answer)).ToList();
                                        var news = appDbContext.FeedRss.Include(x => x.FeedItems).SelectMany(x => x.FeedItems).OrderByDescending(x => x.PubDate).Take(5).ToList();
                                        var cardActions = new List<CardAction>();
                                        foreach (var n in news)
                                        {
                                            cardActions.Add(new CardAction(ActionTypes.OpenUrl, n.Title, value: n.Link));
                                        }
                                        var h = new HeroCard
                                        {
                                            Buttons = cardActions,
                                            Title = "Notizie",
                                            Text = string.Join(" \n\n", answers.Select(f => f.Answer))
                                        };
                                        await turnContext.SendActivityAsync(MessageFactory.Attachment(h.ToAttachment()), cancellationToken);

                                        break;
                                    }
                                case IntentLUIS.REPORTCONTAGI:
                                    {
                                        string culture = "it-IT";
                                        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(culture);
                                        Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(culture);

                                        var date = DateTime.Now;

                                        if (luisResult.Entities.ContainsKey("datetime"))
                                        {
                                            var dstr = luisResult.Entities["datetime"][0].ToString();
                                            var d = Newtonsoft.Json.JsonConvert.DeserializeObject<TempEntity>(dstr);
                                            DateTime.TryParse(d.timex.FirstOrDefault().Replace("XXXX", "2020"), out date);
                                        }
                                        var startDate = new DateTime(2020, 03, 02);
                                        if (date.Date > DateTime.Now)
                                            date = DateTime.Now;
                                        if (date.Date < startDate.Date)
                                            date = startDate.Date;
                                        if (date.Date == DateTime.Now.Date && DateTime.Now.Hour<18)
                                        {
                                            date = date.AddDays(-1);
                                        }
                                        Attachment attachment;
                                        string division = "";
                                        if (turnContext.Activity.Text.ToLower().Contains("provincia")
                                            || turnContext.Activity.Text.ToLower().Contains("province")
                                            || turnContext.Activity.Text.ToLower().Contains("provincie"))
                                        {
                                            attachment = new Attachment()
                                            {
                                                ContentUrl = $"https://github.com/pcm-dpc/COVID-19/raw/master/schede-riepilogative/province/dpc-covid19-ita-scheda-province-{date.ToString("yyyyMMdd")}.pdf",
                                                ContentType = "application/pdf",
                                                Name = "Report Covid-19 Italia"
                                            };
                                            division = " suddiviso per province";
                                        }
                                        else
                                        {
                                            attachment = new Attachment()
                                            {
                                                ContentUrl = $"https://github.com/pcm-dpc/COVID-19/raw/master/schede-riepilogative/regioni/dpc-covid19-ita-scheda-regioni-{date.ToString("yyyyMMdd")}.pdf",
                                                ContentType = "application/pdf",
                                                Name = "Report Covid-19 Italia"
                                            };
                                        }

                                        var temporal = $"di {date.ToString("dddd dd MMMM yyyy")}";
                                        if (date.Date == DateTime.Now.Date)
                                            temporal = "di oggi";
                                        else if (date.Date == DateTime.Now.AddDays(-1).Date)
                                            temporal = "di ieri";


                                        await turnContext.SendActivityAsync(MessageFactory.Text($"Ecco il report{division} {temporal}"), cancellationToken);
                                        await turnContext.SendActivityAsync(MessageFactory.Attachment(attachment), cancellationToken);

                                        break;
                                    }
                                case IntentLUIS.NONE:
                                    {
                                        await StartTour(turnContext, cancellationToken);
                                        break;
                                    }
                                default:
                                    {
                                        var intent = appDbContext.FAQIntents.Include(x => x.FAQAnswers).Include(x => x.FAQQuestions).Where(x => x.Intent == luisResult.GetTopScoringIntent().intent).FirstOrDefault();
                                        if (intent == null)
                                        {
                                            await StartTour(turnContext, cancellationToken);
                                            break;
                                        }
                                        var answers = intent.FAQAnswers.Where(x => !string.IsNullOrWhiteSpace(x.Answer)).ToList();
                                        if (answers.Count > 0)
                                            await turnContext.SendActivityAsync(MessageFactory.Text(string.Join(" \n\n", answers.Select(f => f.Answer))), cancellationToken);
                                        var attachment = intent.FAQAnswers.Where(x => !string.IsNullOrWhiteSpace(x.Filepath)).ToList();
                                        if (attachment.Count > 0)
                                            await turnContext.SendActivityAsync(MessageFactory.Attachment(GetAttachment(attachment.First())), cancellationToken);
                                        break;
                                    }
                            }
                        var userQuestion = new UserQuestion
                        {
                            Intent = luisResult.GetTopScoringIntent().intent,
                            Score = luisResult.GetTopScoringIntent().score,
                            Question = turnContext.Activity.Text,
                            UserId = turnContext.Activity.From.Id,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        };
                        await appDbContext.AddAsync(userQuestion);
                        await appDbContext.SaveChangesAsync();
                    }

                }

            }
            catch (Exception ex)
            {
                string x = ex.Message;
            }
       

        }
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            await SendMessage(turnContext, cancellationToken);

        }
        Attachment IDKLinks(string link)
        {
            return new Attachment
            {
                ContentUrl = link,
                Name = "Sito"
            };
        }
        public async Task<bool> Tour(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var value = turnContext.Activity.Value?.ToString() ??  ((turnContext.Activity.Text != null && turnContext.Activity.Text.StartsWith("[")) ? turnContext.Activity.Text : "") ;
            if (string.IsNullOrWhiteSpace(value))
                return false;
            if (value.Equals("[linkutili]"))
            {
                await IDK(turnContext, cancellationToken);
                return true;
            }
            else if (value.StartsWith("[category]"))
            {

                var text = value.Replace("[category]", "").ToLower();
                var subcategories = appDbContext.FAQIntents.Where(x => x.Category.ToLower().Equals(text)).Select(x => x.SubCategory).Distinct();
                var h = new HeroCard();
                h.Buttons = new List<CardAction>();
                foreach (var s in subcategories)
                {
                    if (string.IsNullOrWhiteSpace(s))
                        continue;
                    var cardAction = new CardAction
                    {
                        Type = ActionTypes.MessageBack,
                        Title = s.ToUpper(),
                        DisplayText = s.ToUpper(),
                        Text = "[subcategory]" + s,
                        Value = "[subcategory]" + s
                    };
                    h.Buttons.Add(cardAction);
                }
                h.Title = text.ToUpper();
                h.Text = "Seleziona una sotto categoria per avere informazioni, oppure scrivi i tuoi dubbi";
                await turnContext.SendActivityAsync(MessageFactory.Attachment(h.ToAttachment()), cancellationToken);
                return true;
            }
            else if (turnContext.Activity.Value.ToString().StartsWith("[subcategory]"))
            {
                var text = value.Replace("[subcategory]", "").ToLower();

                var questions = appDbContext.FAQIntents.Include(x => x.FAQQuestions)
                    .Where(x => x.SubCategory.ToLower().Equals(text))
                    .Select(x =>new { x.FAQQuestions[0].Question, x.Id });
                var h = new HeroCard();
                h.Buttons = new List<CardAction>();
                foreach (var s in questions)
                {

                    if (string.IsNullOrWhiteSpace(s.Question))
                        continue;
                    var cardAction = new CardAction
                    {
                        Type = ActionTypes.MessageBack,
                        Title = s.Question.ToUpper(),
                        DisplayText = s.Question.ToUpper(),
                        Text = "[answer]" + s.Id,
                        Value = "[answer]" + s.Id 
                    };
                    h.Buttons.Add(cardAction);
                }
                h.Title = text.ToUpper();
                h.Text = "Seleziona una domanda per avere informazioni, oppure scrivi i tuoi dubbi";
                await turnContext.SendActivityAsync(MessageFactory.Attachment(h.ToAttachment()), cancellationToken);
                return true;
                 
            }
            else if (value.StartsWith("[answer]"))
            {
               
                var text = value.Replace("[answer]", "").ToLower();
                int questionId = 0;
                int.TryParse(text, out questionId);
                var questions = appDbContext.FAQIntents.Include(x => x.FAQAnswers).Include(x => x.FAQQuestions)
                            .Where(x => x.Id == questionId);
          
                var answer = questions.FirstOrDefault();
                if (answer!=null)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(string.Join("\n\n", answer.FAQAnswers.Select(x => x.Answer))));
                    return true;
                }

            }
            return false;





        }
        public async Task IDK(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var h = new HeroCard
            {
                Buttons = new List<CardAction>()
                {
                    new CardAction(ActionTypes.OpenUrl, "Decreto #IoRestoaCasa", value: "http://www.governo.it/it/faq-iorestoacasa"),
                    new CardAction(ActionTypes.OpenUrl, "Ministero della Salute", value:"http://www.salute.gov.it/portale/nuovocoronavirus/dettaglioFaqNuovoCoronavirus.jsp?lingua=italiano&id=228"),
                    new CardAction(ActionTypes.OpenUrl, "Dashboard Covid", value:"http://opendatadpc.maps.arcgis.com/apps/opsdashboard/index.html#/dae18c330e8e4093bb090ab0aa2b4892")
                },
                Title = "Link Utili",
                Text = "Puoi avere informazioni riguardanti il coronavirus consultando le FAQ presenti nei siti istituzionali"
            };
            await turnContext.SendActivityAsync(MessageFactory.Attachment(h.ToAttachment()),cancellationToken);
            var count = appDbContext.FAQIntents.Count()-1;
            var r = new Random();
            var intent = appDbContext.FAQIntents.Include(x => x.FAQQuestions).Skip(r.Next(count)).Take(1);
            await turnContext.SendActivityAsync(MessageFactory.Text($"Oppure puoi provare a chiedermi:\n\n{intent.First().FAQQuestions[0].Question}"), cancellationToken);

        }
        protected override async Task OnEventActivityAsync(ITurnContext<IEventActivity> turnContext, CancellationToken cancellationToken)
        {

            await turnContext.SendActivityAsync(new Activity { Type = "typing" });
            if (turnContext.Activity.Name == "selfCertification")
            {
                var h = new HeroCard
                {
                    Title = "Autocertificazione",
                    Text = "Vuoi compilare un modulo di autocertificazione o scaricarne uno vuoto?",
                    Buttons = new List<CardAction>
                    {
                        new CardAction
                        {
                            Title =  "Compila Autocertificazione",
                            Text = "Compila Autocertificazione",
                            Value = "Compila Autocertificazione",
                            Type = ActionTypes.MessageBack
                        },
                         new CardAction
                        {
                            Title =  "Scarica Autocertificazione",
                            Text = "Scarica Autocertificazione",
                            Value = "Scarica Autocertificazione",
                            Type = ActionTypes.MessageBack
                        },
                    }
                };
                await turnContext.SendActivityAsync(MessageFactory.Attachment(h.ToAttachment()), cancellationToken);


            }
            else if (turnContext.Activity.Name == "askToDeLuca")
            {
                var hi = "Buongiorno";
                if (DateTime.Now.Hour > 13)
                {
                    hi = "Buonasera";
                }
                await turnContext.SendActivityAsync(MessageFactory.Text(hi), cancellationToken);
                await StartTour(turnContext, cancellationToken, false);
                //await turnContext.SendActivityAsync(MessageFactory.Text("Cosa vuoi sapere?"), cancellationToken);
            }
        }
        async Task StartTour(ITurnContext turnContext,CancellationToken cancellationToken, bool hasImage=true)
        {
            if (hasImage)
                await turnContext.SendActivityAsync(MessageFactory.Attachment(GetDeLucaArrabbiato()), cancellationToken);
            var categories = appDbContext.FAQIntents.Where(x=>(x.Category.ToLower().Equals("decreto #iorestoacasa") || x.Category.ToLower().Equals("#curaitalia"))).Select(x => x.Category).Distinct();
            var h = new HeroCard();
            h.Buttons = new List<CardAction>();
            foreach (var c in categories)
            {
                if (string.IsNullOrWhiteSpace(c))
                    continue;
                var cardAction = new CardAction
                {
                    Type = ActionTypes.MessageBack,
                    Title = c.ToUpper(),
                    Text = "[category]" + c,
                    Value = "[category]" + c
                   
                };
                h.Buttons.Add(cardAction);
            }
            var cardIDKAction = new CardAction
            {
                Type = ActionTypes.MessageBack,
                Title = "Link Utili",
                Text = "[linkutili]",
                Value = "[linkutili]"
            };
            h.Buttons.Add(cardIDKAction);
            var cardDonazioni = new CardAction
            {
                Type = ActionTypes.OpenUrl,
                Title = "Donazioni Regione Campania",
                Text ="Donazioni Regione Campania",
                Value = "http://www.regione.campania.it/regione/it/news/primo-piano/coronavirus-raccolta-fondi-per-l-emergenza-ecco-il-conto-corrente"
            };
            h.Buttons.Add(cardDonazioni);
            h.Title = "Approfondimenti";
            h.Subtitle = "Seleziona una categoria per avere informazioni, oppure scrivi i tuoi dubbi";
            h.Text = "Seleziona una categoria per avere informazioni, oppure scrivi i tuoi dubbi";
            await turnContext.SendActivityAsync(MessageFactory.Attachment(h.ToAttachment()), cancellationToken);

        }
        protected override async Task OnConversationUpdateActivityAsync(ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var id = turnContext.Activity.From.Id;
            if (appDbContext.Users.Count(x => x.Id == id) == 0)
            {
                var user = new User();
                user.Id = id;
                appDbContext.Add(user);
                await appDbContext.SaveChangesAsync();
              //  await turnContext.SendActivityAsync(MessageFactory.Text("Ciao, qui puoi trovare informazioni utili riguardanti il COVID-19, compilare un nuovo modulo di autocertificazione o scaricarne uno vuoto"), cancellationToken);

            }

        }
        public int Id { get; set; }
        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {


            foreach (var member in membersAdded)
            {
                
                if (member.Id != turnContext.Activity.Recipient.Id)
                {   var user = new User();
                    user.Id = member.Id;
                    appDbContext.Add(user);
                    await appDbContext.SaveChangesAsync();
                //    await turnContext.SendActivityAsync(MessageFactory.Text("Ciao, qui puoi trovare informazioni utili riguardanti il COVID-19, compilare un nuovo modulo di autocertificazione o scaricarne uno vuoto"), cancellationToken);
                }
            }
        }
        public static byte[] ReadToEnd(System.IO.Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }
    }
    public class TempEntity
    {
        public string[] timex { get; set; }
        public string type { get; set; }
    }

}
