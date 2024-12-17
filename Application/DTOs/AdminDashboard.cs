namespace Application.DTOs;

public class AdminDashboard
{
    public int TotalStudents { get; set; }
    public int TotalSubjects { get; set; }
    public int TotalExamsTaken { get; set; }
}

public class StudentDashboard
{
    public string Name { get; set; } = string.Empty;
    public int TotalExams { get; set; }
    public int TotalExamsInEvaluation { get; set; }
    public int PassedExams { get; set; }
}