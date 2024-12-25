using Microsoft.AspNetCore.Identity;

namespace Core.Entities;

public class AppUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool isLocked { get; set; } = false;
    public List<ExamResult> ExamResults { get; set; } = [];
}