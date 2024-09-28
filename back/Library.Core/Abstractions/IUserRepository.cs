using Library.Core.Entities;
using Library.Core.Enums;

namespace Library.Persistence.Repositories
{
    public interface IUserRepository
    {
        Task<UserEntity?> Get(UserEntity user);
        Task<UserEntity?> GetByEmail(string email);
        Task<UserEntity> GetInfo(Guid id);
        Task<RoleEntity?> GetRole(Role role);
        Task Register(UserEntity user);
    }
}