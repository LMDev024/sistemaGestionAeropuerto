Public Class Pasajero
    Public Property PasajeroID As Integer
    Public Property TipoDocumento As String
    Public Property NumeroDocumento As String
    Public Property Nombre As String
    Public Property Apellido As String
    Public Property FechaNacimiento As DateTime
    Public Property Nacionalidad As String
    Public Property Email As String
    Public Property Telefono As String

    Public ReadOnly Property NombreCompleto As String
        Get
            Return Nombre & " " & Apellido
        End Get
    End Property

    Public Sub New()
        TipoDocumento = "CC"
        NumeroDocumento = ""
        Nombre = ""
        Apellido = ""
        Nacionalidad = "Colombiana"
        Email = ""
        Telefono = ""
    End Sub
End Class