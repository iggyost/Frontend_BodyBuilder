namespace Frontend_BodyBuilder.Views.ContentPages;

public partial class ProfilePage : ContentPage
{
	public ProfilePage()
	{
		InitializeComponent();
	}

    private void ContentPage_Loaded(object sender, EventArgs e)
    {
		usernameLabel.Text = App.enteredUser.Name;
		emailLabel.Text = App.enteredUser.Email;
    }

    private void exitButton_Clicked(object sender, EventArgs e)
    {
        App.enteredUser = null;
        App.selectedTraining = null;
        Application.Current.MainPage = new NavigationPage(new WelcomePage());
    }
}