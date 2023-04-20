using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GardenApp.ViewModel;

namespace GardenApp.Commands
{
    internal class AreaMovePointInAreaDefinitionCommand : ICommand
    {
        private readonly AreaVM viewModel;
        private readonly int shift;
        public AreaMovePointInAreaDefinitionCommand(AreaVM viewModel, int shift)
        {
            this.viewModel = viewModel;
            this.shift = shift;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            //TODO: I DEFINITELY need to handle the bounds in here.
            return true;

        }

        public void Execute(object parameter)
        {
            Location movedLocation = parameter as Location;
            viewModel.MovePointInArea(movedLocation, shift);
        }
    }
}
