namespace Core.Models;

public class StudentDashboard
{
    public string Name { get; set; } = string.Empty;
    public int TotalExams { get; set; }
    public int TotalExamsInEvaluation { get; set; }
    public int PassedExams { get; set; }
}