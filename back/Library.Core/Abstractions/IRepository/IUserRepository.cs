using Library.Core.Entities;
using Library.Core.Enums;

namespace Library.Core.Abstractions.IRepository
{
    public interface IUserRepository : IGenericRepository<UserEntity>
    {
        Task<UserEntity?> Get(UserEntity user);
        Task<UserEntity?> GetByEmail(string email);
        Task<UserEntity?> GetByUsername(string username);
        Task<UserEntity> GetInfo(Guid id);
        Task<RoleEntity?> GetRole(Role role);
    }
}