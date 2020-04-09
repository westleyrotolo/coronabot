using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace CoronaBot.Models
{
    [Table("ProvinceCovidStats")]
    public class ProvinceCovidStats
    {
        
        public int Id { get; set; }

        [JsonProperty("data")]
        public DateTimeOffset Data { get; set; }

        [JsonProperty("stato")]
        public string Stato { get; set; }

        [JsonProperty("codice_regione")]
        public int CodiceRegione { get; set; }

        [JsonProperty("denominazione_regione")]
        public string DenominazioneRegion { get; set; }

        [JsonProperty("codice_provincia")]
        public int CodiceProvincia { get; set; }

        [JsonProperty("denominazione_provincia")]
        public string DenominazioneProvincia { get; set; }

        [JsonProperty("sigla_provincia")]
        public string SiglaProvincia { get; set; }

        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("long")]
        public double Long { get; set; }

        [JsonProperty("totale_casi")]
        public int TotaleCasi { get; set; }

        [JsonProperty("note_it")]
        public string NoteIt { get; set; }

        [JsonProperty("note_en")]
        public string NoteEn { get; set; }


    }
}
