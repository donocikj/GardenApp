using GardenApp.ViewModel;
using GardenApp.Model;
using Microsoft.Maui.Devices.Sensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GardenApp.Commands
{
    internal class AreaSelectLocationCommand : ICommand
    {
        private AreaVM vm;


        public AreaSelectLocationCommand(AreaVM viewModel) 
        {
            this.vm = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            ObservableLocation selectedLocation = parameter as ObservableLocation;
            vm.SelectedLocation = selectedLocation;
        }
    }
}
