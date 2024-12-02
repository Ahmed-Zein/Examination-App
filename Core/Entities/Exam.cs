using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class Exam : BaseModel
{
    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public TimeSpan Duration { get; set; }

    public List<Question> Questions { get; set; }
}

public class ExamResult : BaseModel
{
    [Range(0, int.MaxValue)] public int TotalScore { get; set; }

    [Range(0, int.MaxValue)] public int StudentScore { get; set; }

    [Required] public string AppUserId { get; set; }

    public AppUser AppUser { get; set; } // Navigational

    [Required] public int ExamId { get; set; }

    public Exam Exam { get; set; } // Navigational
}