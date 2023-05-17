using GardenApp.Model;
using GardenApp.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GardenApp.Drawable;
using Microsoft.Maui.Controls;

namespace GardenApp.ViewModel
{
    public class AreaVM : INotifyPropertyChanged, IQueryAttributable
    {
        private Area area;
        private ObservableLocation selectedLocation;

        private GraphicsDrawable gardenDrawable;

        private ICommand refreshLocation;
        private ICommand addLocationToArea;
        private ICommand removeLocationFromArea;
        private ICommand movePointInAreaUp;
        private ICommand movePointInAreaDown;
        private ICommand finishAreaDefinition;
        private ICommand selectLocation;
        private ICommand shiftLocationN;
        private ICommand shiftLocationS;
        private ICommand shiftLocationW;
        private ICommand shiftLocationE;



        public AreaVM(GraphicsDrawable gardenDrawable)
        {
            this.gardenDrawable = gardenDrawable;
            Debug.WriteLine("constructor for areaVM called");
        }



        public event PropertyChangedEventHandler PropertyChanged;

        public void ApplyQueryAttributes(IDictionary<string, object> attributes)
        {
            //Debug.WriteLine("applying query attributes");
            //todo maybe handle absence of query param
            //area = attributes["area"] as Area;

            if (attributes.TryGetValue("area", out object receivedArea))
                area = receivedArea as Area;

            if (attributes.TryGetValue("gardenContext", out object receivedGarden))
            {
                gardenDrawable.UpdateModel(receivedGarden as Garden);
                onPropertyChanged(nameof(GardenDrawable));
            }

            //I feel kind of dirty for doing this tbh... maybe at least
            //TODO - deregister these on navigating out?
            foreach(ObservableLocation point in area.Points)
            {
                point.PropertyChanged += MapUpdateRequest;
            }

            onPropertyChanged(nameof(Points), nameof(AreaDesc));

        }

        public GraphicsDrawable GardenDrawable
        {
            get { return gardenDrawable; }
        }


        public ObservableCollection<ObservableLocation> Points
        {
            get {
                if (area != null)
                    return area.Points;
                else return null;
            }
            set
            {
                area.Points = value;
                onPropertyChanged(nameof(Points), nameof(AreaDesc));
            }
        }

        public string AreaDesc
        {
            get {
                if (area == null)
                    return "sum fing wong";
                return area.ToString();
            }
        }

        public string CurrentLocationStr
        {
            get {
                if (selectedLocation == null)
                {
                    return "not yet defined";
                }
                return selectedLocation.ToString();
            }

        }

        public ObservableLocation SelectedLocation
        {
            get { return selectedLocation; }
            set { selectedLocation = value; onPropertyChanged(nameof(SelectedLocation), nameof(GardenDrawable)); }
        }

        #region commands

        public ICommand RefreshCurrentLocationCommand
        {
            get {
                if (refreshLocation == null)
                    refreshLocation = new AreaRefreshCurrentLocationCommand(this);
                return refreshLocation;
            }
        }

        public ICommand AddLocationToAreaCommand
        {
            get
            {
                if (addLocationToArea == null)
                {
                    addLocationToArea = new AreaAddLocationCommand(this);
                }
                return addLocationToArea;
            }
        }

        public ICommand RemoveLocationFromAreaCommand
        {
            get
            {
                if (removeLocationFromArea == null)
                {
                    removeLocationFromArea = new AreaRemoveLocationFromAreaCommand(this);
                }
                return removeLocationFromArea;
            }
        }

        public ICommand MovePointInAreaUpCommand
        {
            get
            {
                if (movePointInAreaUp == null)
                {
                    movePointInAreaUp = new AreaMovePointInAreaDefinitionCommand(this, -1);
                }
                return movePointInAreaUp;
            }
        }

        public ICommand MovePointInAreaDownCommand
        {
            get
            {
                if (movePointInAreaDown == null)
                {
                    movePointInAreaDown = new AreaMovePointInAreaDefinitionCommand(this, 1);
                }
                return movePointInAreaDown;
            }
        }

