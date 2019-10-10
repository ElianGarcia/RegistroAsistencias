using RegistroAsistencia.UI.Consultas;
using RegistroAsistencia.UI.Registros;
using System;
using System.Windows.Forms;

namespace RegistroAsistencia
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void AsistenciasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rAsistencias registro = new rAsistencias();
            registro.MdiParent = this;
            registro.Show();

        }

        private void AsistenciasToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            cAsistencias consulta = new cAsistencias();
            consulta.MdiParent = this;
            consulta.Show();
        }
    }
}
