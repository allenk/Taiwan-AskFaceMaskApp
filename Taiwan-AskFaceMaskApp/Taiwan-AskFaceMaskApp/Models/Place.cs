using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.GoogleMaps;

namespace Taiwan_AskFaceMaskApp.Models
{
    public class Place
    {
        public string DrugStoreId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Tel { get; set; }
        public Position Location { get; set; }

        public string Note { get; set; }
    }
}
