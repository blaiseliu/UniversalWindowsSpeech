using Windows.UI.Xaml;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace RecognitionDemo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
public sealed partial class MainPage
{
    public MainPage()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }

    private void MainPage_OnUnloaded(object sender, RoutedEventArgs e)
    {
        var vm = DataContext as MainViewModel;
        vm?.Dispose();
    }
}
}
