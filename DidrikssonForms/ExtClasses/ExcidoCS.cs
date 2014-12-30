using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using Microsoft.VisualBasic;

namespace GCS
{
    // Gcs Common Functions
    public static class GCF
    {
        public static string decSep = CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;
        public static string GarpDecSep = ".";

        /// <summary>
        /// Converts a string to double. 
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns>Double representation of the string, returns zero if it fails to parse</returns>
        public static double stringToDouble(string s)
        {
            double dParse;

            try
            {
                if (double.TryParse(s.Replace('.', ','), out dParse))
                    return dParse;
                else
                    return 0;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Converts a double to string representation with desired delimeter
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static string doubleToString(double d, char delimiter)
        {
            try
            {
                if (delimiter.Equals('.'))
                    return d.ToString().Replace(",", ".");
                else
                    return d.ToString().Replace(".", ",");
            }
            catch
            {
                return "0";
            }
        }

        public static double roundDouble(double d, int dec)
        {
            return Math.Round(d, dec);
        }

        public static string roundString(string s, int dec)
        {
            double d = stringToDouble(s);
            s = doubleToString(roundDouble(d, dec), decSep.ToCharArray()[0]);

            if (s.IndexOf(decSep) != -1)
            {
                for (int i = 0; i < (dec - s.Split(decSep.ToCharArray())[1].Length); i++)
                    s += "0";
            }

            return s;
        }

        public static string setDecSepAsCulture(string s)
        {
            if (decSep.Equals("."))
                return s.Replace(",", ".");
            else
                return s.Replace(".", ",");
        }

        public static string toGarpDecSep(string s)
        {
            return s.Replace(",", ".");
        }

        public static bool isNumeric(object s)
        {
            string s2 = noNULL(s);
            double d;
            return double.TryParse(s2, out d);
        }

        public static string noNULL(object o)
        {
            if (o == null)
                return "";
            else
            {
                if (o.GetType().Equals(TypeCode.String))
                    return (string)o;
                else
                    return o.ToString();
            }
        }
    }
}
