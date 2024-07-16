using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Shared.DTOs.Zibal.Inquery
{
    public class InquiryResponseDTO : ResponseDTO
    {
        public string? RefNumber { get; set; }
        public DateTime? PaidAt { get; set; }
        public DateTime? VerifiedAt { get; set; }
        public int Status { get; set; }
        public long Amount { get; set; }
        public string? OrderId { get; set; }
        public string? Description { get; set; }
        public string? CardNumber { get; set; }
        public object[]? multiplexingInfos { get; set; }
        public int Wage { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
