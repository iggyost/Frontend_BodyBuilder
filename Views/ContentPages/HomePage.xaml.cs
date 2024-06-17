using Frontend_BodyBuilder.ApplicationData;
using Newtonsoft.Json;

namespace Frontend_BodyBuilder.Views.ContentPages;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();
	}
    public class CategoriesContext
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public bool IsFirst { get; set; }
    }
    public async Task GetTrainingsByCategory(int categoryId)
    {
        HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync($"{App.conString}trainings/get/category/{categoryId}");
        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<Training[]>(content).ToList();
            trainingsCarouselView.ItemsSource = data;
        }
    }
    public async Task GetTodayPlanTraining()
    {
        HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync($"{App.conString}trainings/get/today");
        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<Training[]>(content).ToList();
            todayPlanCarouselView.ItemsSource = data;
        }
    }
    public async Task GetTrainingById(int trainingId)
    {
        HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync($"{App.conString}trainings/get/id/{trainingId}");
        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            App.selectedTraining = JsonConvert.DeserializeObject<Training>(content);
        }
    }
    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        categoriesCollectionView.ItemsSource = new List<CategoriesContext>
        {
            new CategoriesContext { CategoryId = 1, Name = "Новичок"},
            new CategoriesContext { CategoryId = 2, Name = "Начинающий" },
            new CategoriesContext { CategoryId = 3, Name = "Продвинутый"}
        };
        await GetTrainingsByCategory(1);
        await GetTodayPlanTraining();
        userNameLabel.Text = App.enteredUser.Name;
    }

    private void categoryTrainingLabel_Loaded(object sender, EventArgs e)
    {
        Label label = sender as Label;
        var categoryId = int.Parse(label.AutomationId.ToString());
        switch (categoryId)
        {
            case 1:
                label.Text = "Для новичков";
                break;
            case 2:
                label.Text = "Для начинающих";
                break;
            case 3:
                label.Text = "Для продвинутых";
                break;
        }
    }

    private async void categoryRadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        RadioButton radioButton = sender as RadioButton;
        radioButton.IsEnabled = false;
        var categoryId = int.Parse(radioButton.AutomationId.ToString());
        switch (categoryId)
        {
            case 1:
                await GetTrainingsByCategory(1);
                break;
            case 2:
                await GetTrainingsByCategory(2);
                break;
            case 3:
                await GetTrainingsByCategory(3);
                break;
        }
        await Task.Delay(2000);
        radioButton.IsEnabled = true;
    }

    private void categoryRadioButton_Loaded(object sender, EventArgs e)
    {
        RadioButton radioButton = sender as RadioButton;
        var categoryId = int.Parse(radioButton.AutomationId.ToString());
        if (categoryId == 1)
        {
            radioButton.IsChecked = true;
        }
        else
        {
            radioButton.IsChecked = false;
        }
    }

    private async void trainingGesture_Tapped(object sender, TappedEventArgs e)
    {
        Border border = sender as Border;
        var trainingId = int.Parse(border.AutomationId.ToString());
        await GetTrainingById(trainingId);
        if (App.selectedTraining != null)
        {
            await Navigation.PushModalAsync(new TrainingPreviewPage(App.selectedTraining));
        }
    }
}