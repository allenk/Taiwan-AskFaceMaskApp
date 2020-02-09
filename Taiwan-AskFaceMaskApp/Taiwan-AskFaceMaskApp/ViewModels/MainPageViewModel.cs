using System.Collections.ObjectModel;

using Xam.Plugin.BaseBindingLibrary;
using Xamarin.Forms.GoogleMaps;

namespace Taiwan_AskFaceMaskApp.ViewModels
{
	public class MainPageViewModel : BaseNotifyProperty
    {
		private CameraUpdate _cameraUpdate;

		public CameraUpdate CameraUpdate
		{
			get { return _cameraUpdate; }
			set { OnPropertyChanged<CameraUpdate>(ref _cameraUpdate, value); }
			
		}

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
			set => OnPropertyChanged(ref _places, value);
		}

		public MainPageViewModel()
		{
			IsEnabled = true;

			CameraUpdate = CameraUpdateFactory.NewCameraPosition(new CameraPosition(new Position(25.033278, 121.540794), 14.6, 0, 0));

			//台灣地理中心位置
			//CameraUpdate = CameraUpdateFactory.NewCameraPosition(new CameraPosition(new Position(23.9739356, 120.9787068), 7.3, 0, 0));
			
			Places = BuildPlaces();
		}

		private ObservableCollection<Models.Place> BuildPlaces()
		{
			var drugStores = Services.DbService.Instance.GetDrugStoreData();

			return new ObservableCollection<Models.Place>()
			{
				new Models.Place() { Address = "台北市大安區信義路三段140號", Name = "衛生福利部 中央健康保險署" , Location = new Position(25.033779, 121.540299)},
			};

			//return new ObservableCollection<PinPlace>(from drugStore in drugStores
			//										  select new PinPlace()
			//										  {
			//											  DrugStoreId = drugStore.DrugStoreId,
			//											  Address = drugStore.Address,
			//											  Name = drugStore.Name,
			//											  Location = new Position(drugStore.Lat, drugStore.Lng),
			//											  Tel = drugStore.Tel
			//										  });

		}
	}
}
