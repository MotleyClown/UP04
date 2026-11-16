using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HelpDesk.Data;
using HelpDesk.Models;

namespace HelpDesk.Views
{
    public partial class MainWindow : Window
    {
        private List<Ticket> _tickets = new List<Ticket>();

        public MainWindow()
        {
            InitializeComponent();

            TestDatabaseConnection();

            LoadStatuses();

            SafeLoadTickets();
        }

        private void TestDatabaseConnection()
        {
            try
            {
                using (SqlConnection connection = Database.GetConnection())
                {
                    connection.Open();
                    MessageBox.Show(
                        "Подключение к базе данных выполнено успешно.",
                        "HelpDesk",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(
                    "Ошибка подключения к SQL Server:\n" + ex.Message,
                    "HelpDesk",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void SafeLoadTickets()
        {
            try
            {
                LoadTickets();
                RefreshTicketsGrid();
            }
            catch (SqlException ex)
            {
                TicketsDataGrid.ItemsSource = null;
                MessageBox.Show(
                    "Не удалось загрузить заявки из базы данных.\n\n" +
                    "Проверьте SQL Server, имя базы и строку подключения.\n\n" +
                    ex.Message,
                    "Ошибка базы данных",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void LoadTickets()
        {
            _tickets = new List<Ticket>();

            string sql = @"
                SELECT 
                    t.Id,
                    t.UserId,
                    t.CategoryId,
                    c.Name AS Category,
                    t.Description,
                    t.PriorityId,
                    p.Name AS Priority,
                    t.StatusId,
                    s.Name AS Status,
                    t.AssignedUserId,
                    t.CreatedAt,
                    t.UpdatedAt
                FROM Tickets t
                INNER JOIN Categories c ON t.CategoryId = c.Id
                INNER JOIN Priorities p ON t.PriorityId = p.Id
                INNER JOIN Statuses s ON t.StatusId = s.Id
                ORDER BY t.Id DESC;";

            using (SqlConnection connection = Database.GetConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Ticket ticket = new Ticket
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            UserId = Convert.ToInt32(reader["UserId"]),
                            CategoryId = Convert.ToInt32(reader["CategoryId"]),
                            Category = reader["Category"].ToString(),
                            Description = reader["Description"].ToString(),
                            PriorityId = Convert.ToInt32(reader["PriorityId"]),
                            Priority = reader["Priority"].ToString(),
                            StatusId = Convert.ToInt32(reader["StatusId"]),
                            Status = reader["Status"].ToString(),
                            CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
                        };

                        if (reader["AssignedUserId"] != DBNull.Value)
                            ticket.AssignedUserId = Convert.ToInt32(reader["AssignedUserId"]);

                        if (reader["UpdatedAt"] != DBNull.Value)
                            ticket.UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"]);

                        _tickets.Add(ticket);
                    }
                }
            }
        }

        private void LoadStatuses()
        {
            StatusComboBox.Items.Clear();
            StatusComboBox.Items.Add("Все");

            string sql = "SELECT Name FROM Statuses ORDER BY Id;";

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
                            StatusComboBox.Items.Add(reader["Name"].ToString());
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Ошибка загрузки статусов: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            StatusComboBox.SelectedIndex = 0;
        }

        private void RefreshTicketsGrid()
        {
            string search = SearchTextBox.Text.Trim().ToLower();
            string status = StatusComboBox.SelectedItem as string;

            IEnumerable<Ticket> query = _tickets;

            if (search.Length > 0)
            {
                query = query.Where(t => t.Description.ToLower().Contains(search));
            }

            if (status != null && status != "Все")
            {
                query = query.Where(t => t.Status == status);
            }

            TicketsDataGrid.ItemsSource = query.ToList();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshTicketsGrid();
        }

        private void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshTicketsGrid();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshTicketsGrid();
        }

        private void AddTicketButton_Click(object sender, RoutedEventArgs e)
        {
            TicketCreateWindow createWindow = new TicketCreateWindow();

            if (createWindow.ShowDialog() == true)
            {
                SafeLoadTickets();
            }
        }

        private void EditTicketButton_Click(object sender, RoutedEventArgs e)
        {
            Ticket selectedTicket = TicketsDataGrid.SelectedItem as Ticket;

            if (selectedTicket == null)
            {
                MessageBox.Show("Выберите заявку в таблице.", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            TicketEditWindow editWindow = new TicketEditWindow(selectedTicket);

            if (editWindow.ShowDialog() == true)
            {
                SafeLoadTickets();
            }
        }

        private void DeleteTicketButton_Click(object sender, RoutedEventArgs e)
        {
            Ticket selectedTicket = TicketsDataGrid.SelectedItem as Ticket;

            if (selectedTicket == null)
            {
                MessageBox.Show("Выберите заявку в таблице.", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBoxResult result = MessageBox.Show(
                "Удалить заявку № " + selectedTicket.Id + "?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _tickets.Remove(selectedTicket);
                RefreshTicketsGrid();
            }
        }
    }
}
