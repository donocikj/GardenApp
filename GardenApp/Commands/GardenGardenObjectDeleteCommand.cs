using GardenApp.Model;
using GardenApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GardenApp.Commands
{
    internal class GardenGardenObjectDeleteCommand : ICommand
    {
        private GardenVM viewModel;

        public GardenGardenObjectDeleteCommand(GardenVM viewModel)
        {
            this.viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            //todo check valid parameter has been passed
            return true;

        }

        public void Execute(object parameter)
        {
            Debug.WriteLine("delete command has been called for execution");
            //recast the parameter object...
            GardenObject toBeRemoved = (GardenObject)parameter;
            //todo some checks if this is actually allowed

            //Debug.WriteLine(toBeRemoved.ToString());

            //request the referenced gardenobject be removed from the model
            viewModel.DeleteObject(toBeRemoved);
        }
    }
}
