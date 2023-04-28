using GardenApp.Drawable;
using GardenApp.ViewModel;
using System.Diagnostics;

namespace GardenApp;

public partial class GardenPage : ContentPage
{
	//private GardenVM vm;

	public GardenPage(GardenVM vm)
	{
		//this.vm = vm;
		Debug.WriteLine("Initializing gardenpage");
		InitializeComponent();
		BindingContext = vm;

		//this.GardenMap.Invalidate();

		//Debug.WriteLine(vm.saveGarden());
	}


	public void OnMapChanged(object sender, EventArgs e)
	{
		this.GardenMap.Invalidate();
	}



	public void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs eventArgs)
	{
		/*
		Debug.Write("pinch updated... scale: ");
		Debug.Write(eventArgs.Scale);
		Debug.Write(", status: ");
		Debug.WriteLine(eventArgs.Status);
		*/

		GraphicsDrawable gardenDrawable = GardenMap.Drawable as GraphicsDrawable;

		gardenDrawable.Zoom(eventArgs.Scale);
		GardenMap.Invalidate();

		//reach for the drawable and call the zoom method
		//invalidate the canvas

	}

    private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
    {
        Debug.WriteLine(String.Format("pan updated... totalX: {0}, totalY {1}", e.TotalX, e.TotalY));
		
		
        GraphicsDrawable gardenDrawable = GardenMap.Drawable as GraphicsDrawable;

		gardenDrawable.Pan(e.TotalX, e.TotalY);
		GardenMap.Invalidate();
		

    }
}