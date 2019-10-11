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
        public rAsistencias()
        {
            InitializeComponent();
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

            if (this.DetalleEstudiantes.Count == 0)
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
            asistencia.AsignaturaID = AsignaturaComboBox.SelectedIndex;
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


            if (AsistenciasBLL.Eliminar(id))
            {
                MessageBox.Show("Eliminada correctamente", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Limpiar();
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
            AsignaturaComboBox.SelectedIndex = asistencia.AsignaturaID;
            this.DetalleEstudiantes = asistencia.Estudiante;
            
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

            this.DetalleEstudiantes = new List<EstudiantesDetalle>();
            CargarGrid();
        }

        private void AgregarEstudiantebutton_Click(object sender, EventArgs e)
        {
            if (dataGridView.DataSource != null)
            {
                this.DetalleEstudiantes = (List<EstudiantesDetalle>)dataGridView.DataSource;
            }


            this.DetalleEstudiantes.Add(new EstudiantesDetalle(
                estudianteID: 0,
                nombre: EstudiantecomboBox.SelectedText
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
            if (dataGridView.Rows.Count > 0 && dataGridView.CurrentRow != null)
            {
                DetalleEstudiantes.RemoveAt(dataGridView.CurrentRow.Index);
                CargarGrid();
            }
        }

        public void LlenarComboBox()
        {
            GenericaBLL<EstudiantesDetalle> genericaBLL = new GenericaBLL<EstudiantesDetalle>();
            List<EstudiantesDetalle> lista = genericaBLL.GetList(p => true);
            EstudiantecomboBox.DataSource = lista;
            EstudiantecomboBox.DisplayMember = "Nombre";
            EstudiantecomboBox.ValueMember = "EstudianteID";

            GenericaBLL<Asignaturas> genericaAsignaturasBLL = new GenericaBLL<Asignaturas>();
            List<Asignaturas> lista1 = new List<Asignaturas>();
            lista1 = genericaAsignaturasBLL.GetList(p => true);
            AsignaturaComboBox.DataSource = lista1;
            AsignaturaComboBox.DisplayMember = "Nombre";
            AsignaturaComboBox.ValueMember = "AsignaturaID";
        }

        private void RAsistencias_Load(object sender, EventArgs e)
        {
            LlenarComboBox();
        }
    }
}
