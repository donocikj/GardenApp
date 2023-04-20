using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GardenApp.Commands
{
    internal class GetCurrentLocationCommand : ICommand
    {

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            //is location available and permission granted?
            throw new NotImplementedException();
        }

        public void Execute(object parameter)
        {
            //request current location and hand it over to the object that requested it
            //(add a point to an area or create new discrete object at the location)
            throw new NotImplementedException();
        }
    }
}
