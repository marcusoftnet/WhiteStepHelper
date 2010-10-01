using System.Windows;

namespace Marcusoft.BDD.WhiteStepHelper.SUT
{
    /// <summary>
    /// Interaction logic for TestDialog.xaml
    /// </summary>
    public partial class TestDialog : Window
    {
        public TestDialog()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