        public ICommand FinishAreaDefinitionCommand
        {
            get {
                if (finishAreaDefinition == null)
                    finishAreaDefinition = new AreaFinishDefinitionCommand();
                return finishAreaDefinition;
            }
        }

        public ICommand SelectLocationCommand
        {
            get
            {
                if (selectLocation == null)
                    selectLocation = new AreaSelectLocationCommand(this);
                return selectLocation;
            }
        }

        public ICommand ShiftLocationNorthCommand
        {
            get
            {
                if (shiftLocationN == null)
                    shiftLocationN = new AreaLocationGeographicShiftByStepCommand(this, Direction.North);
                return shiftLocationN;
            }
        }

        public ICommand ShiftLocationSouthCommand
        {
            get
            {
                if (shiftLocationS == null)
                    shiftLocationS = new AreaLocationGeographicShiftByStepCommand(this, Direction.South);
                return shiftLocationS;
            }
        }

        public ICommand ShiftLocationWestCommand
        {
            get
            {
                if (shiftLocationW == null)
                    shiftLocationW = new AreaLocationGeographicShiftByStepCommand(this, Direction.West);
                return shiftLocationW;
            }
        }

        public ICommand ShiftLocationEastCommand
        {
            get
            {
                if (shiftLocationE == null)
                    shiftLocationE = new AreaLocationGeographicShiftByStepCommand(this, Direction.East);
                return shiftLocationE;
            }
        }

        #endregion


        void OnLocationSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedLocation = e.CurrentSelection.FirstOrDefault() as ObservableLocation;

        }

        #region tasks

        public async Task RefreshCurrentLocation()
        {
            Debug.WriteLine("attempting to obtain current location...");

            GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
            CancellationTokenSource tokenSource = new CancellationTokenSource();

            Location currentLocation = await Geolocation.GetLocationAsync(request, tokenSource.Token);

            if (currentLocation != null ) 
            { 
                this.SelectLocation(new ObservableLocation(currentLocation));
                selectedLocation.PropertyChanged += MapUpdateRequest;
                //this looks like it could cause memory leaks idk...

                //set the location as "current location" on drawable so it shows up and can be worked with...
                //GardenDrawable.
            }
            onPropertyChanged(nameof(CurrentLocationStr), nameof(SelectedLocation));

        }


        public void AddLocationToArea(ObservableLocation location)
        {
            if (area == null)
            {//todo some error handling
                Debug.WriteLine("how did we even get here?");
                return;
            }

            area.AddPoint(location);

            //is it really wise to split origin points of the propertychanged events...?
            onPropertyChanged(nameof(AreaDesc), nameof(GardenDrawable));
        }

        public void RemoveLocationFromArea(ObservableLocation location)
        {
            area.RemovePoint(location);
            onPropertyChanged(nameof(AreaDesc), nameof(GardenDrawable));
        }

        public void MovePointInArea(ObservableLocation location, int shift)
        {
            area.MovePointInList(location, shift);
            //hmm
            onPropertyChanged(nameof(AreaDesc), nameof(GardenDrawable));
        }

        public void EditPoint(Location location)
        {
            throw new NotImplementedException();
        }


        #endregion
        public void MapUpdateRequest(object sender, EventArgs e)
        {
            onPropertyChanged(nameof(GardenDrawable));
        }
        

        protected void onPropertyChanged(params string[] propertyNames)
        {
            foreach (string propertyName in propertyNames)
            {
                PropertyChanged(
                    this,
                    new PropertyChangedEventArgs(propertyName));
            }
        }

        public void SelectLocation(ObservableLocation location)
        {
            this.selectedLocation = location;
            this.GardenDrawable.SelectedLocation = location;
            onPropertyChanged(nameof(GardenDrawable), nameof(SelectedLocation), nameof(CurrentLocationStr));
            
        }


    }
}
