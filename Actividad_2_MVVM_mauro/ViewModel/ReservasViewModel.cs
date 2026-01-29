using Actividad_2_MVVM_mauro.Infrastructure;
using Actividad_2_MVVM_mauro.Model;
using CentroDeportivoView.Repo;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Data.Entity;

namespace Actividad_2_MVVM_mauro.ViewModel
{
    internal class ReservasViewModel : INotifyPropertyChanged
    {
        private readonly RepositorioReservas repo = new RepositorioReservas();

        // Propiedades del Reserva
        private int _id; // Necesario para editar/eliminar
        private DateTime _fecha;
        private int _socioId;
        private int _actividadId;

        // Colecciones para ComboBox
        private ObservableCollection<Socios> _socios = new ObservableCollection<Socios>();
        private ObservableCollection<Actividades> _actividades = new ObservableCollection<Actividades>();
        private ObservableCollection<Reservas> _reservas = new ObservableCollection<Reservas>();

        public ObservableCollection<Reservas> Reservas
        {
            get => _reservas;
            private set
            {
                _reservas = value ?? new ObservableCollection<Reservas>();
                OnPropertyChanged(nameof(Reservas));
            }
        }

        // Exponer colecciones para los ComboBox
        public ObservableCollection<Socios> Socios
        {
            get => _socios;
            private set
            {
                _socios = value ?? new ObservableCollection<Socios>();
                OnPropertyChanged(nameof(Socios));
            }
        }

        public ObservableCollection<Actividades> Actividades
        {
            get => _actividades;
            private set
            {
                _actividades = value ?? new ObservableCollection<Actividades>();
                OnPropertyChanged(nameof(Actividades));
            }
        }

        // Getters y Setters con notificación de cambio
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public DateTime Fecha
        {
            get => _fecha;
            set
            {
                _fecha = value;
                OnPropertyChanged(nameof(Fecha));
            }
        }

        public int SocioId
        {
            get => _socioId;
            set
            {
                _socioId = value;
                OnPropertyChanged(nameof(SocioId));
            }
        }

        public int ActividadId
        {
            get => _actividadId;
            set
            {
                _actividadId = value;
                OnPropertyChanged(nameof(ActividadId));
            }
        }

        // Items seleccionados en el DataGrid
        private Reservas _selectedReserva;
        public Reservas SelectedReserva
        {
            get => _selectedReserva;
            set
            {
                _selectedReserva = value;
                OnPropertyChanged(nameof(SelectedReserva));

                if (_selectedReserva != null)
                {
                    Id = _selectedReserva.Id;
                    Fecha = _selectedReserva.Fecha;
                    SocioId = _selectedReserva.SocioId;
                    ActividadId = _selectedReserva.ActividadId;
                }
            }
        }

        // Comandos
        public ICommand ListarReservas { get; }
        public ICommand AgregarReserva { get; }
        public ICommand EditarReserva { get; }
        public ICommand BorrarReserva { get; }

        public ReservasViewModel()
        {
            _fecha = DateTime.Today;

            ListarReservas = new RelayCommand(ListarTodasLasReservas);
            AgregarReserva = new RelayCommand(AgregarNuevaReserva);
            EditarReserva = new RelayCommand(EditarReservaExistente);
            BorrarReserva = new RelayCommand(BorrarReservaExistente);

            CargarSociosYActividades();
            ListarTodasLasReservas();
        }

