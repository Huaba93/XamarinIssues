using System;
using Akavache;
using Xamarin.Forms;

namespace AkavacheMultipleCachedCalls
{
    public partial class App : Application
    {
        public App()
        {

            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}