using System.Windows;
using System.Windows.Controls;

namespace Marcusoft.BDD.WhiteStepHelper.SUT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void comboBox1_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var it = comboBox1.SelectedItem as ComboBoxItem;
            if (it != null) txtResult.Text = "You selected " + it.Content;
        }

        private void lblResult_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            txtResult.Text = "You double clicked the Result textbox label"; 
        }


        private void btnNewDialog_Click(object sender, RoutedEventArgs e)
        {
            var d = new TestDialog {Title = "New and shiny"};
            d.ShowDialog();

        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            txtResult.Text = "You clicked the Test button"; 
        }

    }
}
