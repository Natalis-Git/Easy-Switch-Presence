
using System;
using System.Windows;




namespace EasySwitchPresence.Views
{

    public partial class OptionsWindow : Window
    {
        public static bool IsOpen { get; private set; }


        public OptionsWindow()
        {
            InitializeComponent();
            IsOpen = true;

            if (disableAfterSetTimeCheckbox.IsChecked == false)
            {
                disableAfterSetTimeCheckbox_Unchecked(null, null);
            }
        }


        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            IsOpen = false;
        }


        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void CloseToTrayCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            if (Owner != null)
            {
                var mainWindow = Owner as MainWindow;
                mainWindow.CloseToSystemTray = true;
            }

        }


        private void CloseToTrayCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (Owner != null)
            {
                var mainWindow = Owner as MainWindow;
                mainWindow.CloseToSystemTray = false;
            }
        }


        private void disableAfterSetTimeCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            if (IsInitialized == false)
            {
                return;
            }

            optionOneRadioButton.IsEnabled = true;
            optionTwoRadioButton.IsEnabled = true;
            optionThreeRadioButton.IsEnabled = true;
            optionFourRadioButton.IsEnabled = true;
        }


        private void disableAfterSetTimeCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (IsInitialized == false)
            {
                return;
            }
            
            optionOneRadioButton.IsEnabled = false;
            optionTwoRadioButton.IsEnabled = false;
            optionThreeRadioButton.IsEnabled = false;
            optionFourRadioButton.IsEnabled = false;
        }
    }

}
