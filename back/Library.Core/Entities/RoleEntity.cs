using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Entities
{
    public class RoleEntity
    {
        public int Id { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public ICollection<UserEntity> Users { get; set; } = new List<UserEntity>();
    }
}
