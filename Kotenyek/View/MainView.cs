﻿using System.Collections.ObjectModel;
using System.ComponentModel;

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
                OnPropertyChanged(nameof(ImageURL));
                OnPropertyChanged(nameof(ImageCount));
            }
        }
        public string ImageCount => $"Képek ({(ImageURL == "" ? 0 : ImageURL.Split(',').Length)}):";
        public static int LabelWidth => 110;
        public static int InputMinWidth => 300;

        public MainView()
        {            
            _colors = new ObservableCollection<ColorsResponse>();
            _availableColors = new ObservableCollection<ColorsResponse>();
            _categories = new ObservableCollection<CategoriesResponse>();
            _washingInstruction = new ObservableCollection<Washes>();
        }

        private ObservableCollection<ColorsResponse> _colors;
        private ObservableCollection<ColorsResponse> _availableColors;
        private ObservableCollection<CategoriesResponse> _categories;
        private ObservableCollection<Washes> _washingInstruction;

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

        public ObservableCollection<Washes> WashingInstruction
        {
            get { return _washingInstruction; }
            set
            {
                _washingInstruction = value;
                OnPropertyChanged(nameof(WashingInstruction));
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
