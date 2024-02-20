namespace WebEscuelaProject.Models
{
    public class PlanFileModel
    {
        public IFormFile file { get; set; }

        public int id_plan { get; set; }
        public string plan_name { get; set; } = String.Empty;
        public string plan_subject { get; set; } = String.Empty;
        public DateTime plan_date { get; set; } = DateTime.UtcNow;
        public string plan_filename { get; set; } = String.Empty;
        public byte[] plan_filecontent { get; set; } = new byte[0];

        public string plan_filextension { get; set; } = String.Empty;

        public string user_firstname { get; set; } = String.Empty;
        public string user_lastname1 { get; set; } = String.Empty;
        public string user_lastname2 { get; set; } = String.Empty;
    }
}
