using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistencia.Entidades
{
    public class Asistencias
    {
        [Key]
        public int AsistenciaID { get; set; }
        public int AsignaturaID { get; set; }
        public DateTime Fecha { get; set; }
        public virtual List<EstudiantesDetalle> Estudiante { get; set; }
        public int Cantidad { get; set; }

        public Asistencias()
        {

        }

        public Asistencias(int asistenciaID, int asignaturaID, int estudianteID, DateTime fecha, List<Asignaturas> asignatura, List<EstudiantesDetalle> estudiante, int cantidad)
        {
            AsistenciaID = asistenciaID;
            AsignaturaID = asignaturaID;
            Fecha = fecha;
            Estudiante = estudiante ?? throw new ArgumentNullException(nameof(estudiante));
            Cantidad = cantidad;
        }
    }
}
