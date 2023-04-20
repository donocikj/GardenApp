using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GardenApp.Model
{
    public class Garden: INotifyPropertyChanged
    {
        private string _name;
        private string _description;
        private Area _location;
        private ObservableCollection<GardenObject> _objects;

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(params string[] propertyNames)
        {
            foreach(string name in propertyNames)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }

        //todo migrate the file handling outside this class
        public static async Task<Garden> LoadGarden(string gardenName)
        {
            string filename = String.Format("{0}.json", gardenName);
            string filepath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),filename);
            using FileStream openStream= File.OpenRead(filepath);

            Garden? garden = await JsonSerializer.DeserializeAsync<Garden>(openStream);
            await openStream.DisposeAsync();
            Debug.Write("garden has been loaded: ");
            Debug.WriteLine(garden.ToString());

            return garden;
        }

        public static async Task<Garden> LoadGardenFromPickedFile()
        {
            Garden toBeReturned = null;

            FilePickerFileType filetype = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.WinUI, new [] {".json"} },
                { DevicePlatform.Android, new [] {"application/json"} }
            });

            PickOptions options = new() { 
                PickerTitle = "please select a garden json",
                FileTypes = filetype
            };


            try
            {
                FileResult result = await FilePicker.Default.PickAsync();
                Debug.WriteLine("picked this in fileresult, now what?");
                Debug.WriteLine(result.FullPath);
                var stream = await result.OpenReadAsync();
                toBeReturned = await JsonSerializer.DeserializeAsync<Garden>(stream);
                await stream.DisposeAsync();
                Debug.WriteLine(toBeReturned.ToString());

            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error: {e}");
            }



            return toBeReturned;


        }

        public Garden()
        {
            _name = "My Garden";
            _description= string.Empty;
            //_areas = new List<Area>();
            //_objects = new List<GardenObject>();
            //_location = new Location();
            _location = new Area();
            _objects = new ObservableCollection<GardenObject>();
            Debug.WriteLine("basic constructor called and hopefully succeeded in creating new garden");

            _location.PropertyChanged += GardenLocationUpdated;

        }

        //public Garden(string serializedGarden)
        //{
        //    Garden? newGarden = JsonSerializer.Deserialize<Garden>(serializedGarden);
        //    //todo handle deserialization failure
        //    
        //}

        public string Name 
        { 
            get { return _name; } 
            set 
            { 
                _name = value; 
                OnPropertyChanged(nameof(Name));
            } 
        }

        public string Description 
        { 
            get { return _description; } 
            set 
            { 
                _description = value; 
                OnPropertyChanged(nameof(Description)); 
            } 
        }

        public Area Location 
        { 
            get { return _location; } 
            set 
            { 
                _location = value;
                _location.PropertyChanged += GardenLocationUpdated;
                OnPropertyChanged(nameof(Location));
            } 
        }

        public ObservableCollection<GardenObject> Objects 
        { 
            get { return _objects; } 
            set { 
                _objects = value; 
                OnPropertyChanged(nameof(Objects));
            } 
        }

        //public List<Area> getAreas() { return _areas; }
        //public void addArea(Area area) => _areas.Add(area);
        //public void removeArea(int index) => _areas.RemoveAt(index);

        //public List<GardenObject> getObjects() { return _objects; }
        //public void addObject(GardenObject obj) => _objects.Add(obj);
        //public void removeObject(int index) => _objects.RemoveAt(index);


        public void AddObject(GardenObject obj)
        {
            _objects.Add(obj);
            OnPropertyChanged(nameof(Objects));
        }

        public void RemoveObject(GardenObject obj)
        {
            if (_objects.Contains(obj))
            {
                _objects.Remove(obj);
                OnPropertyChanged(nameof(Objects));
            }
        }


        private void GardenLocationUpdated(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Location));
        }

        



        public override string ToString()
        {
            return String.Format("Garden with name '{0}', described as {1}", this._name, this._description);
        }

        //todo migrate the file handling outside this class
        public async Task SaveGarden()
        {
            string filename = String.Format("{0}.json", this._name);
            string filepath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),filename);
            using FileStream createStream = File.Create(filepath);
            var options = new JsonSerializerOptions { WriteIndented= true };
            await JsonSerializer.SerializeAsync(createStream, this, options);
            await createStream.DisposeAsync();

        }

        //todo rework this and the previous one
        public async Task ShareGarden()
        {
            string filename = String.Format("{0}.json", this._name);
            string filepath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), filename);

            await SaveGarden();
            await Share.Default.RequestAsync(new ShareFileRequest { Title = "share the garden json file", File = new ShareFile(filepath) });
        }

    }
}
