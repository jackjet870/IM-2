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
using System.Windows.Shapes;
using IM.Model;

namespace IM.Host.Forms
{
    /// <summary>
    /// Interaction logic for frmCloseSalesRoom.xaml
    /// </summary>
    public partial class frmCloseSalesRoom : Window
    {
        private Window mParentWindow = null;

        public frmCloseSalesRoom(Window pParent)
        {
            InitializeComponent();
            mParentWindow = pParent;
        }

        private void btnCloseShows_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnCloseMealTickets_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCloseSales_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCloseGiftsReceipts_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            mParentWindow.Effect = null;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource dsSalesRoom = ((System.Windows.Data.CollectionViewSource)(this.FindResource("dsSalesRoom")));
            // Load data by setting the CollectionViewSource.Source property:
            // dsSalesRoom.Source = [generic data source]

            using (IMEntities dbContext = new IMEntities())
            {
                var result = dbContext.USP_OR_GetSalesRoom("MPS");

                dsSalesRoom.Source = result;
            }

        }
    }
}
