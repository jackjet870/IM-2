using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IM.Base.Helpers
{
  public class IniFileHelper
  {
    private string sBuffer; // Para usarla en las funciones GetSection(s)
    private string mstrPath;

    //--- Declaraciones para leer ficheros INI ---
    // Leer todas las secciones de un fichero INI, esto seguramente no funciona en Win95
    // Esta función no estaba en las declaraciones del API que se incluye con el VB
    [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
    public static extern int GetPrivateProfileSectionNames(
        string lpszReturnBuffer,  // address of return buffer
        int nSize,             // size of return buffer
        string lpFileName         // address of initialization filename
    );

    // Leer una sección completa
    [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
    public static extern int GetPrivateProfileSection(
        string lpAppName,         // address of section name
        string lpReturnedString,  // address of return buffer
        int nSize,             // size of return buffer
        string lpFileName         // address of initialization filename
    );

    // Leer una clave de un fichero INI
    [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
    public static extern int GetPrivateProfileString(
        string lpAppName,        // points to section name
        string lpKeyName,        // points to key name
        string lpDefault,        // points to default string
        string lpReturnedString, // points to destination buffer
        int nSize,            // size of destination buffer
        string lpFileName        // points to initialization filename
    );

    [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
    public static extern int GetPrivateProfileString(
        string lpAppName,        // points to section name
        int lpKeyName,        // points to key name
        string lpDefault,        // points to default string
        string lpReturnedString, // points to destination buffer
        int nSize,            // size of destination buffer
        string lpFileName        // points to initialization filename
        );

    // Escribir una clave de un fichero INI (también para borrar claves y secciones)
    [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
    public static extern int WritePrivateProfileString(
        string lpAppName,  // pointer to section name
        string lpKeyName,  // pointer to key name
        string lpString,   // pointer to string to add
        string lpFileName  // pointer to initialization filename
    );

    [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
    public static extern int WritePrivateProfileString(
        string lpAppName,  // pointer to section name
        string lpKeyName,  // pointer to key name
        int lpString,   // pointer to string to add
        string lpFileName  // pointer to initialization filename
        );

    [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
    public static extern int WritePrivateProfileString(
        string lpAppName,  // pointer to section name
        int lpKeyName,  // pointer to key name
        int lpString,   // pointer to string to add
        string lpFileName  // pointer to initialization filename
        );

    public IniFileHelper(string path)
    {
      mstrPath = path;
    }

    public void IniDeleteKey(string sIniFile, string sSection, string sKey)
    {
      //--------------------------------------------------------------------------
      // Borrar una clave o entrada de un fichero INI
      // Si no se indica sKey, se borrará la sección indicada en sSection
      // En otro caso, se supone que es la entrada (clave) lo que se quiere borrar
      //
      // Para borrar una sección se debería usar IniDeleteSection
      //
      if (sKey == "")
      {
        // Borrar una sección
        WritePrivateProfileString(sSection, 0, 0, sIniFile);
      }
      else
      {
        // Borrar una entrada
        WritePrivateProfileString(sSection, sKey, 0, sIniFile);
      }
    }

    public void IniDeleteSection(string sIniFile, string sSection)
    {
      //--------------------------------------------------------------------------
      // Borrar una sección de un fichero INI
      // Borrar una sección
      WritePrivateProfileString(sSection, 0, 0, sIniFile);
    }

    public string[] IniGetSection(string sFileName, string sSection)
    {
      //--------------------------------------------------------------------------
      // Lee una sección entera de un fichero INI
      // Adaptada para devolver un array de string
      //
      // Esta función devolverá un array de índice cero
      // con las claves y valores de la sección
      //
      // Parámetros de entrada:
      //   sFileName   Nombre del fichero INI
      //   sSection    Nombre de la sección a leer
      // Devuelve:
      //   Un array con el nombre de la clave y el valor
      //   Para leer los datos:
      //       For i = 0 To UBound(elArray) -1 Step 2
      //           sClave = elArray(i)
      //           sValor = elArray(i+1)
      //       Next
      //
      string[] aSeccion;
      int n;
      //
      aSeccion = new string[0];
      //
      // El tamaño máximo para Windows 95
      sBuffer = new string('\0', 32767);
      //
      n = GetPrivateProfileSection(sSection, sBuffer, sBuffer.Length, sFileName);
      //
      if (n > 0)
      {
        // Cortar la cadena al número de caracteres devueltos
        // menos los dos últimos que indican el final de la cadena
        sBuffer = sBuffer.Substring(0, n - 2).TrimEnd();
        //
        // Cada una de las entradas estará separada por un Chr$(0)
        // y cada valor estará en la forma: clave = valor
        aSeccion = sBuffer.Split(new char[] { '\0', '=' });
      }
      // Devolver el array
      return aSeccion;
    }

    public string[] IniGetSections(string sFileName)
    {
      //--------------------------------------------------------------------------
      // Devuelve todas las secciones de un fichero INI
      // Adaptada para devolver un array de string
      //
      // Esta función devolverá un array con todas las secciones del fichero
      //
      // Parámetros de entrada:
      //   sFileName   Nombre del fichero INI
      // Devuelve:
      //   Un array con todos los nombres de las secciones
      //   La primera sección estará en el elemento 1,
      //   por tanto, si el array contiene cero elementos es que no hay secciones
      //
      int n;
      string[] aSections;
      //
      aSections = new string[0];
      //
      // El tamaño máximo para Windows 95
      sBuffer = new string('\0', 32767);
      //
      // Esta función del API no está definida en el fichero TXT
      n = GetPrivateProfileSectionNames(sBuffer, sBuffer.Length, sFileName);
      //
      if (n > 0)
      {
        // Cortar la cadena al número de caracteres devueltos
        // menos los dos últimos que indican el final de la cadena
        sBuffer = sBuffer.Substring(0, n - 2).TrimEnd();
        aSections = sBuffer.Split('\0');
      }
      // Devolver el array
      return aSections;
    }

    #region Get/Set StringValue

    public string readText(string Section, string Key, string Default)
    {
      int ret;
      string sRetVal;
      //
      sRetVal = new string(' ', 255);
      //
      ret = GetPrivateProfileString(Section, Key, Default, sRetVal, sRetVal.Length, mstrPath);
      if (ret == 0)
      {
        return Default;
      }
      else
      {
        return sRetVal.Substring(0, ret);
      }
    }

    public void writeText(string Section, string Key, string Value)
    {
      WritePrivateProfileString(Section, Key, Value, mstrPath);
    }

    #endregion Get/Set StringValue

    #region Get/Set DateValue

    /// <summary>
    /// Lee una fecha de una clave de una seccion de un archivo de configuracion
    /// </summary>
    /// <param name="Section">Section</param>
    /// <param name="Key">Key</param>
    /// <param name="Default">Valor a retornar si la Key es nulo en el archivo</param>
    /// <returns>DateTime</returns>
    /// <history>
    /// [aalcocer]  10/03/2016 Modified. Se agrega el tipo de formato para obtener del texto a la fecha
    /// [aalcocer]  11/03/2016 Modified. Se asigna la fecha Default cuando el texto de la fecha no es valida
    /// </history>
    public DateTime readDate(string Section, string Key, DateTime Default)
    {
      DateTime Value = new DateTime();
      string Text = "";

      Text = readText(Section, Key, "");
      if (string.IsNullOrWhiteSpace(Text) || !DateTime.TryParse(Text, out Value))
        Value = Default;

      return Value;
    }

    public void writeDate(string Section, string Key, DateTime Value)
    {
      WritePrivateProfileString(Section, Key, Value.ToString("dd mmm yyyy hh:mm:ss"), mstrPath);
    }

    #endregion Get/Set DateValue

    #region Get/Set IntValue

    public int readInteger(string Section, string Key, int Default)
    {
      int Value;
      string Text = "";

      Text = readText(Section, Key, "");
      if (Text == "")
        Value = Default;
      else
        Value = Convert.ToInt32(Text);

      return Value;
    }

    public void writeInteger(string Section, string Key, long Value)
    {
      WritePrivateProfileString(Section, Key, Value.ToString(), mstrPath);
    }

    #endregion Get/Set IntValue

    #region Get/Set DoubleValue

    public double readDouble(string Section, string Key, double Default)
    {
      double Value;
      string Text = "";

      Text = readText(Section, Key, "");
      if (Text == "")
        Value = Default;
      else
        Value = Convert.ToDouble(Text);

      return Value;
    }

    public void writeInteger(string Section, string Key, double Value)
    {
      WritePrivateProfileString(Section, Key, Value.ToString(), mstrPath);
    }

    #endregion Get/Set DoubleValue

    #region Get/Set BooleanValue

    public bool readBool(string Section, string Key, bool Default)
    {
      bool Value;
      string Text = "";

      Text = readText(Section, Key, "");
      if (Text == "")
        Value = Default;
      else
        Value = Convert.ToBoolean(Text);

      return Value;
    }

    public void writeInteger(string Section, string Key, bool Value)
    {
      WritePrivateProfileString(Section, Key, Value.ToString(), mstrPath);
    }

    #endregion Get/Set BooleanValue
  }
}