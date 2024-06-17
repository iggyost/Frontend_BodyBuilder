using Frontend_BodyBuilder.ApplicationData;

namespace Frontend_BodyBuilder.Views.ContentPages;

public partial class TrainingPreviewPage : ContentPage
{
    public TrainingPreviewPage()
    {
        InitializeComponent();
    }
    public TrainingPreviewPage(Training selectedTraining)
    {
        InitializeComponent();

    }

    private void ContentPage_Loaded(object sender, EventArgs e)
    {
        backgroundImage.Source = App.selectedTraining.CoverImage;
        titleLabel.Text = App.selectedTraining.Name;
        switch (App.selectedTraining.CategoryId)
        {
            case 1:
                categoryLabel.Text = "��� ��������";
                break;
            case 2:
                categoryLabel.Text = "��� ����������";
                break;
            case 3:
                categoryLabel.Text = "��� �����������";
                break;
        }
        timeLabel.Text = App.selectedTraining.TrainingTime.TotalMinutes.ToString() + " ���";
        caloriesLabel.Text = App.selectedTraining.Calories.ToString() + " ����";
        descLabel.Text = App.selectedTraining.Description;

    }

    private async void startTrainingButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new TrainingPage());
    }

    private async void backButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}