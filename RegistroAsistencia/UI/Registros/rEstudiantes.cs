using RegistroAsistencia.BLL;
using RegistroAsistencia.DAL;
using RegistroAsistencia.Entidades;
using System;
using System.Windows.Forms;

namespace RegistroAsistencia.UI.Registros
{
    public partial class rEstudiantes : Form
    {
        GenericaBLL<Estudiantes> generica;
        public rEstudiantes()
        {
            generica = new GenericaBLL<Estudiantes>();
            InitializeComponent();
        }

        private void Buscarbutton_Click(object sender, EventArgs e)
        {
            int id;
            Estudiantes estudiantes = new Estudiantes();

            generica = new GenericaBLL<Estudiantes>();
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
        }

        private void Nuevobutton_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void Guardarbutton_Click(object sender, EventArgs e)
        {
            Estudiantes estudiantes = new Estudiantes();
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

        private Estudiantes LlenaClase()
        {
            Estudiantes estudiantes = new Estudiantes();
            estudiantes.EstudianteID = Convert.ToInt32(IDnumericUpDown.Value);
            estudiantes.Nombre = NombretextBox.Text;

            return estudiantes;
        }

        private bool Existe()
        {
            Estudiantes estudiantes = generica.Buscar((int)IDnumericUpDown.Value);

            return (estudiantes != null);
        }

        private void Eliminarbutton_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();

            int id;
            int.TryParse(IDnumericUpDown.Text, out id);
            Contexto db = new Contexto();

            Limpiar();

            if (generica.Eliminar(id))
            {
                MessageBox.Show("Eliminado correctamente", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                errorProvider.SetError(IDnumericUpDown, "No se puede eliminar un estudiante inexistente");
            }
        }

        private void Limpiar()
        {
            IDnumericUpDown.Value = 0;
            NombretextBox.Text = string.Empty;
        }

        private void LlenaCampos(Estudiantes estudiante)
        {
            IDnumericUpDown.Value = estudiante.EstudianteID;
            NombretextBox.Text = estudiante.Nombre;
        }
    }
}
