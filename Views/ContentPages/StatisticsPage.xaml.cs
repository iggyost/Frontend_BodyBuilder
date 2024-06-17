using Frontend_BodyBuilder.ApplicationData;
using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;

namespace Frontend_BodyBuilder.Views.ContentPages;

public partial class StatisticsPage : ContentPage
{
    public StatisticsPage()
    {
        InitializeComponent();
        BindingContext = this;
        Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("ru-RU");
    }
    public CultureInfo Culture => new CultureInfo("ru-RU");
    public static DateTime selectedDate = new DateTime();
    public static List<DailyStatisticHealth> statisticHealth = new List<DailyStatisticHealth>();
    public static List<DailyStatisticTraining> statisticTraining = new List<DailyStatisticTraining>();
    public async Task GetDailyStatisticHealthByUserId()
    {
        HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync($"{App.conString}dailystatistichealth/get/{App.enteredUser.UserId}");
        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<DailyStatisticHealth[]>(content).ToList();
            if (data != null)
            {
                statisticHealth = data;
            }
        }
        else
        {
            Console.WriteLine("UpdateDailyStatisticTraining ошибка");
        }
    }
    public async Task GetDailyStatisticTrainingByUserId()
    {
        HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync($"{App.conString}dailystatistictraining/get/{App.enteredUser.UserId}");
        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<DailyStatisticTraining[]>(content).ToList();
            if (data != null)
            {
                statisticTraining = data;
            }
        }
        else
        {
            Console.WriteLine("UpdateDailyStatisticTraining ошибка");
        }
    }

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        await GetDailyStatisticHealthByUserId();
        await GetDailyStatisticTrainingByUserId();
        ccalLabel.Text = "0";
        timeLabel.Text = "00:00:00";
        countTrainingsLabel.Text = "0";
        while (true)
        {
            if (App.isTrainingsUpdated == true)
            {
                await GetDailyStatisticHealthByUserId();
                await GetDailyStatisticTrainingByUserId();
                App.isTrainingsUpdated = false;
            }
            else
            {

            }
            await Task.Delay(3000);
        }
    }

    private async void calendarTraining_SelectedDateChanged(object sender, EventArgs e)
    {
        selectedDate = calendarTraining.SelectedDate.Value;
        var caloriesList = statisticHealth.Where(x => x.Date.Date == selectedDate).Select(x => x.Calories).ToList();
        int totalCalories = 0;
        TimeSpan totalTime = TimeSpan.Zero;
        foreach (var item in caloriesList)
        {
            totalCalories = totalCalories + item;
        }
        var spentTimeList = statisticHealth.Where(x => x.Date.Date == selectedDate).Select(x => x.SpentTime).ToList();
        foreach (var item in spentTimeList)
        {
            totalTime = totalTime + item;
        }
        ccalLabel.Text = totalCalories.ToString();
        timeLabel.Text = string.Format("{0:T}", totalTime);
        countTrainingsLabel.Text = statisticTraining.Where(x => x.Date.Date == selectedDate).Count().ToString();
        await Task.Delay(1500);
        loadingCalendar.IsRunning = false;
        calendarTraining.IsEnabled = true;
    }

    //private async void calendarTraining_ClickedDate(object sender, EventArgs e)
    //{
    //    calendarTraining.IsEnabled = false;
    //    loadingCalendar.IsRunning = true;
    //    selectedDate = calendarTraining.SelectedDate;
    //    var caloriesList = statisticHealth.Where(x => x.Date.Date == selectedDate).Select(x => x.Calories).ToList();
    //    int totalCalories = 0;
    //    TimeSpan totalTime = TimeSpan.Zero;
    //    foreach (var item in caloriesList)
    //    {
    //        totalCalories = totalCalories + item;
    //    }
    //    var spentTimeList = statisticHealth.Where(x => x.Date.Date == selectedDate).Select(x => x.SpentTime).ToList();
    //    foreach (var item in spentTimeList)
    //    {
    //        totalTime = totalTime + item;
    //    }
    //    ccalLabel.Text = totalCalories.ToString();
    //    timeLabel.Text = string.Format("{0:T}", totalTime);
    //    countTrainingsLabel.Text = statisticHealth.Where(x => x.Date.Date == selectedDate).Count().ToString();
    //    await Task.Delay(1500);
    //    loadingCalendar.IsRunning = false;
    //    calendarTraining.IsEnabled = true;
    //}
}
