using BurberDinner.Domain.Entities;

namespace BurberDinner.Application.Common.Interfaces.Persistence;

public interface IUserRepository
{
    void Add(User user);
    User? GetUserByEmail(string email);
}