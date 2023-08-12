namespace GB_Webpage.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public string? ActivtyId { get; set; }

        public int StatusCode { get; set; } 

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public bool ShowActivityId => !string.IsNullOrEmpty(ActivtyId);
    }
}