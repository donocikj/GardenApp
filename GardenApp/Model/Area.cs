using Microsoft.Maui.Devices.Sensors;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace GardenApp.Model
{
    public class Area: INotifyPropertyChanged

    {
        private ObservableCollection<Location> _points;

        public Area()
        {
            Debug.WriteLine("area constructor called");
            _points = new ObservableCollection<Location>();
        }



        public ObservableCollection<Location> Points { 
            get { return _points; }
            set 
            { 
                _points = value;
                OnPropertyChanged(nameof(Points));
            }
        }

        public void AddPoint(Location newPoint)
        {
            Debug.WriteLine("adding a point to location");
            this._points.Add(newPoint);
            
            OnPropertyChanged(nameof(Points));
        }

        public void RemovePoint(Location removedPoint)
        {
            Debug.WriteLine(String.Format("removing a point from location at index {0}",this._points.IndexOf(removedPoint)));
            //todo maybe check and rework this - currently it seems to judge location by its coordinates and disregard its timestamp
            if(this._points.Contains(removedPoint))
            {
                this._points.Remove(removedPoint);
                OnPropertyChanged(nameof(Points));
            }
        }

        public void MovePointInList(Location movedPoint, int shift)
        {
            //todo check bounds or something
            int target = this._points.IndexOf(movedPoint) + shift;
            if(target >= 0 && target < this._points.Count)
            {
                this._points.Remove(movedPoint);
                this._points.Insert(target, movedPoint);
                OnPropertyChanged(nameof(Points));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(params string[] propertyNames)
        {
            foreach(string name in propertyNames)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }

        public override string ToString()
        {
            if (_points == null) return "undefined area";
            if (_points.Count == 0) return "area with no points";
            return String.Format("area defined by {0} points", _points.Count);
            
        }
    }
}
