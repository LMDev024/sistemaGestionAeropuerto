Public Class FrmPrincipal
    Private Sub FrmPrincipal_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Verificar conexión a la base de datos
        If DatabaseConnection.TestConnection() Then
            lblEstadoConexion.Text = "Conectado a la base de datos"
            lblEstadoConexion.ForeColor = Color.Green
        Else
            lblEstadoConexion.Text = "Error de conexión a la base de datos"
            lblEstadoConexion.ForeColor = Color.Red
            MessageBox.Show("No se pudo conectar a la base de datos. Verifique la configuración.",
                          "Error de Conexión", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

        ' Mostrar fecha y hora
        Timer1.Enabled = True
        ActualizarFechaHora()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        ActualizarFechaHora()
    End Sub

    Private Sub ActualizarFechaHora()
        lblFechaHora.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy - HH:mm:ss")
    End Sub

    Private Sub btnGestionVuelos_Click(sender As Object, e As EventArgs) Handles btnGestionVuelos.Click
        Dim frmVuelos As New FrmGestionVuelos()
        frmVuelos.ShowDialog()
    End Sub

    'Private Sub btnGestionReservas_Click(sender As Object, e As EventArgs) Handles btnGestionReservas.Click
    '    Dim frmReservas As New FrmGestionReservas()
    '    frmReservas.ShowDialog()
    'End Sub

    Private Sub btnCheckIn_Click(sender As Object, e As EventArgs) Handles btnCheckIn.Click
        Dim frmCheckIn As New FrmCheckIn()
        frmCheckIn.ShowDialog()
    End Sub

    'Private Sub btnAsignarPuertas_Click(sender As Object, e As EventArgs) Handles btnAsignarPuertas.Click
    '    Dim frmPuertas As New FrmAsignacionPuertas()
    '    frmPuertas.ShowDialog()
    'End Sub

    Private Sub btnSalir_Click(sender As Object, e As EventArgs) Handles btnSalir.Click
        If MessageBox.Show("¿Está seguro que desea salir?", "Confirmar",
                          MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Application.Exit()
        End If
    End Sub
End Class