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

        private static Attachment GetInlineAttachment()
        {
            var imagePath = Path.Combine(Environment.CurrentDirectory, @"Resources/de-luca-nervoso.jpg");
            var imageData = Convert.ToBase64String(File.ReadAllBytes(imagePath));

            return new Attachment
            {
                Name = @"Resources/de-luca-nervoso.jpg",
                ContentType = "image/jpg",
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

            FileStream pdfStream = File.Create("output.pdf");
            pdfDocument.Save(pdfStream);

            var pdfData = Convert.ToBase64String(ReadToEnd(pdfStream));
            return new Attachment
            {
                Name = "autocertificazione.pdf",
                ContentType = "application/pdf",
                ContentUrl = $"data:application/pdf;base64,{pdfData}",
            };

        }
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            try
            {

                var replyText = "";
                
                var user = appDbContext.Users.Include(x => x.SelfCertifications).Where(x => x.Id == turnContext.Activity.From.Id).FirstOrDefault();

                if (user != null
                    && user.SelfCertifications != null
                    && user.SelfCertifications.Count > 0
                    && user.SelfCertifications.Last().IsOpen)
                {
                    var selfCertification = SelfCertificationManager.SetData(turnContext.Activity.Text, user.SelfCertifications.Last());

                    if (turnContext.Activity.Text.Trim().ToLower().Equals("vai indietro")
                        || turnContext.Activity.Text.Trim().ToLower().Equals("indietro"))
                    {
                        selfCertification.Step = Math.Max(0, --selfCertification.Step);
                        appDbContext.Update(selfCertification);
                        await appDbContext.SaveChangesAsync();
                        await turnContext.SendActivityAsync(MessageFactory.Text(SelfCertificationManager.Steps[selfCertification.Step]));
                    }
                    else if (turnContext.Activity.Text.Trim().ToLower().Equals("scarica")
                        || turnContext.Activity.Text.Trim().ToLower().Equals("scarica"))
                    {
                        selfCertification.IsOpen = false;
                        appDbContext.Update(selfCertification);
                        await appDbContext.SaveChangesAsync();
                        await turnContext.SendActivityAsync(MessageFactory.Attachment(GetCustomDoc(selfCertification), "Puoi scaricare la tua autocertificazione cliccando qui:"), cancellationToken);
                    }
                    else if (turnContext.Activity.Text.Trim().ToLower().Equals("annulla")
                        || turnContext.Activity.Text.Trim().ToLower().Equals("stop")) 
                    {

                        selfCertification.IsOpen = false;
                        appDbContext.Update(selfCertification);
                        appDbContext.Remove(user);

                        await appDbContext.SaveChangesAsync();
                        await turnContext.SendActivityAsync(MessageFactory.Text("Compilazione annullata"));

                    }
                    else
                    {
                        if (selfCertification.Step < 22)
                        {
                            appDbContext.Update(selfCertification);
                            await appDbContext.SaveChangesAsync();
                            await turnContext.SendActivityAsync(MessageFactory.Text(SelfCertificationManager.Steps[selfCertification.Step]));

                        }
                        else
                        {
                            selfCertification.IsOpen = false;
                            appDbContext.Remove(user);
                            appDbContext.Update(selfCertification);
                            await appDbContext.SaveChangesAsync();
                            await turnContext.SendActivityAsync(MessageFactory.Attachment(GetCustomDoc(selfCertification), "Puoi scaricare la tua autocertificazione cliccando qui:"), cancellationToken);

                        }
                    }


                }
                else
                {
                    var luisResult = await luisRecognizer.RecognizeAsync(turnContext, new CancellationToken());

                    switch (luisResult.GetTopScoringIntent().intent)
                    {
                        case IntentLUIS.SCARICARE:
                            await turnContext.SendActivityAsync(MessageFactory.Attachment(GetCustomDoc(new SelfCertification())), cancellationToken);
                            break;
                        case IntentLUIS.COMPILARE:
                            {

                                var selfCertification = new SelfCertification();
                                selfCertification.Step = 1;
                                selfCertification.IsOpen = true;
                                if (user.SelfCertifications == null)
                                    user.SelfCertifications = new List<SelfCertification>();
                                user.SelfCertifications.Add(selfCertification);

                                appDbContext.Add(selfCertification);
                                appDbContext.SaveChanges();
                                await turnContext.SendActivityAsync(MessageFactory.Text(SelfCertificationManager.Steps[1]), cancellationToken);
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
                                await turnContext.SendActivityAsync(MessageFactory.Attachment(GetInlineAttachment()));
                                break; ;
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
                        default:
                            {
                                await turnContext.SendActivityAsync(MessageFactory.Text("Purtroppo non ho capito"), cancellationToken);
                                await turnContext.SendActivityAsync(MessageFactory.Text("Puoi avere altre informazioni riguardanti il coronavirus consultando le FAQ presenti nei siti istituzionali"), cancellationToken);
                                await turnContext.SendActivityAsync(MessageFactory.Text("http://www.governo.it/it/faq-iorestoacasa"), cancellationToken);
                                await turnContext.SendActivityAsync(MessageFactory.Text("http://www.salute.gov.it/portale/nuovocoronavirus/dettaglioFaqNuovoCoronavirus.jsp?lingua=italiano&id=228"), cancellationToken);
                                await turnContext.SendActivityAsync(MessageFactory.Text("http://www.anci.it/faq-emergenza-coronavirus/"), cancellationToken);
                                await turnContext.SendActivityAsync(MessageFactory.Text("http://opendatadpc.maps.arcgis.com/apps/opsdashboard/index.html#/dae18c330e8e4093bb090ab0aa2b4892"), cancellationToken); 
                            }
                            break;
                    }
                }


            }
            catch (Exception ex)
            {
                  string x = ex.Message;
            }
            // await turnContext.SendActivityAsync(MessageFactory.Attachment(new Attachment("application/pdf", "https://static.fanpage.it/wp-content/uploads/2020/03/nuovo-modello-autocertificazione.pdf", name: "Autocertificazione.pdf"), "Puoi scaricare la tua autocertificazione cliccando qui:"), cancellationToken);

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
                    await turnContext.SendActivityAsync(MessageFactory.Text("Ciao, qui puoi trovare informazioni utili riguardanti il COVID-19, compilare un nuovo modulo di autocertificazione o scaricarne uno vuoto"), cancellationToken);
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
}
