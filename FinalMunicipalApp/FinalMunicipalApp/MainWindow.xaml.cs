using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FinalMunicipalApp;
using MunicipalApp.Windows;

namespace FinalMunicipalApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnReportIssues_Click(object sender, RoutedEventArgs e)
        {
            var reportWindow = new ReportIssuesWindow();
            reportWindow.Owner = this;
            reportWindow.ShowDialog();
        }

        private void BtnLocalEvents_Click(object sender, RoutedEventArgs e)
        {
            var eventsWindow = new Views.LocalEventsWindow();
            eventsWindow.Owner = this;
            eventsWindow.ShowDialog();
        }

        private void BtnServiceRequest_Click(object sender, RoutedEventArgs e)
        {
            ServiceRequestStatusWindow statusWindow = new ServiceRequestStatusWindow();
            statusWindow.Show();
        }


    }
}
