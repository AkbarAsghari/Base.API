using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace API.Shared.Utilities
{
    public class RegexTool
    {
        public static bool IsValidMobile(string mobileNumber)
        {
            if (String.IsNullOrEmpty(mobileNumber.Trim()))
                return false;
            if (mobileNumber.Length != 11)
                return false;
            if (!mobileNumber.StartsWith("09"))
                return false;

            Regex regex = new Regex("^[0-9]+$");
            return regex.IsMatch(mobileNumber);
        }

        public static bool IsValidEmail(string emailAddress)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            return regex.IsMatch(emailAddress);
        }

        public static bool IsValidUsername(string text)
        {
            if (text.Length > 32 || text.Length < 5)
                return false;

            Regex regex = new Regex("(?!.*__)^[A-Za-z]{1,}[A-Za-z0-9_]{0,}[_]?[A-Za-z0-9]+${1,}");
            return regex.IsMatch(text);
        }
    }
}
