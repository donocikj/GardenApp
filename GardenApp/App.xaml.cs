namespace GardenApp;

public partial class App : Application
{
	public App()
	{
		//System.Diagnostics.Debug.WriteLine("initializing app");
		InitializeComponent();

		MainPage = new AppShell();
		
        
		
    }
}
