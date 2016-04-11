using IM.Model.Enums;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace IM.Launcher.Classes
{
  public class ConfigButton
  {

    #region CreateButtons
    /// <summary>
    /// Crea cada uno de los botones con sus características
    /// </summary>
    /// <returns>Lista de Botones</returns>
    public IEnumerable<Button> CreateButtons()
    {
      var lstButtons = new List<Button>();
      Button button = null;

      foreach (var item in ListOfButtons())
      {
        button = new Button();
        button.Name = "btn" + item.Value.Trim().Replace(" ", "");
        button.Tag = (EnumMenu)item.Key;
        button.TabIndex = item.Key;

        button.Style = StyleButton();
        button.Content = ConfigContentButton(item.Value, (EnumMenu)item.Key);

        lstButtons.Add(button);
      }

      return lstButtons;
    }
    #endregion

    #region ListOfButtons
    /// <summary>
    /// Contiene la lista de opciones con las que contará el menú
    /// </summary>
    /// <history>
    /// [wtorres]  11/Abr/2016 Modified. Ahora genera la lista en base al enumerado EnumMenu
    /// </history>
    public IDictionary<int, string> ListOfButtons()
    {
      var lstMenus = new Dictionary<int, string>();

      lstMenus.Add((int)EnumMenu.Inhouse, EnumToListHelper.GetEnumDescription(EnumMenu.Inhouse)); //el key es el orden en el que aparecerá en el menú cada botón

      lstMenus.Add((int)EnumMenu.Assignment, EnumToListHelper.GetEnumDescription(EnumMenu.Assignment));
      lstMenus.Add((int)EnumMenu.MailOuts, EnumToListHelper.GetEnumDescription(EnumMenu.MailOuts));
      lstMenus.Add((int)EnumMenu.Outhouse, EnumToListHelper.GetEnumDescription(EnumMenu.Outhouse));
      lstMenus.Add((int)EnumMenu.Host, EnumToListHelper.GetEnumDescription(EnumMenu.Host));
      lstMenus.Add((int)EnumMenu.InventoryMovs, EnumToListHelper.GetEnumDescription(EnumMenu.InventoryMovs));
      lstMenus.Add((int)EnumMenu.ProcessorINH, EnumToListHelper.GetEnumDescription(EnumMenu.ProcessorINH));
      lstMenus.Add((int)EnumMenu.ProcessorOUT, EnumToListHelper.GetEnumDescription(EnumMenu.ProcessorOUT));
      lstMenus.Add((int)EnumMenu.ProcessorSales, EnumToListHelper.GetEnumDescription(EnumMenu.ProcessorSales));
      lstMenus.Add((int)EnumMenu.PRStatistics, EnumToListHelper.GetEnumDescription(EnumMenu.PRStatistics));
      lstMenus.Add((int)EnumMenu.Graph, EnumToListHelper.GetEnumDescription(EnumMenu.Graph));
      lstMenus.Add((int)EnumMenu.GuestsByPR, EnumToListHelper.GetEnumDescription(EnumMenu.GuestsByPR));
      lstMenus.Add((int)EnumMenu.SalesByPR, EnumToListHelper.GetEnumDescription(EnumMenu.SalesByPR));
      lstMenus.Add((int)EnumMenu.SalesByLiner, EnumToListHelper.GetEnumDescription(EnumMenu.SalesByLiner));
      lstMenus.Add((int)EnumMenu.SalesByCloser, EnumToListHelper.GetEnumDescription(EnumMenu.SalesByCloser));
      lstMenus.Add((int)EnumMenu.Administrator, EnumToListHelper.GetEnumDescription(EnumMenu.Administrator));
      lstMenus.Add((int)EnumMenu.MailOutsConfig, EnumToListHelper.GetEnumDescription(EnumMenu.MailOutsConfig));
      lstMenus.Add((int)EnumMenu.InvitationsConfig, EnumToListHelper.GetEnumDescription(EnumMenu.InvitationsConfig));
      lstMenus.Add((int)EnumMenu.PrinterConfig, EnumToListHelper.GetEnumDescription(EnumMenu.PrinterConfig));

      return lstMenus;
    }
    #endregion

    #region ConfigContentButton
    /// <summary>
    /// Configura la imagen y el texto del botón
    /// </summary>
    /// <param name="description">Es el texto quse tendrá el botón</param>
    /// <returns>Regresa un StackPanel con la imagen y el texto de botón</returns>
    private StackPanel ConfigContentButton(string description, EnumMenu enumMenu)
    {

      StackPanel stackPnl = new StackPanel();
      stackPnl.Width = 135;

      string pathImages = Path.Combine(Directory.GetCurrentDirectory(), "images\\");
      string uri = String.Format("{0}{1}.png", pathImages, enumMenu.ToString());

      var img = new Image();
      img.Source = new BitmapImage(new Uri(uri));

      img.MaxWidth = 24;
      img.HorizontalAlignment = HorizontalAlignment.Left;
      TextBlock text = new TextBlock();
      text.HorizontalAlignment = HorizontalAlignment.Center;
      text.VerticalAlignment = VerticalAlignment.Center;
      text.Margin = new Thickness(4, -25, 0, 0);
      text.Text = description;

      stackPnl.Children.Add(img);
      stackPnl.Children.Add(text);
      return stackPnl;
    }
    #endregion

    #region StyleButton
    private Style StyleButton()
    {
      var style = new Style();
      style.TargetType = typeof(Button);

      //Estilo por default
      style.Setters.Add(new Setter(Control.BackgroundProperty, Brushes.SteelBlue));
      style.Setters.Add(new Setter(Control.ForegroundProperty, Brushes.AliceBlue));

      var trigger = new Trigger();
      trigger.Property = UIElement.IsMouseOverProperty;
      trigger.Value = true;
      trigger.Setters.Add(new Setter(Control.ForegroundProperty, Brushes.Black));

      style.Triggers.Add(trigger);

      return style;
    } 
    #endregion
  }
}
