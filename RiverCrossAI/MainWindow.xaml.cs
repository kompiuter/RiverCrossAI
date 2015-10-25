using MahApps.Metro.Controls;
using RiverCrossAI.Codes;
using RiverCrossAI.Extensions;
using RiverCrossAI.ListReorderCodes;
using RiverCrossAI.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace RiverCrossAI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        ListViewDragDropManager<FuncWrapper> dragMgr;

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            dragMgr = new ListViewDragDropManager<FuncWrapper>(this.operatorsListView);

            dragMgr.DropComplete += DragMgr_DropComplete;
        }

        private void DragMgr_DropComplete(object sender, EventArgs e)
        {
            var operators = ((ObservableCollection<FuncWrapper>)operatorsListView.ItemsSource);

            // Update order of operators based on order in listview
            foreach (var item in operatorsListView.Items)
                operators.First(o => o == item).Order = operatorsListView.Items.IndexOf(item);
        }

    }
}
