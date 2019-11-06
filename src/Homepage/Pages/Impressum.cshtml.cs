using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Homepage.Pages
{
    public class Impressum : PageModel
    {
        private readonly ILogger<Impressum> _logger;

        public Impressum(ILogger<Impressum> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
