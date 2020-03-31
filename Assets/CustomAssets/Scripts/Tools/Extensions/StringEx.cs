namespace MyTools.Extensions.String
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public static class StringEx
    {

        public static string SubstringFromTo(this string str, char start, char end, bool excludeBounds = false)
        {
            int startIndex = str.LastIndexOf(start);
            int endIndex = str.LastIndexOf(end);
            return str.SubstringFromTo(startIndex, endIndex, excludeBounds);
        }
        public static string SubstringFromTo(this string str, int start, int end, bool excludeBounds = false)
        {
            if (excludeBounds)
            {
                ++start;
                --end;
            }
            var len = end - start + 1;
            return str.Substring(start, len);
        }

        public static string Format(this string str, object arg0)
        { return string.Format(str, arg0); }
        public static string Format(this string str, object arg0, object arg1)
        { return string.Format(str, arg0, arg1); }
        public static string Format(this string str, object arg0, object arg1, object arg2)
        { return string.Format(str, arg0, arg1, arg2); }
        public static string Format(this string str, params object[] args)
        { return string.Format(str, args); }
    }
}
