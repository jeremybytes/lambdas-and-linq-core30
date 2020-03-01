using PeopleViewer.Library;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace PeopleViewer
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private IEnumerable<Person> cachedPeople;

        public IEnumerable<Person> People { get; private set; }

        private bool dateFilterChecked;
        public bool DateFilterChecked
        {
            get { return dateFilterChecked; }
            set
            {
                if (dateFilterChecked == value)
                    return;
                dateFilterChecked = value;
                UpdateFilterAndSort();
            }
        }

        private int dateFilterStartYear;
        public int DateFilterStartYear
        {
            get { return dateFilterStartYear; }
            set
            {
                if (dateFilterStartYear == value)
                    return;
                dateFilterStartYear = value;
                UpdateFilterAndSort();
            }
        }

        private int dateFilterEndYear;
        public int DateFilterEndYear
        {
            get { return dateFilterEndYear; }
            set
            {
                if (dateFilterEndYear == value)
                    return;
                dateFilterEndYear = value;
                UpdateFilterAndSort();
            }
        }

        private bool nameFilterChecked;
        public bool NameFilterChecked
        {
            get { return nameFilterChecked; }
            set
            {
                if (nameFilterChecked == value)
                    return;
                nameFilterChecked = value;
                UpdateFilterAndSort();
            }
        }

        private string nameFilterValue;
        public string NameFilterValue
        {
            get { return nameFilterValue; }
            set
            {
                if (nameFilterValue == value)
                    return;
                nameFilterValue = value;
                UpdateFilterAndSort();
            }
        }

        private bool familyNameSortChecked;
        public bool FamilyNameSortChecked
        {
            get { return familyNameSortChecked; }
            set
            {
                if (familyNameSortChecked == value)
                    return;
                if (value)
                {
                    UncheckAllSort();
                    familyNameSortChecked = value;
                    UpdateFilterAndSort();
                }
            }
        }

        private bool givenNameSortChecked;
        public bool GivenNameSortChecked
        {
            get { return givenNameSortChecked; }
            set
            {
                if (givenNameSortChecked == value)
                    return;
                if (value)
                {
                    UncheckAllSort();
                    givenNameSortChecked = value;
                    UpdateFilterAndSort();
                }
            }
        }

        private bool startDateSortChecked;
        public bool StartDateSortChecked
        {
            get { return startDateSortChecked; }
            set
            {
                if (startDateSortChecked == value)
                    return;
                if (value)
                {
                    UncheckAllSort();
                    startDateSortChecked = value;
                    UpdateFilterAndSort();
                }
            }
        }

        private bool ratingSortChecked;
        public bool RatingSortChecked
        {
            get { return ratingSortChecked; }
            set
            {
                if (ratingSortChecked == value)
                    return;
                if (value)
                {
                    UncheckAllSort();
                    ratingSortChecked = value;
                    UpdateFilterAndSort();
                }
            }
        }

        public MainWindowViewModel()
        {
            dateFilterStartYear = 1985;
            dateFilterEndYear = 2000;
            nameFilterValue = "John";
        }

        public void RefreshData()
        {
            var repository = new PeopleReader();
            repository.GetPeopleCompleted += (s, e) =>
            {
                cachedPeople = e.Result;
                ResetFilters();
                UpdateFilterAndSort();
            };
            repository.GetPeopleAsync();
        }

        private void ResetFilters()
        {
            UncheckAllSort();
            dateFilterChecked = false;
            nameFilterChecked = false;
            UpdateFilterAndSort();
        }

        private void UncheckAllSort()
        {
            familyNameSortChecked = false;
            givenNameSortChecked = false;
            startDateSortChecked = false;
            ratingSortChecked = false;
        }

        private void UpdateFilterAndSort()
        {
            People = cachedPeople;

            if (DateFilterChecked)
                People = People
                    .Where(p => p.StartDate.Year >= dateFilterStartYear)
                    .Where(p => p.StartDate.Year <= dateFilterEndYear);

            if (NameFilterChecked)
                People = People.Where(p => p.GivenName == nameFilterValue);

            if (familyNameSortChecked)
                People = People.OrderBy(p => p.FamilyName);

            if (givenNameSortChecked)
                People = People.OrderBy(p => p.GivenName).ThenBy(p => p.FamilyName);

            if (startDateSortChecked)
                People = People.OrderBy(p => p.StartDate);

            if (ratingSortChecked)
                People = People.OrderByDescending(p => p.Rating);

            RaisePropertyChanged();
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
