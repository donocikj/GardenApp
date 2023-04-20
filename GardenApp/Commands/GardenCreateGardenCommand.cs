using GardenApp.Model;
using GardenApp.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GardenApp.Commands
{
    internal class GardenCreateGardenCommand : ICommand
    {
        private GardenVM viewModel;

        public GardenCreateGardenCommand(GardenVM viewModel)
        {
            this.viewModel = viewModel;
            //this.viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        //private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    return;
        //}

        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter) { return true; }

        public async void Execute(object parameter)
        {
            //create new instance of garden
            //Garden newGarden = new Garden("New Garden");
            viewModel.NewGarden();

            viewModel.GardenDrawable.UpdateModel(viewModel.Garden);

            //

            //navigate to gardenview
            await Shell.Current.GoToAsync(
                nameof(GardenPage));
        }
    }
}
