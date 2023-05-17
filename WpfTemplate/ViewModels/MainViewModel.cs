using System.Windows.Input;
using System.Windows;
using System.ComponentModel;
using System.Collections.ObjectModel;
using WpfTemplate.Models;

namespace WpfTemplate.ViewModels
{
    public class MainViewModel
    {
        public ObservableCollection<PersonModel> People { get; set; }

        private PersonModel _selectedPerson;
        public PersonModel SelectedPerson
        {
            get { return _selectedPerson; }
            set
            {
                _selectedPerson = value;
                OnPropertyChanged(nameof(SelectedPerson));
            }
        }

        public ICommand SaveCommand { get; set; }

        public MainViewModel()
        {
            People = new ObservableCollection<PersonModel>()
        {
            new PersonModel() { Name = "John", Age = 30 },
            new PersonModel() { Name = "Jane", Age = 25 },
            new PersonModel() { Name = "Mike", Age = 35 },
        };

            SaveCommand = new RelayCommand(Save, CanSave);
        }

        private bool CanSave(object parameter)
        {
            return SelectedPerson != null && string.IsNullOrEmpty(SelectedPerson["Name"]);
        }

        private void Save(object parameter)
        {
            // Save logic here
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}