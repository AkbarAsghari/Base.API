using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace API.Shared.Exceptions
{
    public class TicketTextIsEmptyException : BaseException
    {
        public TicketTextIsEmptyException() : base(HttpStatusCode.BadRequest, "Ticket_Text_Is_Empty")
        {
        }
    }
}
