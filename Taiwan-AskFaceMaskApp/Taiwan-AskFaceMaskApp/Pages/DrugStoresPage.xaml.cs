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
    public partial class DrugStoresPage : ContentPage
    {
        public DrugStoresPage()
        {
            InitializeComponent();
        }

        private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var drugStore = ((sender as ListView).SelectedItem) as Models.DrugStore;
            var drugStoreId = drugStore.DrugStoreId;
            var realFaceMaskData = DbService.Instance.GetRealFaceMaskData(drugStoreId);
            await DisplayAlert("資料結果:", $"{drugStore.Name}\r\n\r\n成人口罩剩餘數量: {realFaceMaskData.AdultCount}\r\n兒童口罩剩餘數量: {realFaceMaskData.ChildCount}\r\n\r\n資料更新時間: {realFaceMaskData.DataSourceTime}","好, 知道了");
            (sender as ListView).SelectedItem = null;
        }
    }
}