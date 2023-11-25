using System.Reflection;

namespace Homepage.Helpers
{
    public static class ApplicationInformations
    {
        private static string _version;
        public static string Version
        {
            get
            {
                if (_version == null)
                {
                    var assembly = Assembly.GetEntryAssembly();
                    _version = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? assembly.GetName().Version?.ToString();
                }

                return _version;
            }
        }
    }
}
