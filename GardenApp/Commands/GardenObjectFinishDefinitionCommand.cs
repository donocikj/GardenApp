using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GardenApp.ViewModel;

namespace GardenApp.Commands
{
    internal class GardenObjectFinishDefinitionCommand : ICommand
    {
        private GardenObjectVM viewModel;

        public GardenObjectFinishDefinitionCommand(GardenObjectVM viewModel)
        {
            this.viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            //todo check validity maybe
            return true;
            //throw new NotImplementedException();
        }

        public async void Execute(object parameter)
        {

            await Shell.Current.GoToAsync(
                nameof(GardenPage)//,
                                  //todo change this to event raising because this is ugly as sin
                                  //new Dictionary<string, object>
                                  //{
                                  //    {"objectsUpdated", true }
                                  //}
                );
        }
    }
}
