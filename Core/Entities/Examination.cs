using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class Examination : BaseModel
{
    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public TimeSpan Duration { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } // Navigational

    [Length(minimumLength: 1, maximumLength: 10)]
    public List<Examination> Questions { get; set; }
}

public class ExaminationResult : BaseModel
{
    public int TotalScore { get; set; }
    public int StudentScore { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } // Navigational

    public int ExaminationId { get; set; }
    public Examination Examination { get; set; } // Navigational
}