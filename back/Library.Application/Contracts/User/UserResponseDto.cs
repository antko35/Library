using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Contracts.User
{
    public class UserResponseDto
    {
        public string UserName {  get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
