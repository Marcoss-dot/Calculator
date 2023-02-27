using Calculator.WpfApp.Net.ViewModels;
using MahApps.Metro.Controls;

namespace Calculator.WpfApp.Net
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}
