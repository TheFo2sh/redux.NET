using Windows.UI.Xaml.Controls;
using Microsoft.Practices.ServiceLocation;
using MVRX.Core.Locators;

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
            dynamic bindableServiceLocator = ServiceLocator.Current.GetInstance<BindableServiceLocator>();
            var x = bindableServiceLocator.UserManagementFeature;
            this.DataContext =bindableServiceLocator;

            this.InitializeComponent();
        }
    }
}
