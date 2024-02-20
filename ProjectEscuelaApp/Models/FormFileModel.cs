namespace WebEscuelaProject.Models
{
    public class FormFileModel
    {
        public int file_id { get; set; }
        public string file_consecutive_id { get; set; } = String.Empty;
        public string filename { get; set; } = String.Empty;
        public DateTime filedate { get; set; } = DateTime.UtcNow;
        public string user_firstname { get; set; } = String.Empty;
        public string user_lastname1 { get; set; } = String.Empty;
        public string user_lastname2 { get; set; } = String.Empty;
        public byte[] filecontent { get; set; } = new byte[0];
    }
}
