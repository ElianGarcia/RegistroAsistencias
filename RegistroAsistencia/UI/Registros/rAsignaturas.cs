﻿using RegistroAsistencia.BLL;
using RegistroAsistencia.DAL;
using RegistroAsistencia.Entidades;
using System;
using System.Windows.Forms;

namespace RegistroAsistencia.UI.Registros
{
    public partial class rAsignaturas : Form
    {
        GenericaBLL<Asignaturas> generica;
        public rAsignaturas()
        {
            generica = new GenericaBLL<Asignaturas>();
            InitializeComponent();
        }

        private void Nuevobutton_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void Guardarbutton_Click(object sender, EventArgs e)
        {
            Asignaturas asignatura = new Asignaturas();
            bool realizado = false;

            if (!Validar())
                return;

            asignatura = LlenaClase();


            if (IDnumericUpDown.Value == 0)
                realizado = generica.Guardar(asignatura);
            else
            {
                if (!Existe())
                {
                    MessageBox.Show("NO SE PUEDE MODIFICAR UNA ASIGNATURA INEXISTENTE", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                realizado = generica.Modificar(asignatura);
            }

            if (realizado)
            {
                Limpiar();
                MessageBox.Show("GUARDADA EXITOSAMENTE", "GUARDADO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("NO SE PUDO GUARDAR", "NO GUARDADO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool Existe()
        {
            Asignaturas asignaturas = generica.Buscar((int)IDnumericUpDown.Value);

            return (asignaturas != null);
        }

        private Asignaturas LlenaClase()
        {
            Asignaturas asignatura = new Asignaturas();
            asignatura.AsignaturaID = Convert.ToInt32(IDnumericUpDown.Value);
            asignatura.Nombre = NombretextBox.Text;

            return asignatura;
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

        private void Eliminarbutton_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();

            int id;
            int.TryParse(IDnumericUpDown.Text, out id);
            Contexto db = new Contexto();

            Asignaturas asignatura = new Asignaturas();

            Limpiar();

            if (generica.Eliminar(id))
            {
                MessageBox.Show("Eliminada correctamente", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                errorProvider.SetError(IDnumericUpDown, "No se puede eliminar una asignatura inexistente");
            }
        }

        private void Buscarbutton_Click(object sender, EventArgs e)
        {
            int id;
            Asignaturas asignatura = new Asignaturas();
            generica = new GenericaBLL<Asignaturas>();
            int.TryParse(IDnumericUpDown.Text, out id);

            Limpiar();

            asignatura = generica.Buscar(id);

            if (asignatura != null)
            {
                LlenaCampos(asignatura);
            }
            else
            {
                MessageBox.Show("Asignatura no encontrada");
            }
        }

        private void Limpiar()
        {
            IDnumericUpDown.Value = 0;
            NombretextBox.Text = string.Empty;
        }

        private void LlenaCampos(Asignaturas asignatura)
        {
            IDnumericUpDown.Value = asignatura.AsignaturaID;
            NombretextBox.Text = asignatura.Nombre;
        }
    }
}
