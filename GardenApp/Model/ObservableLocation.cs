using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenApp.Model
{
    public class ObservableLocation : Location, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableLocation() : base() { }

        public ObservableLocation(double lat, double lon) : base(lat, lon) { }

        public ObservableLocation(double lat, double lon, DateTimeOffset time) : base(lat, lon, time) { }

        public ObservableLocation(double lat, double lon, double alt) : base(lat, lon, alt) { }

        public ObservableLocation(Location location) : base(location) { }
        

        public new double Latitude
        {
            get { return (double)base.Latitude; } 
            set 
            { 
                base.Latitude = value;  
                OnPropertyChanged(nameof(Latitude));
            }
        }

        public new double Longitude
        {
            get { return (double)base.Longitude; }
            set
            {
                base.Longitude = value;
                OnPropertyChanged(nameof(Longitude));
            }
        }

        public new double? Accuracy
        {
            get { return (double)base.Accuracy;  }
            set 
            { 
                base.Accuracy = value; 
                OnPropertyChanged(nameof(Accuracy)); 
            } 
        }

        public void OnPropertyChanged(params string[] propertyNames)
        {
            foreach (string name in propertyNames)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }

    }
}
