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

            var result = CheckDrugStoreIsNeedUpdate(ref drugStoreData);

            if (result.Item1)
            {
                try
                {
                    //有更新時重置 DB 的 Table 所有資訊。
                    DrugStoresDbConnection.DropTable<Models.DrugStore>();

                    DrugStoresDbConnection.CreateTable<Models.DrugStore>();

                    var drugStores = JsonConvert.DeserializeObject<List<Models.DrugStore>>(drugStoreData);
                    System.Diagnostics.Debug.WriteLine(drugStores);

                    DrugStoresDbConnection.InsertAll(drugStores, true);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"DBService Exception: {ex.Message}");

                }
                finally
                {
                    Xamarin.Essentials.Preferences.Set("DrugStore_Data_Var", result.Item2);
                }
            }
        }

        private Tuple<bool,string> CheckDrugStoreIsNeedUpdate(ref string drugStoreData)
        {
            var newVerStr = drugStoreData.Substring(0, 10);
            var newVerNumber = long.Parse(newVerStr);
            drugStoreData = drugStoreData.Remove(0, 10);
            var oldVerNumber = long.Parse(Xamarin.Essentials.Preferences.Get("DrugStore_Data_Var", "2020020500"));
            var result = newVerNumber > oldVerNumber;
            return new Tuple<bool, string>(result, newVerStr);
        }
    }
}
