using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GardenApp.ViewModel;

namespace GardenApp.Commands
{
    internal class GardenShareGardenCommand : ICommand
    {
        private GardenVM viewModel;

        public GardenShareGardenCommand(GardenVM viewModel)
        {
            this.viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            //todo check... permissions, model/vm not being bonked etc
            return true;
            //throw new NotImplementedException();
        }

        public void Execute(object parameter)
        {
            viewModel.ShareGarden();
            //throw new NotImplementedException();
        }
    }
}
