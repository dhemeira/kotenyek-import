using System.ComponentModel;

namespace Kotenyek.View
{
    class MessageView : INotifyPropertyChanged
    {
        private string? message;
        private string? title;
        private bool isYesNoDialog;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string Message
        {
            get { return message ?? ""; }
            set
            {
                message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        public string Title
        {
            get { return title ?? ""; }
            set
            {
                title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public bool IsYesNoDialog
        {
            get { return isYesNoDialog; }
            set
            {
                isYesNoDialog = value;
                OnPropertyChanged(nameof(IsYesNoDialog));
            }
        }

        public bool IsOkDialog => !IsYesNoDialog;
        public string YesNoVisibility => IsYesNoDialog ? "Visible" : "Collapsed";
        public string OkVisibility => !IsYesNoDialog ? "Visible" : "Collapsed";

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
