using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JamestsaiTW.Utilities
{
    public class ConvertDataHelper
    {
		public static List<T> CsvToJson<T>(string data)
		{
			try
			{
				var csv = new List<string[]>();
				var lines = data.Replace("\r", "").Split('\n');

				foreach (string line in lines)
					csv.Add(line.Split(','));

				var tmpProperties = lines[0].Split(',');
				var properties = ConvertCsvTitle(tmpProperties);

				var listObjResult = new List<Dictionary<string, string>>();

				for (int i = 1; i < lines.Length - 1; i++)
				{
					var objResult = new Dictionary<string, string>();
					for (int j = 0; j < properties.Length; j++)
						objResult.Add(properties[j], csv[i][j]);

					listObjResult.Add(objResult);
				}
				var jsonData = JsonConvert.SerializeObject(listObjResult);
				var finalData = JsonConvert.DeserializeObject<List<T>>(jsonData);
				return finalData;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}
			return null;
		}



		private static string[] ConvertCsvTitle(string[] tmpProperties)
		{
			Dictionary<string, string> keyValuePairs = new Dictionary<string, string>() {
				{ "醫事機構代碼","DrugStoreId" },
				{ "醫事機構名稱","Name" },
				{ "醫事機構地址","Address" },
				{ "醫事機構電話","Tel" },
				{ "成人口罩剩餘數","AdultCount" },
				{ "兒童口罩剩餘數","ChildCount" },
				{ "來源資料時間","DataSourceTime" }
			};

			var newProperties = new string[tmpProperties.Length];

			for (var index = 0; index < tmpProperties.Length; index++)
			{
				if (keyValuePairs.ContainsKey(tmpProperties[index]))
					newProperties[index] = keyValuePairs[tmpProperties[index]];
			}
			return newProperties;
		}
	}
}
