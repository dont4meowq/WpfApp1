using System.Windows;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AgentButton_Click(object sender, RoutedEventArgs e)
        {
            AgentWindow agentWindow = new AgentWindow();
            agentWindow.Show();
            this.Close();
        }

        private void BuyerButton_Click(object sender, RoutedEventArgs e)
        {
            BuyerWindow buyerWindow = new BuyerWindow();
            buyerWindow.Show();
            this.Close();
        }
    }
}
