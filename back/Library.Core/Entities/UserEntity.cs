using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Library.Core.Entities
{
    public class UserEntity : Entity
    {
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        [JsonIgnore]
        public ICollection<RoleEntity> Roles { get; set; } = new List<RoleEntity>();

        [JsonIgnore] // убрать циклы
        public ICollection<BookEntity> Books { get; set; } = new List<BookEntity>();
        //public ICollection<UserRoleEntity> UserRoles { get; set; } = new List<UserRoleEntity>();
    }
}
