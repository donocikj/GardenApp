using GardenApp.Commands;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenApp.ViewModel
{
    using GardenApp.Drawable;
    using Model;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;

    public class GardenVM : INotifyPropertyChanged //, IQueryAttributable
    {
        private Garden garden;
        private ICommand createGardenCommand;
        private ICommand saveGardenCommand;
        private ICommand loadGardenCommand;
        private ICommand pickGardenCommand;
        private SetAreaCommand editAreaCommand;
        private ICommand editGardenObjectCommand;
        private ICommand shareGardenCommand;
        private ICommand gardenObjectDeleteCommand;
        private ICommand returnToMainCommand;

        private GraphicsDrawable gardenDrawable;


        //private string currentLocation = "to be dealt with later";

        


        public GardenVM(GraphicsDrawable gardenDrawable)
        {
            Debug.WriteLine("GardenVM constructor called");
            this.gardenDrawable = gardenDrawable;

        }


        


        #region commands
        public ICommand CreateGardenCommand
        {
            get
            {
                if (createGardenCommand == null)
                {
                    createGardenCommand = new GardenCreateGardenCommand(this);
                }
                return createGardenCommand;
            }
        }

        public ICommand SaveGardenCommand
        {
            get
            {
                if (saveGardenCommand == null)
                {
                    saveGardenCommand = new GardenSaveGardenCommand(this);
                }
                return saveGardenCommand;
            }
        }


        public ICommand LoadGardenCommand
        {
            get
            {
                if (loadGardenCommand == null)
                {
                    loadGardenCommand = new GardenLoadDefaultGardenCommand(this);
                }
                return loadGardenCommand;
            }
        }

        public ICommand PickGardenCommand
        {
            get
            {
                if( pickGardenCommand == null)
                {
                    pickGardenCommand = new GardenLoadWithPickerCommand(this);
                }
                return pickGardenCommand;
            }
        }

        //using SetAreaCommand seems ugly here... tbchanged later
        public SetAreaCommand EditAreaCommand
        {
            get
            {
                if( editAreaCommand == null)
                {
                    editAreaCommand = new SetAreaCommand();
                }
                return editAreaCommand;
            }
        }

        public ICommand EditGardenObjectCommand
        {
            get
            {
                if( editGardenObjectCommand == null )
                {
                    editGardenObjectCommand = new GardenEditGardenObjectCommand(this);
                }
                return editGardenObjectCommand;
            }
        }

        public ICommand ShareGardenCommand
        {
            get
            {
                if(shareGardenCommand == null)
                    shareGardenCommand = new GardenShareGardenCommand(this);
                return shareGardenCommand;
            }
        }

        public ICommand GardenObjectDeleteCommand
        {
            get
            {
                if(gardenObjectDeleteCommand == null)
                    gardenObjectDeleteCommand = new GardenGardenObjectDeleteCommand(this);
                return gardenObjectDeleteCommand;
            }
        }

        public ICommand ReturnToMainCommand
        {
            get
            {
                if (returnToMainCommand == null)
                    returnToMainCommand = new GardenViewReturnToMainCommand(this);
                return returnToMainCommand;
            }
        }

        #endregion


        #region fields
       
        public GraphicsDrawable GardenDrawable
        {
            get
            {
                return this.gardenDrawable;
            }
        }

        public Garden Garden
        {
            get { return this.garden; }
            set
            {
                this.garden = value;
            }
        }

        // objects
        // for view: tbd
        //
        public ObservableCollection<GardenObject> GardenObjects
        {
            get { 
                return this.garden?.Objects; 
            }
            set
            {
                if(garden != null)
                {
                    this.garden.Objects = value;
                    OnPropertyChanged(nameof(this.GardenObjects));
                }
            }
        }


        #endregion


        #region tasks and mutations

        public void AddObject(GardenObject obj)
        {
            if(garden != null)
            {
                garden.AddObject(obj);
                OnPropertyChanged(nameof(GardenObjects));
            }
        }

        public void DeleteObject(GardenObject obj) 
        {
            garden.RemoveObject(obj);
            OnPropertyChanged(nameof(GardenObjects));
        }


        //command to create new instance of garden

        public void NewGarden()
        {
            Debug.WriteLine("newgarden method has been called");
            garden = new Garden();
            garden.Objects.CollectionChanged += this.OnCollectionChanged;
            OnPropertyChanged(nameof(Garden));

        }


        // save and load?
        // this looks ugly, to be refactored
        public async void saveGarden()
        {
            Debug.WriteLine("savegarden method has been called! passing it to the model.");
            await garden.SaveGarden();

            // return filename
            //Debug.WriteLine(result);
        }

        public async void ShareGarden()
        {
            Debug.WriteLine("share request received");
            await garden.ShareGarden();
        }

        public async Task PickGardenFile()
        {
            /*
            if (garden != null)
            {
                garden.Objects.CollectionChanged -= this.OnCollectionChanged;
            }
            */

            Debug.WriteLine("gonna load garden from filepicked file...");
            garden = await Garden.LoadGardenFromPickedFile();
            garden.Objects.CollectionChanged += this.OnCollectionChanged;
            OnPropertyChanged(nameof(Garden));

            if (editAreaCommand == null)
                editAreaCommand = new SetAreaCommand();
            editAreaCommand.GardenContext = garden;
        }


        public async Task LoadGarden(string gardenName)
        {
            //open file named after garden.json (accept filename)

            //read and feed into the deserializer
            garden = await Garden.LoadGarden(gardenName);

            Debug.WriteLine("loaded a garden...");
            Debug.WriteLine(garden.ToString());


            garden.Objects.CollectionChanged += this.OnCollectionChanged;
            OnPropertyChanged(nameof(Garden));


            if (editAreaCommand == null)
                editAreaCommand = new SetAreaCommand();
            editAreaCommand.GardenContext = garden;
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Debug.WriteLine("oncollectionchanged has been called");
            if(e.NewItems != null)
            {
                foreach(object item in e.NewItems)
                {
                    Debug.WriteLine("New item?");
                    Debug.WriteLine(item.ToString());
                }
            }
            if(e.OldItems != null)
            {
                foreach(object item in e.OldItems)
                {
                    Debug.WriteLine("an old item (removed)");
                    Debug.WriteLine(item.ToString());
                }
            }
        }


        protected void OnPropertyChanged(params string[] propertyNames) 
        { 
            foreach(string propertyName in propertyNames)
            {
                PropertyChanged(
                    this,
                    new PropertyChangedEventArgs(propertyName));
            }
            //todo away with this blasphemy

        }
        /*
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            object updateInformation;
            //todo - literals OUT!
            if(query.TryGetValue("locationUpdated", out updateInformation))
            {
                if ((bool)updateInformation)
                {
                    //onPropertyChanged(nameof(GardenLocation));
                }

            }

            //add logic to update object collection as well and for the love of all that is holy find a way to refactor this
            if(query.TryGetValue("objectsUpdated", out updateInformation))
            {
                if ((bool)updateInformation)
                {
                    //Debug.WriteLine("list of object has been requested to be updated...");
                    foreach(object item in GardenObjects)
                    {
                        GardenObject wat = (GardenObject)item;
                        //now how to trigger refresh of all the individual items or at least the changed ones?
                        //PropertyChanged(wat, new PropertyChangedEventArgs("Name"));
                        

                    }
                    onPropertyChanged(nameof(GardenObjects));
                }

            }
           
        }*/
    }
}
