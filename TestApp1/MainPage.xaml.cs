using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Uwp.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TestApp1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Random _random;

        public MainPage()
        {
            this.InitializeComponent();

            _random = new Random();
            Models = new ObservableCollection<Model>(Enumerable.Range(0, 20).Select(i => new Model
            {
                Id = i + 1,
                Title = $"Title: {i + 1}",
                Year = _random.Next(2015, 2020)
            }));

            View1 = new AdvancedCollectionView(Models, true);
            View1.ObserveFilterProperty(nameof(Model.Year));
            View1.Filter = model => ((Model)model).Year <= 2020;

            View2 = new AdvancedCollectionView(Models, true);
            View2.ObserveFilterProperty(nameof(Model.Year));
            View2.Filter = model => ((Model)model).Year >= 2021;
        }


        public ObservableCollection<Model> Models { get; set; }
        public IAdvancedCollectionView View1 { get; set; }
        public IAdvancedCollectionView View2 { get; set; }

        private async void OnDrop(object sender, DragEventArgs e)
        {
            if (int.TryParse(await e.DataView.GetTextAsync(), out var id))
            {
                var model = Models.FirstOrDefault(x => x.Id == id);

                if (model != null)
                {
                    model.Year = _random.Next(2021, 2030);
                }
            }
        }

        private void OnDragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Move;
        }

        private void OnDragStarting(object sender, DragItemsStartingEventArgs e)
        {
            if (e.Items.Count == 1 && e.Items[0] is Model model)
            {
                e.Data.SetText($"{model.Id}");
                e.Data.RequestedOperation = DataPackageOperation.Move;
            }
        }
    }

    public class Model : ObservableObject
    {
        private int _id;

        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private string _title;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private int _year;

        public int Year
        {
            get { return _year; }
            set { SetProperty(ref _year, value); }
        }

    }
}
