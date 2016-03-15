#region using
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
using IM.Base.Enums;
using IM.BusinessRules.BR;
using IM.Model.Enums;
using IM.Model.Classes;
using IM.Base.Helpers;
#endregion 

namespace IM.Inhouse.Forms
{
    /// <summary>
    /// Formulario que asigna los dias libres al personal
    /// </summary>
    /// <history>
    /// [ECANUL]  08/03/2016 Created
    /// </history>
    public partial class frmDaysOff : Window
    {
        #region Atributos
        //Tipo de equipo
        private EnumTeamType teamType;
        //Clave del Lugar
        private string placeID;
        private CollectionViewSource cvsPersonnelDaysOff;
        private List<Model.PersonnelDayOff> personnelDaysOff = new List<Model.PersonnelDayOff>();
        #endregion

        #region metodos de funcion
        /// <summary>
        /// Carga el Grid con los datos de BD
        /// </summary>
        /// <history>[ECANUL] 09-03-2016 Create</history>
        void LoadGrid()
        {
            StaStart("Loading Personnel Days Off...");
            if(cvsPersonnelDaysOff!=null)
            {
                cvsPersonnelDaysOff.Source = BRPersonnelDayOff.GetPersonnelDaysOff(placeID,teamType );
            }
            StaEnd();
        }

        /// <summary>
        /// Habilita o deshabilita los controles del formulario
        /// </summary>
        /// <param name="stats">0-Deshabilita Edicion, 1-Habilita Edicion</param>
        void EnableControls (int stats)
        {
            if (stats == 0) //Modo Solo vista
            {
                grdCheks.IsEnabled = false;
                btnSave.IsEnabled = false;
                btnCalcel.IsEnabled = false;
                btnShow.IsEnabled = true;
                btnEdit.IsEnabled = true;
                dtgpersonnelDayOff.IsEnabled = true;


            }
            else //Modo Edicion
            {
                grdCheks.IsEnabled = true;
                btnSave.IsEnabled = true;
                btnCalcel.IsEnabled = true;
                btnShow.IsEnabled = false;
                btnEdit.IsEnabled = false;
                dtgpersonnelDayOff.IsEnabled = false;

            }
        }

        /// <summary>
        /// Limpia los controles del Formulario
        /// <history>[ECANUL] 09-03-2016 Created</history>
        /// </summary>
        void CleanControls()
        {
            txtDope.Text = "";
            txtpeN.Text = "";
            chkMonday.IsChecked = false;
            chkTuestday.IsChecked = false;
            chkWednesday.IsChecked = false;
            chkThursday.IsChecked = false;
            chkFriday.IsChecked = false;
            chkSaturday.IsChecked = false;
            chkSunday.IsChecked = false;
        }

        /// <summary>
        /// Llena el formulario con datos del Personal Seleccionado
        /// <history>[ECANUL] 09-03-2016 Created</history>
        /// </summary>
        void LoadPersonnelInfo()
        {
            StaStart("Load Personnel Info... ");
            CleanControls();
            List<Model.PersonnelDayOff> personnelDaysOff = dtgpersonnelDayOff.SelectedItems.OfType<Model.PersonnelDayOff>().ToList();
            txtDope.Text = personnelDaysOff[0].dope;
            txtpeN.Text = personnelDaysOff[0].peN;
            if (personnelDaysOff[0].doMonday == true)
                chkMonday.IsChecked = true;
            if (personnelDaysOff[0].doTuesday == true)
                chkTuestday.IsChecked = true;
            if (personnelDaysOff[0].doWednesday == true)
                chkWednesday.IsChecked = true;
            if (personnelDaysOff[0].doThursday == true)
                chkThursday.IsChecked = true;
            if (personnelDaysOff[0].doFriday == true)
                chkFriday.IsChecked = true;
            if (personnelDaysOff[0].doSaturday == true)
                chkSaturday.IsChecked = true;
            if (personnelDaysOff[0].doSunday == true)
                chkSunday.IsChecked = true;
            btnEdit.IsEnabled = true;
            StaEnd();
        }

        /// <summary>
        /// Revisa si un Chek esta activo e incrementa un contador
        /// </summary>
        /// <param name="numDays"> Variable para incrementar el contador</param>
        /// <param name="chk">CheckBox a revisar</param>
        /// <returns>Contador actualizado</returns>
        /// <history>[ECANUL] 10-03-2016 Created</history>
        int CountNumDays(int numDays, CheckBox chk)
        {
            if (chk.IsChecked == true)
                numDays = numDays + 1;
            return numDays;
        }

