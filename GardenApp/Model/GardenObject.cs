
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenApp.Model
{
    public class GardenObject: INotifyPropertyChanged
    {
        private string _name;
        private string _type;
        private string _description;
        private Area _location;


        public GardenObject()
        {
            Debug.WriteLine("garden object constructor called");
            _name = "new object";
            _type = "generic";
            _description = "";
            _location = new Area();
            _location.PropertyChanged += LocationUpdated;


        }

        public string Name
        { 
            get { return _name; } 
            set 
            { 
                _name = value;
                OnPropertyChanged(nameof(Name));
            } 
        }

        public string Type
        {
            get { return _type; } 
            set { _type = value; OnPropertyChanged(nameof(Type)); } 
        }

        public string Description
        { 
            get { return _description; } 
            set { _description = value; OnPropertyChanged(nameof(Description)); } 
        }

        public Area ObjectLocation
        { 
            get { return _location; } 
            set 
            { 
                _location = value; 
                //todo rework this maybe with custom deserializer...
                _location.PropertyChanged += LocationUpdated;
                OnPropertyChanged(nameof(ObjectLocation)); 
            } 
        }



        public event PropertyChangedEventHandler PropertyChanged;

        private void LocationUpdated(object sender, PropertyChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("location update has been called on a garden object");
            OnPropertyChanged(nameof(ObjectLocation));
        }

        public void OnPropertyChanged(params string[] propertyNames)
        {
            foreach(string name in propertyNames) 
            { 
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
