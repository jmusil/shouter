using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Shouter.Models;

namespace Shouter.Helpers
{
    public class HtmlHelpers
    {
        public static string ShoutAge(DateTime shoutCreationTime)
        {
            DateTime now = DateTime.Now;
            TimeSpan age = now.Subtract(shoutCreationTime);

            if (age.TotalDays >= 1)
            {
                double daysAgo = age.TotalDays;
                string days = "days";
                return GetShoutAge(daysAgo, days);
            }
            else if (age.TotalHours >= 1)
            {
                double hoursAgo = age.TotalHours;
                string hours = "hours";
                return GetShoutAge(hoursAgo, hours);
            }
            else if (age.TotalMinutes >= 1)
            {
                double minutesAgo = age.TotalMinutes;
                string minutes = "minutes";
                return GetShoutAge(minutesAgo, minutes);
            }
            else
            {
                double secondsAgo = age.TotalSeconds;
                string seconds = "seconds";
                return GetShoutAge(secondsAgo, seconds);
            }
        }

        public static string TrimPlural(string plural)
        {
            string singular = plural.Remove(plural.Length-1);
            return singular;
        }

        public static string GetShoutAge(double ago, string timeUnits)
        {
            if (Convert.ToInt32(ago) == 1) timeUnits = TrimPlural(timeUnits);
            return String.Format("Posted more than {0} {1} ago.", (Convert.ToInt32(ago)).ToString(), timeUnits);
        }
    }
}