using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HelpDesk.Models;

namespace HelpDesk.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml.
    /// Заявки временно хранятся в оперативной памяти (список List);
    /// подключение базы данных MS SQL Server будет выполнено
    /// на следующих практических работах.
    /// </summary>
    public partial class MainWindow : Window
    {
        // Временное хранилище заявок (вместо таблицы Tickets базы данных)
        private List<Ticket> _tickets = new List<Ticket>();

        // Счётчик для присвоения номеров новым заявкам
        private int _nextId = 1;

        public MainWindow()
        {
            InitializeComponent();

            // Наполнение фильтра по статусу из справочника Status
            StatusComboBox.Items.Add("Все");

            foreach (Status status in Status.GetAll())
            {
                StatusComboBox.Items.Add(status.Name);
            }

            StatusComboBox.SelectedIndex = 0;

            FillTicketsWithTestData();
            RefreshTicketsGrid();
        }

        /// <summary>
        /// Тестовые заявки для проверки работы таблицы
        /// (после подключения базы данных заменяются выборкой из MS SQL Server).
        /// </summary>
        private void FillTicketsWithTestData()
        {
            _tickets.Add(new Ticket
            {
                Id = _nextId++,
                Description = "Не включается системный блок в кабинете 305",
                Category = "Техника",
                Priority = "Высокий",
                Status = "Новая",
                CreatedAt = DateTime.Now.AddDays(-2)
            });

            _tickets.Add(new Ticket
            {
                Id = _nextId++,
                Description = "Нет доступа к сети Интернет в библиотеке",
                Category = "Сеть",
                Priority = "Средний",
                Status = "В работе",
                CreatedAt = DateTime.Now.AddDays(-1)
            });

            _tickets.Add(new Ticket
            {
                Id = _nextId++,
                Description = "Не запускается программа 1С на компьютере бухгалтера",
                Category = "Программное обеспечение",
                Priority = "Низкий",
                Status = "Выполнена",
                CreatedAt = DateTime.Now.AddHours(-5)
            });
        }

        /// <summary>
        /// Заполнение таблицы с учётом поиска и фильтра по статусу.
        /// </summary>
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
                Ticket newTicket = createWindow.NewTicket;
                newTicket.Id = _nextId++;
                _tickets.Add(newTicket);
                RefreshTicketsGrid();
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
                RefreshTicketsGrid();
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
