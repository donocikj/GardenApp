using GardenApp.Model;
using Microsoft.Maui.Devices.Sensors;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenApp.Drawable
{
    public class GraphicsDrawable : IDrawable
    {
        //fields for what is meant to be represented..
        private Garden garden;
        private GardenObject selectedObject;
        private Location selectedLocation;

        private MapContext mapContext;

        private double centerX;
        private double centerY;

        private double westBound;
        private double eastBound;
        private double southBound;
        private double northBound;

        private double areaWidth;
        private double areaHeight;

        private float mapWidth;
        private float mapHeight;


        //constructor?
        public GraphicsDrawable(MapContext mapContext)
        {
            this.mapContext = mapContext;
        }

        //needed: translation from geographical coordinates into range 0-100, 0-100
        //todo: zoom in/zoom out + viewport handling

        public void Center(ICanvas canvas, RectF dirtyRect)
        {

            mapContext.setContext(garden.Location, dirtyRect);

            /*
             * 
            Debug.WriteLine("centering the drawable...");
            if (garden.Location != null && garden.Location.Points != null && garden.Location.Points.Count > 0)
            {

                centerX = garden.Location.Points.Aggregate(0.0, (sum, loc) =>
                {
                    return sum + loc.Longitude;
                }) / garden.Location.Points.Count;

                centerY = garden.Location.Points.Aggregate(0.0, (sum, loc) =>
                {
                    return sum + loc.Latitude;
                }) / garden.Location.Points.Count;

                Debug.WriteLine(String.Format("centerpoint - lat: {0}, lon: {1}", centerY, centerX));

                //range the default viewport needs to cover

                double minRange = garden.Location.Points.Aggregate(0.0, (range, loc) =>
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

                southBound = centerY - deltaLat;
                northBound = centerY + deltaLat;

                double deltaLon = (lonRange / ((Math.PI) * (Math.Cos(northBound * Math.PI / 180) * 6378))) * 180;

                westBound = centerX - deltaLon;
                eastBound = centerX + deltaLon;

                Debug.WriteLine(String.Format("delta lat: {0}, delta lon: {1}", deltaLat, deltaLon));

                Debug.WriteLine(String.Format("Lat boundaries: {0} to {1}", southBound, northBound));
                Debug.WriteLine(String.Format("Lon boundaries: {0} to {1}", westBound, eastBound));

                */

                areaWidth = mapContext.EastBoundary- mapContext.WestBoundary;

                areaHeight = mapContext.NorthBoundary- mapContext.SouthBoundary;

                mapWidth = dirtyRect.Width;
                mapHeight = dirtyRect.Height;

            // }


        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            Debug.WriteLine("drawing initiated");
            if (garden == null || garden.Location.Points == null || garden.Location.Points.Count == 0)
            {
                canvas.StrokeColor = Colors.Aqua;
                canvas.StrokeSize = 4;
                canvas.DrawLine(0, 0, 100, 100);
                canvas.StrokeColor = Colors.Navy;
                canvas.DrawLine(0, 100, 100, 0);
            }

            else
            {

                
                if(garden.Location != null && garden.Location.Points != null && garden.Location.Points.Count > 0)
                {
                    
                    Debug.WriteLine("logic has reached the point where points will be examined...");

                    this.Center(canvas, dirtyRect);

                    DrawArea(canvas, garden.Location, Colors.Brown);


                    //general garden objects 

                    foreach(GardenObject gardenObject in garden.Objects)
                    {
                        DrawArea(canvas, gardenObject.ObjectLocation, Colors.DarkGreen);
                    }

                    //if selected object is defined, highlight it
                    if (SelectedObject != null)
                    {
                        if (SelectedObject.ObjectLocation != null)
                        {
                            DrawArea(canvas, SelectedObject.ObjectLocation, Colors.Red);
                        }
                    }

                    //if selected point is defined, highlight it too
                    if (SelectedLocation != null)
                    {
                        DrawPoint(canvas, SelectedLocation, Colors.Red);
                    }

                    //TEST
                    //DrawPoint(canvas, new Location(centerY, centerX), Colors.Black);

                }
            }
        }

        public void UpdateModel(Garden garden)
        {
            this.garden= garden;
        }

        public GardenObject SelectedObject { private get; set; } = null;
        public Location SelectedLocation { private get; set;} = null;

        public void DrawPoint(ICanvas canvas, Location point, Color color)
        {
            canvas.StrokeColor = color;
            canvas.FillColor = color;
            canvas.StrokeSize = 2;

            float pointX = (float)((point.Longitude - mapContext.WestBoundary) / areaWidth) * mapWidth;
            float pointY = (float)((mapContext.NorthBoundary - point.Latitude) / areaHeight) * mapHeight;

            canvas.DrawCircle(pointX, pointY, 2.0f);
        }

        public void DrawArea(ICanvas canvas, Area area, Color color)
        {
            if(area != null & area.Points != null && area.Points.Count > 0)
            {
                canvas.StrokeColor = color;
                canvas.StrokeSize = 2;

                if(area.Points.Count == 1)
                {
                    Debug.WriteLine("single point being rendered");
                    float pointX = (float)((area.Points[0].Longitude - mapContext.WestBoundary) / areaWidth) * mapWidth;
                    float pointY = (float)((mapContext.NorthBoundary - area.Points[0].Latitude) / areaHeight) * mapHeight;
                    canvas.DrawCircle(pointX, pointY, 2.0f);


                } 
                else
                {
                    Debug.WriteLine("shape getting rendered");

                    PathF boundaryPath = new();

                    for (int i = 0; i < area.Points.Count; i++)
                    {
                        float pointX = (float)((area.Points[i].Longitude - mapContext.WestBoundary) / areaWidth) * mapWidth;
                        float pointY = (float)((mapContext.NorthBoundary - area.Points[i].Latitude) / areaHeight) * mapHeight;
                        Debug.WriteLine(String.Format("rendering point at lon: {0}, lat: {1}; calculated to x: {2}, y: {3}", area.Points[i].Longitude, area.Points[i].Latitude, pointX, pointY));
                        if (i == 0)
                        {
                            boundaryPath.MoveTo(pointX, pointY);
                        }
                        else
                        {
                            boundaryPath.LineTo(pointX, pointY);
                        }
                        
                    }

                    boundaryPath.Close();
                    canvas.DrawPath(boundaryPath);
                    
                    //this looks nasty tbh
                    foreach(Location point in area.Points)
                    {
                        float pointX = (float)((point.Longitude - mapContext.WestBoundary) / areaWidth) * mapWidth;
                        float pointY = (float)((mapContext.NorthBoundary - point.Latitude) / areaHeight) * mapHeight;
                        canvas.DrawCircle(pointX, pointY, 2.0f);

                    }

                }
            }
        }


    }
}
