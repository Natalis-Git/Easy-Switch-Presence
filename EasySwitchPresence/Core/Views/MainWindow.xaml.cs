
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Data;
using System.ComponentModel;




namespace EasySwitchPresence.Views
{

    public partial class MainWindow : Window
    {
        /// <summary>
        /// Whether or not window will hide to notify bar instead of closing application
        /// </summary>
        public bool CloseToSystemTray { get; set; }


        public MainWindow()
        {
            InitializeComponent();
        }


        protected override void OnClosing(CancelEventArgs e)
        {
            if (CloseToSystemTray == true)
            {
                e.Cancel = true;
                this.Hide();
            }

            base.OnClosing(e);
        }


        private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(searchBox.Text))
            {
                resultsListBox.Visibility = Visibility.Visible;
            }
            else
            {
                resultsListBox.Visibility = Visibility.Hidden;
            }
        }


        private void mainGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Keyboard.ClearFocus();
        }


        private void searchBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            resultsListBox.Visibility = Visibility.Hidden;
        }


        private void optionsButton_Click(object sender, RoutedEventArgs e)
        {
            if (OptionsWindow.IsOpen) 
            {
                return;
            }
            
            var window = new OptionsWindow(); // FIXME: See radio button FIXME in OptionsWindow.xaml for info
            window.Owner = this;
            window.Title = "Settings";
            window.Icon = Icon;

            window.Show();
        }


        private void discordRenderTimestamp_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            if (String.IsNullOrEmpty(discordRenderTimestamp.Text))
            {
                discordRenderTimestamp.Visibility = Visibility.Hidden;
            }
            else
            {
                discordRenderTimestamp.Visibility = Visibility.Visible;
            }
        }


        private void discordRenderTimestamp_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (discordRenderTimestamp.IsVisible == false)
            {
                discordRenderStatus.Margin = new Thickness(105, 30.5, 0, 0);
                discordRenderDetails.Margin = new Thickness(105, 47.5, 0, 0);
            }
            else
            {
                discordRenderStatus.Margin = new Thickness(105, 23, 0, 0);
                discordRenderDetails.Margin = new Thickness(105, 40, 0, 0);
            }
        }
    }
    
}
