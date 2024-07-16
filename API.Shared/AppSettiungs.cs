﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Shared
{
    public sealed class JWTSettings
    {
        public const string Secret = "7xnC3Ew6zjQAfgyqWaCrvFqSEb6zFEVC285FCbBNm4YsansX9TYeaeWF2SQj8CLLmtgPuHdEn3Hp2YWLvLQfBDLe7dp6FKGR3zg8S6p6KV5BLRXRdDWF7kw5pJLEj4mX";
    }

    public sealed class AppSettiungs
    {
        public const string ApplicationDBContextConnectionString = "Data Source=.;Initial Catalog=ApplicationDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";
    }

    public sealed class Zibal
    {
        public const string APIUrl = "https://gateway.zibal.ir";
#if DEBUG
        public const string Merchant = "zibal";
        public const string CallBackUrl = "https://localhost:5066//payment/callback";
#else
        public const string Merchant = "64c3d877cbbc27001b930173";
        public const string CallBackUrl = "https://dnslab.link/payment/callback";
#endif
    }
}
