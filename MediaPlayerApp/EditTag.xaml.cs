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

namespace MediaPlayerApp
{
    /// <summary>
    /// Interaction logic for EditTag.xaml
    /// </summary>
    public partial class EditTag : UserControl
    {
        Mp3 mp = new Mp3();
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
 
        public EditTag()
        {
            InitializeComponent();

        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.setTags();

        }
    }
}
