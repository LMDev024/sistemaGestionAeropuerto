Public Class Reserva
    Public Property ReservaID As Integer
    Public Property CodigoReserva As String
    Public Property VueloID As Integer
    Public Property NumeroVuelo As String
    Public Property PasajeroID As Integer
    Public Property NombrePasajero As String
    Public Property NumeroAsiento As String
    Public Property Clase As String
    Public Property Estado As String
    Public Property Precio As Decimal
    Public Property Equipaje As Integer
    Public Property FechaReserva As DateTime

    Public Sub New()
        CodigoReserva = ""
        NumeroVuelo = ""
        NombrePasajero = ""
        NumeroAsiento = ""
        Clase = "Economica"
        Estado = "Confirmada"
        Equipaje = 1
    End Sub
End Class