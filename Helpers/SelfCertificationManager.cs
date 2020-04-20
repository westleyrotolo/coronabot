using System;
using CoronaBot.Models;

namespace CovidBot.Helpers
{
    public static class SelfCertificationManager
    {
        public static SelfCertification SetData(string message, SelfCertification SelfCertification)
        {
            switch (SelfCertification.NextStepType)
            {
                case StepType.Name:
                    {
                        SelfCertification.Name = message;
                        SelfCertification.StepType = StepType.Name;
                        SelfCertification.NextStepType = StepType.PlaceOfBorn;
                        SelfCertification.Step++;
                        break;
                    }
                case StepType.PlaceOfBorn:
                    {
                        SelfCertification.PlaceOfBorn = message;
                        SelfCertification.StepType = StepType.PlaceOfBorn;
                        SelfCertification.NextStepType = StepType.DateOfBorn;
                        SelfCertification.Step++;
                        break;
                    }
                case StepType.DateOfBorn:
                    {
                        SelfCertification.DateOfBorn = message;
                        SelfCertification.StepType = StepType.DateOfBorn;
                        SelfCertification.NextStepType = StepType.Residence;
                        SelfCertification.Step++;
                        break;
                    }
                case StepType.Residence:
                    {
                        var temp = message.Split(",");
                        if (temp.Length > 1)
                        {
                            SelfCertification.ResidenceCity = temp[0];
                            SelfCertification.ResidenceAddress = temp[1];
                            SelfCertification.StepType = StepType.Residence;
                            SelfCertification.NextStepType = StepType.DomicileAsk;
                            SelfCertification.Step++;
                        }
                        break;
                    }
                case StepType.DomicileAsk: // Il domicilio è lo stesso della residenza?
                    {
                        if (message.ToLower().Equals("si")
                            || message.ToLower().Equals("sì"))
                        {

                            SelfCertification.DomicileCity = SelfCertification.ResidenceCity;
                            SelfCertification.DomicileAddress = SelfCertification.ResidenceAddress;
                            SelfCertification.Step+=2;
                            SelfCertification.StepType = StepType.DomicileAsk;
                            SelfCertification.NextStepType = StepType.IdentificationType;
                        }
                        else if (message.ToLower().Equals("no"))
                        {
                                SelfCertification.Step++;
                                SelfCertification.StepType = StepType.DomicileAsk;
                                SelfCertification.NextStepType = StepType.Domicile;
                        }
                        break;
                    }
                case StepType.Domicile:
                    {
                        var temp = message.Split(",");
                        if (temp.Length > 1)
                        {

                            SelfCertification.DomicileCity = temp[0];
                            SelfCertification.DomicileAddress = temp[1];
                            SelfCertification.StepType = StepType.Domicile;
                            SelfCertification.NextStepType = StepType.IdentificationType;
                            SelfCertification.Step++;
                        }
                    break;
                    }
                case StepType.IdentificationType:
                    {
                        SelfCertification.IdentificationType = message;
                        SelfCertification.StepType = StepType.IdentificationType;
                        SelfCertification.NextStepType = StepType.IdentificationNumber;
                        SelfCertification.Step++;
                        break;
                    }
                case StepType.IdentificationNumber:
                    {
                        SelfCertification.IdentificationNumber = message;
                        SelfCertification.StepType = StepType.IdentificationNumber;
                        SelfCertification.NextStepType = StepType.IdentificationReleased;
                        SelfCertification.Step++;
                        break;
                    }
                case StepType.IdentificationReleased:
                    {
                        SelfCertification.IdentificationReleased = message;
                        SelfCertification.StepType = StepType.IdentificationReleased;
                        SelfCertification.NextStepType = StepType.IdentificationDate;
                        SelfCertification.Step++;
                        break;
                    }
                case StepType.IdentificationDate:
                    {
                        SelfCertification.IdentificationDate = message;
                        SelfCertification.StepType = StepType.IdentificationDate;
                        SelfCertification.NextStepType = StepType.PhoneNumber;
                        SelfCertification.Step++;
                        break;
                    }
                case StepType.PhoneNumber:
                    {
                        SelfCertification.PhoneNumber = message;
                        SelfCertification.StepType = StepType.PhoneNumber;
                        SelfCertification.NextStepType = StepType.StartPlaceAsk;
                        SelfCertification.Step++;
                        break;
                    }
                case StepType.StartPlaceAsk:
                    {
                        if (message.ToLower().Equals("si", StringComparison.InvariantCultureIgnoreCase)
                           || message.ToLower().Equals("sì", StringComparison.InvariantCultureIgnoreCase))
                        {
                            SelfCertification.StartPlace = $"{SelfCertification.ResidenceCity} {SelfCertification.ResidenceAddress}";
                            SelfCertification.StepType = StepType.StartPlaceAsk;
                            SelfCertification.NextStepType = StepType.EndPlaceAsk;
                            SelfCertification.Step+=2;
                        }
                        else 
                        {
                            SelfCertification.StepType = StepType.StartPlaceAsk;
                            SelfCertification.NextStepType = StepType.StartPlace;
                            SelfCertification.Step++;
                        }
                        break;
                    }
                case StepType.StartPlace:
                {
                        SelfCertification.StartPlace =message;
                        SelfCertification.StepType = StepType.StartPlace;
                        SelfCertification.NextStepType = StepType.EndPlaceAsk;
                        SelfCertification.Step++;

                        break;
                }
                case StepType.EndPlaceAsk:
                    {
                        if (message.ToLower().Equals("si", StringComparison.InvariantCultureIgnoreCase)
                           || message.ToLower().Equals("sì", StringComparison.InvariantCultureIgnoreCase))
                        {
                            SelfCertification.EndPlace = $"{SelfCertification.ResidenceCity} {SelfCertification.ResidenceAddress}";
                            SelfCertification.StepType = StepType.EndPlaceAsk;
                            SelfCertification.NextStepType = StepType.StartRegion;
                            SelfCertification.Step+=2;
                        }
                        else
                        {

                            SelfCertification.StepType = StepType.EndPlaceAsk;
                            SelfCertification.NextStepType = StepType.EndPlace;
                            SelfCertification.Step++;
                        }
                        break;
                    }
                case StepType.EndPlace:
                    {
                        SelfCertification.EndPlace = message;
                        SelfCertification.StepType = StepType.EndPlace;
                        SelfCertification.NextStepType = StepType.StartRegion;
                        SelfCertification.Step++;

                        break;
                    }
                case StepType.StartRegion:
                    {

                        SelfCertification.StartRegion = message;
                        SelfCertification.StepType = StepType.StartRegion;
                        SelfCertification.NextStepType = StepType.EndRegion;
                        SelfCertification.Step++;
                        break;
                    }
                case StepType.EndRegion:
                    {
                        SelfCertification.EndRegion = message;
                        SelfCertification.StepType = StepType.EndRegion;
                        SelfCertification.NextStepType = StepType.ChooseMotive;
                        SelfCertification.Step++;
                        break;

                    }
                case StepType.ChooseMotive:
                    {
                        if (message.ToLower().StartsWith("comprovate esigenze lavorative"))
                        {
                            SelfCertification.X1Work = true;
                            SelfCertification.Step++;
                            SelfCertification.StepType = StepType.ChooseMotive;
                            SelfCertification.NextStepType = StepType.Note;
                        }
                        else if (message.ToLower().StartsWith("motivi di assoluta urgenza"))
                        {
                            SelfCertification.X2Urgency = true;
                            SelfCertification.Step++;
                            SelfCertification.StepType = StepType.ChooseMotive;
                            SelfCertification.NextStepType = StepType.Note;
                        }
                        else if (message.ToLower().StartsWith("motivi di necessit"))
                        {
                            SelfCertification.X3Necessary = true;
                            SelfCertification.Step++;
                            SelfCertification.StepType = StepType.ChooseMotive;
                            SelfCertification.NextStepType = StepType.Note;
                        }
                        else if (message.ToLower().StartsWith("motivi di salute"))
                        {
                            SelfCertification.X4Health = true;
                            SelfCertification.Step++;
                            SelfCertification.StepType = StepType.ChooseMotive;
                            SelfCertification.NextStepType = StepType.Note;
                        }
                        break;
                    }
                case StepType.Note:
                    {
                        SelfCertification.Note = message;

                        SelfCertification.NextStepType = StepType.Download;
                        SelfCertification.StepType = StepType.Note;
                        SelfCertification.Step++;
                        break;
                    }
                default: break;
            }
            return SelfCertification;
        }
        public static string[] Steps = new string[]
        {
            "",
            "Come ti chiami?",
            "Inserisci il luogo di nascita",
            "Inserisci la data di nascita",
            "Inserisci città ed indirizzo di residenza separata da una ',' (ad es Roma, Via Area Del Pero)",
            "Il tuo domicilio corrisponde con la tua residenza?",
            "Inserisci città ed indirizzo di domiclio separato da una ',' (ad es Roma, Via Area del Pero)",
            "Qual è il documento di riconoscimento? (ad es. carta d'identità, patente..)",
            "Inserisci il numero del documento di riconscimento",
            "Da chi è stato rilasciato il documento di riconoscimento?",
            "Quando è stato rilasciato il documento di riconoscimento?",
            "Inserisci il numero di telefono",
            "Il luogo di partenza è lo stesso della tua residenza?",
            "Inserisci il luogo di partenza",
            "Il luogo di arrivo è lo stesso della tua residenza?",
            "Inserisci il luogo di arrivo",
            "Qual è la tua regione di partenza?",
            "Qual è la tua regione di arrivo?",
            "Per quale motivo ti stai spostando?",
            "Inserisci eventuali note (ad es. lavoro presso .., devo effettuare una visita medica)"
        };

        public static string[] Regions = new string[]
        {
            "Abruzzo",
            "Basilicata",
            "Calabria",
            "Campania",
            "Emilia Romagna",
            "Friuli-Venezia Giulia",
            "Lazio",
            "Liguria",
            "Lombardia",
            "Marche",
            "Molise",
            "Piemonte",
            "Puglia",
            "Sardegna",
            "Sicilia",
            "Toscana",
            "Trentino-Alto Adige",
            "Umbria",
            "Val d'Aosta",
            "Veneto"
        };
        public static string[] ChooseMotive = new string[]
        {
            "Comprovate esigenze lavorative,",
            "Motivi di assoluta urgenza (per spostamenti in altro comune)",
            "Motivi di necessità (per spostamenti all'interno del proprio comune)",
            "Motivi di salute"
        };
    }
}

