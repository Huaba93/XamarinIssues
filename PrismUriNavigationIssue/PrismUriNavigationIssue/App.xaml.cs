using Prism.DryIoc;
using Prism.Ioc;
using Xamarin.Forms;

namespace PrismUriNavigationIssue
{
    public partial class App : PrismApplication
    {
        public App()
        {
            InitializeComponent();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage>();
            containerRegistry.RegisterForNavigation<Page1>();
            containerRegistry.RegisterForNavigation<Page2>();

                
        }

        protected override async void OnInitialized()
        {
            await NavigationService.NavigateAsync($"/NavigationPage/MainPage").ConfigureAwait(false);
        }

        protected override void OnResume()
        {
        }
    }
}
