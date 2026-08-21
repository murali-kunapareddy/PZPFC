namespace PFCWebAPP.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public string ErrorView { get; set; } = "_Error500";
        public string ErrorCode { get; set; }
        public Exception Exception { get; set; }
    }
}