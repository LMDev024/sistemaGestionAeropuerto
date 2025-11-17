Imports System.Data.SqlClient

Public Class FrmCheckIn
    Private reservaDAO As New ReservaDAO()
    Private reservaEncontrada As Reserva = Nothing

    Private Sub FrmCheckIn_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LimpiarFormulario()
    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        If String.IsNullOrWhiteSpace(txtCodigoReserva.Text) Then
            MessageBox.Show("Ingrese un código de reserva", "Advertencia",
                          MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            reservaEncontrada = reservaDAO.BuscarPorCodigo(txtCodigoReserva.Text.Trim().ToUpper())

            If reservaEncontrada IsNot Nothing Then
                MostrarDetallesReserva(reservaEncontrada)
                btnRealizarCheckIn.Enabled = (reservaEncontrada.Estado = "Confirmada")
            Else
                MessageBox.Show("No se encontró una reserva con ese código", "No encontrado",
                              MessageBoxButtons.OK, MessageBoxIcon.Information)
                LimpiarDetalles()
            End If
        Catch ex As Exception
            MessageBox.Show($"Error al buscar reserva: {ex.Message}", "Error",
                          MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub MostrarDetallesReserva(reserva As Reserva)
        lblNumeroVuelo.Text = reserva.NumeroVuelo
        lblPasajero.Text = reserva.NombrePasajero
        lblAsiento.Text = reserva.NumeroAsiento
        lblClase.Text = reserva.Clase
        lblEstado.Text = reserva.Estado
        lblEquipaje.Text = $"{reserva.Equipaje} maleta(s)"
        lblPrecio.Text = reserva.Precio.ToString("C")

        ' Cambiar color según estado
        Select Case reserva.Estado
            Case "Confirmada"
                lblEstado.ForeColor = Color.Green
            Case "CheckIn"
                lblEstado.ForeColor = Color.Blue
            Case "Abordado"
                lblEstado.ForeColor = Color.Purple
            Case "Cancelada"
                lblEstado.ForeColor = Color.Red
        End Select

        GroupBox2.Enabled = True
    End Sub

    Private Sub LimpiarDetalles()
        lblNumeroVuelo.Text = "-"
        lblPasajero.Text = "-"
        lblAsiento.Text = "-"
        lblClase.Text = "-"
        lblEstado.Text = "-"
        lblEquipaje.Text = "-"
        lblPrecio.Text = "-"
        GroupBox2.Enabled = False
        btnRealizarCheckIn.Enabled = False
        reservaEncontrada = Nothing
    End Sub

    Private Sub btnRealizarCheckIn_Click(sender As Object, e As EventArgs) Handles btnRealizarCheckIn.Click
        If reservaEncontrada Is Nothing Then
            Return
        End If

        If MessageBox.Show($"¿Confirmar check-in para el pasajero {reservaEncontrada.NombrePasajero}?",
                          "Confirmar Check-In", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Try
                ' Actualizar estado a CheckIn (hacerlo directamente con SQL)
                Using conn As SqlConnection = DatabaseConnection.GetConnection()
                    Dim query As String = "UPDATE Reservas SET Estado = 'CheckIn' WHERE ReservaID = @ReservaID"
                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@ReservaID", reservaEncontrada.ReservaID)
                        conn.Open()
                        cmd.ExecuteNonQuery()
                    End Using
                End Using

                MessageBox.Show("Check-in realizado exitosamente", "Éxito",
                              MessageBoxButtons.OK, MessageBoxIcon.Information)

                ' Recargar la reserva
                btnBuscar_Click(Nothing, Nothing)
            Catch ex As Exception
                MessageBox.Show($"Error al realizar check-in: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub btnLimpiar_Click(sender As Object, e As EventArgs) Handles btnLimpiar.Click
        LimpiarFormulario()
    End Sub

    Private Sub LimpiarFormulario()
        txtCodigoReserva.Clear()
        LimpiarDetalles()
        txtCodigoReserva.Focus()
    End Sub

    Private Sub btnCerrar_Click(sender As Object, e As EventArgs) Handles btnCerrar.Click
        Me.Close()
    End Sub

    Private Sub txtCodigoReserva_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtCodigoReserva.KeyPress
        ' Convertir a mayúsculas automáticamente
        If Char.IsLetter(e.KeyChar) Then
            e.KeyChar = Char.ToUpper(e.KeyChar)
        End If

        ' Presionar Enter para buscar
        If e.KeyChar = Convert.ToChar(Keys.Enter) Then
            e.Handled = True
            btnBuscar_Click(Nothing, Nothing)
        End If
    End Sub
End Class