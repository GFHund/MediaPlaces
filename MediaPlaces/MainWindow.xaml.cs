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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SQLite;

namespace MediaPlaces
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SQLiteConnection mSqlC = new SQLiteConnection();
        public MainWindow()
        {
            InitializeComponent();
            
            this.mSqlC.ConnectionString = "DataSource=database.sqlite";
            this.mSqlC.Open();
            updateTable();
        }

        public void updateTable()
        {
            try
            {
                SQLiteCommand command = new SQLiteCommand(this.mSqlC);
                command.CommandText = "CREATE TABLE IF NOT EXISTS media(mediaID INTEGER NOT NULL PRIMARY KEY, title TEXT NOT NULL,place INTEGER NOT NULL)";
                command.ExecuteNonQuery();
                command.CommandText = "CREATE TABLE IF NOT EXISTS place(placeID INTEGER NOT NULL PRIMARY KEY, placeName TEXT)";
                command.ExecuteNonQuery();
                command.CommandText = "SELECT media.title,place.placeName FROM media,place WHERE media.place = place.placeID";
                SQLiteDataReader reader = command.ExecuteReader();
                MediaList.ItemsSource = reader;
            }
            catch (SQLiteException e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void MenuItem_Open(object sender,RoutedEventArgs e)
        {
            //ToDo: open a file dialog and import a file
        }
        private void MenuItem_Exit(object sender,RoutedEventArgs e)
        {
            Close();
        }
        private void MenuItem_About(object sender,RoutedEventArgs e)
        {
            //toDo: open a about dialog
        }

        private void menuItemAddMedia_Click(object sender, RoutedEventArgs e)
        {
            //aaaa
            addMediaDialog dlg = new addMediaDialog(this,this.mSqlC);
            dlg.Show();
            //updateTable();
        }
    }
}
