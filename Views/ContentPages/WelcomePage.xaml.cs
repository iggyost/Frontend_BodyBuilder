namespace Frontend_BodyBuilder.Views.ContentPages;

public partial class WelcomePage : ContentPage
{
	public static List<WelcomeContext> welcomeContexts = new List<WelcomeContext>();
	public WelcomePage()
	{
		InitializeComponent();

        welcomeContexts = new List<WelcomeContext>
        {
            new WelcomeContext { Source = "a_back_welcome.png", Text = "Начните свой путь сегодня"},
            new WelcomeContext { Source = "b_back_welcome.png", Text = "Составьте план тренировок чтобы оставаться в форме"},
            new WelcomeContext { Source = "c_back_welcome.png", Text = "Действие - это ключ к успеху"},
        };
		backgroundCarouselView.ItemsSource = welcomeContexts;
		textCarouselView.ItemsSource = welcomeContexts;
    }
	public class WelcomeContext
	{
		public string Source { get; set; } 
		public string Text { get; set; } 
	}

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
		while (true)
		{
			await Task.Delay(7500);
			backgroundCarouselView.CurrentItem = welcomeContexts.ElementAt(1);
			textCarouselView.CurrentItem = welcomeContexts.ElementAt(1);
            await Task.Delay(7500);
            backgroundCarouselView.CurrentItem = welcomeContexts.ElementAt(2);
            textCarouselView.CurrentItem = welcomeContexts.ElementAt(2);
            await Task.Delay(7500);
            backgroundCarouselView.CurrentItem = welcomeContexts.ElementAt(0);
            textCarouselView.CurrentItem = welcomeContexts.ElementAt(0);
        }
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new EnterPage());
    }
}