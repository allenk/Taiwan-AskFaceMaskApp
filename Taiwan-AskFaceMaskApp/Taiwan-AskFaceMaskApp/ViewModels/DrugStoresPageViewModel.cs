using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using Xam.Plugin.BaseBindingLibrary;
using Taiwan_AskFaceMaskApp.Services;

namespace Taiwan_AskFaceMaskApp.ViewModels
{
    public class DrugStoresPageViewModel : BaseNotifyProperty
    {
		private ObservableCollection<Models.DrugStore> _drugStores;
		public ObservableCollection<Models.DrugStore> DrugStores
		{
			get { return _drugStores; }
			set => OnPropertyChanged(ref _drugStores, value);
		}

		public DrugStoresPageViewModel()
		{
			var test = DbService.Instance;
		}
	}
}
