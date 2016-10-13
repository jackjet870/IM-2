using IM.BusinessRules.BR;
using IM.Model;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace IM.Base.Classes
{
  public class SalesmenChangesRules
  {
    #region GetSalesmenChanges
    /// <summary>
    /// Verifica si hubo algun cambio en el combobox, si hubo cambio agrega un salesmenchaged a la lista
    /// </summary>
    /// <param name="entity">Objeto con los datos anteriores</param>
    /// <param name="comboBox">Combobox a validar</param>
    /// <param name="position">posicion del salesmen (Liner1,Closer3,etc..)</param>
    /// <param name="lstSalesmenChanges">Lista para guardar los cambios que se hayan generado</param>
    /// <param name="role">Role del personnel seleccionado en el combobox/param>
    /// <history>
    /// [emoguel] 10/11/2016 created
    /// </history>
    public static void GetSalesmenChanges(object entity, ComboBox comboBox, byte position, List<SalesmenChanges> lstSalesmenChanges, string role)
    {
      var propertyName = comboBox.GetBindingExpression(ComboBox.SelectedValueProperty).ResolvedSourcePropertyName;
      var propertyOld = entity.GetType().GetProperty(propertyName).GetValue(entity)?.ToString() ?? string.Empty;//Valor anterior
      var propertyNew = comboBox.SelectedValue?.ToString() ?? string.Empty;//Valor nuevo

      if (propertyOld != propertyNew && !propertyOld.Equals(propertyNew))//verificamos que tenga algun cambio
      {
        var lstNames = comboBox.ItemsSource.OfType<object>().ToList();//Obtenemos la lista de personnel del combobox
        var objOldName = lstNames.FirstOrDefault(p => (p.GetType().GetProperty("peID").GetValue(p)?.ToString() ?? string.Empty) == propertyOld);//Buscamos si está el nombre en la lista
        var objNewName = lstNames.FirstOrDefault(p => (p.GetType().GetProperty("peID").GetValue(p)?.ToString() ?? string.Empty) == propertyNew);//Buscamos si está el nombre en la lista
        string oldName = objOldName?.GetType().GetProperty("peN").GetValue(objOldName)?.ToString() ?? string.Empty;//Verificamos si tiene el nombre
        string newName = objNewName?.GetType().GetProperty("peN").GetValue(objNewName)?.ToString() ?? string.Empty;//Verificamos si tiene el nombre
        //Agregamos el salesmenChanges
        lstSalesmenChanges.Add(new SalesmenChanges
        {
          schNewSalesman = propertyNew,
          schOldSalesman = propertyOld,
          schPosition = position,
          roN = role,
          OldSalesmanN = (string.IsNullOrWhiteSpace(propertyOld)) ? string.Empty : (string.IsNullOrWhiteSpace(oldName)) ? string.Empty : BRPersonnel.GetPersonnelById(propertyOld).peN,
          NewSalesmanN = (string.IsNullOrWhiteSpace(propertyNew)) ? string.Empty : (string.IsNullOrWhiteSpace(newName)) ? string.Empty : BRPersonnel.GetPersonnelById(propertyNew).peN
        });
      }

    }
    #endregion
  }
}
