using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace Taiwan_AskFaceMaskApp.Pages
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : BasePage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Pin_Clicked(object sender, EventArgs e)
        {
            var pinPlace = (sender as Pin).BindingContext as Models.Place;
            System.Diagnostics.Debug.WriteLine($"{pinPlace?.DrugStoreId}");

            if (!string.IsNullOrEmpty(pinPlace?.DrugStoreId))
            {
                var faceMaskInDrugStore = Services.DbService.Instance.GetFaceMaskData(pinPlace?.DrugStoreId);
                var needNavigation = await DisplayAlert("資料結果", $"{pinPlace.Name}\r\n\r\n成人口罩剩餘數: { faceMaskInDrugStore.AdultCount}\r\n兒童口罩剩餘數: {faceMaskInDrugStore.ChildCount}\r\n\r\n來源資料時間: {faceMaskInDrugStore.DataSourceTime}", "導航至藥局", "好，知道了!");

                if (needNavigation)
                {
                    var mapLaunchOption = new Xamarin.Essentials.MapLaunchOptions()
                    {
                        Name = pinPlace.Name,
                        NavigationMode = Xamarin.Essentials.NavigationMode.Driving
                    };
                    await Xamarin.Essentials.Map.OpenAsync(pinPlace.Location.Latitude, pinPlace.Location.Longitude, mapLaunchOption);
                }
            }
           
        }

        private void Map_PinClicked(object sender, PinClickedEventArgs e)
        {
            var map = sender as Map;
            var currentCameraPosition = new CameraPosition(e.Pin.Position, map.CameraPosition.Zoom, 0, 0);
            map.MoveCamera(CameraUpdateFactory.NewCameraPosition(currentCameraPosition));
            
            Xamarin.Essentials.Preferences.Set("LastCameraPosition", JsonConvert.SerializeObject(new Utilities.ExtensionCameraPosition(currentCameraPosition)));
            (BindingContext as ViewModels.MainPageViewModel).MapCameraUpdate = CameraUpdateFactory.NewCameraPosition(currentCameraPosition);
        }
    }
}
