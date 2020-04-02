using System;
using CoronaBot.Models;

namespace CovidBot.Helpers
{
    public static class SelfCertificationManager
    {
        public static SelfCertification SetData(string message, SelfCertification SelfCertification)
        {
            switch (SelfCertification.Step)
            {
                case 1:
                    {
                        SelfCertification.Name = message;
                        SelfCertification.Step++;
                        break;
                    }
                case 2:
                    {
                        SelfCertification.PlaceOfBorn = message;
                        SelfCertification.Step++;
                        break;
                    }
                case 3:
                    {
                        SelfCertification.DateOfBorn = message;
                        SelfCertification.Step++;
                        break;
                    }
                case 4:
                    {
                        var temp = message.Split(",");
                        if (temp.Length > 1)
                        {
                            SelfCertification.ResidenceCity = temp[0];
                            SelfCertification.ResidenceAddress = temp[1];
                            SelfCertification.Step++;
                        }
                        break;
                    }
                case 5: // Il domicilio è lo stesso della residenza?
                    {
                        if (message.ToLower().Equals("si", StringComparison.InvariantCultureIgnoreCase)
                            || message.ToLower().Equals("sì", StringComparison.InvariantCultureIgnoreCase))
                        {
                            SelfCertification.DomicileCity = SelfCertification.ResidenceCity;
                            SelfCertification.DomicileAddress = SelfCertification.ResidenceAddress;
                            SelfCertification.Step = SelfCertification.Step + 2;
                        }
                        else if (message.ToLower().Equals("no", StringComparison.InvariantCultureIgnoreCase)
                            || message.ToLower().Equals("none", StringComparison.InvariantCultureIgnoreCase))
                        {
                            SelfCertification.Step++;
                        }
                        break;
                    }
                case 6:
                    {
                        var temp = message.Split(",");
                        if (temp.Length > 1)
                        {
                            SelfCertification.DomicileCity = temp[0];
                            SelfCertification.DomicileAddress = temp[1];
                            SelfCertification.Step++;
                        }
                        break;
                    }
                case 7:
                    {
                        SelfCertification.IdentificationType = message;
                        SelfCertification.Step++;
                        break;
                    }
                case 8:
                    {
                        SelfCertification.IdentificationNumber = message;
                        SelfCertification.Step++;
                        break;
                    }
                case 9:
                    {
                        SelfCertification.IdentificationReleased = message;
                        SelfCertification.Step++;
                        break;
                    }
                case 10:
                    {
                        SelfCertification.IdentificationDate = message;
                        SelfCertification.Step++;
                        break;
                    }
                case 11:
                    {
                        SelfCertification.PhoneNumber = message;
                        SelfCertification.Step++;
                        break;
                    }
                case 12:
                    {
                        if (message.ToLower().Equals("si", StringComparison.InvariantCultureIgnoreCase)
                           || message.ToLower().Equals("sì", StringComparison.InvariantCultureIgnoreCase))
                        {
                            SelfCertification.StartPlace = $"{SelfCertification.ResidenceCity} {SelfCertification.ResidenceAddress}";
                            SelfCertification.Step = SelfCertification.Step + 2;
                        }
                        else if (message.ToLower().Equals("no", StringComparison.InvariantCultureIgnoreCase)
                            || message.ToLower().Equals("none", StringComparison.InvariantCultureIgnoreCase))
                        {
                            SelfCertification.Step++;
                        }
                        break;
                    }
                case 13:
                    {
                        SelfCertification.StartPlace = message;
                        SelfCertification.Step++;
                        break;
                    }
                case 14:
                    {
                        if (message.ToLower().Equals("si", StringComparison.InvariantCultureIgnoreCase)
                          || message.ToLower().Equals("sì", StringComparison.InvariantCultureIgnoreCase))
                        {
                            SelfCertification.EndPlace = $"{SelfCertification.ResidenceCity} {SelfCertification.ResidenceAddress}";
                            SelfCertification.Step = SelfCertification.Step + 2;
                        }
                        else if (message.ToLower().Equals("no", StringComparison.InvariantCultureIgnoreCase)
                            || message.ToLower().Equals("none", StringComparison.InvariantCultureIgnoreCase))
                        {
                            SelfCertification.Step++;
                        }
                        break;
                    }
                case 15:
                    {
                        SelfCertification.EndPlace = message;
                        SelfCertification.Step++;
                        break;
                    }
                case 16:
                    {
                        if (message.Contains(","))
                        {
                            var temp = message.Split(",");
                            SelfCertification.StartRegion = temp[0].Trim();
                            SelfCertification.EndPlace = temp[1].Trim();
                        }
                        else
                        {
                            SelfCertification.StartRegion =
                                SelfCertification.EndRegion = message;
                        }
                        SelfCertification.Step++;
                        break;
                    }
                case 17:
                    {
                        if (message.ToLower().Equals("si", StringComparison.InvariantCultureIgnoreCase)
                         || message.ToLower().Equals("sì", StringComparison.InvariantCultureIgnoreCase))
                        {
                            SelfCertification.X1Work = true;
                            SelfCertification.Step++;
                        }
                        else if (message.ToLower().Equals("no", StringComparison.InvariantCultureIgnoreCase)
                            || message.ToLower().Equals("none", StringComparison.InvariantCultureIgnoreCase))
                        {
                            SelfCertification.X1Work = false;
                            SelfCertification.Step++;

                        }
                        break;
                    }
                case 18:
                    {
                        if (message.ToLower().Equals("si", StringComparison.InvariantCultureIgnoreCase)
                         || message.ToLower().Equals("sì", StringComparison.InvariantCultureIgnoreCase))
                        {
                            SelfCertification.X2Urgency = true;
                            SelfCertification.Step++;

                        }
                        else if (message.ToLower().Equals("no", StringComparison.InvariantCultureIgnoreCase)
                            || message.ToLower().Equals("none", StringComparison.InvariantCultureIgnoreCase))
                        {
                            SelfCertification.X2Urgency = false;
                            SelfCertification.Step++;

                        }
                        break;
                    }
                case 19:
                    {
                        if (message.ToLower().Equals("si", StringComparison.InvariantCultureIgnoreCase)
                         || message.ToLower().Equals("sì", StringComparison.InvariantCultureIgnoreCase))
                        {
                            SelfCertification.X3Necessary = true;
                            SelfCertification.Step++;

                        }
                        else if (message.ToLower().Equals("no", StringComparison.InvariantCultureIgnoreCase)
                            || message.ToLower().Equals("none", StringComparison.InvariantCultureIgnoreCase))
                        {
                            SelfCertification.X3Necessary = false;
                            SelfCertification.Step++;

                        }
                        break;
                    }
                case 20:
                    {
                        if (message.ToLower().Equals("si", StringComparison.InvariantCultureIgnoreCase)
                         || message.ToLower().Equals("sì", StringComparison.InvariantCultureIgnoreCase))
                        {
                            SelfCertification.X4Health = true;
                            SelfCertification.Step++;

                        }
                        else if (message.ToLower().Equals("no", StringComparison.InvariantCultureIgnoreCase)
                            || message.ToLower().Equals("none", StringComparison.InvariantCultureIgnoreCase))
                        {
                            SelfCertification.X4Health = false;
                            SelfCertification.Step++;

                        }
                        break;
                    }
                case 21:
                    {
                        SelfCertification.Note = message;
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
            "Iniziamo con la compilazione del modulo di autocertificazione\n"
            +"Qualora volessi tornare indietro di uno step per modificare un informazione inserita, "
            +"ti basterà scrivere 'indietro'\n"
            +"Per interrompere la compilazione del modulo 'annulla'.    \n"
            +"Come ti chiami?",
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
            "Inserisci la ragione di partenza e di arrivo separata da una ','\nInserisci solo la regione di partenza se è la stessa di quella di arrivo",
            "Ti stai spostando per comprovate esigenze lavorative?",
            "Ti stai spostando per motivi di assoluta urgenza? (per spostamenti in altro comune)",
            "Ti stai spostando per motivi di necessità? (per spostamenti all'interno del proprio comune)",
            "Ti stai spostando per motivi di salute?",
            "Inserisci eventuali note (ad es. lavoro presso .., devo effettuare una visita medica)"
        };
    }
}
