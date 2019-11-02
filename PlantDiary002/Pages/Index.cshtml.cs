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
                // get the raw plant metadata
                string plantData = webClient.DownloadString("http://www.plantplaces.com/perl/mobile/viewplantsjsonarray.pl?WetTolerant=on");
                // parse to objects
                QuickTypePlant.Plant[] allPlants = QuickTypePlant.Plant.FromJson(plantData);

                // put our plant definitions in a dictionary.
                IDictionary<long, QuickTypePlant.Plant> plantDictionary = new Dictionary<long, QuickTypePlant.Plant>();

                // populate our dictionary (assign seats in our aircraft)
                foreach(QuickTypePlant.Plant plant in allPlants)
                {
                    plantDictionary.Add(plant.Id, plant);
                }

                // get the raw JSON data.
                string jsonData = webClient.DownloadString("https://www.plantplaces.com/perl/mobile/viewspecimenlocations.pl?Lat=39.14455075&Lng=-84.5093939666667&Range=0.5&Source=location&Version=2");
                // Marshall the data into a series of objects.
                QuickType.Welcome welcome = QuickType.Welcome.FromJson(jsonData);
                // get the list (collection) of specimens
                List<QuickType.Specimen> allSpecimens = welcome.Specimens;
                
                // make a collection of ONLY specimens that like water.
                IList<QuickType.Specimen> waterLovingSpecimens = new List<QuickType.Specimen>();
                
                // iterate over the specimens so we can shake hands with them.
                foreach (QuickType.Specimen specimen in allSpecimens)
                {
                    // shake hands with one specimen at a time.
                    Console.WriteLine(specimen);

                    // see if this specimen likes water.
                    if (plantDictionary.ContainsKey(specimen.PlantId) )
                    {
                        // we are only here if the plant dictionary contains the plant that is associated with this specimen.
                        waterLovingSpecimens.Add(specimen);
                    }
                }

                // make the specimen data available to our web page.
                ViewData["allSpecimens"] = waterLovingSpecimens;

            }


        }
    }
}
