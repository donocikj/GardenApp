using GardenApp.ViewModel;

namespace GardenApp;

public partial class MainPage : ContentPage
{

	public MainPage(GardenVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

}

