using Core.Entities;

namespace Core.Interfaces.Repositories;

public interface IQuestionRepository
{
    // GetQuestionForSubject
    Task<List<Question>> GetQuestionsAsync(int subjectId);

    // GetOneQuestion
    Task<Question> GetQuestionByIdAsync(int questionId);

    // Add Question
    Task AddQuestionAsync(Question question);

    // Delete Question
    Task DeleteQuestion(Question question);
}