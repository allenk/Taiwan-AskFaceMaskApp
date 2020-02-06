using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using Newtonsoft.Json;
using System.Windows.Input;
using Xamarin.Forms;

namespace Taiwan_AskFaceMaskApp.Models
{
    public class DrugStore
    {
        [PrimaryKey, AutoIncrement]
        [JsonIgnore]
        public int Id { get; set; }
        public string DrugStoreId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Tel { get; set; }
        public float Lat { get; set; }
        public float Lng { get; set; }
        public string Area { get; set; }

        [Ignore]
        public ICommand CallPhoneToDrugStoreCommand
        {
            get
            {
                return new Command<string>((tel) =>
                {
                    Xamarin.Essentials.PhoneDialer.Open(tel);
                });
            }
        }
    }
}
