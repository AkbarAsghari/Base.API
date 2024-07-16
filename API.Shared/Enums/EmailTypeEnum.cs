using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Shared.Enums
{
    public enum EmailTypeEnum
    {
        BackUp,
        ConfirmEmail,
        ForgetPassword,
        ResendActivationLink,
        AnswerTicket,
        DNSIpAddressChange,
        SubscriptionWillBeExpired
    }
}
