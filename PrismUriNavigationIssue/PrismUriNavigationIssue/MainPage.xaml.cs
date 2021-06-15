using System;
using Prism.Navigation;
using Xamarin.Forms;

namespace PrismUriNavigationIssue
{
    public partial class MainPage : ContentPage
    {
        private const string NavigationPath = "Page1/Page2";
        private const string QueryString = "Test=true";
        private static readonly NavigationParameters NavigationParams = new NavigationParameters(QueryString);

        private readonly INavigationService _navigationService;

        public MainPage(INavigationService navigationService)
        {
            _navigationService = navigationService;
            InitializeComponent();
        }

        private void ButtonUri_OnClicked(object sender, EventArgs e)
        {
            //NOT working
            _navigationService.NavigateAsync(NavigationPath + NavigationParams);
        }

        private void ButtonQuery_OnClicked(object sender, EventArgs e)
        {
            //NOT working
            _navigationService.NavigateAsync($"{NavigationPath}?{QueryString}");
        }

        private void ButtonNavigationParameters_OnClicked(object sender, EventArgs e)
        {
            //working as expected
            _navigationService.NavigateAsync(NavigationPath, NavigationParams);
        }
    }
}