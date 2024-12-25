using Microsoft.AspNetCore.Identity;

namespace Core.Entities;

public class AppUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public bool IsLocked { get; set; } = false;
    public List<ExamResult> ExamResults { get; set; } = [];
}