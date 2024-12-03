using Core.Entities;
using Core.Repositories;

namespace Core.Interfaces.Repositories;

public interface IStudentRepository : IRepository<AppUser, int>
{
}