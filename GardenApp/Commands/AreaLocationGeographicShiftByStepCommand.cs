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
    public enum Direction
    {
        North,
        South,
        West,
        East
    }

    internal class AreaLocationGeographicShiftByStepCommand: ICommand
    {
        private AreaVM viewModel;
        private Direction direction;

        public AreaLocationGeographicShiftByStepCommand(AreaVM viewModel, Direction direction)
        {
            this.viewModel = viewModel;
            this.direction = direction;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
            //throw new NotImplementedException();
        }

        public void Execute(object parameter)
        {
            
            ObservableLocation movedLocation = parameter as ObservableLocation;

            int step = 10;

            switch(direction)
            {
                case Direction.North:
                    movedLocation.Latitude += viewModel.GardenDrawable.PxToDegLat(step);
                    break;
                case Direction.South:
                    movedLocation.Latitude -= viewModel.GardenDrawable.PxToDegLat(step);
                    break;
                case Direction.West:
                    movedLocation.Longitude -= viewModel.GardenDrawable.PxToDegLon(step);
                    break;
                case Direction.East:
                    movedLocation.Longitude += viewModel.GardenDrawable.PxToDegLon(step);
                    break;
                default:
                    return;
            }

            return;

            //throw new NotImplementedException();
        }
    }
}
