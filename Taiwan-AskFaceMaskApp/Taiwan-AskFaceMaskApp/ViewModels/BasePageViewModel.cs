using Xam.Plugin.BaseBindingLibrary;

namespace Taiwan_AskFaceMaskApp.ViewModels
{
    public class BasePageViewModel : BaseNotifyProperty
    {
		private bool _isOnInternet;

		public bool IsOnInternet
		{
			get { return _isOnInternet; }
			set { OnPropertyChanged<bool>(ref _isOnInternet, value); }
		}
	}
}