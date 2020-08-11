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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Covid19API1;

namespace CovidClient1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        State state = State.Infected;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DateTime? time = calendar.SelectedDate;

            long id = API.CreateEntry(state, time);

            id_label.Content = id;
        }

        private void Infected_Checked(object sender, RoutedEventArgs e)
        {
            state = State.Infected;
        }

        private void Recovered_Checked(object sender, RoutedEventArgs e)
        {
            state = State.Recovered;
        }

        private void Deceased_Checked(object sender, RoutedEventArgs e)
        {
            state = State.Deceased;
        }
        State oldState = State.Infected;
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            DateTime? time = calendar.SelectedDate;
            string id_text = id_for_update.Text;
            bool id_given = long.TryParse(id_text, out long id);
            
            bool success = id_given? API.UpdateEntry(id, state, time): API.UpdateEntry(search_calendar.SelectedDate.Value, oldState, state, calendar.SelectedDate);
            string successful = "Update successful";
            if (!success)
            {
                successful = "Update failed";
            }
            update_success.Content = successful;
        }

        private void InfectedOld_Checked(object sender, RoutedEventArgs e)
        {
            oldState = State.Infected;
        }

        private void RecoveredOld_Checked(object sender, RoutedEventArgs e)
        {
            oldState = State.Recovered;
        }

        private void DeceasedOld_Checked(object sender, RoutedEventArgs e)
        {
            oldState = State.Deceased;
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            bool? filter = filter_by_state.IsChecked;
            DateTime searchDate = search_calendar.SelectedDate.Value;
            List<Entry> results = null;
            if (filter.HasValue && filter.Value)
            {
                results = API.FindCases(searchDate, oldState);
            }
            else
            {
                results = API.FindCases(searchDate, null);
            }

            found.Text = results.Aggregate("", (text, x) => text + x + "\n");
        }
    }
}
