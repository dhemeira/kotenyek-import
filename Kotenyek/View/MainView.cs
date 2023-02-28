using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(imageURL)));
            }
        }
    }
}
