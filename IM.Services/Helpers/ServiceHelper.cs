using System;
using System.Collections;
using PalaceResorts.Common.PalaceTools;
using PalaceResorts.Common.Security;


namespace IM.Services.Helpers
{
  public class ServiceHelper
  {
    #region GetPrincipal

    /// <summary>
    /// Obtiene el principal para consumir la capa de seguridad de Web Palace
    /// </summary>
    /// <returns>Objeto PalacePrincipal </returns>
    /// <history>
    /// [ecanul] 05/Abr/2016 Created
    /// </history>
    public static PalacePrincipal GetPrincipal()
    {
      //Forzar la configuracion de la capa de seguridad
      System.Configuration.ConfigurationManager.GetSection(PalaceSecurityDefinitionSettings.SECTIONNAME);

      //Recupera el id de la aplicacion
      string str = ConfigHelper.GetString("Security.ApplicationId");
      int appId = Convert.ToInt32(str);
      //Obtiene el login y la contraseña del usuario
      string login = ConfigHelper.GetString("Security.Login");
      string password = ConfigHelper.GetString("Security.Password");

      //Obtener el pasaporte de estas credenciales
      PalacePrincipal principal = new PalacePrincipal(login, password, appId);

      return principal;

    }

    #endregion

    #region GetSecurityTokenHeaders

    /// <summary>
    /// Se recupera una cabecera SOAP con el token de autenticacion para el usuario en sesion
    /// </summary>
    /// <returns>Objeto con la cabecera SOAP</returns>
    /// 
    /// <history>
    /// [ecanul] 06/Abr/2016 Created
    /// </history>
    public static object[] GetSecurityTokenHeaders()
    {
      // recuperamos el Principal del usuario en sesion
      PalacePrincipal principal = GetPrincipal();
      if (principal == null)
        return null;

      // aqui depositaremos las cabeceras que agregaremos
      ArrayList headerList = new ArrayList();

      try //Se usa el try finally para poder reiniciar las variables principal y HeaderList a null
      {
        // aqui depositaremos el token de autenticacion
        object Token = null;

        // si se encuentra habilitada la encriptacion de tokens,
        if (Globales.gEnableEncryptTokens && Globales.gEnableEncryption)
        {
          // creamos un paquete nuevo
          PalacePassportAuthenticationTokenPackage package = new PalacePassportAuthenticationTokenPackage();

          // codificamos las credenciales del usuario en sesion
          package.Encrypt(principal.Login, principal.Password);

          // obtenemos el token serializado
          Token = package.ToArray();
          package = null;
        }
        // de otra forma se envia un token serializado
        else
        {
          // creamos un token de autenticacion
          PalacePassportAuthenticationToken authToken = new PalacePassportAuthenticationToken(principal.Login, principal.Password);

          // y depositamos el objeto serializado en el contenedor del token
          Token = authToken.ToArray();
          authToken = null;
        }

        // agregamos las cabeceras a la solicitud recien creada

        // si se encuentra habilitado el mecanismo de encriptacion,
        // agregamos una cabecera que especifique que sera necesario desencriptar la informacion
        object[] encryptionHeader = new object[2];
        encryptionHeader[0] = AutenticationTokenMessageKey.EncryptedToken;
        encryptionHeader[1] = Globales.gEnableEncryption && Globales.gEnableEncryptTokens;

        // agregamos la cabecera a la lista principal
        headerList.Add(encryptionHeader);
        encryptionHeader = null;

        // agregamos el token de autenticacion
        object[] header = new object[2];
        header[0] = AutenticationTokenMessageKey.AuthenticationToken;
        header[1] = Token;

        // agregamos la cabecera a la lista principal
        headerList.Add(header);
        header = null;

        //Se ontiene el resultado 
        return headerList.ToArray();
      }
      finally
      {
        principal = null;
        headerList = null;
      }
    }

    #endregion
  }
}
