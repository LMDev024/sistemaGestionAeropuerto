Public Class Vuelo
    Public Property VueloID As Integer
    Public Property NumeroVuelo As String
    Public Property AerolineaID As Integer
    Public Property NombreAerolinea As String
    Public Property AeronaveID As Integer
    Public Property ModeloAeronave As String
    Public Property Origen As String
    Public Property Destino As String
    Public Property FechaSalida As DateTime
    Public Property FechaLlegada As DateTime
    Public Property Estado As String
    Public Property PuertaID As Integer?
    Public Property NumeroPuerta As String
    Public Property AsientosDisponibles As Integer
    Public Property PrecioBase As Decimal

    Public Sub New()
        NumeroVuelo = ""
        NombreAerolinea = ""
        ModeloAeronave = ""
        Origen = ""
        Destino = ""
        Estado = "Programado"
        NumeroPuerta = ""
    End Sub
End Class