using System;

namespace Homepage.Settings
{
    public static class Owner
    {
        public static string FirstName => "Nemrud";
        public static string LastName => "Demir";
        public static string FullName => $"{FirstName} {LastName}";
        public static string Job => "Full-Stack Developer";
        public static string City => "Stuttgart";
        public static string Country => "Germany";
        public static string Base => $"{City}, {Country}";
        public static readonly DateTime BirthDate = new DateTime(1995, 08, 16);

        public static int Age
        {
            get
            {
                var today = DateTime.Today;
                var age = today.Year - BirthDate.Year;
                if (today.Month < BirthDate.Month || (today.Month == BirthDate.Month && today.Day < BirthDate.Day))
                    age--;
                return age;
            }
        }
    }
}
