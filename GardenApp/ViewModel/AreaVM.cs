﻿using GardenApp.Model;
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

namespace GardenApp.ViewModel
{
    public class AreaVM : INotifyPropertyChanged, IQueryAttributable
    {
        private Area area;
        private Location currentLocation;

        private GraphicsDrawable gardenDrawable;

        private ICommand refreshLocation;
        private ICommand addLocationToArea;
        private ICommand removeLocationFromArea;
        private ICommand movePointInAreaUp;
        private ICommand movePointInAreaDown;
        private ICommand finishAreaDefinition;

        public AreaVM()
        {
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
            onPropertyChanged(nameof(Points), nameof(AreaDesc));

        }

        public GraphicsDrawable GardenDrawable
        {
            get { return gardenDrawable; }
        }
       

        public ObservableCollection<Location> Points
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
                if (currentLocation == null)
                {
                    return "not yet defined";
                }
                return currentLocation.ToString(); 
            }
            
        }

        public Location CurrentLocation
        {
            get { return currentLocation; }
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
                if(removeLocationFromArea == null)
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
                if(movePointInAreaUp == null)
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

        #endregion

        #region tasks

        public async Task RefreshCurrentLocation()
        {
            Debug.WriteLine("attempting to obtain current location...");
            currentLocation = await Geolocation.GetLocationAsync();
            onPropertyChanged(nameof(CurrentLocationStr), nameof(CurrentLocation));
            
        }


        public void AddLocationToArea(Location location)
        {
            if (area == null)
            {//todo some error handling
                Debug.WriteLine("how did we even get here?");
                return;
            }

            area.AddPoint(location);
            
            //is it really wise to split origin points of the propertychanged events...?
            onPropertyChanged(nameof(AreaDesc));
        }

        public void RemoveLocationFromArea(Location location)
        {
            area.RemovePoint(location);
            onPropertyChanged(nameof(AreaDesc));
        }

        public void MovePointInArea(Location location, int shift)
        {
            area.MovePointInList(location, shift);
            //hmm
            onPropertyChanged(nameof(AreaDesc));
        }

        public void EditPoint(Location location)
        {
            throw new NotImplementedException();
        }


        #endregion

        //todo: remove a specific point from the area (something something collectionview stuff

        protected void onPropertyChanged(params string[] propertyNames)
        {
            foreach (string propertyName in propertyNames)
            {
                PropertyChanged(
                    this,
                    new PropertyChangedEventArgs(propertyName));
            }
        }



    }
}