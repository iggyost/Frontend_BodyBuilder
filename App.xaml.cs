using Frontend_BodyBuilder.ApplicationData;
using Frontend_BodyBuilder.Views.ContentPages;

namespace Frontend_BodyBuilder;

public partial class App : Application
{
    public static string conString = "http://192.168.0.10:45455/api/";
	public static Training selectedTraining = new Training();
	public static User enteredUser = new User();
	public static bool isTrainingsUpdated = false;
    public App()
	{
		InitializeComponent();

		MainPage = new NavigationPage(new WelcomePage());
	}
}
