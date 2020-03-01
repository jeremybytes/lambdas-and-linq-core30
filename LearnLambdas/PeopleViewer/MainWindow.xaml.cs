using PeopleViewer.Library;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace PeopleViewer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            Person selectedPerson = PersonListBox.SelectedItem as Person;

            var repository = new PeopleReader();
            repository.GetPeopleCompleted += (s, a) =>
                {
                    PersonListBox.ItemsSource = ApplySort(ApplyFilters(a.Result));

                    PersonListBox.SelectedItem = 
                        PersonListBox.Items.OfType<Person>()
                            .FirstOrDefault(p => p.Id == selectedPerson?.Id);
                };
            //selectedPerson = null;
            repository.GetPeopleAsync();
        }

        private IEnumerable<Person> ApplyFilters(IEnumerable<Person> data)
        {
            int startYear = int.Parse(StartDateTextBox.Text);
            int endYear = int.Parse(EndDateTextBox.Text);

            if (DateFilterCheckBox.IsChecked.Value)
                data = data.Where(p => p.StartDate.Year >= startYear)
                        .Where(p => p.StartDate.Year <= endYear);

            if (NameFilterCheckBox.IsChecked.Value)
                data = data.Where(p => p.GivenName == NameTextBox.Text);

            return data;
        }

        private IOrderedEnumerable<Person> ApplySort(IEnumerable<Person> data)
        {
            if (FamilyNameSortButton.IsChecked.Value)
                return data.OrderBy(p => p.FamilyName);

            if (GivenNameSortButton.IsChecked.Value)
                return data.OrderBy(p => p.GivenName).ThenBy(p => p.FamilyName);

            if (DateSortButton.IsChecked.Value)
                return data.OrderBy(p => p.StartDate);

            if (RatingSortButton.IsChecked.Value)
                return data.OrderByDescending(p => p.Rating);

            return data.OrderBy(p => p.FamilyName);
        }

        //private void Repository_GetPeopleCompleted(object sender, GetPeopleCompletedEventArgs e)
        //{
        //    PersonListBox.ItemsSource = e.Result;
        //}
    }
}
