namespace WebEscuelaProject.Models
{
    public class PermissionFormModel
    {
        public string idCard { get; set; } = String.Empty;
        public string name { get; set; } = String.Empty;
        public string lastname1 { get; set; } = String.Empty;
        public string lastname2 { get; set; } = String.Empty;
        public string workMode { get; set; } = String.Empty;
        public DateTime fromWorkingHours { get; set; }
        public DateTime toWorkingHours { get; set; }

        public string justificationType { get; set; } = String.Empty;
        public DateTime justificationDate { get; set; }
        public string reason { get; set; } = String.Empty;
        public string additionalComms { get; set; } = String.Empty;

        public string signature { get; set; } = String.Empty;
    }
}
