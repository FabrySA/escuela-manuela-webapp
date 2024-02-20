namespace WebEscuelaProject.Models
{
    public class PublicFileModel
    {
        public int file_id { get; set; }
        public string filename { get; set; } = String.Empty;
        public string file_visualName { get; set; } = String.Empty;
        public DateTime filedate { get; set; } = DateTime.UtcNow;
        public string filetype { get; set; } = String.Empty;
        public IFormFile file { get; set; }
        public byte[] filecontent { get; set; } = new byte[0];
        public string filextension { get; set; } = String.Empty;
    }
}
