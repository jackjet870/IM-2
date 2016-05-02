using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Model.Helpers
{
  /// <summary>
  /// Recupera todos los resultados(Selects)  de un store procedure.
  /// </summary>
  public static class EntityMultipleResultSets
  {    
    public static MultipleResultSetWrapper MultipleResults(this ObjectResult objResult)
    {
      return new MultipleResultSetWrapper(objResult);
    }

    public class MultipleResultSetWrapper
    {
      private ObjectResult _objResult;
      private List<IEnumerable> lstResut = new List<IEnumerable>();
      bool isFirstEntity = true;
      public MultipleResultSetWrapper(ObjectResult objResult)
      {
        _objResult = objResult;
      }

      public MultipleResultSetWrapper With<TResult>()
      {
        ObjectResult resultObjAux = (isFirstEntity) ? _objResult : _objResult.GetNextResult<TResult>();
        _objResult = resultObjAux;
        lstResut.Add(resultObjAux.OfType<TResult>().ToList());
        if (isFirstEntity)
          isFirstEntity = !isFirstEntity;   
            
        return this;
      }

      public List<IEnumerable> GetValues()
      {
        return lstResut;
      }
    }
  }
}
