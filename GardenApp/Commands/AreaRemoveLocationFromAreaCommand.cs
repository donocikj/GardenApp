using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GardenApp.Model;
using GardenApp.ViewModel;

namespace GardenApp.Commands
{
    internal class AreaRemoveLocationFromAreaCommand : ICommand
    {
        private AreaVM viewModel;

        public AreaRemoveLocationFromAreaCommand(AreaVM viewModel)
        {
            this.viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
            //throw new NotImplementedException();
        }

        public void Execute(object parameter)
        {

            ObservableLocation locationToRemove = parameter as ObservableLocation;
            viewModel.RemoveLocationFromArea(locationToRemove);
        }
    }
}
