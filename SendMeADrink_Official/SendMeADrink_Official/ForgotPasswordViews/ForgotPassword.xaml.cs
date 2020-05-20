using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Mail;

namespace SendMeADrink_Official.ForgotPasswordViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgotPassword : ContentPage
    {
        public string randomCode;

        public ForgotPassword()
        {
            InitializeComponent();
        }

        /*Forgot password button clicked handeler*/
        private async void ForgotPasswordButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EmailEntry.Text))
            {
                ErrorMessage.Text = "Enter your email";
            }
            else
            {
                HttpClient client = new HttpClient(new HttpClientHandler());

                /*Creating a new variable of the type "FormUrlEncodedContent" to store the data that will be send to our database*/
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("Email", EmailEntry.Text)
                });

                HttpResponseMessage res = await client.PostAsync("http://send-meadrink.com/SMAD_App/ForgotPassword/forgot.php", content);

                /*Checks if the data is retreived from the database*/
                if (res.IsSuccessStatusCode)
                {
                    var json = await res.Content.ReadAsStringAsync();
                    var ValidEmail = JsonConvert.DeserializeObject(json);

                    /*Checks if the email entered is present in our database*/
                    if (ValidEmail.ToString() == "true")
                    {
                        try
                        {
                            Random rand = new Random();
                            randomCode = (rand.Next(999999)).ToString(); //Store a randomly generated number the variable "randomcode"

                            MailMessage mail = new MailMessage();
                            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                            /*Email header*/
                            mail.From = new MailAddress("noreply.sendmeadrink@gmail.com");
                            mail.To.Add(EmailEntry.Text);
                            mail.Subject = "Reset Password";
                            mail.Body = "Use this verfication code in the app to reset your password: " + randomCode; //op deze pagina wordt repeatPassword.php opgeroepen om het password te veranderen.

                            /*Connection with the SMTP server*/
                            SmtpServer.Port = 587;
                            SmtpServer.Host = "smtp.gmail.com";
                            SmtpServer.EnableSsl = true;
                            SmtpServer.UseDefaultCredentials = false;
                            SmtpServer.Credentials = new System.Net.NetworkCredential("noreply.sendmeadrink@gmail.com", "u4nTek#KSR4O[RQ");

                            /*Sending the email*/
                            SmtpServer.Send(mail);
                            await DisplayAlert("Email has been sent to " + EmailEntry.Text, null, null, "OK"); //Alerting the user the email has been send
                            await Navigation.PushAsync(new ValidateCode(randomCode, EmailEntry.Text)); //Navigating to the validate code page
                        }
                        catch (Exception ex)
                        {
                            await DisplayAlert("Failed", ex.Message, "OK"); //Alert the user that the email isn't send
                        }
                    }
                    else
                    {
                        /*Alert the user that something went wrong*/
                        await DisplayAlert("Failed to send email!", "No account found registered with that email address!", null, "OK");
                    }
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