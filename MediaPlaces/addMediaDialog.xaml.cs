using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Data.SQLite;

namespace MediaPlaces
{
    /// <summary>
    /// Interaktionslogik für addMediaDialog.xaml
    /// </summary>
    public partial class addMediaDialog : Window
    {
        private SQLiteConnection mSqlC;
        public addMediaDialog(Window owner, SQLiteConnection sqlC)
        {
            InitializeComponent();
            this.mSqlC = sqlC;
            this.Owner = owner;
            updateComboBox();
        }

        /*
         * Add new data to the sqlite database
        */
        private void newData(String name,String place)
        {
            try
            {
                SQLiteCommand command = new SQLiteCommand(this.mSqlC);

                command.CommandText = "SELECT * FROM place WHERE place.placeName = \"" + place + "\"";
                SQLiteDataReader reader = command.ExecuteReader();
                if(!reader.HasRows)
                {
                    reader.Close();
                    command.CommandText = "INSERT INTO place(placeName) VALUES('" + place + "')";
                    command.ExecuteNonQuery();
                }
                reader.Close();
                command.CommandText = "INSERT INTO media(title,place) VALUES('" + name + "',(SELECT place.placeID FROM place WHERE place.placeName = '" + place + "'))";
                command.ExecuteNonQuery();
            }
            catch (SQLiteException exception)
            {
                MessageBox.Show(exception.Message + "\n" + exception.Source + "\n" + exception.StackTrace);
            }
        }
        /*
         * fill the combo box with all places in the table place
         */
        public void updateComboBox()
        {
            try
            {
                SQLiteCommand command = new SQLiteCommand(this.mSqlC);
                command.CommandText = "SELECT place.placeName FROM place";
                SQLiteDataReader reader = command.ExecuteReader();
                List<String> places = new List<string>();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        places.Add(reader.GetString(0));
                    }
                }
                
                placeComboBox.ItemsSource = places;
            }
            catch (SQLiteException e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private void addAndNextButton_Click(object sender, RoutedEventArgs e)
        {
            String mediaName = nameTextBox.Text;
            String placeName = placeComboBox.Text;
            int placeIndex = placeComboBox.SelectedIndex;
            Console.Out.Write(placeIndex);
            updateComboBox();
            MainWindow mw = (MainWindow)this.Owner;
            mw.updateTable();
            newData(mediaName, placeName);
            nameTextBox.Text = "";
            placeComboBox.SelectedIndex = -1;
        }

        private void addAndCloseButton_Click(object sender, RoutedEventArgs e)
        {
            String mediaName = nameTextBox.Text;
            String placeName = placeComboBox.Text;
            newData(mediaName, placeName);
            MainWindow mw = (MainWindow)this.Owner;
            mw.updateTable();
            Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = (MainWindow)this.Owner;
            mw.updateTable();
            Close();
        }
    }
}
