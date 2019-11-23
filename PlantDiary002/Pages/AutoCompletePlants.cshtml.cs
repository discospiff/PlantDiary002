using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PlantDiary002
{
    
    public class AutoCompletePlantsModel : PageModel
    {
        [BindProperty]
        private string Term { get; set; }
        private IList<string> plantNames = new List<string>();

        public JsonResult OnGet()
        {
            Term = HttpContext.Request.Query["term"];

            AddPlants("Redbud");
            AddPlants("Red Maple");
            AddPlants("Red Oak");
            AddPlants("Red Rose");
            AddPlants("Red Wine");
            return new JsonResult(plantNames);
        }

        private void AddPlants(string plantName)
        {
            if (plantName.Contains(Term))
            {
                plantNames.Add(plantName);
            }

        }
    }
}
