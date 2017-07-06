Imports System.IO
Imports System.Text


Module mdlUtileria
    Public info As System.IO.FileInfo

    '-----------------------------------------------------------
    'Funcion para obtener la version de un archivo

    Function obtener_version(ByVal directorio As String) As Object
        'Si hay un error mandar un mensaje con descripción del error
        On Error GoTo MsgError
        Dim ObSiAr As Object
        'Se crea el objeto de sistema de archivos
        ObSiAr = CreateObject("scripting.FileSystemObject")
        obtener_version = ObSiAr.GetFileVersion(directorio)
        Exit Function
MsgError:
        MsgBox(Err.Description, vbCritical)
    End Function
    '-----------------------------------------------------------

   
End Module
