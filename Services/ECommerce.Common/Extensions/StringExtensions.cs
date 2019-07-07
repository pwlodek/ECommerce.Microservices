using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Common.Extensions
{
    public static class StringExtensions
    {
        public static Guid ToGuid(this string identifier)
        {
            return Guid.Parse(identifier);
        }
    }
}
