using GardenApp.Model;
using GardenApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GardenApp.Commands
{
    internal class GardenEditGardenObjectCommand : ICommand
    {
        private GardenVM viewModel;

        public GardenEditGardenObjectCommand(GardenVM viewModel)
        {
            this.viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            //todo maybe check if the garden object is instantiated properly
            return true;
        }

        public async void Execute(object parameter)
        {
            GardenObject accessedObject;

            //todo call for instantiation of a new gardenobject here - or pass the existing one.
            if (parameter == null)
            {
                accessedObject = new GardenObject();
                //todo! register the new object with the garden by calling appropriate viewmodel method
                //viewModel.AddObject(accessedObject); or something like that
                viewModel.AddObject(accessedObject);
                //todo maybe defer adding the object, but then I need to send or access the instance of garden or a delegate to add it later...
            }
            else
            {
                //todo check if this cast is actually possible
                accessedObject = (GardenObject)parameter;
            }


            await Shell.Current.GoToAsync(
                nameof(GardenObjectPage),
                new Dictionary<string, object>
                {
                    {"gardenObject", accessedObject},
                    {"parentGarden", viewModel.Garden}
                });
        }
    }
}
