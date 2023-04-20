using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GardenApp.Commands
{
    class AreaFinishDefinitionCommand : ICommand
    {

        //public FinishAreaDefinitionCommand() 
        //{ }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            //todo did anything actually change?
            return true;
        }

        public async void Execute(object parameter)
        {
            //and now to ensure the changes persist... is the area that has been updated actually the one found back on the gardenpage?
            await Shell.Current.GoToAsync("..", new Dictionary<string, object>
            {
                //todo confirm if that is in fact the case
                {"locationUpdated", true }
            });

        }
    }
}
