using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SQLite;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

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
        private SQLiteConnection DrugStoresDbConnection
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

        internal Models.FaceMaskInDrugStore GetFaceMaskData(string drugStoreId)
        {
            var faceMaskInDrugStore = new Models.FaceMaskInDrugStore();

            try
            {
                faceMaskInDrugStore = DrugStoresDbConnection.Get<Models.FaceMaskInDrugStore>(drugStoreId);
            }
            catch (InvalidOperationException invalidOperationEx)
            {
                System.Diagnostics.Debug.WriteLine(invalidOperationEx.Message);
                faceMaskInDrugStore.DrugStoreId = drugStoreId;
                faceMaskInDrugStore.AdultCount = "無資料";
                faceMaskInDrugStore.ChildCount = "無資料";
                faceMaskInDrugStore.DateSourceTime = "無資料";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return faceMaskInDrugStore;
        }

        internal ObservableCollection<Models.DrugStore> GetDrugStoreData(string query = "")
        {
            ObservableCollection<Models.DrugStore> tmpObservableCollection;
            if (!string.IsNullOrEmpty(query))
            {
                var queryData = DrugStoresDbConnection.Table<Models.DrugStore>().Where(item => item.Name.Contains(query));
                tmpObservableCollection = new ObservableCollection<Models.DrugStore>(queryData);
            }
            else
            {
                tmpObservableCollection = new ObservableCollection<Models.DrugStore>(DrugStoresDbConnection.Table<Models.DrugStore>());
            }
            return tmpObservableCollection;
        }

        private string rootFolder { get { return Xamarin.Essentials.FileSystem.AppDataDirectory; } }
        public DbService()
        {
            BuildDrugStoreBaseData();
            BuildFaceMaskInDrugStoreData();
        }

        private async void BuildFaceMaskInDrugStoreData()
        {
            var result = CheckFaceMaskInDrugStoreDataIsNeedUpdate();
            if (result)
            {
                await UpdateFaceMaskInDrugStoreData();
            }
        }

        public async Task UpdateFaceMaskInDrugStoreData()
        {
            //call api
            var faceMaskData = await OpenDataService.Instance.GetFaceMaskData();

            try
            {
                //有資料更新時重置 DB 的該 Table 所有資訊。
                DrugStoresDbConnection.DropTable<Models.FaceMaskInDrugStore>();

                DrugStoresDbConnection.CreateTable<Models.FaceMaskInDrugStore>();

                System.Diagnostics.Debug.WriteLine(faceMaskData?.Count);

                DrugStoresDbConnection.InsertAll(faceMaskData, true);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DBService Exception: {ex.Message}");
            }
            finally
            {
                Xamarin.Essentials.Preferences.Set("FaceMaskDataUpdateDateTime", DateTime.Now);
            }
        }

        private bool CheckFaceMaskInDrugStoreDataIsNeedUpdate()
        {
            if (!Xamarin.Essentials.Preferences.ContainsKey("FaceMaskDataUpdateDateTime"))
                return true;

            var oldUpdateDateTime = Xamarin.Essentials.Preferences.Get("FaceMaskDataUpdateDateTime", DateTime.Now);
            
            //NHI 公告資料每 90 秒更新一次。
            return oldUpdateDateTime.AddSeconds(90) < DateTime.Now;
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
                    //有資料更新時重置 DB 的該 Table 所有資訊。
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
