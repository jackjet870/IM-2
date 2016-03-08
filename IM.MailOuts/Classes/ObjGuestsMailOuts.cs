using System.Drawing;
using System.Reflection;

namespace IM.MailOuts.Classes
{
  public class ObjGuestsMailOuts : Model.GuestMailOut
  {
    public Model.GuestMailOut Parent { get; set; }

    public ObjGuestsMailOuts(Model.GuestMailOut parent)
    {
      Parent = parent;

      foreach (PropertyInfo prop in parent.GetType().GetProperties())
        GetType().GetProperty(prop.Name).SetValue(this, prop.GetValue(parent, null), null);
    }

    public string toolTip
    {
      get
      {
        string _tooltip = "";
        switch (guStatus)
        {
          case 0:
            _tooltip = "Available no info";
            break;

          case 1:
            _tooltip = "Info, no Invit";
            break;

          case 2:
            _tooltip = "Invit Stand-by";
            break;

          case 3:
            _tooltip = "Invit, Show";
            break;

          case 4:
            _tooltip = "Invit, No Show";
            break;

          case 5:
            _tooltip = "Invit canceled";
            break;

          case 6:
            _tooltip = "Unavailable";
            break;

          case 7:
            _tooltip = "Check-Out";
            break;

          case 8:
            _tooltip = "No Check-In";
            break;
        }
        return _tooltip;
      }
    }

    public string color
    {
      get
      {
        Color _color = new Color();
        switch (guStatus)
        {
          case 0:
            _color = Color.FromArgb(0, 255, 128);      // Disponible sin Info;
            break;

          case 1:
            _color = Color.FromArgb(0, 200, 200); // Con Info, sin Invitacion
            break;

          case 2:
            _color = Color.FromArgb(255, 255, 128); // Con Invitacion, stand-by
            break;

          case 3:
            _color = Color.FromArgb(255, 184, 0); // Con Invitacion, show
            break;

          case 4:
            _color = Color.FromArgb(255, 128, 0); // Con Invitacion, No show
            break;

          case 5:
            _color = Color.FromArgb(220, 0, 0); // Con Invitacion, Cancelada
            break;

          case 6:
            _color = Color.FromArgb(160, 0, 255); // No Disponible
            break;

          case 7:
            _color = Color.FromArgb(0, 0, 160); // Check-Out
            break;

          case 8:
            _color = Color.FromArgb(128, 128, 128); // Sin Check-In
            break;
        }
        return "#" + _color.R.ToString("X2") + _color.G.ToString("X2") + _color.B.ToString("X2");
      }
    }
  }
}