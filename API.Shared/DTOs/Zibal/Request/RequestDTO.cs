using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Shared.DTOs.Zibal.Request
{
    public class RequestDTO
    {
        public string Merchant { get; set; }
        public int Amount { get; set; }
        public string CallbackUrl { get; set; }
        public string Description { get; set; }
        public string OrderId { get; set; }
        public string Mobile { get; set; }
    }
}
