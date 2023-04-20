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
}