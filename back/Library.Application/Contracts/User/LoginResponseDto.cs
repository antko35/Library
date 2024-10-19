using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Contracts.User
{
    public class LoginResponseDto
    {
        public string access_token { get; set; } = string.Empty;
        public string username { get; set; } = string.Empty;
    }
}
