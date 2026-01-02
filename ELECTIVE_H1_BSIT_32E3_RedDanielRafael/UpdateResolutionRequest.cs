namespace ELECTIVE_H1_BSIT_32E3_RedDanielRafael.Models
{
    public class UpdateResolutionRequest
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsDone { get; set; }
    }
}
