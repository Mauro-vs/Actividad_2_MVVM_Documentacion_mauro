using Actividad_2_MVVM_mauro.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivoView.Repo
{
    public class RepositorioReservas
    {
        private readonly CentroDeportivoEntities1 _db = new CentroDeportivoEntities1();

        public List<Reservas> Selecionar()
        {
            return _db.Reservas.ToList();
        }

        public void Agregar(Reservas reservas)
        {
            _db.Reservas.Add(reservas);
            _db.SaveChanges();
        }

        public void Editar(Reservas reservas)
        {
            var existenteReserva = _db.Reservas.Find(reservas.Id);
            if (existenteReserva != null)
            {
                existenteReserva.Id = reservas.Id;
                existenteReserva.SocioId = reservas.SocioId;
                existenteReserva.ActividadId = reservas.ActividadId;
                existenteReserva.Fecha = reservas.Fecha;
                _db.SaveChanges();
            }
        }

        public void Borrar(Reservas reservas)
        {
            var existenteReserva = _db.Reservas.Find(reservas.Id);
            if (existenteReserva != null)
            {
                _db.Reservas.Remove(existenteReserva);
                _db.SaveChanges();
            }
        }

        public Reservas SelecionarPorId(int id)
        {
            return _db.Reservas.Find(id);
        }

        public int ContarReservasPorActividadYFecha(int actividadId, DateTime fecha)
        {
            int contador = 0;

            foreach (var r in _db.Reservas)
            {
                if (r.ActividadId == actividadId && r.Fecha == fecha)
                {
                    contador++;
                }
            }

            return contador;
        }

        public int ObtenerAforoMaximo(int actividadId)
        {
            var actividad = _db.Actividades.Find(actividadId);
            return actividad?.AforoMaximo ?? 0;
        }

        //* VALIDACIONES *//
        // Validar que la fecha no sea anterior a hoy
        public bool fechaActualDisponible(DateTime fecha)
        {
            return fecha.Date >= DateTime.Today;
        }

        // Validar si se puede crear otra reserva en memoria sin tocar la BD
        public bool PuedeCrearOtraReserva(int aforoMaximo, int reservasExistentes)
        {
            if (aforoMaximo <= 0)
            {
                return true;
            }

            return reservasExistentes < aforoMaximo;
        }
    }
}
