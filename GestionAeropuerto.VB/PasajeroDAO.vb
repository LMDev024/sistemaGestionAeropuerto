Imports System.Data.SqlClient

Public Class PasajeroDAO
    ' Obtener todos los pasajeros
    Public Function ObtenerTodos() As List(Of Pasajero)
        Dim pasajeros As New List(Of Pasajero)

        Try
            Using conn As SqlConnection = DatabaseConnection.GetConnection()
                Dim query As String = "SELECT * FROM Pasajeros ORDER BY Nombre, Apellido"

                Using cmd As New SqlCommand(query, conn)
                    conn.Open()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim pasajero As New Pasajero With {
                                .PasajeroID = Convert.ToInt32(reader("PasajeroID")),
                                .TipoDocumento = reader("TipoDocumento").ToString(),
                                .NumeroDocumento = reader("NumeroDocumento").ToString(),
                                .Nombre = reader("Nombre").ToString(),
                                .Apellido = reader("Apellido").ToString(),
                                .FechaNacimiento = Convert.ToDateTime(reader("FechaNacimiento")),
                                .Nacionalidad = reader("Nacionalidad").ToString(),
                                .Email = If(IsDBNull(reader("Email")), "", reader("Email").ToString()),
                                .Telefono = If(IsDBNull(reader("Telefono")), "", reader("Telefono").ToString())
                            }
                            pasajeros.Add(pasajero)
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception("Error al obtener pasajeros: " & ex.Message)
        End Try

        Return pasajeros
    End Function

    ' Buscar por documento
    Public Function BuscarPorDocumento(tipoDoc As String, numeroDoc As String) As Pasajero
        Try
            Using conn As SqlConnection = DatabaseConnection.GetConnection()
                Dim query As String = "SELECT * FROM Pasajeros WHERE TipoDocumento = @Tipo AND NumeroDocumento = @Numero"

                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@Tipo", tipoDoc)
                    cmd.Parameters.AddWithValue("@Numero", numeroDoc)
                    conn.Open()

                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            Return New Pasajero With {
                                .PasajeroID = Convert.ToInt32(reader("PasajeroID")),
                                .TipoDocumento = reader("TipoDocumento").ToString(),
                                .NumeroDocumento = reader("NumeroDocumento").ToString(),
                                .Nombre = reader("Nombre").ToString(),
                                .Apellido = reader("Apellido").ToString(),
                                .FechaNacimiento = Convert.ToDateTime(reader("FechaNacimiento")),
                                .Nacionalidad = reader("Nacionalidad").ToString(),
                                .Email = If(IsDBNull(reader("Email")), "", reader("Email").ToString()),
                                .Telefono = If(IsDBNull(reader("Telefono")), "", reader("Telefono").ToString())
                            }
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception("Error al buscar pasajero: " & ex.Message)
        End Try

        Return Nothing
    End Function
End Class