using Core.Enums;

namespace Core.Entities;

public class ExamResult : BaseModel
{
    public int TotalScore { get; set; }

    public decimal StudentScore { get; set; }

    public ExamResultStatus Status { get; set; } = ExamResultStatus.UnSubmitted;

    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public string AppUserId { get; set; }

    public AppUser AppUser { get; set; } // Navigational

    public int? ExamId { get; set; }

    public Exam? Exam { get; set; } // Navigational
}