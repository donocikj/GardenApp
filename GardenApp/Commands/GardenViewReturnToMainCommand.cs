using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GardenApp.ViewModel;

namespace GardenApp.Commands
{
    class GardenViewReturnToMainCommand : ICommand
    {
        private GardenVM vm;

        public GardenViewReturnToMainCommand(GardenVM vm)
        {
            this.vm = vm;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
            //throw new NotImplementedException();
        }

        public async void Execute(object parameter)
        {
            //clear viewmodel's garden?


            //navigate
            await Shell.Current.GoToAsync(string.Format("//{0}", nameof(MainPage)));
        }
    }
}
