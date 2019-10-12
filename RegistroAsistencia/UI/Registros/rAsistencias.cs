using RegistroAsistencia.BLL;
using RegistroAsistencia.DAL;
using RegistroAsistencia.Entidades;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RegistroAsistencia.UI.Registros
{
    public partial class rAsistencias : Form
    {
        public List<EstudiantesDetalle> DetalleEstudiantes { get; set; }
        public GenericaBLL<EstudiantesDetalle> generica;
        private int Cantidad;

        public rAsistencias()
        {
            InitializeComponent();
            LlenarComboBox();
            this.DetalleEstudiantes = new List<EstudiantesDetalle>();
            this.generica = new GenericaBLL<EstudiantesDetalle>();
        }

        private void RegistrarEstudiantebutton_Click(object sender, EventArgs e)
        {
            rEstudiantes registro = new rEstudiantes();
            registro.ShowDialog();
            CargarGrid();
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

            if (string.IsNullOrWhiteSpace(CantidadtextBox.Text))
            {
                errorProvider.SetError(IDnumericUpDown, "EL CAMPO CANTIDAD NO PUEDE ESTAR VACIO");
                CantidadtextBox.Focus();
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
            CargarGrid();
        }

        private void Buscarbutton_Click(object sender, EventArgs e)
        {
            int id;
            Asistencias asistencia = new Asistencias();
            int.TryParse(IDnumericUpDown.Text, out id);

            Limpiar();
            if(id > 0)
            {
                asistencia = AsistenciasBLL.Buscar(id);
            }
            
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
            EstudiantecomboBox.SelectedIndex = -1;
            AsignaturaComboBox.SelectedIndex = -1;
            AsistenciacheckBox.Checked = false;
            CantidadtextBox.Text = "0";
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

            EstudiantesDetalle ed = generica.Buscar(EstudiantecomboBox.SelectedIndex + 1);
            if(ed != null)
            {
                this.DetalleEstudiantes.Add(new EstudiantesDetalle(
                    estudianteID: EstudiantecomboBox.SelectedIndex,
                    nombre: ed.Nombre,
                    presente: AsistenciacheckBox.Checked
                    )
                );
            }
            
            CargarGrid();
            Cantidad += 1;
            CantidadtextBox.Text = Cantidad.ToString();
            EstudiantecomboBox.SelectedIndex = AsignaturaComboBox.SelectedIndex = -1;
        }

        private void CargarGrid()
        {
            DataGridViewCheckBoxColumn columna = new DataGridViewCheckBoxColumn();
            
            dataGridView.DataSource = null;
            dataGridView.DataSource = this.DetalleEstudiantes;
            
        }

        private void RemoverFilabutton_Click(object sender, EventArgs e)
        {
            if (dataGridView.Rows.Count > 0 && dataGridView.CurrentRow != null)
            {
                DetalleEstudiantes.RemoveAt(dataGridView.CurrentRow.Index);
                CargarGrid();
                Cantidad -= 1;
                CantidadtextBox.Text = Cantidad.ToString();
            }
        }

        public void LlenarComboBox()
        {
            EstudiantecomboBox.DataSource = null;
            GenericaBLL<Estudiantes> genericaBLL = new GenericaBLL<Estudiantes>();
            List<Estudiantes> lista = genericaBLL.GetList(p => true);
            EstudiantecomboBox.DataSource = lista;
            EstudiantecomboBox.DisplayMember = "Nombre";
            EstudiantecomboBox.ValueMember = "EstudianteID";

            AsignaturaComboBox.DataSource = null;
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
