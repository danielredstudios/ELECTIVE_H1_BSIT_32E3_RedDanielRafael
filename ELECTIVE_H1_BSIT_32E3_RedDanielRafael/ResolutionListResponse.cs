namespace ELECTIVE_H1_BSIT_32E3_RedDanielRafael.Models
{
    public class ResolutionListResponse
    {
        public List<ResolutionDto> Items { get; set; } = new();
    }

    public class ResolutionDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsDone { get; set; }
    }
}
