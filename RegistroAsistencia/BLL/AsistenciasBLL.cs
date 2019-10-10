using RegistroAsistencia.DAL;
using RegistroAsistencia.Entidades;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistencia.BLL
{
    public class AsistenciasBLL
    {
        public static bool Guardar(Asistencias asistencia)
        {
            bool realizado = false;
            Contexto db = new Contexto();

            try
            {
                if (db.Asistencia.Add(asistencia) != null)
                    realizado = db.SaveChanges() > 0;
            }
            catch (Exception)
            {
                throw;
            }

            finally
            {
                db.Dispose();
            }

            return realizado;
        }

        public static bool Modificar(Asistencias asistencia)
        {
            bool realizado = false;
            Contexto db = new Contexto();

            try
            {
                var Anterior = db.Asistencia.Find(asistencia.AsistenciaID);
                foreach(var item in Anterior.Estudiante)
                {
                    if (!asistencia.Estudiante.Exists(d => d.EstudianteID == item.EstudianteID))
                        db.Entry(item).State = EntityState.Deleted;
                }
                db.Entry(asistencia).State = EntityState.Modified;
                realizado = (db.SaveChanges() > 0);
            }
            catch (Exception)
            {
                throw;
            }

            finally
            {
                db.Dispose();
            }

            return realizado;
        }

        public static bool Eliminar(int id)
        {
            bool realizado = false;
            Contexto db = new Contexto();

            try
            {
                var eliminar = db.Asistencia.Find(id);
                db.Entry(eliminar).State = EntityState.Deleted;
                realizado = (db.SaveChanges() > 0);
            }
            catch (Exception)
            {
                throw;
            }

            finally
            {
                db.Dispose();
            }

            return realizado;
        }

        public static Asistencias Buscar(int id)
        {
            Contexto db = new Contexto();
            Asistencias asistencia = new Asistencias();

            try
            {
                asistencia = db.Asistencia.Find(id);
                asistencia.Estudiante.Count();
            }
            catch (Exception)
            {
                throw;
            }

            finally
            {
                db.Dispose();
            }

            return asistencia;
        }

        public static List<Asistencias> GetList(Expression<Func<Asistencias, bool>> asistencia)
        {
            List<Asistencias> Lista = new List<Asistencias>();
            Contexto db = new Contexto();

            try
            {
                Lista = db.Asistencia.Where(asistencia).ToList();
            }
            catch (Exception)
            {
                throw;
            }

            finally
            {
                db.Dispose();
            }

            return Lista;
        }
    }
}
