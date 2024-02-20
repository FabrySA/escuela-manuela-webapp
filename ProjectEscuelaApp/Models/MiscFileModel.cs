namespace WebEscuelaProject.Models
{
    public class MiscFileModel
    {
        public int file_id { get; set; }
        public string filename { get; set; } = String.Empty;
        public string file_visualName { get; set; } = String.Empty;
        public DateTime filedate { get; set; } = DateTime.UtcNow;
        public IFormFile file { get; set; }
        public byte[] filecontent { get; set; } = new byte[0];
        public string filextension { get; set; } = String.Empty;
        public string filetype { get; set; } = String.Empty;


        public string user_firstname { get; set; } = String.Empty;
        public string user_lastname1 { get; set; } = String.Empty;
        public string user_lastname2 { get; set; } = String.Empty;
    }
}
