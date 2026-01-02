namespace ELECTIVE_H1_BSIT_32E3_RedDanielRafael.Models
{
    public class ErrorResponse
    {
        public string Error { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public List<string> Details { get; set; } = new();
    }
}
