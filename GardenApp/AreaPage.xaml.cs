using GardenApp.ViewModel;

namespace GardenApp;

public partial class AreaPage : ContentPage
{
	//private AreaVM viewModel;
	
	public AreaPage(AreaVM viewModel)
	{
		System.Diagnostics.Debug.WriteLine("area page constructor called");
		//this.viewModel = viewModel;
		BindingContext= viewModel;
        //System.Diagnostics.Debug.WriteLine("context should have been bound to viewmodel:");
        //System.Diagnostics.Debug.WriteLine(viewModel.ToString());
        InitializeComponent();
        //System.Diagnostics.Debug.WriteLine("component initialized");
    }
}