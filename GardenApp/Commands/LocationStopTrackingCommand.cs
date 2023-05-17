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
    internal class LocationStopTrackingCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private MapContext _mapContext;
        private GardenVM _vm;

        public LocationStopTrackingCommand(MapContext mapContext, GardenVM viewModel)
        {
            _mapContext = mapContext;
            _vm = viewModel;
        }   

        public bool CanExecute(object parameter)
        {
            return true;
            //throw new NotImplementedException();
        }

        public void Execute(object parameter)
        {
            _mapContext.CurrentLocationTracker.CurrentLocationEstimateUpdated -= _vm.MapUpdateRequest;
            _mapContext.CurrentLocationTracker.StopTracking();
            _vm.MapUpdateRequest(this, new EventArgs());
            
            //throw new NotImplementedException();
        }
    }
}
