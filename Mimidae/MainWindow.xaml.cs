using System;
using System.Windows;

namespace Mimidae
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            TorrentsListBox.Items.Add(DateTime.Now.Ticks);
        }

        private void StartButton_OnClick(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void PauseButton_OnClick(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void StopButton_OnClick(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedItems = TorrentsListBox.SelectedItems;
            if (selectedItems.Count is 1)
            {
                var item = selectedItems[0];
                TorrentsListBox.Items.Remove(item);
            }
        }
    }
}