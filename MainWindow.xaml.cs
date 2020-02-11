using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SimpleWPF.Models;

namespace SimpleWPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            refreshDataGridPersons();
            clearPersonForm();
        }
        void btnClear_Click(object sender, RoutedEventArgs e)
        {
            clearPersonForm();
        }
        void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Person selectedPerson = (Person)dataGridPersons.SelectedItem;
            if (selectedPerson == null)
            {
                addPeson();
            }
            else
            {
                updatePerson(selectedPerson);
            }
        }

        void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Person person = (Person)dataGridPersons.SelectedItem;
            if (person != null)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure to delete user id: " + person.Id,
                    "Delete Confirmation", MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    using (var db = new PrehPesonsContext())
                    {
                        db.Remove(person);
                        db.SaveChanges();
                    }
                } 
            }
            else
            {
                MessageBox.Show("First select user to detete!","Alert!");
            }
            refreshDataGridPersons();
        }
        void refreshDataGridPersons()
        {
            using (var db = new PrehPesonsContext())
            {
                var persons = db.Persons.OrderBy(person => person.Id);
                dataGridPersons.ItemsSource = persons.ToList();
            }
        }

        void dataGridPersons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Person person = (Person)dataGridPersons.SelectedItem;
            if(person != null)
            {
                tboxFirstName.Text = person.FirstName;
                tboxLastName.Text = person.LastName;
                setGenderOnForm(person);
            }
        }
        void clearPersonForm()
        {
            tboxFirstName.Text = "";
            tboxLastName.Text = "";
            radioBtnMale.IsChecked = true;
            Person selectedPerson = (Person)dataGridPersons.SelectedItem;

            if (selectedPerson != null)
            {
                dataGridPersons.UnselectAll();
            }
        }
        void addPeson()
        {
            if (tboxFirstName.Text.Equals("") || tboxLastName.Text.Equals(""))
            {
                MessageBox.Show("You need insert data before save!", "Alert!");
            }
            else
            {
                using (var db = new PrehPesonsContext())
                {
                    db.Add(new Person
                    {
                        FirstName = tboxFirstName.Text.ToString(),
                        LastName = tboxLastName.Text.ToString(),
                        Gender = getGenderFromForm()

                    });
                    db.SaveChanges();
                }
            }
            refreshDataGridPersons();
            clearPersonForm();
        }

        void updatePerson(Person person)
        {
            Person editedPerson = new Person
            {
                Id = person.Id,
                FirstName = tboxFirstName.Text,
                LastName = tboxLastName.Text,
                Gender = getGenderFromForm()
            };

            if (person.Equals(editedPerson))
            {
                clearPersonForm();
            }
            else
            {
                if (editedPerson.Id.Equals(person.Id))
                {
                    if (tboxFirstName.Text.Equals("") || tboxLastName.Text.Equals(""))
                    {
                        MessageBox.Show("You need insert data before save!", "Alert!");
                    }
                    else
                    {
                        using (var db = new PrehPesonsContext())
                        {
                            Person personToUpdate = db.Persons.SingleOrDefault(b => b.Id == editedPerson.Id);
                            if (personToUpdate != null)
                            {
                                personToUpdate.FirstName = tboxFirstName.Text;
                                personToUpdate.LastName = tboxLastName.Text;
                                personToUpdate.Gender = getGenderFromForm();
                                db.SaveChanges();
                            }
                        }
                    } 
                }
            }
            clearPersonForm();
            refreshDataGridPersons();
        }
        
        private Gender getGenderFromForm()
        {
            Gender gender = new Gender(); 
            if (radioBtnMale.IsChecked == true)
            {
                gender = Gender.Male;
            }
            else if (radioBtnFemale.IsChecked == true)
            {
                gender = Gender.Female;
            }
            return gender;
        }

        private void setGenderOnForm(Person person)
        {
            if (person.Gender.Equals(Gender.Male))
            {
                radioBtnMale.IsChecked = true;
            }
            else if (person.Gender.Equals(Gender.Female))
            {
                radioBtnFemale.IsChecked = true;
            }
        }
    }
}
