using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace IM.Launcher.Classes
{
    public class ConfigButton
    {

      /// <summary>
      /// Crea cada uno de los botones con sus características
      /// </summary>
      /// <returns>Lista de Botones</returns>
      public IEnumerable<Button> CreateButtons()
      {
          var lstButtons = new List<Button>();
          Button button = null;
          int x = 10; //los botones estarán alineados 10px en el eje X
          int y = 41; //los botones iniciarán con 41px en el eje Y

          foreach(var item in ListOfButtons())
          {
            button = new Button();
            button.Name = "btn" + item.Value.Trim().Replace(" ", "");
            button.Content = ConfigContentButton(item.Value, (EnumMenu) item.Key);
            button.HorizontalAlignment = HorizontalAlignment.Left;
            button.VerticalAlignment = VerticalAlignment.Top;
            button.Margin = new Thickness(x, y, 0, 0);
            button.Width = 145;
            button.Height = 30;
            //button.Foreground = Brushes.AliceBlue;
            //button.Background = Brushes.SteelBlue;
            button.Tag = (EnumMenu)item.Key;
            button.TabIndex = item.Key;

            button.Style = StyleButton();    

            y += 35; // la variable y aumenta 36 pixeles para comodar el botón abajo del anterior
            lstButtons.Add(button);
          }

          return lstButtons;
      }

      /// <summary>
      /// Contiene la lista de opciones con las que contará el menú
      /// </summary>
      /// <returns>Lista de strings</returns>
      private IDictionary<int,string> ListOfButtons()
      {
          var lstMenus = new Dictionary<int, string>();
          int i = 0;
                        
          lstMenus.Add(++i,"Inhouse"); //el key es el orden en el que aparecerá en el menú cada botón

          lstMenus.Add(++i,"Assignment");

          lstMenus.Add(++i,"Mail Outs");

          lstMenus.Add(++i,"Animation");

          lstMenus.Add(++i,"Regen");

          lstMenus.Add(++i,"Outhouse");

          lstMenus.Add(++i,"Host");

          lstMenus.Add(++i,"Inventory Movs");

          lstMenus.Add(++i,"Processor INH");

          lstMenus.Add(++i,"Processor OUT");

          lstMenus.Add(++i,"Processor GRAL");

          lstMenus.Add(++i,"Processor Sales");

          lstMenus.Add(++i,"PR Statistics");

          lstMenus.Add(++i,"Graph");

          lstMenus.Add(++i,"Guests by PR");

          lstMenus.Add(++i,"Sales by PR");

          lstMenus.Add(++i,"Sales by Liner");

          lstMenus.Add(++i,"Sales by Closer");

          lstMenus.Add(++i,"Administrator");

          lstMenus.Add(++i,"Mail Outs Config");

          lstMenus.Add(++i,"Invitations Config");

          lstMenus.Add(++i,"Printer Config");


          return lstMenus;
      }

       
      /// <summary>
      /// Configura la imagen y el texto del botón
      /// </summary>
      /// <param name="description">Es el texto quse tendrá el botón</param>
      /// <returns>Regresa un StackPanel con la imagen y el texto de botón</returns>
      private StackPanel ConfigContentButton(string description, EnumMenu enumMenu)
        {
          string pathImages = Path.Combine(Directory.GetCurrentDirectory(), "images\\");
          string uri = String.Format("{0}{1}.png", pathImages, enumMenu.ToString());

          var img = new Image();
          img.Source = new BitmapImage(new Uri(uri));
            
          img.Height = 24;
          img.Width = 24;
          img.HorizontalAlignment = HorizontalAlignment.Left;
            
          TextBlock text = new TextBlock();
          text.HorizontalAlignment = HorizontalAlignment.Center;
          text.Margin = new Thickness(4,-18,0,0);
          text.Text = description;
            
          StackPanel stackPnl = new StackPanel();
          stackPnl.Width = 135;
          stackPnl.Height= 30;

          stackPnl.Children.Add(img);
          stackPnl.Children.Add(text);
          return stackPnl;
        }

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
    }
}
