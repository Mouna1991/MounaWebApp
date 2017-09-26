using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Airport
    {
        public string iata { get; set; }
        public decimal lon { get; set; }
        public string iso { get; set; }
        public int status { get; set; }
        public string name { get; set; }
        public string continent { get; set; }

        public string type { get; set; }

        public decimal lat { get; set; }

        public string size { get; set; }

    }
}