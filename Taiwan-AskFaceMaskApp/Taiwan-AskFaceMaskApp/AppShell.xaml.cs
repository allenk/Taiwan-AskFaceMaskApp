using Xamarin.Forms;

namespace Taiwan_AskFaceMaskApp
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }

        private void CallPhoneMenuItem_Clicked(object sender, System.EventArgs e)
        {
            Xamarin.Essentials.PhoneDialer.Open("1922");
        }

        private async void MohwNewsMenuItem_Clicked(object sender, System.EventArgs e)
        {
            await Xamarin.Essentials.Browser.OpenAsync("https://www.mohw.gov.tw/lp-4635-1.html").ConfigureAwait(false);
        }
    }
}
