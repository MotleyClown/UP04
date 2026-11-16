using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using HelpDesk.Data;
using HelpDesk.Models;

namespace HelpDesk.Views
{
    public partial class TicketCreateWindow : Window
    {
        public Ticket NewTicket { get; private set; }

        public TicketCreateWindow()
        {
            InitializeComponent();

            LoadCategories();
            LoadPriorities();
        }

        private void LoadCategories()
        {
            CategoryComboBox.Items.Clear();
            string sql = "SELECT Id, Name FROM Categories ORDER BY Name;";

            try
            {
                using (SqlConnection connection = Database.GetConnection())
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ComboBoxItem item = new ComboBoxItem
                            {
                                Content = reader["Name"].ToString(),
                                Tag = Convert.ToInt32(reader["Id"])
                            };
                            CategoryComboBox.Items.Add(item);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Ошибка загрузки категорий: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (CategoryComboBox.Items.Count > 0)
                CategoryComboBox.SelectedIndex = 0;
        }

        private void LoadPriorities()
        {
            PriorityComboBox.Items.Clear();
            string sql = "SELECT Id, Name FROM Priorities ORDER BY SortOrder;";

            try
            {
                using (SqlConnection connection = Database.GetConnection())
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ComboBoxItem item = new ComboBoxItem
                            {
                                Content = reader["Name"].ToString(),
                                Tag = Convert.ToInt32(reader["Id"])
                            };
                            PriorityComboBox.Items.Add(item);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Ошибка загрузки приоритетов: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (PriorityComboBox.Items.Count > 1)
                PriorityComboBox.SelectedIndex = 1;
            else if (PriorityComboBox.Items.Count > 0)
                PriorityComboBox.SelectedIndex = 0;
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

            ComboBoxItem selectedCategory = CategoryComboBox.SelectedItem as ComboBoxItem;
            ComboBoxItem selectedPriority = PriorityComboBox.SelectedItem as ComboBoxItem;

            NewTicket = new Ticket
            {
                Description = description,
                CategoryId = selectedCategory != null ? Convert.ToInt32(selectedCategory.Tag) : 0,
                Category = selectedCategory != null ? selectedCategory.Content.ToString() : "",
                PriorityId = selectedPriority != null ? Convert.ToInt32(selectedPriority.Tag) : 0,
                Priority = selectedPriority != null ? selectedPriority.Content.ToString() : "",
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
