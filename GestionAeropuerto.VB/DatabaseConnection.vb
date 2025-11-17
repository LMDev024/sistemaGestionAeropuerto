Imports System.Configuration
Imports System.Data.SqlClient

Public Class DatabaseConnection
    Private Shared connectionString As String = ConfigurationManager.ConnectionStrings("AeropuertoConnection").ConnectionString

    Public Shared Function GetConnection() As SqlConnection
        Return New SqlConnection(connectionString)
    End Function

    Public Shared Function TestConnection() As Boolean
        Try
            Using conn As SqlConnection = GetConnection()
                conn.Open()
                Return True
            End Using
        Catch ex As Exception
            Return False
        End Try
    End Function
End Class