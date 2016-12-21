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

namespace MatReqs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<CalculatedItem> resultingItems = new List<CalculatedItem>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_Calculate_Click(object sender, RoutedEventArgs e)
        {
            resultingItems.Add(new CalculatedItem() { Title = "Test 1", Amount = 2, InInventory = false });
            resultingItems.Add(new CalculatedItem() { Title = "Test 2", Amount = 4, InInventory = false });
            resultingItems.Add(new CalculatedItem() { Title = "Test 3", Amount = 8, InInventory = false });

            listBox_CalculatedResult.ItemsSource = resultingItems;
        }
        /* // Future implementation maybe (When clicking on a row, it should change the checkbox accordingly)
        private void listBox_CalculatedResult_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(listBox_CalculatedResult.SelectedItem != null)
            {
                CalculatedItem selectedItem = (listBox_CalculatedResult.SelectedItem as CalculatedItem);

                selectedItem.InInventory = selectedItem.InInventory ? false : true;

                listBox_CalculatedResult.ItemsSource = resultingItems;
            }
        }*/
    }

    public class CalculatedItem
    {
        public string Title { get; set; }
        public int Amount { get; set; }
        public bool InInventory { get; set; }
    }
}