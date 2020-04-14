using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoronaBot.Models
{
    public class SelfCertification
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Step { get; set; }
        public StepType StepType { get; set; }
        public StepType NextStepType { get; set; }
        public string Name { get; set; }
        public string DateOfBorn { get; set; }
        public string PlaceOfBorn { get; set; }
        public string ResidenceCity { get; set; }
        public string ResidenceAddress { get; set; }
        public string DomicileCity { get; set; }
        public string DomicileAddress { get; set; }
        public string IdentificationType { get; set; }
        public string IdentificationNumber { get; set; }
        public string IdentificationReleased { get; set; }
        public string IdentificationDate { get; set; }
        public string PhoneNumber { get; set; }
        public string StartPlace { get; set; }
        public string EndPlace { get; set; }
        public string StartRegion { get; set; }
        public string EndRegion { get; set; }
        public string Reason { get; set; }
        public string Note { get; set; }
        public bool IsOpen { get; set; }
        public bool X1Work { get; set; }
        public bool X2Urgency { get; set; }
        public bool X3Necessary { get; set; }
        public bool X4Health {get;set;}

    }

    public enum StepType
    {
        Name,
        DateOfBorn,
        PlaceOfBorn,
        Residence,
        DomicileAsk,
        Domicile,
        IdentificationType,
        IdentificationNumber,
        IdentificationReleased,
        IdentificationDate,
        PhoneNumber,
        StartPlaceAsk,
        StartPlace,
        EndPlaceAsk,
        EndPlace,
        StartRegion,
        EndRegion,
        ChooseMotive,
        Note,
        Download
    }
}
