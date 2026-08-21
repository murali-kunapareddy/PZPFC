namespace PFCRepository.Repositories.PriceList.Models
{
    public class ProcessBar
    {
        public long PriceFileHeaderID { get; set; }
        public string ZipFileName { get; set; }
        public Double StatusPercentage { get; set; }
        public string StatusData { get; set;}
        public string Status { get; set; }
        public bool IsCompleted { get; set; } = false;
    }
}
