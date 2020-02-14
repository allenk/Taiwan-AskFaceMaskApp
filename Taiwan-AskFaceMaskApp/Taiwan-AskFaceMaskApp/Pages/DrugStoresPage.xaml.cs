using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taiwan_AskFaceMaskApp.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Taiwan_AskFaceMaskApp.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DrugStoresPage : BasePage
    {
        public DrugStoresPage()
        {
            InitializeComponent();
        }

        private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selectedDrugStore = (sender as ListView).SelectedItem as Models.DrugStore;
            var faceMaskInDrugStore = DbService.Instance.GetFaceMaskData(selectedDrugStore.DrugStoreId);
            var needNavigation = await DisplayAlert("資料結果", $"{selectedDrugStore.Name}\r\n\r\n成人口罩剩餘數: { faceMaskInDrugStore.AdultCount}\r\n兒童口罩剩餘數: {faceMaskInDrugStore.ChildCount}\r\n\r\n來源資料時間: {faceMaskInDrugStore.DataSourceTime}", "導航至藥局", "好，知道了!");

            if (needNavigation)
            {
                var mapLaunchOption = new Xamarin.Essentials.MapLaunchOptions()
                {
                    Name = selectedDrugStore.Name,
                    NavigationMode = Xamarin.Essentials.NavigationMode.Driving
                };
                await Xamarin.Essentials.Map.OpenAsync(selectedDrugStore.Lat, selectedDrugStore.Lng, mapLaunchOption);
            }

            (sender as ListView).SelectedItem = null;
        }

        private void KeywrodSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var vm = (sender as SearchBar).BindingContext as ViewModels.DrugStoresPageViewModel;
            if (vm.IsSearched && e.NewTextValue?.Length == 0 && e.OldTextValue?.Length > 0)
            {
                vm.SearchCommand.Execute(string.Empty);
                vm.IsSearched = false;
            }
        }
    }
}