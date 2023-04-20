using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GardenApp.ViewModel;

namespace GardenApp.Commands
{
    class AreaRefreshCurrentLocationCommand : ICommand
    {
        private AreaVM viewModel;
        public AreaRefreshCurrentLocationCommand(AreaVM viewModel)
        {
            this.viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            //todo check location abilities
            return true;
        }

        public async void Execute(object parameter)
        {
            System.Diagnostics.Debug.WriteLine("executing command to refresh current location..");
            await viewModel.RefreshCurrentLocation();
        }
    }
}
