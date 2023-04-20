using GardenApp.Model;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GardenApp.Commands
{
    internal class SetAreaCommand : ICommand
    {


        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
            //throw new NotImplementedException();
        }

        public async void Execute(object parameter)
        {
            Area area = (Area)parameter;


            if (area == null)
            {
                area = new Area();
            }

            await Shell.Current.GoToAsync(
                nameof(AreaPage),
                new Dictionary<string, object>
                {
                    {"area", area},
                    //todo how to get this...
                    //{"gardenContext",  }
                });
            //throw new NotImplementedException();
        }
    }
}
