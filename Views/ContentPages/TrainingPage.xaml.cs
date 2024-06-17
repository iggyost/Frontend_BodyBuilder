using DevExpress.Utils.Svg;
using Frontend_BodyBuilder.ApplicationData;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Platform;
using Newtonsoft.Json;

namespace Frontend_BodyBuilder.Views.ContentPages;

public partial class TrainingPage : ContentPage
{
    public TrainingPage()
    {
        InitializeComponent();

        exercisesViews.Clear();
        imageCollectionView.ItemsSource = null;
        isPaused = true;
        currentExercise = null;

    }
    public static List<ExercisesView> exercisesViews = new List<ExercisesView>();
    public static bool isPaused = true;
    public static ExercisesView currentExercise = new ExercisesView();
    public static bool isExerciseCompleted = false;
    public async Task GetExercisesViewByTrainingId()
    {
        HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync($"{App.conString}exercisesview/get/{App.selectedTraining.TrainingId}");
        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ExercisesView[]>(content).ToList();
            exercisesViews = data;
        }
    }
    public async Task UpdateDailyStatisticHealth()
    {
        HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync($"{App.conString}dailystatistichealth/update/{App.enteredUser.UserId}/{App.selectedTraining.TrainingId}");
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("UpdateDailyStatisticHealth успешно");
        }
        else
        {
            Console.WriteLine("UpdateDailyStatisticHealth ошибка");
        }
    }
    public async Task UpdateDailyStatisticTraining()
    {
        HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync($"{App.conString}dailystatistictraining/update/{App.enteredUser.UserId}/{App.selectedTraining.TrainingId}");
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("UpdateDailyStatisticTraining успешно");
        }
        else
        {
            Console.WriteLine("UpdateDailyStatisticTraining ошибка");
        }
    }

    private void pauseButton_Clicked(object sender, EventArgs e)
    {
        pauseButton.Clicked -= pauseButton_Clicked;
        pauseButton.Clicked += continueButton_Clicked;
        pauseButton.Text = "ПРИОСТАНОВИТЬ";
        isPaused = false;
    }
    private void continueButton_Clicked(object sender, EventArgs e)
    {
        pauseButton.Clicked -= continueButton_Clicked;
        pauseButton.Clicked += pauseButton_Clicked;
        pauseButton.Text = "ВОЗОБНОВИТЬ";
        isPaused = true;
    }
    public async Task ExerciseTimer(TimeSpan exerTime)
    {
        var currentExerciseTime = exerTime;
        while (currentExerciseTime != TimeSpan.Zero)
        {
            if (isExerciseCompleted == true)
            {
                isExerciseCompleted = false;
                return;
            }

            if (isPaused == false)
            {
                timeLabel.Text = string.Format("{0:T}", currentExerciseTime);
                currentExerciseTime -= TimeSpan.FromSeconds(1);
                //animationExerciseImage.IsAnimationPlaying = true;
            }
            else
            {
                //animationExerciseImage.IsAnimationPlaying = false;
            }
            await Task.Delay(1000);
        }

    }
    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        try
        {
            await GetExercisesViewByTrainingId();
            foreach (var item in exercisesViews)
            {
                exerciseNameLabel.Text = item.Name;
                imageCollectionView.ItemsSource = exercisesViews;
                imageCollectionView.CurrentItem = item;
                //animationExerciseImage.Source = item.CoverImage;
                timeLabel.Text = string.Format("{0:T}", item.RequiredTime);
                await ExerciseTimer(item.RequiredTime);
            }
            await DisplayAlert("Тренировка окончена!", "", "ОК");
            await UpdateDailyStatisticHealth();
            await UpdateDailyStatisticTraining();
            App.isTrainingsUpdated = true;
            await Navigation.PopModalAsync();
        }
        catch (Exception)
        {

        }
    }

    private async void nextButton_Clicked(object sender, EventArgs e)
    {
        nextButton.IsEnabled = false;
        isExerciseCompleted = true;
        await Task.Delay(1000);
        nextButton.IsEnabled = true;
    }
    public static Image exerciseImage = new Image();
    private async void animationExerciseImage_Loaded(object sender, EventArgs e)
    {
        Image image = sender as Image;
        while (true)
        {
            if (isPaused == true)
            {
                image.IsAnimationPlaying = false;
            }
            else
            {
                image.IsAnimationPlaying = true;
            }
            await Task.Delay(500);
        }
    }

}