        /// <summary>
        /// Valida que los Chesk cumplan con las reglas antes de fuardar
        /// </summary>
        /// <returns></returns>
        bool CheksValidate()
        {
            int numDays = 0;
            //Contamos el numero de dias libres
            numDays = CountNumDays(numDays, chkMonday);
            numDays = CountNumDays(numDays, chkTuestday);
            numDays = CountNumDays(numDays, chkWednesday);
            numDays = CountNumDays(numDays, chkThursday);
            numDays = CountNumDays(numDays, chkFriday);
            numDays = CountNumDays(numDays, chkSaturday);
            numDays = CountNumDays(numDays, chkSunday);
            //Valida que al menos haya un dia libre
            if (numDays == 0)
            {
                MessageBox.Show("Specify at least one day off", "Specify a day", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            //Valida que no tenga mas de 2 dias libres
            if (numDays>2)
            {
                MessageBox.Show("The Personnel should not take more than 2 days off", "Maximum 2 days", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Indica en la barra de estado que se inicio un proceso
        /// </summary>
        /// <param name="message">mensaje a mostrar</param>
        /// <history>[ECANUL] 09-03-2016 Created </history>
        private void StaStart(String message)
        {
            lblStatusBarMessage.Content = message;
            imgStatusBarMessage.Visibility = Visibility.Visible;
            this.Cursor = Cursors.Wait;
        }

        /// <summary>
        /// Indica en la barra de estado que se finalizo un proceso
        /// </summary>
        /// <history>[ECANUL] 09-03-2016 Created </history>
        private void StaEnd()
        {
            lblStatusBarMessage.Content = null;
            imgStatusBarMessage.Visibility = Visibility.Hidden;
            this.Cursor = null;
        }

        #endregion

        #region metodos de Formulario
        /// <summary>
        /// Carga e Inicializa el Formulario
        /// </summary>
        /// <param name="_teamType">El Tipo de equipo de Trabajo PRs o Salesmen</param>
        public frmDaysOff(Model.Enums.EnumTeamType _teamType)
        {
            InitializeComponent();
            teamType = _teamType;
            if (teamType == EnumTeamType.TeamPRs)
                placeID = App.User.LeadSource.lsID;
            else
                placeID = App.User.SalesRoom.srID;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Capital)
            {
                KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
            }
            else if (e.Key == Key.Insert)
            {
                KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
            }
            else if (e.Key == Key.NumLock)
            {
                KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            KeyboardHelper.CkeckKeysPress(StatusBarCap, Key.Capital);
            KeyboardHelper.CkeckKeysPress(StatusBarIns, Key.Insert);
            KeyboardHelper.CkeckKeysPress(StatusBarNum, Key.NumLock);

            cvsPersonnelDaysOff = ((CollectionViewSource)(this.FindResource("personnelDayOffViewSource")));
            //Carga de grddaysOff
            LoadGrid();

        }

        private void btnShow_Click(object sender, RoutedEventArgs e)
        {
            LoadPersonnelInfo();
        }

        private void dtgpersonnelDayOff_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StatusBarReg.Content = string.Format("{0}/{1}", dtgpersonnelDayOff.Items.IndexOf(dtgpersonnelDayOff.CurrentItem) + 1, dtgpersonnelDayOff.Items.Count);
            btnEdit.IsEnabled = false;
            CleanControls();
        }

        private void dtgpersonnelDayOff_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            LoadPersonnelInfo();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            EnableControls(1);
        }

        private void btnCalcel_Click(object sender, RoutedEventArgs e)
        {
            EnableControls(0);
            CleanControls();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            int nRes = 0;
            if (CheksValidate())
            {
                StaStart("Guardando Datos...");
                Model.DayOff daysOff = new Model.DayOff();
                daysOff.dope = txtDope.Text;
                daysOff.doMonday = chkMonday.IsChecked.Value;
                daysOff.doTuesday = chkTuestday.IsChecked.Value;
                daysOff.doWednesday = chkWednesday.IsChecked.Value;
                daysOff.doThursday = chkThursday.IsChecked.Value;
                daysOff.doFriday = chkFriday.IsChecked.Value;
                daysOff.doSaturday = chkSaturday.IsChecked.Value;
                daysOff.doSunday = chkSunday.IsChecked.Value;
              
                nRes = BRPersonnelDayOff.SavePersonnelDayOff(daysOff);
                CleanControls();
                LoadGrid();
                EnableControls(0);
                StaEnd();
            }

        }
        #endregion

       
    }
}
