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
        public MainWindow()
        {
            InitializeComponent();
           
            SQLiteConnection sqlC = new SQLiteConnection();
            
            sqlC.ConnectionString = "DataSource=database.sqlite";
            sqlC.Open();
            
            try
            {
                SQLiteCommand command = new SQLiteCommand(sqlC);
                command.CommandText = "CREATE TABLE IF NOT EXISTS media(mediaID INTEGER NOT NULL PRIMARY KEY, title VARCHAR(100) NOT NULL,place INTEGER NOT NULL)";
                command.ExecuteNonQuery();
                command.CommandText = "CREATE TABLE IF NOT EXISTS place(placeID INTEGER NOT NULL PRIMARY KEY, placeName VARCHAR(100))";
                command.ExecuteNonQuery();
                command.CommandText = "SELECT media.title,place.placeName FROM media,place WHERE media.place = place.placeID";
                SQLiteDataReader reader = command.ExecuteReader();
                MediaList.ItemsSource = reader;
                //MediaList.DataContext = reader;
            }
            catch(SQLiteException e)
            {
                MessageBox.Show(e.Message);
            }
           
            
            //MediaList.DataContext = reader;
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
    }
}
