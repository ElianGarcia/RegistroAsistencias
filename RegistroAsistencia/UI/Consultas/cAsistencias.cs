using RegistroAsistencia.BLL;
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

namespace RegistroAsistencia.UI.Consultas
{
    public partial class cAsistencias : Form
    {
        public cAsistencias()
        {
            InitializeComponent();
        }

        private void BtConsulta_Click(object sender, EventArgs e)
        {
            var listado = new List<Asistencias>();

            if (tbCriterio.Text.Trim().Length > 0)
            {
                switch (cbFiltrar.SelectedIndex)
                {
                    case 0:
                        listado = AsistenciasBLL.GetList(asistencia => true);
                        break;

                    case 1:
                        int id = Convert.ToInt32(tbCriterio.Text);
                        listado = AsistenciasBLL.GetList(asistencia => asistencia.AsistenciaID == id);
                        break;
                    case 2:
                        listado = AsistenciasBLL.GetList(asistencia => asistencia.AsignaturaID == Convert.ToInt32(tbCriterio.Text));
                        break;
                }

                listado = listado.Where(c => c.Fecha.Date >= DesdeDateTimePicker.Value.Date && c.Fecha.Date <= HastaDateTimePicker.Value.Date).ToList();
            }
            else
            {
                listado = AsistenciasBLL.GetList(p => true);
            }

            ConsultaDataGridView.DataSource = null;
            ConsultaDataGridView.DataSource = listado;
        }
    }
}
