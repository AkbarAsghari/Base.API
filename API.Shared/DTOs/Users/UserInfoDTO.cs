using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Shared.DTOs.Users
{
    public class UserInfoDTO
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public bool IsApproved { get; set; }
    }
}
