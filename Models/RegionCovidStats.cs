using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace CoronaBot.Models
{
    [Table("RegionCovidStats")]
    public class RegionCovidStats
    {

        public int Id { get; set; }

        [JsonProperty("data")]
        public DateTimeOffset Data { get; set; }

        [JsonProperty("stato")]
        public string Stato { get; set; }

        [JsonProperty("codice_regione")]
        public int CodiceRegione { get; set; }

        [JsonProperty("denominazione_regione")]
        public string DenominazionRegione { get; set; }

        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("long")]
        public double Long { get; set; }

        [JsonProperty("ricoverati_con_sintomi")]
        public int RicoveratiConSintomi { get; set; }

        [JsonProperty("terapia_intensiva")]
        public int TerapiaIntensiva { get; set; }

        [JsonProperty("totale_ospedalizzati")]
        public int TotaleOspedalizzati { get; set; }

        [JsonProperty("isolamento_domiciliare")]
        public int IsolamentoDomiciliare { get; set; }

        [JsonProperty("totale_positivi")]
        public int TotalePositivi { get; set; }

        [JsonProperty("variazione_totale_positivi")]
        public int VariazioneTotalePositivi { get; set; }

        [JsonProperty("nuovi_positivi")]
        public int NuoviPositivi { get; set; }

        [JsonProperty("dimessi_guariti")]
        public int DimessiGuariti { get; set; }

        [JsonProperty("deceduti")]
        public int Deceduti { get; set; }

        [JsonProperty("totale_casi")]
        public int TotaleCasi { get; set; }

        [JsonProperty("tamponi")]
        public int Tamponi { get; set; }

        [JsonProperty("note_it")]
        public string NoteIt { get; set; }

        [JsonProperty("note_en")]
        public string NoteEn { get; set; }
    }
}
