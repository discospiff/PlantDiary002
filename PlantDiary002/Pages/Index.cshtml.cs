using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace PlantDiary002.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            String myName = "Brandan Jones";
            int age = 31;
            ViewData["MyName"] = myName;
            ViewData["age"] = age;

            // download the JSON data.
            // a web client gives us access to data on the internet.
            using(WebClient webClient = new WebClient())
            {
                // get the raw JSON data.
                string jsonData = webClient.DownloadString("https://www.plantplaces.com/perl/mobile/viewspecimenlocations.pl?Lat=39.14455075&Lng=-84.5093939666667&Range=0.5&Source=location&Version=2");
                // Marshall the data into a series of objects.
                QuickType.Welcome welcome = QuickType.Welcome.FromJson(jsonData);
                // get the list (collection) of specimens
                List<QuickType.Specimen> allSpecimens = welcome.Specimens;
                // make the specimen data available to our web page.
                ViewData["allSpecimens"] = allSpecimens;

                // iterate over the specimens so we can shake hands with them.

                foreach (QuickType.Specimen specimen in allSpecimens)
                {
                    // shake hands with one specimen at a time.
                    Console.WriteLine(specimen);
                }
            }


        }
    }
}
