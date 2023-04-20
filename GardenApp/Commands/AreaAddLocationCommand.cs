using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GardenApp.ViewModel;

namespace GardenApp.Commands
{
    class AreaAddLocationCommand : ICommand
    {
        private AreaVM viewModel;

        public AreaAddLocationCommand(AreaVM viewModel)
        {
            this.viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            //todo determine if location to be added to the area
            return true;
            //throw new NotImplementedException();
        }

        public void Execute(object parameter)
        {
            //todo confirm the viewmodel has an area that can be added to

            viewModel.AddLocationToArea(viewModel.CurrentLocation);
            //throw new NotImplementedException();
        }
    }
}
