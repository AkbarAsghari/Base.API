using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace API.Shared.Exceptions
{
    public class TicketTitleIsEmptyException : BaseException
    {
        public TicketTitleIsEmptyException() : base(HttpStatusCode.BadRequest, "Ticket_Title_Is_Empty")
        {
        }
    }
}
