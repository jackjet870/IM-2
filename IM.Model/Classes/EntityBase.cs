using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace IM.Model.Classes
{
  public class EntityBase: INotifyPropertyChanged
  {
    #region Implementacion INotifyPropertyChange
    public event PropertyChangedEventHandler PropertyChanged;
    
    #region OnPropertyChanged
    /// <summary>
    /// Invoka al evento PropertyChanged (Notifica cambio en la propiedad)
    /// </summary>
    /// <param name="propertyName">Nombre de la propiedad</param>
    /// <history>
    /// [erosado] 17/08/2016  Created.
    /// </history>
    private void OnPropertyChanged(string propertyName)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion 

    #region SetField
    /// <summary>
    /// Sirve para setear valores a una propiedad, implementa INotifyPropertyChanged
    /// Si el nuevo valor es diferente del que ya tenia asignado Se lo asigna.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="field">ref _atributo o propiedad</param>
    /// <param name="value">value</param>
    /// <param name="propertyName">Nombre de la propiedad</param>
    /// <history>
    /// [erosado] 17/08/2016  Created.
    /// </history>
    public void SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
      if (EqualityComparer<T>.Default.Equals(field, value)) return;
      field = value;
      OnPropertyChanged(propertyName);
    }
    #endregion

    #endregion
  }
}
