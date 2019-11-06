using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Homepage.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private DateTime birthdate = new DateTime(1995, 08, 16);
        public int Age {
            get {
                var today = DateTime.Today;
                var age = today.Year - birthdate.Year;
                if (today.Month < birthdate.Month
                    || (today.Month == birthdate.Month && today.Day < birthdate.Day))
                    age--;
                return age;
            }
        }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}
