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

        private double viewportWestBoundary;
        private double viewportEastBoundary;
        private double viewportSouthBoundary;
        private double viewportNorthBoundary;

        private double areaWidth;
        private double areaHeight;

        private float mapWidth;
        private float mapHeight;

        private double zoomFactor = 1.0;
        private double xOffset = 0.0;
        private double yOffset = 0.0;

        private double degLatPerPx = 0;
        private double degLonPerPx = 0;


        //constructor?
        public GraphicsDrawable(MapContext mapContext)
        {
            this.mapContext = mapContext;
        }

        //needed: translation from geographical coordinates into range 0-100, 0-100
        //todo: zoom in/zoom out + viewport handling

        public void Center(ICanvas canvas, RectF dirtyRect)
        {

            //todo - get rid of passing the rect - it should only be relevant to viewport
            mapContext.SetContext(garden.Location, dirtyRect);

            //this should refresh map context with garden's location parameters


            //todo handle viewport aspect issues



            //now to handle zoom and offset of the viewport
            viewportWestBoundary = mapContext.CenterX + xOffset * (mapContext.DeltaLonArc / 2) - (mapContext.DeltaLonArc / (zoomFactor * 2));
            viewportEastBoundary = mapContext.CenterX + xOffset * (mapContext.DeltaLonArc / 2) + (mapContext.DeltaLonArc / (zoomFactor * 2));
            viewportSouthBoundary = mapContext.CenterY + yOffset * (mapContext.DeltaLatArc / 2) - (mapContext.DeltaLatArc / (zoomFactor * 2));
            viewportNorthBoundary = mapContext.CenterY + yOffset * (mapContext.DeltaLatArc / 2) + (mapContext.DeltaLatArc / (zoomFactor * 2));

            /*
            areaWidth = mapContext.EastBoundary- mapContext.WestBoundary;

            areaHeight = mapContext.NorthBoundary- mapContext.SouthBoundary;
            */

            areaWidth = viewportEastBoundary - viewportWestBoundary;

            areaHeight = viewportNorthBoundary - viewportSouthBoundary;


            mapWidth = dirtyRect.Width;
            mapHeight = dirtyRect.Height;

            degLonPerPx = areaWidth / mapWidth;
            degLatPerPx = areaHeight / mapHeight;
            Debug.WriteLine(String.Format("calculated ratio deg lon per px: {0}", degLonPerPx));
            Debug.WriteLine(String.Format("calculated ratio deg lat per px: {0}", degLatPerPx));


        }

        public void Pan(double xPan, double yPan)
        {
            this.xOffset -= (xPan / mapWidth) / (zoomFactor * 2);
            this.yOffset += (yPan / mapHeight) / (zoomFactor * 2);
        }

        public void Zoom(double change)
        {
            this.zoomFactor = this.zoomFactor * change;
        }

        public void ResetZoom()
        {
            this.zoomFactor = 1.0;
            this.xOffset = 0.0;
            this.yOffset = 0.0;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            //Debug.WriteLine("drawing initiated");
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
                    
                    //Debug.WriteLine("logic has reached the point where points will be examined...");

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
        public ObservableLocation SelectedLocation { private get; set;} = null;

        public void DrawPoint(ICanvas canvas, ObservableLocation point, Color color)
        {
            canvas.StrokeColor = color;
            canvas.FillColor = color;
            canvas.StrokeSize = 2;

            float pointX = (float)((point.Longitude - viewportWestBoundary) / areaWidth) * mapWidth;
            float pointY = (float)((viewportNorthBoundary - point.Latitude) / areaHeight) * mapHeight;

            //todo add boundary check?

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
                    //Debug.WriteLine("single point being rendered");
                    float pointX = (float)((area.Points[0].Longitude - viewportWestBoundary) / areaWidth) * mapWidth;
                    float pointY = (float)((viewportNorthBoundary - area.Points[0].Latitude) / areaHeight) * mapHeight;
                    canvas.DrawCircle(pointX, pointY, 2.0f);


                } 
                else
                {
                    //Debug.WriteLine("shape getting rendered");

                    PathF boundaryPath = new();

                    for (int i = 0; i < area.Points.Count; i++)
                    {
                        float pointX = (float)((area.Points[i].Longitude - viewportWestBoundary) / areaWidth) * mapWidth;
                        float pointY = (float)((viewportNorthBoundary - area.Points[i].Latitude) / areaHeight) * mapHeight;
                        //Debug.WriteLine(String.Format("rendering point at lon: {0}, lat: {1}; calculated to x: {2}, y: {3}", area.Points[i].Longitude, area.Points[i].Latitude, pointX, pointY));
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
                    foreach(ObservableLocation point in area.Points)
                    {
                        float pointX = (float)((point.Longitude - viewportWestBoundary) / areaWidth) * mapWidth;
                        float pointY = (float)((viewportNorthBoundary - point.Latitude) / areaHeight) * mapHeight;
                        canvas.DrawCircle(pointX, pointY, 2.0f);

                    }

                }
            }
        }

        public double PxToDegLon(int px)
        {
            //take viewport size boundaries...
            //todo maybe move calculations here from center so it's not done more often than needed

            return px * degLonPerPx;
        }

        public double PxToDegLat(int px)
        {
            return px * degLatPerPx;
        }


    }
}
