using System;
using Prism.Navigation;
using Xamarin.Forms;

namespace PrismUriNavigationIssue
{
    public class Page2 : ContentPage, IInitialize
    {
        public void Initialize(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("Test"))
            {
                Console.WriteLine("Received from Page2");
            }
        }
    }
}