using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SQLite;
using Newtonsoft.Json;
namespace Taiwan_AskFaceMaskApp.Services
{
    public class DbService
    {
        private static DbService _instance;
        public static DbService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DbService();
                }
                return _instance;

            }
        }

        private SQLiteConnection _drugStoresDbConnection;
        public SQLiteConnection DrugStoresDbConnection
        {
            get
            {
                if (_drugStoresDbConnection == null)
                {
                    var dbPath = System.IO.Path.Combine(rootFolder, "DrugStores.db");
                    _drugStoresDbConnection = new SQLiteConnection(dbPath);
                }
                return _drugStoresDbConnection;

            }
        }

        private string rootFolder { get { return Xamarin.Essentials.FileSystem.AppDataDirectory; } }
        public DbService()
        {
            //DrugStoresDbConnection.CreateTable<Models.DrugStore>();
            
            //if(TableExists("DrugStore"))
                BuildDrugStoreBaseData();

        }

        private async void BuildDrugStoreBaseData()
        {
            var drugStoreData = string.Empty;
            var dataStream = await Xamarin.Essentials.FileSystem.OpenAppPackageFileAsync("drugstore-data.txt");
            using (var streamReader = new StreamReader(dataStream, Encoding.UTF8))
            {
                drugStoreData = streamReader.ReadToEnd();
            }

            try
            {
                var drugStores = JsonConvert.DeserializeObject<List<Models.DrugStore>>(drugStoreData);
                System.Diagnostics.Debug.WriteLine(drugStores);

                //foreach (var item in drugStores)
                //{
                //    DrugStoresDbConnection.Insert(item);
                //}
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DBService Exception: {ex.Message}");

            }
        }

        public bool TableExists(string tableName)
        {
            bool sw = false;
            try
            {

                    string query = string.Format("SELECT name FROM sqlite_master WHERE type='table' AND name='{0}';", tableName);
                    SQLiteCommand cmd = DrugStoresDbConnection.CreateCommand(query);
                    var item = DrugStoresDbConnection.Query<object>(query);
                    if (item.Count > 0)
                        sw = true;
                    return sw;
                
            }
            catch (SQLiteException ex)
            {
                System.Diagnostics.Debug.WriteLine($"DBService Exception:{ex.Message}");
                return false;
            }
           
        }
    }
}
