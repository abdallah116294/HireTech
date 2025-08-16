using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Uitilities.DTO.User
{
    public class ResetPasswordDTO
    {
        public string Email { get; set; }
        public string otp { get; set; }
        public string NewPassword { get; set; }
    }
}
