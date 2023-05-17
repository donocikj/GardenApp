using GardenApp.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GardenApp.LocationService
{
    public class LocationTracker
    {
        private List<Location> _locations;
        private ObservableLocation currentLocationEstimate;
        private Timer locationRequestTimer;
        private Timer locationEstimateTimer;
        private AutoResetEvent autoEvent;
        private bool locationBeingRequested = false;
        private int locationReqTick = 1000;
        private int locationReqLongerTick = 5000;
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public LocationTracker()
        {


        }

        public ObservableLocation CurrentLocationEstimate
        {
            get { return currentLocationEstimate; }
        }

        public void StartTracking()
        {
            _locations = new List<Location>();
            autoEvent = new AutoResetEvent(false);
            //Debug.WriteLine("starting the timer");
            locationRequestTimer = new Timer(this.FeedLocationList, autoEvent, 0, locationReqTick);
            locationRequestTimer = new Timer(this.RefreshCalculatedLocation, autoEvent, 0, locationReqLongerTick);
        }

        public void StopTracking()
        {
            autoEvent.Set();
            cancellationTokenSource.Cancel();

            if (locationRequestTimer != null)
            {
                locationRequestTimer.Dispose();
            }

            if (locationEstimateTimer != null)
            {
                locationEstimateTimer.Dispose();
            }

            //Thread.Sleep(locationReqLongerTick);

            currentLocationEstimate = null;
            //_locations = null;
        }


        public async void FeedLocationList(object StateInfo)
        {
            if (locationBeingRequested == false)
            {
                locationBeingRequested = true;
                Location newCurrentLocation = null;
                AutoResetEvent timerEvent = (AutoResetEvent)StateInfo;
                //todo unfuck this mess
                //if timerEvent.
                GeolocationRequest geolocationRequest = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromMicroseconds(locationReqTick * 500));


                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    try
                    {
                        newCurrentLocation = await Geolocation.GetLocationAsync(geolocationRequest, cancellationTokenSource.Token);
                    }
                    catch(Exception ex)
                    {
                        if (ex is TaskCanceledException)
                        {
                            Debug.WriteLine("task cancelled successfully");
                        }
                        else
                        {
                            Debug.WriteLine("Exception: " + ex.GetType().Name);
                        }
                    }

                    /*
                    catch (AggregateException ae)
                    {
                        foreach(Exception e in ae.InnerExceptions)
                        {
                            if(e is TaskCanceledException)
                            {
                                Debug.WriteLine("task cancelled successfully");
                            }
                            else 
                            {
                                Debug.WriteLine("Exception: " + e.GetType().Name);
                            }
                        }
                    }
                    */
                });

                if (newCurrentLocation != null)
                    _locations.Add(newCurrentLocation);
                //maybe discard based on declared accuracy?

                //todo use autoEvent

                locationBeingRequested = false;
            }
        }

        public void RefreshCalculatedLocation(object StateInfo)
        {
            //refresh list
            RefreshList();

            //update "current location"
            double avgLat = 0;
            double avgLon = 0;
            //todo deal with the altitude nullable thing
            double? avgAlt = 0;

            foreach (var location in _locations)
            {
                avgLat += location.Latitude;
                avgLon += location.Longitude;
                avgAlt += location.Altitude;
            }
            avgLat /= _locations.Count; 
            avgLon /= _locations.Count;
            avgAlt /= _locations.Count;

            if (avgAlt != null)
                currentLocationEstimate = new ObservableLocation(avgLat, avgLon, (double)avgAlt);
            else 
                currentLocationEstimate = new ObservableLocation(avgLat, avgLon);

            Debug.WriteLine($"calculated location: {currentLocationEstimate}");
            Debug.WriteLine($"currently location list has {_locations.Count} entries");

            CurrentLocationEstimateUpdated.Invoke(this, new EventArgs());
        }

        public void RefreshList()
        {
            if (_locations != null)
            {


                //filter older than 5 secs?
                _locations = _locations.FindAll((Point) =>
                {
                    if (Point.Timestamp > DateTime.Now.AddSeconds(-5.0))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                });
            }
        }

        public event EventHandler CurrentLocationEstimateUpdated;
        
    }
}
