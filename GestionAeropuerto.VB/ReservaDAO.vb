Imports System.Data.SqlClient

Public Class ReservaDAO
    ' Obtener todas las reservas
    Public Function ObtenerTodas() As List(Of Reserva)
        Dim reservas As New List(Of Reserva)

        Try
            Using conn As SqlConnection = DatabaseConnection.GetConnection()
                Dim query As String = "
                    SELECT 
                        R.ReservaID, R.CodigoReserva, R.VueloID, R.PasajeroID,
                        R.NumeroAsiento, R.Clase, R.Estado, R.Precio, R.Equipaje,
                        R.FechaReserva,
                        P.Nombre + ' ' + P.Apellido AS NombrePasajero,
                        V.NumeroVuelo
                    FROM Reservas R
                    INNER JOIN Pasajeros P ON R.PasajeroID = P.PasajeroID
                    INNER JOIN Vuelos V ON R.VueloID = V.VueloID
                    WHERE R.Estado != 'Cancelada'
                    ORDER BY R.FechaReserva DESC"

                Using cmd As New SqlCommand(query, conn)
                    conn.Open()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim reserva As New Reserva With {
                                .ReservaID = Convert.ToInt32(reader("ReservaID")),
                                .CodigoReserva = reader("CodigoReserva").ToString(),
                                .VueloID = Convert.ToInt32(reader("VueloID")),
                                .NumeroVuelo = reader("NumeroVuelo").ToString(),
                                .PasajeroID = Convert.ToInt32(reader("PasajeroID")),
                                .NombrePasajero = reader("NombrePasajero").ToString(),
                                .NumeroAsiento = reader("NumeroAsiento").ToString(),
                                .Clase = reader("Clase").ToString(),
                                .Estado = reader("Estado").ToString(),
                                .Precio = Convert.ToDecimal(reader("Precio")),
                                .Equipaje = Convert.ToInt32(reader("Equipaje")),
                                .FechaReserva = Convert.ToDateTime(reader("FechaReserva"))
                            }
                            reservas.Add(reserva)
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception("Error al obtener reservas: " & ex.Message)
        End Try

        Return reservas
    End Function

    ' Obtener reservas por vuelo
    Public Function ObtenerPorVuelo(vueloId As Integer) As List(Of Reserva)
        Dim reservas As New List(Of Reserva)

        Try
            Using conn As SqlConnection = DatabaseConnection.GetConnection()
                Using cmd As New SqlCommand("sp_ObtenerPasajerosPorVuelo", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.AddWithValue("@VueloID", vueloId)

                    conn.Open()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim reserva As New Reserva With {
                                .ReservaID = Convert.ToInt32(reader("ReservaID")),
                                .CodigoReserva = reader("CodigoReserva").ToString(),
                                .NombrePasajero = reader("NombreCompleto").ToString(),
                                .NumeroAsiento = reader("NumeroAsiento").ToString(),
                                .Clase = reader("Clase").ToString(),
                                .Estado = reader("EstadoReserva").ToString(),
                                .Precio = Convert.ToDecimal(reader("Precio")),
                                .Equipaje = Convert.ToInt32(reader("Equipaje"))
                            }
                            reservas.Add(reserva)
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception("Error al obtener reservas del vuelo: " & ex.Message)
        End Try

        Return reservas
    End Function

    ' Buscar por código
    Public Function BuscarPorCodigo(codigo As String) As Reserva
        Try
            Using conn As SqlConnection = DatabaseConnection.GetConnection()
                Dim query As String = "
                    SELECT 
                        R.ReservaID, R.CodigoReserva, R.VueloID, R.PasajeroID,
                        R.NumeroAsiento, R.Clase, R.Estado, R.Precio, R.Equipaje,
                        R.FechaReserva,
                        P.Nombre + ' ' + P.Apellido AS NombrePasajero,
                        V.NumeroVuelo
                    FROM Reservas R
                    INNER JOIN Pasajeros P ON R.PasajeroID = P.PasajeroID
                    INNER JOIN Vuelos V ON R.VueloID = V.VueloID
                    WHERE R.CodigoReserva = @Codigo"

                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@Codigo", codigo)
                    conn.Open()

                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            Return New Reserva With {
                                .ReservaID = Convert.ToInt32(reader("ReservaID")),
                                .CodigoReserva = reader("CodigoReserva").ToString(),
                                .VueloID = Convert.ToInt32(reader("VueloID")),
                                .NumeroVuelo = reader("NumeroVuelo").ToString(),
                                .PasajeroID = Convert.ToInt32(reader("PasajeroID")),
                                .NombrePasajero = reader("NombrePasajero").ToString(),
                                .NumeroAsiento = reader("NumeroAsiento").ToString(),
                                .Clase = reader("Clase").ToString(),
                                .Estado = reader("Estado").ToString(),
                                .Precio = Convert.ToDecimal(reader("Precio")),
                                .Equipaje = Convert.ToInt32(reader("Equipaje")),
                                .FechaReserva = Convert.ToDateTime(reader("FechaReserva"))
                            }
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception("Error al buscar reserva: " & ex.Message)
        End Try

        Return Nothing
    End Function

    ' Cancelar reserva
    Public Function Cancelar(reservaId As Integer, motivo As String) As Boolean
        Try
            Using conn As SqlConnection = DatabaseConnection.GetConnection()
                Using cmd As New SqlCommand("sp_CancelarReserva", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.AddWithValue("@ReservaID", reservaId)
                    cmd.Parameters.AddWithValue("@Motivo", If(String.IsNullOrEmpty(motivo), DBNull.Value, motivo))

                    conn.Open()
                    cmd.ExecuteNonQuery()
                    Return True
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception("Error al cancelar reserva: " & ex.Message)
        End Try
    End Function
End Class