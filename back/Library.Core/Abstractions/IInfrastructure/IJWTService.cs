using Library.Core.Entities;

namespace Library.Core.Abstractions.IInfrastructure
{
    public interface IJWTService
    {
        string Gerenate(UserEntity user);
    }
}