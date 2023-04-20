using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GardenApp.ViewModel;

namespace GardenApp.Commands
{
    internal class GardenLoadWithPickerCommand : ICommand
    {
        private GardenVM viewModel;

        public GardenLoadWithPickerCommand(GardenVM viewModel)
        {
            this.viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
            //throw new NotImplementedException();
        }

        public async void Execute(object parameter)
        {
            await viewModel.PickGardenFile();

            viewModel.GardenDrawable.UpdateModel(viewModel.Garden);

            await Shell.Current.GoToAsync(nameof(GardenPage));

            //throw new NotImplementedException();
        }
    }
}
