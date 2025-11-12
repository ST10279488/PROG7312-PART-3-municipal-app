using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FinalMunicipalApp.Models;

namespace FinalMunicipalApp.Views
{
    public partial class AddEventDialog : Window
    {
        public Event CreatedEvent { get; private set; }

        public AddEventDialog()
        {
            InitializeComponent();
            DpDate.SelectedDate = DateTime.Today;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtTitle.Text) || string.IsNullOrWhiteSpace(TxtCategory.Text))
            {
                MessageBox.Show("Please provide at least a title and category.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            CreatedEvent = new Event
            {
                Title = TxtTitle.Text.Trim(),
                Category = TxtCategory.Text.Trim(),
                Description = TxtDescription.Text.Trim(),
                Date = DpDate.SelectedDate ?? DateTime.Today,
                Priority = 3,
                Tags = TxtTags.Text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(t => t.Trim()).ToList()
            };

            this.DialogResult = true;
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}

