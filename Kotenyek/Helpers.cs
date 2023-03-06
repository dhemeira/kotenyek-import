using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace Kotenyek
{
    class Helpers
    {
        public static bool ShowMessage(string message)
        {
            //plug the custom dialog here
            MessageBox.Show(message, "");
            return true;
        }
        public static bool ShowMessage(string message, string title)
        {
            //plug the custom dialog here
            MessageBox.Show(message, title);
            return true;
        }
        public static bool ShowMessage(string message, string title, bool isYesNoDialog)
        {
            if (isYesNoDialog)
            {
                //plug the custom dialog here
                var result = MessageBox.Show(message, title, MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes) return true;
                else return false;
            }
            return ShowMessage(message, title);
        }
    }

    public class Product
    {
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Price { get; set; }
        public string Category { get; set; }
        public string Images { get; set; }
        public string Color { get; set; }
        public string AvailableColors { get; set; }
        public string UID { get; set; }

        public Product(string name, string shortdescription, string description, int length, int width, int price, string category, string images, string color, string availablecolors, string uid)
        {
            Name = name;
            ShortDescription = shortdescription;
            Description = description;
            Length = length;
            Width = width;
            Price = price;
            Category = category;
            Images = images;
            Color = color;
            AvailableColors = availablecolors;
            UID = uid;
        }
    }

    public class TokenResponse
    {
        [JsonPropertyName("token")]
        public string? Token { get; set; }
    }

    public class ColorsResponse : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("slug")]
        public string? Slug { get; set; }

        private bool _checked = false;
        public bool Checked
        {
            get { return _checked; }
            set
            {
                _checked = value;
                OnPropertyChanged(nameof(Checked));
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class CategoriesResponse : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("slug")]
        public string? Slug { get; set; }

        [JsonPropertyName("parent")]
        public int Parent { get; set; }

        private bool _checked = false;
        public bool Checked {
            get { return _checked; } 
            set
            {
                _checked = value;
                OnPropertyChanged(nameof(Checked));
            }
        }

        private bool _enabled = true;
        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                OnPropertyChanged(nameof(Enabled));
            }
        }

        private int _checkedChildCount = 0;
        public int CheckedChildCount
        {
            get { return _checkedChildCount; }
            set
            {
                _checkedChildCount = value;
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
