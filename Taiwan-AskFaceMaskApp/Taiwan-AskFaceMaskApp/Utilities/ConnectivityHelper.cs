using System;
using System.Collections.Generic;
using System.Text;
using Taiwan_AskFaceMaskApp;
using Xamarin.Essentials;

namespace JamestsaiTW.Utilities
{
    public class ConnectivityHelper 
    {
        private static ConnectivityHelper _instance;
        public static ConnectivityHelper Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ConnectivityHelper();
                return _instance;
            }
        }
        public bool IsOnInternet { get; private set; }
  
        private ConnectivityHelper()
        {

            CheckIsInternet(Connectivity.NetworkAccess);
            // Register for connectivity changes, be sure to unsubscribe when finished
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        private void CheckIsInternet(NetworkAccess networkAccess)
        {
            IsOnInternet = networkAccess == NetworkAccess.Internet;
        }

        internal void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            var networkAccess = e.NetworkAccess;
            var connectionProfile = e.ConnectionProfiles;

            CheckIsInternet(networkAccess);

            ((App.Current as App).CurrentPageViewModel).IsOnInternet = IsOnInternet;

            System.Diagnostics.Debug.WriteLine(connectionProfile);
        }
    }

}
