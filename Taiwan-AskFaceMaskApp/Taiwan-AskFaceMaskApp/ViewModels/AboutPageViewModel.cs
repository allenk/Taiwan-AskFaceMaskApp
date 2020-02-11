using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Taiwan_AskFaceMaskApp.ViewModels
{
    public class AboutPageViewModel : BasePageViewModel
    {
		private string _nhiUrl;

		public string NhiUrl
		{
			get { return _nhiUrl; }
			set { OnPropertyChanged<string>(ref _nhiUrl, value); }
		}

		private string _appVersion;

		public string AppVersion
		{
			get { return _appVersion; }
			set { OnPropertyChanged<string>(ref _appVersion, value); }
		}

		public AboutPageViewModel()
		{
			AppVersion = Xamarin.Essentials.AppInfo.VersionString;
			NhiUrl = "https://www.nhi.gov.tw/Content_List.aspx?n=395F52D193F3B5C7";
		}

		public ICommand OpenNhiUrlCommand
		{
			get
			{
				return new Command(async () =>
				{
					await Xamarin.Essentials.Browser.OpenAsync(NhiUrl);
				});
			}
		}
	}
}
