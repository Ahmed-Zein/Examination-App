using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities;

public class AppUser : IdentityUser
{
    [StringLength(maximumLength: 256, MinimumLength = 3)]
    public string FirstName { get; set; }

    [StringLength(maximumLength: 256, MinimumLength = 3)]
    public string LastName { get; set; }

    public List<ExamResult> ExamResults { get; set; } = [];
}