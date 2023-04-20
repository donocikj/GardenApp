using GardenApp.ViewModel;

namespace GardenApp;

public partial class GardenObjectPage : ContentPage
{

	//private GardenObjectVM viewModel;


	public GardenObjectPage(GardenObjectVM viewModel)
	{
		BindingContext= viewModel;
		InitializeComponent();
	}
}