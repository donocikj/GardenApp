using GardenApp.Drawable;
using GardenApp.ViewModel;
using Microsoft.Maui.Controls;

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

		//this looks excessive way to force updates...
		viewModel.PropertyChanged += OnMapUpdateRequest;
    }

	public void OnMapUpdateRequest(object sender, EventArgs e)
	{
		GardenMap.Invalidate();
	}


	public void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs args)
	{
		GraphicsDrawable gardenDrawable = GardenMap.Drawable as GraphicsDrawable;
		gardenDrawable.Zoom(args.Scale);
		GardenMap.Invalidate();
	}

	public void OnPanUpdated(object sender, PanUpdatedEventArgs args)
	{
		GraphicsDrawable gardenDrawable = GardenMap.Drawable as GraphicsDrawable;
		gardenDrawable.Pan(args.TotalX, args.TotalY);
		GardenMap.Invalidate();
	}


}