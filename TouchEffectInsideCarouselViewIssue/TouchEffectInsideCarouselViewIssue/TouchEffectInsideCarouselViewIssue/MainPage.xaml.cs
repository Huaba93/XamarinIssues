using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouchEffectInsideCarouselViewIssue.SampleData;
using Xamarin.Forms;

namespace TouchEffectInsideCarouselViewIssue
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            CarouselView.ItemsSource = SampleDataCreator.CreateSampleData(100);
        }
    }
}