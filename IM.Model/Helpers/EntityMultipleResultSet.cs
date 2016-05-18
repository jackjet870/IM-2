using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace IM.Model.Helpers
{
  /// <summary>
  /// Recupera todos los resultados(Selects)  de un store procedure.
  /// </summary>
  public static class EntityMultipleResultSets
  {
    /// <summary>
    /// Obtiene multiples resultados de un Store Procedure
    /// </summary>
    /// <param name="objResult">Resultado a procesar del store procedure</param>
    /// <returns>Entity</returns>
    /// <history>
    /// [erodriguez] 2/05/2016 Created
    /// </history>
    public static MultipleResultSetWrapper MultipleResults(this ObjectResult objResult)
    {
      return new MultipleResultSetWrapper(objResult);
    }

    public class MultipleResultSetWrapper
    {
      private ObjectResult _objResult;
      private readonly List<IEnumerable> _lstResut = new List<IEnumerable>();
      private bool _isFirstEntity = true;

      internal MultipleResultSetWrapper(ObjectResult objResult)
      {
        _objResult = objResult;
      }

      /// <summary>
      /// Obtiene el listado de un tipo objeto definido
      /// </summary>
      /// <typeparam name="TResult">Entidad obtener</typeparam>
      /// <returns>Entity</returns>
      /// <history>
      /// [erodriguez] 2/05/2016 Created
      /// [aalcocer]   17/05/2016 Modified. Se valida cuando el store procedure no retorna tablas
      /// </history>
      public MultipleResultSetWrapper With<TResult>()
      {
        ObjectResult resultObjAux = (_isFirstEntity) ? _objResult : _objResult?.GetNextResult<TResult>();
        _objResult = resultObjAux;
        List<TResult> lstResultAux = new List<TResult>();
        if (resultObjAux != null)
          lstResultAux.AddRange(resultObjAux.OfType<TResult>());
        _lstResut.Add(lstResultAux);
        if (_isFirstEntity)
          _isFirstEntity = !_isFirstEntity;

        return this;
      }

      /// <summary>
      /// Obtiene listado de Entidades del store procedure
      /// </summary>
      /// <returns><list type="IEnumerable"></list></returns>
      /// <history>
      /// [erodriguez] 2/05/2016 Created
      /// </history>
      public List<IEnumerable> GetValues()
      {
        return _lstResut;
      }
    }
  }
}