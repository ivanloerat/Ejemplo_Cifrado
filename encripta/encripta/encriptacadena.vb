Imports System.IO
Imports System.Text
Imports System.Security.Cryptography

Public Class encriptacadena
    Private Shared DES As New Security.Cryptography.TripleDESCryptoServiceProvider
    Private Shared MD5 As New Security.Cryptography.MD5CryptoServiceProvider

    Public Function Encriptar(ByVal cadena As String) As String
        Try
            Return FnEncripta(cadena)
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Public Function Desencripta(ByVal cadena As String) As String
        Try
            Return FnDesencripta(cadena)
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Public Shared Function MD5Hash(ByVal value As String) As Byte()
        Return MD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(value))
    End Function

    Public Shared Function FnEncripta(ByVal stringToEncrypt As String) As String
        Dim key As String = "LLAVE DE CIFRADO"
        DES.Key = MD5Hash(key)
        DES.Mode = Security.Cryptography.CipherMode.ECB
        Dim Buffer As Byte() = ASCIIEncoding.ASCII.GetBytes(stringToEncrypt)
        Return Convert.ToBase64String(DES.CreateEncryptor().TransformFinalBlock(Buffer, 0, Buffer.Length))
    End Function

    Public Shared Function FnDesencripta(ByVal encryptedString As String) As String
        Dim key As String = "LLAVE DE CIFRADO"
        DES.Key = MD5Hash(key)
        DES.Mode = Security.Cryptography.CipherMode.ECB
        Dim Buffer As Byte() = Convert.FromBase64String(encryptedString)
        Return ASCIIEncoding.ASCII.GetString(DES.CreateDecryptor().TransformFinalBlock(Buffer, 0, Buffer.Length))
    End Function

End Class
