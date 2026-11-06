using System.Windows;
using HelpDesk.Models;

namespace HelpDesk.Views
{
    /// <summary>
    /// Логика взаимодействия для TicketEditWindow.xaml.
    /// Окно получает заявку из главного окна и изменяет её поля.
    /// </summary>
    public partial class TicketEditWindow : Window
    {
        // Редактируемая заявка (передаётся по ссылке из главного окна)
        private Ticket _ticket;

        public TicketEditWindow(Ticket ticket)
        {
            InitializeComponent();

            _ticket = ticket;

            // Наполнение списков из справочников Category, Priority и Status
            foreach (Category category in Category.GetAll())
            {
                CategoryComboBox.Items.Add(category.Name);
            }

            foreach (Priority priority in Priority.GetAll())
            {
                PriorityComboBox.Items.Add(priority.Name);
            }

            foreach (Status status in Status.GetAll())
            {
                StatusComboBox.Items.Add(status.Name);
            }

            // Заполнение полей текущими значениями заявки
            DescriptionTextBox.Text = _ticket.Description;
            CategoryComboBox.SelectedItem = _ticket.Category;
            PriorityComboBox.SelectedItem = _ticket.Priority;
            StatusComboBox.SelectedItem = _ticket.Status;
            CreatedAtTextBlock.Text = "Дата создания: " +
                _ticket.CreatedAt.ToString("dd.MM.yyyy HH:mm");
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string description = DescriptionTextBox.Text.Trim();

            if (description.Length == 0)
            {
                MessageBox.Show("Введите описание проблемы.", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Изменение существующей заявки по ссылке
            _ticket.Description = description;
            _ticket.Category = CategoryComboBox.SelectedItem as string;
            _ticket.Priority = PriorityComboBox.SelectedItem as string;
            _ticket.Status = StatusComboBox.SelectedItem as string;

            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
