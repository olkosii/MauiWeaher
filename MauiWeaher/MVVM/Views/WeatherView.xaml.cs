using MauiWeaher.MVVM.ViewModels;

namespace MauiWeaher.MVVM.Views;

public partial class WeatherView : ContentPage
{
	private WeatherViewModel _weatherViewModel;
	public WeatherView()
	{
		InitializeComponent();

		_weatherViewModel = new WeatherViewModel();

		BindingContext = _weatherViewModel;
	}
}