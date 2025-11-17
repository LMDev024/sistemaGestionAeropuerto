Imports System.Data.SqlClient

Public Class FrmGestionVuelos
    Private vueloDAO As New VueloDAO()

    Private Sub FrmGestionVuelos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ConfigurarDataGridView()
        CargarVuelos()
        CargarEstados()
    End Sub

    Private Sub ConfigurarDataGridView()
        dgvVuelos.AutoGenerateColumns = False
        dgvVuelos.Columns.Clear()

        dgvVuelos.Columns.Add(New DataGridViewTextBoxColumn With {
            .HeaderText = "ID",
            .DataPropertyName = "VueloID",
            .Width = 50
        })

        dgvVuelos.Columns.Add(New DataGridViewTextBoxColumn With {
            .HeaderText = "Número Vuelo",
            .DataPropertyName = "NumeroVuelo",
            .Width = 100
        })

        dgvVuelos.Columns.Add(New DataGridViewTextBoxColumn With {
            .HeaderText = "Aerolínea",
            .DataPropertyName = "NombreAerolinea",
            .Width = 120
        })

        dgvVuelos.Columns.Add(New DataGridViewTextBoxColumn With {
            .HeaderText = "Origen",
            .DataPropertyName = "Origen",
            .Width = 150
        })

        dgvVuelos.Columns.Add(New DataGridViewTextBoxColumn With {
            .HeaderText = "Destino",
            .DataPropertyName = "Destino",
            .Width = 150
        })

        dgvVuelos.Columns.Add(New DataGridViewTextBoxColumn With {
            .HeaderText = "Fecha Salida",
            .DataPropertyName = "FechaSalida",
            .Width = 140,
            .DefaultCellStyle = New DataGridViewCellStyle With {.Format = "dd/MM/yyyy HH:mm"}
        })

        dgvVuelos.Columns.Add(New DataGridViewTextBoxColumn With {
            .HeaderText = "Estado",
            .DataPropertyName = "Estado",
            .Width = 100
        })

        dgvVuelos.Columns.Add(New DataGridViewTextBoxColumn With {
            .HeaderText = "Puerta",
            .DataPropertyName = "NumeroPuerta",
            .Width = 70
        })

        dgvVuelos.Columns.Add(New DataGridViewTextBoxColumn With {
            .HeaderText = "Asientos Disp.",
            .DataPropertyName = "AsientosDisponibles",
            .Width = 100
        })

        dgvVuelos.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvVuelos.MultiSelect = False
        dgvVuelos.AllowUserToAddRows = False
        dgvVuelos.ReadOnly = True
    End Sub

    Private Sub CargarVuelos()
        Try
            Dim vuelos As List(Of Vuelo) = vueloDAO.ObtenerTodos()
            dgvVuelos.DataSource = vuelos
            lblTotal.Text = $"Total de vuelos: {vuelos.Count}"
        Catch ex As Exception
            MessageBox.Show($"Error al cargar vuelos: {ex.Message}", "Error",
                          MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub CargarEstados()
        cmbEstado.Items.Clear()
        cmbEstado.Items.Add("Programado")
        cmbEstado.Items.Add("Abordando")
        cmbEstado.Items.Add("En Vuelo")
        cmbEstado.Items.Add("Aterrizado")
        cmbEstado.Items.Add("Retrasado")
        cmbEstado.Items.Add("Cancelado")
    End Sub

    Private Sub btnBuscarFecha_Click(sender As Object, e As EventArgs) Handles btnBuscarFecha.Click
        Try
            Dim vuelos As List(Of Vuelo) = vueloDAO.ObtenerPorFecha(dtpFecha.Value)
            dgvVuelos.DataSource = vuelos
            lblTotal.Text = $"Vuelos encontrados: {vuelos.Count}"
        Catch ex As Exception
            MessageBox.Show($"Error al buscar vuelos: {ex.Message}", "Error",
                          MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnTodos_Click(sender As Object, e As EventArgs) Handles btnTodos.Click
        CargarVuelos()
    End Sub

    Private Sub btnActualizarEstado_Click(sender As Object, e As EventArgs) Handles btnActualizarEstado.Click
        If dgvVuelos.SelectedRows.Count = 0 Then
            MessageBox.Show("Seleccione un vuelo", "Advertencia",
                          MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If String.IsNullOrEmpty(cmbEstado.Text) Then
            MessageBox.Show("Seleccione un estado", "Advertencia",
                          MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Dim vueloSeleccionado As Vuelo = CType(dgvVuelos.SelectedRows(0).DataBoundItem, Vuelo)

            If MessageBox.Show($"¿Actualizar estado del vuelo {vueloSeleccionado.NumeroVuelo} a '{cmbEstado.Text}'?",
                             "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then

                vueloDAO.ActualizarEstado(vueloSeleccionado.VueloID, cmbEstado.Text, txtObservaciones.Text)

                MessageBox.Show("Estado actualizado exitosamente", "Éxito",
                              MessageBoxButtons.OK, MessageBoxIcon.Information)

                CargarVuelos()
                cmbEstado.SelectedIndex = -1
                txtObservaciones.Clear()
            End If
        Catch ex As Exception
            MessageBox.Show($"Error al actualizar estado: {ex.Message}", "Error",
                          MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnRefrescar_Click(sender As Object, e As EventArgs) Handles btnRefrescar.Click
        CargarVuelos()
    End Sub

    Private Sub btnCerrar_Click(sender As Object, e As EventArgs) Handles btnCerrar.Click
        Me.Close()
    End Sub

    Private Sub dgvVuelos_SelectionChanged(sender As Object, e As EventArgs) Handles dgvVuelos.SelectionChanged
        If dgvVuelos.SelectedRows.Count > 0 Then
            Dim vuelo As Vuelo = CType(dgvVuelos.SelectedRows(0).DataBoundItem, Vuelo)
            MostrarDetallesVuelo(vuelo)
        End If
    End Sub

    Private Sub MostrarDetallesVuelo(vuelo As Vuelo)
        lblDetalleVuelo.Text = $"Vuelo: {vuelo.NumeroVuelo}" & vbCrLf &
                              $"Aerolínea: {vuelo.NombreAerolinea}" & vbCrLf &
                              $"Aeronave: {vuelo.ModeloAeronave}" & vbCrLf &
                              $"Ruta: {vuelo.Origen} → {vuelo.Destino}" & vbCrLf &
                              $"Salida: {vuelo.FechaSalida:dd/MM/yyyy HH:mm}" & vbCrLf &
                              $"Llegada: {vuelo.FechaLlegada:dd/MM/yyyy HH:mm}" & vbCrLf &
                              $"Estado: {vuelo.Estado}" & vbCrLf &
                              $"Puerta: {If(String.IsNullOrEmpty(vuelo.NumeroPuerta), "Sin asignar", vuelo.NumeroPuerta)}" & vbCrLf &
                              $"Asientos Disponibles: {vuelo.AsientosDisponibles}" & vbCrLf &
                              $"Precio Base: {vuelo.PrecioBase:C}"
    End Sub
End Class