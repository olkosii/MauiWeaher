using MauiWeaher.MVVM.Views;

namespace MauiWeaher;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new WeatherView();
	}
}
