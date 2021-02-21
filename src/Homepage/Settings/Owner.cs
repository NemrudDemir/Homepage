namespace Homepage.Settings
{
    public static class Owner
    {
        private const string _firstName = "Nemrud";
        private const string _lastName = "Demir";
        private const string _city = "Stuttgart";
        private const string _country = "Germany";

        public static string FullName => $"{_firstName} {_lastName}";
        public static string Job => "Full-Stack Developer"; 
        public static string Base => $"{_city}, {_country}";
    }
}