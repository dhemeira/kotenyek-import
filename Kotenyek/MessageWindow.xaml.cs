using Kotenyek.View;
using System.Windows;

namespace Kotenyek
{
    /// <summary>
    /// Interaction logic for MessageWindow.xaml
    /// </summary>
    public partial class MessageWindow : Window
    {
        readonly MessageView messageView = new();
        public MessageWindow(string message, string title, bool isYesNoDialog)
        {
            InitializeComponent();
            DataContext = messageView;
         
            messageView.Message = message;
            messageView.Title = title;
            messageView.IsYesNoDialog = isYesNoDialog;
        }

        private void Yes_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void No_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult= false;
        }
    }
}
