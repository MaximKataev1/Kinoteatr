using Kino_Kataev.Classes;
using Kino_Kataev.Models;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Kino_Kataev.Pages
{
    public partial class Kinoteatr : Page
    {
        ObservableCollection<KinoteatrModel> kinoteatrs = new ObservableCollection<KinoteatrModel>();
        public Kinoteatr()
        {
            InitializeComponent();
            SetPlaceholder(txtName, "Название");
            SetPlaceholder(txtZal, "Кол-во залов");
            SetPlaceholder(txtCount, "Кол-во мест");
            LoadData();
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

        private void LoadData() 
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
            dgKinoteatr.ItemsSource = kinoteatrs;
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            using (var conn = Connection.OpenConnection())
            {
                string sql = "INSERT INTO Kinoteatr (name, count_zal, count) VALUES (@n, @z, @c)";
                var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@n", txtName.Text);
                cmd.Parameters.AddWithValue("@z", txtZal.Text);
                cmd.Parameters.AddWithValue("@c", txtCount.Text);
                cmd.ExecuteNonQuery();
            }
            LoadData();
        }

        private void Edit(object sender, RoutedEventArgs e)
        {
            if (dgKinoteatr.SelectedItem is KinoteatrModel selected)
            {
                using (var conn = Connection.OpenConnection())
                {
                    string sql = "UPDATE Kinoteatr SET name=@n, count_zal=@z, count=@c WHERE id=@id";
                    var cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@n", txtName.Text);
                    cmd.Parameters.AddWithValue("@z", txtZal.Text);
                    cmd.Parameters.AddWithValue("@c", txtCount.Text);
                    cmd.Parameters.AddWithValue("@id", selected.id);
                    cmd.ExecuteNonQuery();
                }
                LoadData();
            }
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            if (dgKinoteatr.SelectedItem is KinoteatrModel selected)
            {
                using (var conn = Connection.OpenConnection())
                {
                    string sql = "DELETE FROM Kinoteatr WHERE id=@id";
                    var cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", selected.id);
                    cmd.ExecuteNonQuery();
                }
                LoadData();
            }
        }
    }
}
