namespace WebEscuelaProject.Models
{
    public class InabilityFormModel
    {
        public string lastname1 { get; set; } = String.Empty;
        public string lastname2 { get; set; } = String.Empty;
        public string location { get; set; } = String.Empty;
        public string name { get; set; } = String.Empty;
        public string idCard { get; set; } = String.Empty;
        public string numInability { get; set; } = String.Empty;
        public string position { get; set; } = String.Empty;
        public DateTime effectiveDate { get; set; }
        public DateTime expiresDate { get; set; }
        public string issuedBy { get; set; } = String.Empty;
        public string reason { get; set; } = String.Empty;
        public string reasonOther { get; set; } = String.Empty;
        public string signature { get; set; } = String.Empty;

    }
}
