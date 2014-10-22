using Xamarin.Forms;

namespace TestGPS
{

	public class GpsPage : ContentPage
	{
		private readonly Label _gpsLabel;
		private ILocationService _locationProvider;

		public GpsPage()
		{
			_locationProvider = DependencyService.Get<ILocationService>();
			_gpsLabel = new Label 
			{
				Text = "Let's do some GPS!",
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
			};

			MessagingCenter.Subscribe<ILocationService,string>(this, Messaging.LocationUpdated, HandleLocationUpdate);

			Content = _gpsLabel;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			_locationProvider.Start();
		}

		private void HandleLocationUpdate(ILocationService service, string newLocation)
		{
			Device.BeginInvokeOnMainThread(() => _gpsLabel.Text = newLocation);
		}
	}
}
