using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using Newtonsoft.Json;
using System.Net.Http;

namespace WebApplication1.Controllers
{
    public class AirportController : Controller
    {

        IEnumerable<Airport> airports = GetAirports("https://raw.githubusercontent.com/jbrooksuk/JSON-Airports/master/airports.json");
        // GET

        //This get method is populating the list of countries and the list of airports when the app 
        //gets launched. countries and European airports are both stored in the ViewBag
       
        public ActionResult Create()
        {
         
            //This is a LINQ query to retrieve all European airports since the assignment is focusing on 
            //airports in Europe
            IEnumerable<Airport> EuropeanAirports = from n in airports
                           where n.continent.Equals("EU")
                           select n;
            IEnumerable<Country> countries = GetCountries();
            ViewBag.countries = countries;
            ViewBag.EuropeanAirports = EuropeanAirports; 
            return View(new Country());
        }







        [HttpPost]
        public ActionResult Create(Country postedCountry)
        {
            IEnumerable<Country> countries = GetCountries();
            //retrieving the country selected by the end user in the view 
            var selectedCountry = postedCountry.country;
            //retrieving the abbreviation corresponding to the country that was previously selected by the user 
            //in the view 
            var abbreviation = (from n in countries
                                where n.country
                                .Equals(selectedCountry, StringComparison.InvariantCultureIgnoreCase) // ignore case
                                select n.abbr).FirstOrDefault();
            //retrieving the list of airports within the specific country selected by the end user 
            List<Airport> EuropeanAirports = (from n in airports
                                    where n.iso.Equals(abbreviation, StringComparison.InvariantCultureIgnoreCase)
                                    select n).ToList();
           
            ViewBag.EuropeanAirports = EuropeanAirports;
            return Json(EuropeanAirports, JsonRequestBehavior.AllowGet);

        }








       //This method is retrieving the list of airports from the JSON file that I was provided with 

        public static IEnumerable<Airport> GetAirports(string url)
        {
            IEnumerable<Airport> airports = null;
            HttpResponseMessage response;
            // write some code that retrieves the content from the url, and convert it to a list of airports.
            using (var client = new HttpClient())
            {
                try
                {
                    response = client.GetAsync(url).Result;
                }
                catch (Exception e)
                {

                    throw e;
                }
                if (response.IsSuccessStatusCode)
                {
                    // by calling .Result you are performing a synchronous call
                    var responseContent = response.Content;

                    // by calling .Result you are synchronously reading the result
                    string responseString = responseContent.ReadAsStringAsync().Result;
                    airports = JsonConvert.DeserializeObject<List<Airport>>(responseString);

                }
            }
            return airports;
        }







        //This method is retrieving the list of countries from a JSON file that I created online, this will be
        //used to populate a separate drop down list for countries in the view 
        public static IEnumerable<Country> GetCountries()
        {
            //The list of countries and abbreviations is stored in a JSON file that I created online, here is the link
            //leading to the file . 
            string path = "https://api.myjson.com/bins/11x7yp";

            IEnumerable<Country> countries = null;
            HttpResponseMessage response;
            // write some code that retrieves the content from the url, and convert it to a list of airports.
            using (var client = new HttpClient())
            {
                try
                {
                    response = client.GetAsync(path).Result;
                }
                catch (Exception e)
                {

                    throw e;
                }
                if (response.IsSuccessStatusCode)
                {
                    // by calling .Result you are performing a synchronous call
                    var responseContent = response.Content;

                    // by calling .Result you are synchronously reading the result
                    string responseString = responseContent.ReadAsStringAsync().Result;
                    countries = JsonConvert.DeserializeObject<List<Country>>(responseString);

                }
            }
            return countries;
        }
    }
}