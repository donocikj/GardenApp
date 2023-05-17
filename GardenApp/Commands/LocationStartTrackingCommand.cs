using GardenApp.Drawable;
using GardenApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GardenApp.Commands
{
    internal class LocationStartTrackingCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private MapContext _mapContext;
        private GardenVM viewModel;

        public LocationStartTrackingCommand(MapContext mapContext, GardenVM viewModel)
        {
            _mapContext = mapContext;
            this.viewModel = viewModel;
        }   

        public bool CanExecute(object parameter)
        {
            return true;
            //throw new NotImplementedException();
        }

        public void Execute(object parameter)
        {
            _mapContext.CurrentLocationTracker.CurrentLocationEstimateUpdated += viewModel.MapUpdateRequest;
            _mapContext.CurrentLocationTracker.StartTracking();
            
            //throw new NotImplementedException();
        }
    }
}
