using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using JamestsaiTW.Utilities;
using System.Net.Http;
using System.Threading.Tasks;

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

		public HttpClient HttpClient
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

			var result = await HttpClient.GetStringAsync(nhiOpenDataUrl).ConfigureAwait(false);
			var data = ConvertDataHelper.CsvToJson<Models.FaceMaskInDrugStore>(result);
			var faceMaskInDrugStores = new ObservableCollection<Models.FaceMaskInDrugStore>(data);

			System.Diagnostics.Debug.WriteLine(faceMaskInDrugStores);

			Xamarin.Essentials.Preferences.Set("FaceMaskDataUpdateDateTime", DateTime.Now);

			return faceMaskInDrugStores;
		}

	}
}
