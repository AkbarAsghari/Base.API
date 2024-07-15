using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Shared.DTOs.Zibal.Verify
{
    public class VerifyResponseDTO : ResponseDTO
    {
        public DateTime? PaidAt { get; set; }
        public long? Amount { get; set; }
        public int? Status { get; set; }
        public string? RefNumber { get; set; }
        public string? Description { get; set; }
        public string? CardNumber { get; set; }
        public string? OrderId { get; set; }
    }
}
