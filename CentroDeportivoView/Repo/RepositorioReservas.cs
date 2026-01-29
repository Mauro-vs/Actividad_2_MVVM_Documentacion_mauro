using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivoView.Repo
{
    internal class RepositorioReservas
    {
        private readonly CentroDeportivoEntities _db = new CentroDeportivoEntities();

        public List<Reservas> Selecionar()
        {
            return _db.Reservas.ToList();
        }

        public void Agregar(Reservas reservas)
        {
            _db.Reservas.Add(reservas);
            _db.SaveChanges();
        }

        public void Editar(Reservas reservas) {
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
    }
}
