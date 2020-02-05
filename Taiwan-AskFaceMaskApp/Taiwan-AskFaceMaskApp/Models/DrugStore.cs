using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using Newtonsoft.Json;

namespace Taiwan_AskFaceMaskApp.Models
{
    public class DrugStore
    {
        [PrimaryKey, AutoIncrement]
        [JsonIgnore]
        public string Id { get; set; }
        public string DrugStoreId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Tel { get; set; }
        public float Lat { get; set; }
        public float Lng { get; set; }
        public object Area { get; set; }
    }




}
