using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GardenApp.ViewModel;

namespace GardenApp.Commands
{
    internal class GardenSaveGardenCommand : ICommand
    {
        private GardenVM viewModel;

        public GardenSaveGardenCommand(GardenVM viewModel)
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

        public void Execute(object parameter)
        {
            //call the method to save the garden
            viewModel.saveGarden();
        }
    }
}
