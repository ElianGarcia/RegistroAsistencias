using RegistroAsistencia.BLL;
using RegistroAsistencia.DAL;
using RegistroAsistencia.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegistroAsistencia.UI.Registros
{
    public partial class rAsistencias : Form
    {
        public List<EstudiantesDetalle> DetalleEstudiantes { get; set; }
        public List<AsignaturasDetalle> DetalleAsignaturas { get; set; }
        public rAsistencias()
        {
            InitializeComponent();
            this.DetalleAsignaturas = new List<AsignaturasDetalle>();
            this.DetalleEstudiantes = new List<EstudiantesDetalle>();
        }

        private void RegistrarEstudiantebutton_Click(object sender, EventArgs e)
        {
            rEstudiantes registro = new rEstudiantes();
            registro.ShowDialog();
        }

        private void Guardarbutton_Click(object sender, EventArgs e)
        {
            Asistencias asistencia = new Asistencias();
            bool realizado = false;

            if (!Validar())
                return;

            asistencia = LlenaClase();


            if (IDnumericUpDown.Value == 0)
                realizado = AsistenciasBLL.Guardar(asistencia);
            else
            {
                if (!Existe())
                {
                    MessageBox.Show("NO SE PUEDE MODIFICAR UNA ASISTENCIA INEXISTENTE", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                realizado = AsistenciasBLL.Modificar(asistencia);
            }

            if (realizado)
            {
                Limpiar();
                MessageBox.Show("GUARDADO EXITOSAMENTE", "GUARDADO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("NO SE PUDO GUARDAR", "NO GUARDADO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool Existe()
        {
            Asistencias asistencia = AsistenciasBLL.Buscar((int)IDnumericUpDown.Value);

            return (asistencia != null);
        }

        private bool Validar()
        {
            bool realizado = true;
            errorProvider.Clear();

            if (string.IsNullOrWhiteSpace(IDnumericUpDown.Text))
            {
                errorProvider.SetError(IDnumericUpDown, "EL CAMPO ID NO PUEDE ESTAR VACIO");
                IDnumericUpDown.Focus();
                realizado = false;
            }

            if(this.DetalleEstudiantes.Count == 0)
            {
                errorProvider.SetError(dataGridView, "Debe agregar algun estudiante");
                EstudiantecomboBox.Focus();
                realizado = false;
            }

            return realizado;
        }

        private Asistencias LlenaClase()
        {
            Asistencias asistencia = new Asistencias();
            asistencia.AsistenciaID = Convert.ToInt32(IDnumericUpDown.Value);
            asistencia.Asignatura = this.DetalleAsignaturas;
            asistencia.Estudiante = this.DetalleEstudiantes;
            asistencia.Cantidad = Convert.ToInt32(CantidadtextBox.Text);
            asistencia.Fecha = FechadateTimePicker.Value;
            return asistencia;
        }

        private void Nuevobutton_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void Eliminarbutton_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();

            int id;
            int.TryParse(IDnumericUpDown.Text, out id);
            Contexto db = new Contexto();

            Asistencias asistencia = new Asistencias();

            Limpiar();

            if (AsistenciasBLL.Eliminar(id))
            {
                MessageBox.Show("Eliminada correctamente", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                errorProvider.SetError(IDnumericUpDown, "No se puede eliminar una asistencia inexistente");
            }
        }

        private void AgregarAsignaturabutton_Click(object sender, EventArgs e)
        {
            rAsignaturas registroAsignaturas = new rAsignaturas();
            registroAsignaturas.ShowDialog();
        }

        private void Buscarbutton_Click(object sender, EventArgs e)
        {
            int id;
            Asistencias asistencia = new Asistencias();
            int.TryParse(IDnumericUpDown.Text, out id);

            Limpiar();

            asistencia = AsistenciasBLL.Buscar(id);

            if (asistencia != null)
            {
                LlenaCampos(asistencia);
            }
            else
            {
                MessageBox.Show("Asistencia no encontrada");
            }
        }

        private void LlenaCampos(Asistencias asistencia)
        {
            IDnumericUpDown.Value = asistencia.AsistenciaID;
            CantidadtextBox.Text = asistencia.Cantidad.ToString();
            FechadateTimePicker.Value = asistencia.Fecha;

            this.DetalleEstudiantes = asistencia.Estudiante;
            this.DetalleAsignaturas = asistencia.Asignatura;

            CargarGrid();
        }

        private void Limpiar()
        {
            IDnumericUpDown.Value = 0;
            EstudiantecomboBox.SelectedIndex = 0;
            AsignaturaComboBox.SelectedIndex = 0;
            AsistenciacheckBox.Checked = false;
            CantidadtextBox.Text = string.Empty;
            FechadateTimePicker.Value = DateTime.Now;

            this.DetalleAsignaturas = new List<AsignaturasDetalle>();
            this.DetalleEstudiantes = new List<EstudiantesDetalle>();
            CargarGrid();
        }

        private void AgregarEstudiantebutton_Click(object sender, EventArgs e)
        {
            if (dataGridView.DataSource != null)
                this.DetalleEstudiantes = (List<EstudiantesDetalle>)dataGridView.DataSource;
                this.DetalleAsignaturas = (List<AsignaturasDetalle>)dataGridView.DataSource;

            this.DetalleEstudiantes.Add(new EstudiantesDetalle(
                estudianteID: 0,
                nombre: EstudiantecomboBox.SelectedItem.ToString()
                )
            );

            this.DetalleAsignaturas.Add(
                new AsignaturasDetalle(
                    asignaturaID: 0,
                    nombre: AsignaturaComboBox.Text
                    )
                );
            CargarGrid();
            EstudiantecomboBox.SelectedIndex = 0;
        }

        private void CargarGrid()
        {
            dataGridView.DataSource = null;
            dataGridView.DataSource = this.DetalleEstudiantes;
        }

        private void RemoverFilabutton_Click(object sender, EventArgs e)
        {
            if(dataGridView.Rows.Count > 0 && dataGridView.CurrentRow != null)
            {
                DetalleEstudiantes.RemoveAt(dataGridView.CurrentRow.Index);
                CargarGrid();
            }
        }

        private void RAsistencias_Load(object sender, EventArgs e)
        {
            EstudiantecomboBox.Items = lista;
        }
    }
}
