using System;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Homepage.Pages
{
    public class IndexModel : PageModel
    {
        private static DateTime Birthdate => new DateTime(1995, 08, 16);
        public int Age {
            get {
                var today = DateTime.Today;
                var age = today.Year - Birthdate.Year;
                if (today.Month < Birthdate.Month
                    || (today.Month == Birthdate.Month && today.Day < Birthdate.Day))
                    age--;
                return age;
            }
        }

        public void OnGet()
        {
            //Nothing to do
        }
    }
}
