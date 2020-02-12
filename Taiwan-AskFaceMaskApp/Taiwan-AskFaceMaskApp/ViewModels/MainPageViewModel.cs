using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Linq;

using Xamarin.Forms.GoogleMaps;
using System.Windows.Input;
using Xamarin.Forms;

namespace Taiwan_AskFaceMaskApp.ViewModels
{
    public class MainPageViewModel : BasePageViewModel
	{
		private bool _isEnabled;

		public bool IsEnabled
		{
			get { return _isEnabled; }
			set { OnPropertyChanged<bool>(ref _isEnabled, value); }
		}

		private ObservableCollection<Models.Place> _places;

		public ObservableCollection<Models.Place> Places
		{
			get { return _places; }
			set { OnPropertyChanged<ObservableCollection<Models.Place>>(ref _places, value); }
		}

		private CameraUpdate _mapCameraUpdate;

		public CameraUpdate MapCameraUpdate
		{
			get { return _mapCameraUpdate; }
			set { OnPropertyChanged<CameraUpdate>(ref _mapCameraUpdate, value); }
		}

		private bool _isRunning;

		public bool IsRunning
		{
			get { return _isRunning; }
			set { OnPropertyChanged<bool>(ref _isRunning, value); }
		}

		public MainPageViewModel()
		{
			IsEnabled = true;

			//Hack 1:
			//搭配只顯示 衛生福利部中央健康保險署 地標
			//MapCameraUpdate = CameraUpdateFactory.NewCameraPosition(new CameraPosition(new Position(25.033308, 121.540232), 14.6, 0, 0));

			//台灣地理中心 : 搭配地圖顯示全台灣的藥局
			MapCameraUpdate = CameraUpdateFactory.NewCameraPosition(new CameraPosition(new Position(23.974004, 120.979703), 7.3, 0, 0));

			Places = BuildPlaces();
		}

		private ObservableCollection<Models.Place> BuildPlaces()
		{
			//Hack
			//衛生福利部中央健康保險署
			//return new ObservableCollection<Models.Place>() 
			//{ 
			//	new Models.Place() { Name= "衛生福利部中央健康保險署", Address = "台北市大安區信義路三段140號" , Tel = "02-27065866" , Location = new Position(25.033308, 121.540232) } 
			//};

			var drugStorePlaces = Services.DbService.Instance.GetDrugStoreData();

			return new ObservableCollection<Models.Place>(
					from drugStore in drugStorePlaces
					select new Models.Place
					{
						Name = drugStore.Name,
						Address = drugStore.Address,
						Tel = drugStore.Tel,
						Location = new Position(drugStore.Lat, drugStore.Lng),
						DrugStoreId = drugStore.DrugStoreId
					}
				);
		}

		public ICommand ToolbarItemCommand
		{
			get
			{
				return new Command(async () =>
				{
					if (IsOnInternet)
					{
						IsRunning = true;
						await Services.DbService.Instance.UpdateFaceMaskInDrugStoreData();
						IsRunning = false;
						return;
					}
					await (App.Current as App).MainPage.DisplayAlert("網路連線錯誤", "請檢查裝置連線狀態...", "好");
				});
			}
		}
	}
}
