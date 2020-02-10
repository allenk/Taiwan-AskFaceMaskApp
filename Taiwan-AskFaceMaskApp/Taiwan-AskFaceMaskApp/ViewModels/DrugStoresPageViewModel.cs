using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

using Taiwan_AskFaceMaskApp.Services;
using System.Windows.Input;
using Xamarin.Forms;

namespace Taiwan_AskFaceMaskApp.ViewModels
{
    public class DrugStoresPageViewModel : BasePageViewModel
	{
		private ObservableCollection<Models.DrugStore> _drugStores;
		public ObservableCollection<Models.DrugStore> DrugStores
		{
			get { return _drugStores; }
			set => OnPropertyChanged(ref _drugStores, value);
		}

		private bool _isRunning;

		public bool IsRunning
		{
			get { return _isRunning; }
			set { OnPropertyChanged<bool>(ref _isRunning, value); }
		}


		public DrugStoresPageViewModel()
		{
			DrugStores = DbService.Instance.GetDrugStoreData();
			System.Diagnostics.Debug.WriteLine(DrugStores?.Count);
		}

		public ICommand SearchCommand
		{
			get
			{
				return new Command<string>((query) =>
				{
					DrugStores = DbService.Instance.GetDrugStoreData(query);
				});
			}
		}

		public ICommand ToolbarItemCommand
		{
			get
			{
				return new Command(async () =>
				{
					if(IsOnInternet)
					{ 
						IsRunning = true;
						await DbService.Instance.UpdateFaceMaskInDrugStoreData();
						IsRunning = false;
						return;
					}
					await (App.Current as App).MainPage.DisplayAlert("網路不通", "請檢查設備連線狀態...", "好");
				});
			}
		}
	}
}
