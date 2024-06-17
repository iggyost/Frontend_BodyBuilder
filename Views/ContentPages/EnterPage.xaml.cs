using Frontend_BodyBuilder.ApplicationData;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Frontend_BodyBuilder.Views.ContentPages;

public partial class EnterPage : ContentPage
{
    public EnterPage()
    {
        InitializeComponent();
    }
    Regex emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
    private async void registrationButton_Clicked(object sender, EventArgs e)
    {
        registrationButton.IsEnabled = false;
        if (emailEntry.Text != null)
        {
            if (nameEntry.Text != null)
            {
                if (passwordEntry.Text != null)
                {
                    if (emailRegex.IsMatch(emailEntry.Text))
                    {
                        if (passwordEntry.Text.Length > 3)
                        {

                            HttpClient client = new HttpClient();
                            var response = await client.GetAsync($"{App.conString}users/reg/{nameEntry.Text}/{emailEntry.Text}/{passwordEntry.Text}");
                            if (response.IsSuccessStatusCode)
                            {
                                string content = await response.Content.ReadAsStringAsync();
                                App.enteredUser = JsonConvert.DeserializeObject<User>(content);
                                Application.Current.MainPage = new AppShell();
                            }
                            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                            {
                                await DisplayAlert("Ошибка!", "Пользователь с таким E-mail уже есть!", "Закрыть");
                            }
                            else
                            {
                                await DisplayAlert("Ошибка!", "Неправильные данные пользователя!", "Закрыть");
                            }
                        }
                        else
                        {
                            await DisplayAlert("Ошибка!", "Минимальная длина пароля - 4 символа!", "Закрыть");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Ошибка!", "E-mail не соответствует формату!", "Закрыть");
                    }
                }
                else
                {
                    await DisplayAlert("Ошибка!", "Поле для пароля не может быть пустым", "Закрыть");
                }
            }
            else
            {
                await DisplayAlert("Ошибка!", "Поле для имени не может быть пустым", "Закрыть");
            }
        }
        else
        {
            await DisplayAlert("Ошибка!", "Поле для E-mail не может быть пустым", "Закрыть");
        }
        registrationButton.IsEnabled = true;
    }

    private async void loginButton_Clicked(object sender, EventArgs e)
    {
        loginButton.IsEnabled = false;
        if (emailAuthEntry.Text != null)
        {
            if (passwordAuthEntry.Text != null)
            {
                if (emailRegex.IsMatch(emailAuthEntry.Text))
                {
                    if (passwordAuthEntry.Text.Length > 3)
                    {
                        HttpClient client = new HttpClient();
                        var response = await client.GetAsync($"{App.conString}users/enter/{emailAuthEntry.Text}/{passwordAuthEntry.Text}");
                        if (response.IsSuccessStatusCode)
                        {
                            string content = await response.Content.ReadAsStringAsync();
                            App.enteredUser = JsonConvert.DeserializeObject<User>(content);
                            Application.Current.MainPage = new AppShell();
                        }
                        else
                        {
                            await DisplayAlert("Ошибка!", "Неправильные данные пользователя!", "Закрыть");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Ошибка!", "Минимальная длина пароля - 4 символа!", "Закрыть");
                    }
                }
                else
                {
                    await DisplayAlert("Ошибка!", "E-mail не соответствует формату!", "Закрыть");
                }
            }
            else
            {
                await DisplayAlert("Ошибка!", "Введите пароль!", "Закрыть");
            }
        }
        else
        {
            await DisplayAlert("Ошибка!", "Введите E-mail!", "Закрыть");
        }
        loginButton.IsEnabled = true;
    }

    private void registrationRb_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (registrationRb.IsChecked == true)
        {
            registrationGrid.IsVisible = true;
            loginGrid.IsVisible = false;
        }
        else
        {

        }
    }

    private void loginRb_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (loginRb.IsChecked == true)
        {
            registrationGrid.IsVisible = false;
            loginGrid.IsVisible = true;
        }
        else
        {

        }

    }
}