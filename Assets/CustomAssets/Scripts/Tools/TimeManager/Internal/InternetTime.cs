using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace MyTools.Internal
{
    public static class InternetTime
    {
        const string m_Format = "ddd, dd MMM yyyy HH:mm:ss 'GMT'";

        public static async Task<DateTime> GetUtcNowAsync()
        {
            var request = await Task.Run(() => WebRequest.Create("http://www.microsoft.com"));
            using (var response = await Task.Run(() => request.GetResponse()))
            {
                string date = response.Headers["date"];
                var provider = CultureInfo.InvariantCulture;
                var style = DateTimeStyles.AdjustToUniversal;
                return DateTime.ParseExact(date, m_Format, provider, style);
            }
        }
    }
}