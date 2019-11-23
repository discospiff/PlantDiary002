using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PlantDiary002.Pages
{
    public class ConsumeXMLModel : PageModel
    {
        private IHostingEnvironment _environment;
        private string result = "";
        public ConsumeXMLModel(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        [BindProperty]
        public IFormFile Upload { get; set; }
        public void OnGet()
        {

        }

        public void OnPost()
        {
            string fileName = Upload.FileName;
            string file = Path.Combine(_environment.ContentRootPath, "uploads", fileName);
            using (var fileStream = new FileStream(file, FileMode.Create))
            {
                Upload.CopyTo(fileStream);
            }
            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            XmlNode node = doc.SelectSingleNode("/plant/specimens/specimen[latitude>0]");
            ValidateXML(file);

        }

        private void ValidateXML(string file)
        {
            // delcare our validation preferences
            XmlReaderSettings settings = new XmlReaderSettings();
            string xsdPath = Path.Combine(_environment.ContentRootPath, "uploads", "plants.xsd");
            settings.Schemas.Add(null, xsdPath);

            // validate with XSD
            settings.ValidationType = ValidationType.Schema;

            // a couple more flags.
            settings.ValidationFlags |= System.Xml.Schema.XmlSchemaValidationFlags.ProcessSchemaLocation;
            settings.ValidationFlags |= System.Xml.Schema.XmlSchemaValidationFlags.ReportValidationWarnings;

            // who do we call when things go wrong?
            settings.ValidationEventHandler += new System.Xml.Schema.ValidationEventHandler(this.ValidationEventHandler);

            XmlReader xmlReader = XmlReader.Create(file, settings);
            try
            {
                while (xmlReader.Read())
                {

                }
                ViewData["result"] = "Validated!";
            }
            catch (Exception e)
            {
                // we only get here if there is a validation error.
                ViewData["result"] = e.Message;
            }


        }

        private void ValidationEventHandler(object sender, ValidationEventArgs args)
        {
            result = "validation failed.  Message: " + args.Message;
            throw new Exception("validation failed.  Message: " + args.Message);
        }
    }
}