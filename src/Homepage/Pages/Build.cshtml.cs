using System;
using System.Collections.Generic;
using Homepage.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace Homepage.Pages
{
    public class Build : PageModel
    {
        private BuildProperties _buildProperties;
        public void OnGet([FromServices]IOptions<BuildProperties> buildProperties)
        {
            _buildProperties = buildProperties.Value;
        }

        public string PropertiesAsString()
        {
            if (_buildProperties == null)
                return "Section not found in appsettings";
            var properties = new List<string>();
            foreach(var property in _buildProperties.GetType().GetProperties())
                properties.Add($"{property.Name}: {property.GetValue(_buildProperties)}");
            return string.Join(Environment.NewLine, properties);
        }
    }
}
