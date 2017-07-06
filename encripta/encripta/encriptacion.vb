
Imports System.Security.Cryptography
Imports System.IO
Imports System.Text

Public Class encriptacion



    Function GetSha(ByVal filePath As String)
        Dim sha As SHA1CryptoServiceProvider = New SHA1CryptoServiceProvider
        Dim f As FileStream = New FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 8192)
        sha.ComputeHash(f)
        f.Close()
        Dim hash As Byte() = sha.Hash
        Dim buff As StringBuilder = New StringBuilder
        Dim hashbyte As Byte
        For Each hashbyte In hash
            buff.Append(String.Format("{0:X2}", hashbyte))
        Next
        Dim shastring As String
        shastring = buff.ToString()
        Return shastring

    End Function


    Function GetMD5(ByVal filePath As String)

        Dim md5 As MD5CryptoServiceProvider = New MD5CryptoServiceProvider
        Dim f As FileStream = New FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 8192)
        md5.ComputeHash(f)
        f.Close()
        Dim hash As Byte() = md5.Hash
        Dim buff As StringBuilder = New StringBuilder
        Dim hashbyte As Byte
        For Each hashbyte In hash
            buff.Append(String.Format("{0:X2}", hashbyte))
        Next
        Dim shastring As String
        shastring = buff.ToString()
        Return shastring

    End Function





End Class
