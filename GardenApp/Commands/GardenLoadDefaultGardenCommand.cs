using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GardenApp.ViewModel;

namespace GardenApp.Commands
{
    internal class GardenLoadDefaultGardenCommand : ICommand
    {
        private GardenVM viewModel;

        public GardenLoadDefaultGardenCommand(GardenVM viewModel)
        {
            this.viewModel = viewModel;
        }

        //todo maybe check validity of editable fields?
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            //todo
            return true;
        }

        public async void Execute(object parameter)
        {
            //todo figure out parameter handling in this
            //call the method to save the garden
            await viewModel.LoadGarden("My Garden");

            viewModel.GardenDrawable.UpdateModel(viewModel.Garden);


            //navigate to gardenview
            await Shell.Current.GoToAsync(
                nameof(GardenPage));
        }
    }
}
