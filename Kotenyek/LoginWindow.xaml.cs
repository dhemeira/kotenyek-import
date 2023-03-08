using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Windows.Controls;
using System;
using System.Diagnostics;

namespace Kotenyek
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        string lastUser = "";
        string lastUrl = "";
        int attemptsWithSameName = 1;
        public LoginWindow()
        {
            InitializeComponent();         
            if(Properties.Settings.Default.Sites == null)
            {
                Properties.Settings.Default.Sites = new StringCollection();
            }
            siteURLCB.ItemsSource = Properties.Settings.Default.Sites;
        }
     
        private async Task LoginUser()
        {
            mainLoginStackPanel.IsEnabled = false;
            loginSpinner.Visibility = Visibility.Visible;
            if (string.IsNullOrWhiteSpace(siteURLCB.Text) || string.IsNullOrWhiteSpace(siteUsernameTB.Text) || string.IsNullOrWhiteSpace(sitePasswordTB.Password))
                {
                ShowMessage("Nem töltötted ki az összes mezőt!", "Hiba");
                return;
            }

            var siteURL = "https://" + siteURLCB.Text.Trim().Split("/").ToList().Find(x => x.Contains(".hu") || x.Contains(".com"));
            if (!siteURL.Contains(".hu") && !siteURL.Contains(".com"))
            {
                ShowMessage("Hibás oldal URL!", "Hiba");
                return;
            }          

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, $"{siteURL}/wp-json/jwt-auth/v1/token")
            {
                Content = new StringContent($"{{\"username\":\"{siteUsernameTB.Text.Trim()}\",\"password\":\"{sitePasswordTB.Password.Trim()}\"}}", Encoding.UTF8, "application/json")
            };

            try
            {
                var response = await client.SendAsync(request);
                await HandleResponse(response, siteURL);
            }
            catch (System.Exception)
            {
                ShowMessage("Ismeretlen hiba!", "Hiba");
            }            
        }

        private async Task HandleResponse(HttpResponseMessage? response, string siteURL)
        {
            if (response == null)
            {
                ShowMessage("Az oldal jelenleg nem elérhető, próbáld újra később!", "Hiba");
                return;
            }

            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    var responseContent = await response.Content.ReadFromJsonAsync<TokenResponse>();
                    Properties.Settings.Default.SiteURL = siteURL;
                    Properties.Settings.Default.AuthToken = responseContent?.Token;
                    if(!Properties.Settings.Default.Sites.Contains(siteURLCB.Text)) Properties.Settings.Default.Sites.Add(siteURLCB.Text);
                    Properties.Settings.Default.Save();
                    this.Close();
                    break;
                case System.Net.HttpStatusCode.RequestTimeout:
                case System.Net.HttpStatusCode.GatewayTimeout:
                    ShowMessage("Az oldal jelenleg nem elérhető, próbáld újra később!", "Hiba");
                    break;
                case System.Net.HttpStatusCode.Forbidden:
                    if (lastUser == siteUsernameTB.Text && lastUrl == siteURL)
                    {
                        attemptsWithSameName++;
                    }
                    else
                    {
                        lastUser = siteUsernameTB.Text;
                        lastUrl = siteURL;
                        attemptsWithSameName = 1;
                    }
                    DateTime dateTime = DateTime.Now;
                    if (attemptsWithSameName >= 3 || Properties.Settings.Default.LastFailedAttempt.AddMinutes(10) >= dateTime)
                    {
                        if (Properties.Settings.Default.LastFailedAttempt.AddMinutes(10) < dateTime)
                        {
                            Properties.Settings.Default.LastFailedAttempt = DateTime.Now;
                            Properties.Settings.Default.Save();
                        }
                        ShowMessage($"Túl sok hibás próbálkozás, próbáld újra {10 - dateTime.Subtract(Properties.Settings.Default.LastFailedAttempt).Minutes} perc múlva!", "Hiba");
                    } else
                    {
                        ShowMessage("Hibás adatok!", "Hiba");
                    }                 
                    break;
                default:
                    ShowMessage("Ismeretlen hiba!", "Hiba");
                    break;
            }
        }

        private void ShowMessage(string message, string title)
        {
            Helpers.ShowMessage(this, message, title);
            mainLoginStackPanel.IsEnabled = true;
            loginSpinner.Visibility = Visibility.Hidden;
        }

        private async void LoginUser_Click(object sender, RoutedEventArgs e)
        {
            await LoginUser();
            siteURLCB.Focus();
        }

        private async void SitePasswordTB_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await LoginUser();
                siteURLCB.Focus();
            }           
        }

        private void SiteURLTB_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                siteUsernameTB.Focus();
        }

        private void SiteUsernameTB_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                sitePasswordTB.Focus();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var textBox = (siteURLCB.Template.FindName("PART_EditableTextBox", siteURLCB) as TextBox);
            if (textBox != null)
            {
                textBox.Focus();
                textBox.SelectionStart = textBox.Text.Length;
            }
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            var sInfo = new ProcessStartInfo(e.Uri.AbsoluteUri)
            {
                UseShellExecute = true,
            };
            Process.Start(sInfo);
            e.Handled = true;
        }
    }
}
