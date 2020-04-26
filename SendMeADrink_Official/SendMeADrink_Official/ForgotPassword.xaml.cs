using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Mail;
using Xamarin.Essentials;

namespace SendMeADrink_Official
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgotPassword : ContentPage
    {
        public ForgotPassword()
        {
            InitializeComponent();
        }

        private async void ForgotPasswordButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EmailEntry.Text))
            {
                await DisplayAlert("Enter your Email", "", "OK");
            }
            else
            {
                HttpClient client = new HttpClient(new HttpClientHandler());

                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("Email", EmailEntry.Text),
                });

                HttpResponseMessage res = await client.PostAsync("http://send-meadrink.com/SMAD_App/ForgotPassword/forgot.php", content);
                var json = await res.Content.ReadAsStringAsync();
                var Email_DB = JsonConvert.DeserializeObject(json);

                if (Email_DB.ToString() == EmailEntry.Text)
                {
                    try
                    {
                        MailMessage mail = new MailMessage();
                        SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                        /*Opmaak header email*/
                        mail.From = new MailAddress("noreply.sendmeadrink@gmail.com");
                        mail.To.Add(EmailEntry.Text);
                        mail.Subject = "Reset Password";
                        mail.Body = "Please use this link to reset your password: " + "http://send-meadrink.com/SMAD_App/ForgotPassword/ResetPassword/resetPassword.html"; //op deze pagina wordt repeatPassword.php opgeroepen om het password te veranderen.

                        /*Connectie met de smtp server*/
                        SmtpServer.Port = 587;
                        SmtpServer.Host = "smtp.gmail.com";
                        SmtpServer.EnableSsl = true;
                        SmtpServer.UseDefaultCredentials = false;
                        SmtpServer.Credentials = new System.Net.NetworkCredential("noreply.sendmeadrink@gmail.com", "u4nTek#KSR4O[RQ");

                        /*Versturen van de email*/
                        SmtpServer.Send(mail);
                        await DisplayAlert("Email has been sent to " + EmailEntry.Text, null, null, "OK");
                        Application.Current.MainPage = new NavigationPage(new Login());
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Failed", ex.Message, "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Failed to send email!", "No account found registered with that email address!", null, "OK");
                }

            }
        }

        /*Removes the last page in the stack*/
        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}