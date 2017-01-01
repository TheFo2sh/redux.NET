using Windows.UI.Xaml.Controls;
using Microsoft.Practices.ServiceLocation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ReduxDotNet
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.DataContext =ServiceLocator.Current.GetInstance<MainPageViewModel>();

            this.InitializeComponent();
        }
    }
}