        // Cargar datos para los ComboBox
        private void CargarSociosYActividades()
        {
            try
            {
                using (var db = new CentroDeportivoEntities1())
                {
                    var socios = db.Socios
                        .Where(s => s.Activo)
                        .OrderBy(s => s.Nombre)
                        .ToList();
                    Socios = new ObservableCollection<Socios>(socios);

                    var actividades = db.Actividades
                        .OrderBy(a => a.Nombre)
                        .ToList();
                    Actividades = new ObservableCollection<Actividades>(actividades);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudieron cargar socios y actividades.\n" + ex.Message,
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                Socios = new ObservableCollection<Socios>();
                Actividades = new ObservableCollection<Actividades>();
            }
        }

        // Listar Reservas
        private void ListarTodasLasReservas()
        {
            try
            {
                var lista = repo.Selecionar();
                Reservas = new ObservableCollection<Reservas>(lista);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudieron cargar las reservas.\n" + ex.Message,
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                Reservas = new ObservableCollection<Reservas>();
            }
        }

        // Agregar Reserva
        private void AgregarNuevaReserva()
        {
            if (SocioId <= 0)
            {
                MessageBox.Show(
                    "Debe seleccionar un socio.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            if (ActividadId <= 0)
            {
                MessageBox.Show(
                    "Debe seleccionar una actividad.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            // Validar que haya fecha seleccionada
            if (Fecha == default(DateTime))
            {
                MessageBox.Show(
                    "Debe seleccionar una fecha válida.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            // Validar que la fecha no sea anterior a hoy usando el repositorio
            if (!repo.fechaActualDisponible(Fecha))
            {
                MessageBox.Show(
                    "La fecha de la reserva no puede ser anterior a hoy.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            try
            {
                int aforoMaximo = repo.ObtenerAforoMaximo(ActividadId);
                int reservasExistentes = repo.ContarReservasPorActividadYFecha(ActividadId, Fecha.Date);

                // Validar aforo máximo usando la función del repositorio
                if (!repo.PuedeCrearOtraReserva(aforoMaximo, reservasExistentes))
                {
                    MessageBox.Show(
                        "El aforo de esta actividad para la fecha seleccionada está completo.",
                        "Aforo completo",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo comprobar el aforo de la actividad.\n" + ex.Message,
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            var nuevaReserva = new Reservas
            {
                SocioId = SocioId,
                ActividadId = ActividadId,
                Fecha = Fecha
            };

            try
            {
                repo.Agregar(nuevaReserva);
                ListarTodasLasReservas();
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo agregar la reserva.\n" + ex.Message,
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        // Editar Reserva
        private void EditarReservaExistente()
        {
            if (SelectedReserva == null)
            {
                MessageBox.Show(
                    "Debe seleccionar una reserva para editar.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            if (Fecha == default(DateTime))
            {
                MessageBox.Show(
                    "Debe seleccionar una fecha válida.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            // Validar fecha actual usando repositorio
            if (!repo.fechaActualDisponible(Fecha))
            {
                MessageBox.Show(
                    "La fecha de la reserva no puede ser anterior a hoy.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            try
            {
                int aforoMaximo = repo.ObtenerAforoMaximo(ActividadId);
                int reservasExistentes = repo.ContarReservasPorActividadYFecha(ActividadId, Fecha.Date);

                if (SelectedReserva.ActividadId == ActividadId &&
                    SelectedReserva.Fecha.Date == Fecha.Date)
                {
                    // si es la misma reserva/actividad/fecha, no debe contarse doble
                    reservasExistentes -= 1;
                }

                if (!repo.PuedeCrearOtraReserva(aforoMaximo, reservasExistentes))
                {
                    MessageBox.Show(
                        "El aforo de esta actividad para la fecha seleccionada está completo.",
                        "Aforo completo",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo comprobar el aforo de la actividad.\n" + ex.Message,
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            SelectedReserva.SocioId = SocioId;
            SelectedReserva.ActividadId = ActividadId;
            SelectedReserva.Fecha = Fecha;

            try
            {
                repo.Editar(SelectedReserva);
                ListarTodasLasReservas();
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo editar la reserva.\n" + ex.Message,
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        // Borrar Reserva
        private void BorrarReservaExistente()
        {
            if (SelectedReserva == null)
            {
                MessageBox.Show(
                    "Debe seleccionar una reserva para borrar.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            try
            {
                repo.Borrar(SelectedReserva);
                ListarTodasLasReservas();
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo borrar la reserva.\n" + ex.Message,
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        // Limpiar formulario
        private void LimpiarFormulario()
        {
            Fecha = DateTime.Today;
            SocioId = 0;
            ActividadId = 0;
            SelectedReserva = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
