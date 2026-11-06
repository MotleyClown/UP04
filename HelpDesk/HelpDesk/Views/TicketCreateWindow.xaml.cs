using System;
using System.Windows;
using HelpDesk.Models;

namespace HelpDesk.Views
{
    /// <summary>
    /// Логика взаимодействия для TicketCreateWindow.xaml.
    /// Созданная заявка возвращается в главное окно
    /// через свойство NewTicket и DialogResult.
    /// </summary>
    public partial class TicketCreateWindow : Window
    {
        /// <summary>
        /// Заявка, созданная в этом окне (null, если сохранение не выполнено).
        /// </summary>
        public Ticket NewTicket { get; private set; }

        public TicketCreateWindow()
        {
            InitializeComponent();

            // Наполнение списков из справочников Category и Priority
            foreach (Category category in Category.GetAll())
            {
                CategoryComboBox.Items.Add(category.Name);
            }

            foreach (Priority priority in Priority.GetAll())
            {
                PriorityComboBox.Items.Add(priority.Name);
            }

            CategoryComboBox.SelectedIndex = 0;
            PriorityComboBox.SelectedIndex = 1;
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

            // Формирование новой заявки: номер присваивает главное окно
            NewTicket = new Ticket
            {
                Description = description,
                Category = CategoryComboBox.SelectedItem as string,
                Priority = PriorityComboBox.SelectedItem as string,
                Status = "Новая",
                CreatedAt = DateTime.Now
            };

            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
