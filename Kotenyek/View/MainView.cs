using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Kotenyek.View
{
    public class MainView : INotifyPropertyChanged
    {
        private string? imageURL;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string ImageURL
        {
            get { return imageURL ?? ""; }
            set
            {
                imageURL = value;
                OnPropertyChanged(nameof(imageURL));
            }
        }

        public MainView()
        {            
            _colors = new ObservableCollection<ColorsResponse>();
            _availableColors = new ObservableCollection<ColorsResponse>();
            _categories = new ObservableCollection<CategoriesResponse>();
        }

        private ObservableCollection<ColorsResponse> _colors;
        private ObservableCollection<ColorsResponse> _availableColors;
        private ObservableCollection<CategoriesResponse> _categories;

        public ObservableCollection<ColorsResponse> Colors
        {
            get { return _colors; }
            set
            {
                _colors = value;
                OnPropertyChanged(nameof(Colors));
            }
        }

        public ObservableCollection<ColorsResponse> AvailableColors
        {
            get { return _availableColors; }
            set
            {
                _availableColors = value;
                OnPropertyChanged(nameof(AvailableColors));
            }
        }

        public ObservableCollection<CategoriesResponse> Categories
        {
            get { return _categories; }
            set
            {
                _categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
