using System;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;

namespace TouchEffectNullReference
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            var absoluteLabel = new Label {Text = "Absolute Navigation", HeightRequest = 50};

            TouchEffect.SetCommandParameter(absoluteLabel, "Absolute");
            TouchEffect.SetCommand(absoluteLabel, new Command((obj) =>
            {
                Console.WriteLine(obj);
                Application.Current.MainPage = new MainPage();
            }));

            var relativeLabel = new Label {Text = "Relative Navigation", HeightRequest = 50};

            TouchEffect.SetCommandParameter(relativeLabel, "Relative");
            TouchEffect.SetCommand(relativeLabel, new Command(async (obj) =>
            {
                Console.WriteLine(obj);
                await Navigation.PushAsync(new MainPage());
            }));

            var goBackLabel = new Label {Text = "Back Navigation", HeightRequest = 50};

            TouchEffect.SetCommandParameter(goBackLabel, "Back");
            TouchEffect.SetCommand(goBackLabel, new Command(async (obj) =>
            {
                Console.WriteLine(obj);
                await Navigation.PopAsync();
            }));

            this.Content = new StackLayout
            {
                Children =
                {
                    absoluteLabel, relativeLabel, goBackLabel
                }
            };
        }
    }
}