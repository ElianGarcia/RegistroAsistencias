using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistencia.Entidades
{
    public class AsignaturasDetalle
    {
        [Key]
        public int AsignaturaID { get; set; }
        public string Nombre { get; set; }

        public AsignaturasDetalle()
        {
        }

        public AsignaturasDetalle(int asignaturaID, string nombre)
        {
            AsignaturaID = asignaturaID;
            Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre));
        }
    }
}
