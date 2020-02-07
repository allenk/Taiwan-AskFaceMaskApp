using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using JamestsaiTW.Utilities;

namespace Taiwan_AskFaceMaskApp.Services
{
    public class OpenDataService
    {
        private static OpenDataService _instance;
        public static OpenDataService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new OpenDataService();
                }
                return _instance;

            }
        }

        private HttpClient _httpClient;
        private HttpClient HttpClient
        {
            get
            {
                if (_httpClient == null)
                {
                    _httpClient = new HttpClient();
                }
                return _httpClient;

            }
        }

        private readonly string nhiOpenDataUrl = "https://data.nhi.gov.tw/resource/mask/maskdata.csv";

        public async Task<ObservableCollection<Models.FaceMaskInDrugStore>> GetFaceMaskData()
        {
           var result = await HttpClient.GetStringAsync(nhiOpenDataUrl);

			var data = ConvertDataHelper.CsvToJson<Models.FaceMaskInDrugStore>(result);

			System.Diagnostics.Debug.WriteLine(data);

			Xamarin.Essentials.Preferences.Set("RealFaceMaskDataUpdateDateTime", DateTime.Now);

			return new ObservableCollection<Models.FaceMaskInDrugStore>(data);
        }


	}
}
