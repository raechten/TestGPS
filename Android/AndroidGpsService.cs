using System;
using Android.Locations;
using Xamarin.Forms;
using Android.Content;
using TestGPS.Android;

[assembly: Dependency(typeof(AndroidLocationService))]
namespace TestGPS.Android
{
	public class AndroidLocationService : Java.Lang.Object, ILocationService, ILocationListener
	{
		private LocationManager _locationManager;
		private string _locationProvider;
		private Location _currentLocation { get; set; }

		public AndroidLocationService()
		{
			this.InitializeLocationManager();
		}
			
		void InitializeLocationManager()
		{
			_locationManager = (LocationManager)Forms.Context.GetSystemService(Context.LocationService);
		}

		//ILocationService methods
		public void Start()
		{
			// Probably add some more error handling here (make sure _locationManager is set, etc.)

			var criteriaForLocationService = new Criteria { Accuracy = Accuracy.Fine };
			try
			{
				_locationProvider = _locationManager.GetBestProvider(criteriaForLocationService, true);
				if (!string.IsNullOrEmpty(_locationProvider) && _locationManager.IsProviderEnabled(_locationProvider))
				{
					_locationManager.RequestLocationUpdates(_locationProvider, 2000, 1, this);
				}
				else
				{
					// Do something here to notify the user we can't get the location updates set up properly
					System.Diagnostics.Debug.WriteLine("No (enabled) location provider available.");
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex);
			}

		}


		public void SetLocation()
		{
			var currentLocationString = _currentLocation == null 
											? "Can't determine the current address." 
											: string.Format("{0} - {1}", _currentLocation.Latitude, _currentLocation.Longitude);

			MessagingCenter.Send<ILocationService, string> (this, Messaging.LocationUpdated, currentLocationString);

		}
			
		// ILocationListener methods
		public void OnLocationChanged(Location location)
		{
			_currentLocation = location;
			SetLocation();
		}

		public void OnStatusChanged(string provider, Availability status, global::Android.OS.Bundle extras)
		{
			//Not Implemented
		}

		public void OnProviderDisabled(string provider)
		{
			//Not Implemented
		}

		public void OnProviderEnabled(string provider)
		{
			//Not Implemented
		}
	}
}

