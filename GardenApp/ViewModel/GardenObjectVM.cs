using GardenApp.Model;
using GardenApp.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GardenApp.Drawable;

namespace GardenApp.ViewModel
{


    public class GardenObjectVM : INotifyPropertyChanged, IQueryAttributable
    {

        private GardenObject _object;
        private GraphicsDrawable gardenDrawable;

        private ICommand returnCommand;
        private ICommand editLocationCommand;

        public GardenObjectVM(GraphicsDrawable gardenDrawable)
        {
            System.Diagnostics.Debug.WriteLine("constructor for garden object vm called");
            this.gardenDrawable= gardenDrawable;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /*
        public string Name
        {
            get { return _object?.Name; }
            set {
                _object.Name = value;
                onPropertyChanged(nameof(Name));
            }
        }

        public string Type
        {
            get { return _object?.Type; }
            set {
                _object.Type = value;
                onPropertyChanged(nameof(Type));
            }
        }

        public string Description
        {
            get { return _object?.Description; }
            set {
                _object.Description = value;
                onPropertyChanged(nameof(Description));
            }
        }

        public Area ObjectLocation
        {   
            get { return _object?.ObjectLocation; }
            set { 
                _object.ObjectLocation = value;
                onPropertyChanged(nameof(ObjectLocation), nameof(ObjectLocationStr));
            }
        }
        */

        public string ObjectLocationStr
        {
            get { return _object?.ObjectLocation.ToString();}
        }

        public GardenObject GardenObject
        {
            get { return _object; }
            set { _object = value; }
        }

        public GraphicsDrawable GardenDrawable
        {
            get { return gardenDrawable; }
        }

        //todo command to edit object location - editareacommand
        public ICommand EditLocationCommand
        {
            get
            {
                if (editLocationCommand == null)
                    editLocationCommand = new SetAreaCommand();
                return editLocationCommand;
            }
        }

        //todo command to return (and maybe put registration of new object here?)

        public ICommand ReturnCommand
        {
            get
            {
                if (returnCommand == null)
                    returnCommand = new GardenObjectFinishDefinitionCommand(this);
                return returnCommand;
            }
        }


        protected void OnPropertyChanged(params string[] propertyNames)
        {
            foreach (string property in propertyNames) 
            {
                PropertyChanged(
                    this,
                    new PropertyChangedEventArgs(property));
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {

            if (query.TryGetValue("gardenObject", out object receivedGardenObject))
            {
                _object = receivedGardenObject as GardenObject;
                //onPropertyChanged(nameof(Name), nameof(Type), nameof(Description), nameof(ObjectLocation), nameof(ObjectLocationStr));
                OnPropertyChanged(nameof(GardenObject), nameof(ObjectLocationStr));
            }

            if (query.TryGetValue("parentGarden", out object receivedParentGarden))
            {
                gardenDrawable.UpdateModel(receivedParentGarden as Garden);
                gardenDrawable.SelectedObject = _object;
                OnPropertyChanged(nameof(GardenDrawable));
            }
            /*
            object locationUpdated;
            if (query.TryGetValue("locationUpdated", out locationUpdated))
            {
                if ((bool)locationUpdated)
                    onPropertyChanged(nameof(ObjectLocationStr));
            }

            //todo logic for updating location
            //maybe todo - default initial location set as "here" for new objects
            */
        }
    }
}
