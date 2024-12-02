namespace Core.Entities;

public class Subject: BaseModel
{
    public string Name { get; set; } = string.Empty;

    public List<Question> Questions { get; set; } = [];
}