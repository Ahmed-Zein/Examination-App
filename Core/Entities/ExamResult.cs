using System.ComponentModel.DataAnnotations;
using Core.Enums;

namespace Core.Entities;

public class ExamResult : BaseModel
{
    [Range(0, int.MaxValue)] public int TotalScore { get; set; }

    [Range(0, 1000)] public decimal StudentScore { get; set; }

    public ExamResultStatus Status { get; set; } = ExamResultStatus.UnSubmitted;

    [Required] public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    [Required] public string AppUserId { get; set; }

    public AppUser AppUser { get; set; } // Navigational

    [Required] public int ExamId { get; set; }

    public Exam Exam { get; set; } // Navigational
}