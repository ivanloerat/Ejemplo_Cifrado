Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Configuration
Imports encripta.encriptacadena
Imports encripta.encriptacion

Module Extractor

    'Programa que extrae la información de los archivos 
    Dim lectura As String = "Select * from RutasporAplicacion"

#Region "Variables de configuracion"
    Dim flEnc As New encripta.encriptacion
    Dim stEnc As New encripta.encriptacadena
    Dim id As Integer
    Dim estatus As Integer
    Dim ruta As String
#End Region
#Region "Variables SQL"
    Dim servidor As String
    Dim cadena As String
    Dim consulta As String
    Dim conexionstng As String
    Dim da As SqlDataAdapter
    Dim dt As New DataTable
    Dim conexion As SqlConnection
    Dim conexion2 As SqlConnection
    Dim comando As SqlCommand
    Dim comandolectura As SqlCommand
    Dim lector As SqlDataReader
#End Region
#Region "Variables para servicios"
    Dim servidorservicio As String
    Dim idServidorServicio As Integer
    Dim validador As Integer
    Dim rutaServicio As String



#End Region

    Sub Main(ByVal parametro() As String)
        'Solamente se pasa como parametro la ip\instancia del servidor de BD donde desea guardar la información
        'El programa obtiene la informacion de los archivos de las rutas que se tienen en la base de datos que se manda como parametro

        cadena = stEnc.Desencripta(System.Configuration.ConfigurationSettings.AppSettings("CST"))
        consulta = stEnc.Desencripta(System.Configuration.ConfigurationSettings.AppSettings("QRY"))
        conexionstng = Replace(cadena, "#PARAMETRO#", parametro(0))

        Try
            conexion = New SqlConnection(conexionstng)
            comandolectura = New SqlCommand
            comandolectura.CommandText = lectura
            comandolectura.CommandType = CommandType.Text
            comandolectura.Connection = conexion
            da = New SqlDataAdapter(comandolectura)
            da.Fill(dt)
            SqlConnection.ClearPool(conexion)

            conexion = New SqlConnection(conexionstng)
            comando = New SqlCommand(consulta, conexion)
            comando.Parameters.Add(New SqlParameter("@idservidor", SqlDbType.VarChar))
            comando.Parameters.Add(New SqlParameter("@id", SqlDbType.Int))
            comando.Parameters.Add(New SqlParameter("@NombreArchivo", SqlDbType.VarChar))
            comando.Parameters.Add(New SqlParameter("@VersionEnsamblado", SqlDbType.VarChar))
            comando.Parameters.Add(New SqlParameter("@tamanio", SqlDbType.VarChar))
            comando.Parameters.Add(New SqlParameter("@FechaCompilacion", SqlDbType.VarChar))
            comando.Parameters.Add(New SqlParameter("@ruta", SqlDbType.VarChar))
            comando.Parameters.Add(New SqlParameter("@hArcHash", SqlDbType.VarChar))

            'estatus = conexion.State
            'lector = comandolectura.ExecuteReader



            For Each Drw As DataRow In dt.Rows

                servidor = Drw(0)
                comando.Parameters("@idservidor").Value = servidor
                id = Drw(1)
                comando.Parameters("@id").Value = id
                ruta = Drw(2)

                If validador = 0 Then

                    servidorservicio = servidor
                    rutaServicio = ruta
                    idServidorServicio = id

                End If

                'While lector.Read()
                '    servidor = lector("idServidor")
                '    comando.Parameters("@idservidor").Value = servidor
                '    id = lector("ClaveAplicacion")
                '    comando.Parameters("@id").Value = id
                '    ruta = lector("Ruta")


                If My.Computer.FileSystem.DirectoryExists(ruta) Then
                    For Each foundFile As String In My.Computer.FileSystem.GetFiles(ruta)
                        info = My.Computer.FileSystem.GetFileInfo(foundFile)

                        If info.Extension <> ".jpg" And info.Extension <> ".png" And info.Extension <> ".gif" And info.Extension <> ".jpeg" And info.Extension <> ".JPG" And info.Extension <> ".PNG" And info.Extension <> ".GIF" And info.Extension <> ".JPEG" Then
                            'Nombre del archivo
                            Dim archivo As String
                            archivo = info.Name
                            comando.Parameters("@NombreArchivo").Value = archivo

                            'Tamaño del archivo
                            comando.Parameters("@tamanio").Value = info.Length

                            'Version del archivo
                            Dim rutaxarchivo As String
                            rutaxarchivo = info.FullName
                            comando.Parameters("@VersionEnsamblado").Value = obtener_version(rutaxarchivo)

                            'Fecha de compilación
                            comando.Parameters("@FechaCompilacion").Value = info.LastWriteTime

                            'Ruta del archivo
                            comando.Parameters("@ruta").Value = info.DirectoryName

                            'HASH del archivo
                            If info.Extension <> ".config" And info.Extension <> ".Config" And info.Extension <> ".INI" And info.Extension <> ".Ini" And info.Extension <> ".ini" Then
                                Dim hash As String
                                hash = info.FullName
                                comando.Parameters("@hArcHash").Value = flEnc.GetSha(hash)
                            Else
                                ' Console.WriteLine("no se extrajo hash del archivo: " & info.Name)
                                comando.Parameters("@hArcHash").Value = "0"
                            End If


                            conexion.Open()
                            comando.ExecuteNonQuery()
                            conexion.Close()
                        End If



                    Next
                Else

                    conexion.Open()
                    Dim errorlog As SqlCommand
                    Dim log As String = "insert tblog_lerrors values('La ruta no existe: " & ruta & "')"
                    errorlog = New SqlCommand(log, conexion)
                    errorlog.ExecuteNonQuery()
                    conexion.Close()

                End If
            Next



            'End While
            conexion.Close()
            Console.WriteLine("Archivos guardados correctamente")

        Catch ex As Exception
            conexion.Open()
            Dim errorlog As SqlCommand
            Dim log As String = "insert tblog_lerrors values('Error al extraer o almacenar archivos: " & vbCrLf & ex.Message & "')"
            errorlog = New SqlCommand(log, conexion)
            errorlog.ExecuteNonQuery()
            conexion.Close()

        End Try
        SqlConnection.ClearAllPools()
        End
    End Sub

End Module
