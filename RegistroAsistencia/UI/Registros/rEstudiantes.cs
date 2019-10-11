using RegistroAsistencia.BLL;
using RegistroAsistencia.DAL;
using RegistroAsistencia.Entidades;
using System;
using System.Windows.Forms;

namespace RegistroAsistencia.UI.Registros
{
    public partial class rEstudiantes : Form
    {
        GenericaBLL<EstudiantesDetalle> generica;
        rAsistencias asistencias = new rAsistencias();
        public rEstudiantes()
        {
            generica = new GenericaBLL<EstudiantesDetalle>();
            InitializeComponent();
        }

        private void Buscarbutton_Click(object sender, EventArgs e)
        {
            int id;
            EstudiantesDetalle estudiantes = new EstudiantesDetalle();

            generica = new GenericaBLL<EstudiantesDetalle>();
            int.TryParse(IDnumericUpDown.Text, out id);

            Limpiar();

            estudiantes = generica.Buscar(id);

            if (estudiantes != null)
            {
                LlenaCampos(estudiantes);
            }
            else
            {
                MessageBox.Show("Estudiante no encontrado");
            }
            asistencias.LlenarComboBox();
        }

        private void Nuevobutton_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void Guardarbutton_Click(object sender, EventArgs e)
        {
            EstudiantesDetalle estudiantes = new EstudiantesDetalle();
            bool realizado = false;

            if (!Validar())
                return;

            estudiantes = LlenaClase();


            if (IDnumericUpDown.Value == 0)
                realizado = generica.Guardar(estudiantes);
            else
            {
                if (!Existe())
                {
                    MessageBox.Show("NO SE PUEDE MODIFICAR UN ESTUDIANTE INEXISTENTE", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                realizado = generica.Modificar(estudiantes);
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
            
            asistencias.LlenarComboBox();

        }

        private bool Validar()
        {
            bool realizado = true;
            errorProvider.Clear();

            if (string.IsNullOrWhiteSpace(NombretextBox.Text))
            {
                errorProvider.SetError(NombretextBox, "EL CAMPO NOMBRE NO PUEDE ESTAR VACIO");
                NombretextBox.Focus();
                realizado = false;
            }

            return realizado;
        }

        private EstudiantesDetalle LlenaClase()
        {
            EstudiantesDetalle estudiantes = new EstudiantesDetalle();
            estudiantes.EstudianteID = Convert.ToInt32(IDnumericUpDown.Value);
            estudiantes.Nombre = NombretextBox.Text;

            return estudiantes;
        }

        private bool Existe()
        {
            EstudiantesDetalle estudiantes = generica.Buscar((int)IDnumericUpDown.Value);

            return (estudiantes != null);
        }

        private void Eliminarbutton_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();

            int id;
            int.TryParse(IDnumericUpDown.Text, out id);
            Contexto db = new Contexto();

            EstudiantesDetalle estudiantes = new EstudiantesDetalle();

            Limpiar();

            if (generica.Eliminar(id))
            {
                MessageBox.Show("Eliminado correctamente", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                errorProvider.SetError(IDnumericUpDown, "No se puede eliminar un estudiante inexistente");
            }
            asistencias.LlenarComboBox();
        }

        private void Limpiar()
        {
            IDnumericUpDown.Value = 0;
            NombretextBox.Text = string.Empty;
        }

        private void LlenaCampos(EstudiantesDetalle estudiante)
        {
            IDnumericUpDown.Value = estudiante.EstudianteID;
            NombretextBox.Text = estudiante.Nombre;
        }
    }
}
