using GardenApp.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenApp.Drawable
{
    public class MapContext
    {
        private double westBoundary;
        private double eastBoundary;
        private double northBoundary;
        private double southBoundary;

        private double centerX;
        private double centerY;

        public double WestBoundary { get { return westBoundary; } }
        public double EastBoundary { get { return eastBoundary; }  }
        public double NorthBoundary { get { return northBoundary; } }
        public double SouthBoundary { get { return southBoundary;} }

        public double CenterX { get { return centerX; } }
        public double CenterY { get { return centerY; } }

        public double DeltaLatArc 
        { 
            get { return northBoundary - southBoundary; } 
        }

        public double DeltaLonArc
        {
            get { return eastBoundary - westBoundary; }
        }

        

        public void SetContext(Area area, RectF dirtyRect)
        {
            Debug.WriteLine("centering the drawable... but in MapContext");
            if (area != null && area.Points != null && area.Points.Count > 0)
            {


                centerX = area.Points.Aggregate(0.0, (sum, loc) =>
                {
                    return sum + loc.Longitude;
                }) / area.Points.Count;

                centerY = area.Points.Aggregate(0.0, (sum, loc) =>
                {
                    return sum + loc.Latitude;
                }) / area.Points.Count;

                Debug.WriteLine(String.Format("centerpoint - lat: {0}, lon: {1}", centerY, centerX));

                //range the default viewport needs to cover

                double minRange = area.Points.Aggregate(0.0, (range, loc) =>
                {
                    double dist = loc.CalculateDistance(new Location(centerY, centerX), DistanceUnits.Kilometers);
                    return range > dist ? range : dist;
                });

                Debug.WriteLine(String.Format("calculated range: {0} m", minRange * 1000));

                //set up bounds

                double latRange;
                double lonRange;

                if (dirtyRect.Height > dirtyRect.Width)
                {
                    //tall order
                    Debug.WriteLine("going tall: height - {0}, width - {1}", dirtyRect.Height, dirtyRect.Width);
                    lonRange = minRange;
                    latRange = minRange * (dirtyRect.Height / dirtyRect.Width);
                }
                else
                {
                    //landscape view - or square
                    Debug.WriteLine("going wide: height - {0}, width - {1}", dirtyRect.Height, dirtyRect.Width);
                    latRange = minRange;
                    lonRange = minRange * (dirtyRect.Width / dirtyRect.Height);
                }

                //todo something about that ugly constant at the end
                double deltaLat = (latRange / ((Math.PI / 2) * 6378)) * 90;

                southBoundary = centerY - deltaLat;
                northBoundary = centerY + deltaLat;

                double deltaLon = (lonRange / ((Math.PI) * (Math.Cos(northBoundary * Math.PI / 180) * 6378))) * 180;

                westBoundary = centerX - deltaLon;
                eastBoundary = centerX + deltaLon;

                Debug.WriteLine(String.Format("delta lat: {0}, delta lon: {1}", deltaLat, deltaLon));

                Debug.WriteLine(String.Format("Lat boundaries: {0} to {1}", southBoundary, northBoundary));
                Debug.WriteLine(String.Format("Lon boundaries: {0} to {1}", westBoundary, eastBoundary));


                /*
                areaWidth = eastBoundary - westBoundary;

                areaHeight = northBoundary - southBoundary;

                mapWidth = dirtyRect.Width;
                mapHeight = dirtyRect.Height;
                */

            }
        }
    }
}
