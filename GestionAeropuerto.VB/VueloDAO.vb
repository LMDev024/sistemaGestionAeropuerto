Imports System.Data.SqlClient

Public Class VueloDAO
    ' Obtener todos los vuelos
    Public Function ObtenerTodos() As List(Of Vuelo)
        Dim vuelos As New List(Of Vuelo)

        Try
            Using conn As SqlConnection = DatabaseConnection.GetConnection()
                Dim query As String = "
                    SELECT 
                        V.VueloID, V.NumeroVuelo, V.AerolineaID, V.AeronaveID,
                        V.Origen, V.Destino, V.FechaSalida, V.FechaLlegada,
                        V.Estado, V.PuertaID, V.AsientosDisponibles, V.PrecioBase,
                        A.Nombre AS NombreAerolinea,
                        AE.Modelo AS ModeloAeronave,
                        P.Numero AS NumeroPuerta
                    FROM Vuelos V
                    INNER JOIN Aerolineas A ON V.AerolineaID = A.AerolineaID
                    INNER JOIN Aeronaves AE ON V.AeronaveID = AE.AeronaveID
                    LEFT JOIN Puertas P ON V.PuertaID = P.PuertaID
                    ORDER BY V.FechaSalida DESC"

                Using cmd As New SqlCommand(query, conn)
                    conn.Open()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim vuelo As New Vuelo With {
                                .VueloID = Convert.ToInt32(reader("VueloID")),
                                .NumeroVuelo = reader("NumeroVuelo").ToString(),
                                .AerolineaID = Convert.ToInt32(reader("AerolineaID")),
                                .NombreAerolinea = reader("NombreAerolinea").ToString(),
                                .AeronaveID = Convert.ToInt32(reader("AeronaveID")),
                                .ModeloAeronave = reader("ModeloAeronave").ToString(),
                                .Origen = reader("Origen").ToString(),
                                .Destino = reader("Destino").ToString(),
                                .FechaSalida = Convert.ToDateTime(reader("FechaSalida")),
                                .FechaLlegada = Convert.ToDateTime(reader("FechaLlegada")),
                                .Estado = reader("Estado").ToString(),
                                .AsientosDisponibles = Convert.ToInt32(reader("AsientosDisponibles")),
                                .PrecioBase = Convert.ToDecimal(reader("PrecioBase"))
                            }

                            If Not IsDBNull(reader("PuertaID")) Then
                                vuelo.PuertaID = Convert.ToInt32(reader("PuertaID"))
                                vuelo.NumeroPuerta = reader("NumeroPuerta").ToString()
                            End If

                            vuelos.Add(vuelo)
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception("Error al obtener vuelos: " & ex.Message)
        End Try

        Return vuelos
    End Function

    ' Obtener vuelos por fecha
    Public Function ObtenerPorFecha(fecha As Date) As List(Of Vuelo)
        Dim vuelos As New List(Of Vuelo)

        Try
            Using conn As SqlConnection = DatabaseConnection.GetConnection()
                Using cmd As New SqlCommand("sp_ObtenerVuelosPorFecha", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.AddWithValue("@Fecha", fecha.Date)

                    conn.Open()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim vuelo As New Vuelo With {
                                .VueloID = Convert.ToInt32(reader("VueloID")),
                                .NumeroVuelo = reader("NumeroVuelo").ToString(),
                                .NombreAerolinea = reader("Aerolinea").ToString(),
                                .ModeloAeronave = reader("Aeronave").ToString(),
                                .Origen = reader("Origen").ToString(),
                                .Destino = reader("Destino").ToString(),
                                .FechaSalida = Convert.ToDateTime(reader("FechaSalida")),
                                .FechaLlegada = Convert.ToDateTime(reader("FechaLlegada")),
                                .Estado = reader("Estado").ToString(),
                                .AsientosDisponibles = Convert.ToInt32(reader("AsientosDisponibles")),
                                .PrecioBase = Convert.ToDecimal(reader("PrecioBase"))
                            }

                            If Not IsDBNull(reader("Puerta")) Then
                                vuelo.NumeroPuerta = reader("Puerta").ToString()
                            End If

                            vuelos.Add(vuelo)
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception("Error al obtener vuelos por fecha: " & ex.Message)
        End Try

        Return vuelos
    End Function

    ' Actualizar estado del vuelo
    Public Function ActualizarEstado(vueloId As Integer, nuevoEstado As String, observaciones As String) As Boolean
        Try
            Using conn As SqlConnection = DatabaseConnection.GetConnection()
                Using cmd As New SqlCommand("sp_ActualizarEstadoVuelo", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.AddWithValue("@VueloID", vueloId)
                    cmd.Parameters.AddWithValue("@NuevoEstado", nuevoEstado)
                    cmd.Parameters.AddWithValue("@Observaciones", If(String.IsNullOrEmpty(observaciones), DBNull.Value, observaciones))

                    conn.Open()
                    cmd.ExecuteNonQuery()
                    Return True
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception("Error al actualizar estado: " & ex.Message)
        End Try
    End Function

    ' Asignar puerta
    Public Function AsignarPuerta(vueloId As Integer, puertaId As Integer) As Boolean
        Try
            Using conn As SqlConnection = DatabaseConnection.GetConnection()
                Using cmd As New SqlCommand("sp_AsignarPuerta", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.AddWithValue("@VueloID", vueloId)
                    cmd.Parameters.AddWithValue("@PuertaID", puertaId)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                    Return True
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception("Error al asignar puerta: " & ex.Message)
        End Try
    End Function
End Class