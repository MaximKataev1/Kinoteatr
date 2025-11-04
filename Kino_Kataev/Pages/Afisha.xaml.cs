using Kino_Kataev.Classes;
using Kino_Kataev.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Kino_Kataev.Pages
{
    public partial class Afisha : Page
    {
        ObservableCollection<Models.Afisha> afishas = new ObservableCollection<Models.Afisha>();
        ObservableCollection<KinoteatrModel> kinoteatrs = new ObservableCollection<KinoteatrModel>();
        public Afisha()
        {
            InitializeComponent();
            SetPlaceholder(txtName, "Название");
            SetPlaceholder(txtPrice, "Цена");
            LoadKinoteatrs();
            LoadAfisha();
        }

        private void SetPlaceholder(TextBox textBox, string placeholderText)
        {
            textBox.Tag = placeholderText;
            textBox.Text = placeholderText;
            textBox.Foreground = Brushes.Gray;
            textBox.GotFocus += (sender, e) =>
            {
                if (textBox.Text == placeholderText)
                {
                    textBox.Text = "";
                    textBox.Foreground = Brushes.Black;
                }
            };
            textBox.LostFocus += (sender, e) =>
            {
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    textBox.Text = placeholderText;
                    textBox.Foreground = Brushes.Gray;
                }
            };
        }

        private void LoadKinoteatrs()
        {
            kinoteatrs.Clear();
            using (var conn = Connection.OpenConnection())
            {
                string sql = "SELECT * FROM Kinoteatr";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    kinoteatrs.Add(new KinoteatrModel(
                        reader.GetInt32("id"),
                        reader.GetString("name"),
                        reader.GetInt32("count_zal"),
                        reader.GetInt32("count")
                    ));
                }
            }
            cbKino.ItemsSource = kinoteatrs;
        }

        private void LoadAfisha()
        {
            afishas.Clear();
            using (var conn = Connection.OpenConnection())
            {
                string sql = "SELECT * FROM Afisha";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    afishas.Add(new Models.Afisha(
                        reader.GetInt32("id"),
                        reader.GetInt32("id_kinoteatr"),
                        reader.GetString("name"),
                        reader.GetDateTime("time"),
                        reader.GetInt32("price")
                    ));
                }
            }
            dgAfisha.ItemsSource = afishas;
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            if (dgAfisha.SelectedItem is Models.Afisha selected)
            {
                using (var conn = Connection.OpenConnection())
                {
                    string sql = "DELETE FROM Afisha WHERE id=@id";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", selected.id);
                    cmd.ExecuteNonQuery();
                }
                LoadAfisha();
            }
        }

        private void Edit(object sender, RoutedEventArgs e)
        {
            if (dgAfisha.SelectedItem is Models.Afisha selected)
            {
                using (var conn = Connection.OpenConnection())
                {
                    string sql = "UPDATE Afisha SET id_kinoteatr=@kino, name=@name, time=@time, price=@price WHERE id=@id";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@kino", cbKino.SelectedValue);
                    cmd.Parameters.AddWithValue("@name", txtName.Text);
                    cmd.Parameters.AddWithValue("@time", dpTime.SelectedDate.Value);
                    cmd.Parameters.AddWithValue("@price", txtPrice.Text);
                    cmd.Parameters.AddWithValue("@id", selected.id);
                    cmd.ExecuteNonQuery();
                }
                LoadAfisha();
            }
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            if (cbKino.SelectedValue == null || dpTime.SelectedDate == null)
            {
                MessageBox.Show("Выберите кинотеатр и дату!");
                return;
            }

            using (var conn = Connection.OpenConnection())
            {
                string sql = "INSERT INTO Afisha (id_kinoteatr, name, time, price) VALUES (@kino, @name, @time, @price)";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@kino", cbKino.SelectedValue);
                cmd.Parameters.AddWithValue("@name", txtName.Text);
                cmd.Parameters.AddWithValue("@time", dpTime.SelectedDate.Value);
                cmd.Parameters.AddWithValue("@price", txtPrice.Text);
                cmd.ExecuteNonQuery();
            }

            LoadAfisha();
        }

        private void dgAfisha_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgAfisha.SelectedItem is Models.Afisha selected)
            {
                cbKino.SelectedValue = selected.id_kinoteatr;
                txtName.Text = selected.name;
                dpTime.SelectedDate = selected.time;
                txtPrice.Text = selected.price.ToString();
            }
        }
    }
}
