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
using FinalMunicipalApp.Data;
using FinalMunicipalApp.Models;
using FinalMunicipalApp.Services;

namespace FinalMunicipalApp.Views
{
    public partial class LocalEventsWindow : Window
    {
        public LocalEventsWindow()
        {
            InitializeComponent();
            LoadFilters();
            LoadEvents(EventRepository.GetAll());
        }

        private void LoadFilters()
        {
            CboCategoryFilter.Items.Clear();
            CboCategoryFilter.Items.Add("-- All --");
            foreach (var c in EventRepository.UniqueCategories.OrderBy(s => s)) CboCategoryFilter.Items.Add(c);
            CboCategoryFilter.SelectedIndex = 0;
        }

        private void LoadEvents(IEnumerable<Event> events)
        {
            var list = events.Select(e => new {
                Date = e.Date.ToString("yyyy-MM-dd"),
                e.Title,
                e.Category,
                e.Priority,
                TagsString = string.Join(", ", e.Tags ?? new List<string>())
            }).OrderBy(e => e.Date).ToList();

            LvEvents.ItemsSource = list;
            TxtStatus.Text = $"Showing {list.Count} events.";
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            string category = CboCategoryFilter.SelectedIndex > 0 ? CboCategoryFilter.SelectedItem.ToString() : null;
            DateTime? date = DatePickerFilter.SelectedDate;
            string query = TxtSearchQuery.Text;

            RecommendationEngine.LogSearch(category, query);

            var results = EventRepository.Search(category, date, query);
            LoadEvents(results);
        }

        private void BtnRecommend_Click(object sender, RoutedEventArgs e)
        {
            var recs = RecommendationEngine.Recommend(10);
            LoadEvents(recs);
        }

        private void BtnAddEvent_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new AddEventDialog();
            if (dlg.ShowDialog() == true)
            {
                EventRepository.AddEvent(dlg.CreatedEvent);
                LoadFilters();
                LoadEvents(EventRepository.GetAll());
            }
        }
    }
}


