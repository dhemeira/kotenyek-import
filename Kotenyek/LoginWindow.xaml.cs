using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Kotenyek
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            siteURLTB.Focus();
        }
     
        private async Task LoginUser()
        {
            mainLoginStackPanel.IsEnabled = false;
            loginSpinner.Visibility = Visibility.Visible;
            if (string.IsNullOrWhiteSpace(siteURLTB.Text) || string.IsNullOrWhiteSpace(siteUsernameTB.Text) || string.IsNullOrWhiteSpace(sitePasswordTB.Password))
            {
                ShowMessage("Nem töltötted ki az összes mezőt!", "Hiba");
                return;
            }

            var siteURL = "https://" + siteURLTB.Text.Trim().Split("/").ToList().Find(x => x.Contains(".hu") || x.Contains(".com"));
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
                    Properties.Settings.Default.Save();
                    this.Close();
                    break;
                case System.Net.HttpStatusCode.RequestTimeout:
                case System.Net.HttpStatusCode.GatewayTimeout:
                    ShowMessage("Az oldal jelenleg nem elérhető, próbáld újra később!", "Hiba");
                    break;
                case System.Net.HttpStatusCode.Forbidden:
                    ShowMessage("Hibás adatok!", "Hiba");
                    break;
                default:
                    ShowMessage("Ismeretlen hiba!", "Hiba");
                    break;
            }
        }

        private void ShowMessage(string message, string title)
        {
            Helpers.ShowMessage(message, title);
            mainLoginStackPanel.IsEnabled = true;
            loginSpinner.Visibility = Visibility.Hidden;
        }

        private async void LoginUser_Click(object sender, RoutedEventArgs e)
        {
            await LoginUser();
            siteURLTB.Focus();
        }

        private async void SitePasswordTB_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await LoginUser();
                siteURLTB.Focus();
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
    }
}
