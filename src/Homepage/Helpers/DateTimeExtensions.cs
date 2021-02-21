using System;

namespace Homepage.Helpers
{
    public static class DateTimeExtensions
    {
        public static int GetYears(this DateTime date)
        {
            return GetYears(date, DateTime.Now);
        }

        public static int GetYears(this DateTime date, DateTime relativeDate)
        {
            var years = relativeDate.Year - date.Year;
            if(relativeDate.Month < date.Month || (relativeDate.Month == date.Month && relativeDate.Day < date.Day))
                years--;
            return years;
        }
    }
}
