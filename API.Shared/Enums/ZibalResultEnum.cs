using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Shared.Enums
{
    public enum ZibalResultEnum
    {
        Success = 100,
        MerchantNotFound = 102,
        MerchantIsNotActive = 103,
        MerchantIsIvalid = 104,
        AmountMustBeGreaterThan1000Rials = 105,
        TheCallbackUrlIsInvalid_StartWithHttpOrHttps = 106,
        TheAmountOfTheTransactionIsMoreThanTheLimitOfTheAmountOfTheTransaction = 113,
        AlreadyApproved = 201,
        TheOrderHasNotBeenPaidOrHasFailed_ReadTheStatusTable = 202,
        TrackIdIsInvalid = 203
    }
}
