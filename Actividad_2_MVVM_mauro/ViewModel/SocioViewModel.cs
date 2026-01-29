using Actividad_2_MVVM_mauro.Infrastructure;
using Actividad_2_MVVM_mauro.Model;
using CentroDeportivoView.Repo;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace CentroDepoertivoViewModel
{
    internal class SocioViewModel : INotifyPropertyChanged
    {
        private readonly RepositorioSocio repo = new RepositorioSocio();

        // Propiedades del Socio
        private int _id; // Necesario para editar/eliminar
        private string _nombre;
        private string _email;
        private bool _activo;

        // Lista para DataGrid
        // El ObservableCollection notifica automáticamente cambios a la vista
        // Lo utilizo pq con este si se actualiza el DataGrid al modificar/eliminar los socios
        private ObservableCollection<Socios> _socios = new ObservableCollection<Socios>();
        public ObservableCollection<Socios> Socios
        {
            get => _socios;
            private set
            {
                _socios = value ?? new ObservableCollection<Socios>();
                OnPropertyChanged(nameof(Socios));
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

        public string Nombre
        {
            get => _nombre;
            set
            {
                _nombre = value;
                OnPropertyChanged(nameof(Nombre));
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public bool Activo
        {
            get => _activo;
            set
            {
                _activo = value;
                OnPropertyChanged(nameof(Activo));
            }
        }

        // Item seleccionado en DataGrid
        private Socios _selectedSocio;
        public Socios SelectedSocio
        {
            get => _selectedSocio;
            set
            {
                _selectedSocio = value;
                OnPropertyChanged(nameof(SelectedSocio));

                if (value != null)
                {
                    // Rellenar formulario
                    Id     = value.Id;
                    Nombre = value.Nombre;
                    Email  = value.Email;
                    Activo = value.Activo;
                }
            }
        }

        // Comandos
        public ICommand ListarSocios { get; }
        public ICommand AgregarSocio { get; }
        public ICommand EditarSocio { get; }
        public ICommand EliminarSocio { get; }

        // Constructor
        public SocioViewModel()
        {
            AgregarSocio = new RelayCommand(AgregarNuevoSocio);
            ListarSocios = new RelayCommand(ListarTodosLosSocios);
            EditarSocio  = new RelayCommand(EditarSocioExistente);
            EliminarSocio= new RelayCommand(EliminarSocioExistente);

            // Carga inicial
            ListarTodosLosSocios();
            Activo = true;
        }

        // Listar todos los socios
        private void ListarTodosLosSocios()
        {
            try
            {
                var lista = repo.Selecionar();
                Socios = new ObservableCollection<Socios>(lista);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudieron cargar los socios.\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Socios = new ObservableCollection<Socios>();
            }
        }

        // Agregar nuevo socio
        private void AgregarNuevoSocio()
        {
            if (string.IsNullOrWhiteSpace(Nombre))
            {
                MessageBox.Show("El nombre no puede estar vacio.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(Email))
            {
                MessageBox.Show("El email no puede estar vacio.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (repo.emailValido(Email))
            {
                MessageBox.Show("El formato del email es incorrecto.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var nuevoSocio = new Socios
            {
                Nombre = this.Nombre,
                Email = this.Email,
                Activo = this.Activo
            };
            repo.Agregar(nuevoSocio);

            ListarTodosLosSocios();
            Limpiar();
        }

        // Editar socio existente
        private void EditarSocioExistente()
        {
            if (SelectedSocio == null || Id <= 0)
            {
                MessageBox.Show("Seleccione un socio de la lista para editar.", "Atención", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(Nombre))
            {
                MessageBox.Show("El nombre no puede estar vacio.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(Email))
            {
                MessageBox.Show("El email no puede estar vacio.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("El formato del email es incorrecto.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var socioExistente = repo.SelecionarPorId(Id);
            if (socioExistente == null)
            {
                MessageBox.Show("No se encontró el socio seleccionado.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            socioExistente.Nombre = this.Nombre;
            socioExistente.Email  = this.Email;
            socioExistente.Activo = this.Activo;

            repo.Editar(socioExistente);

            ListarTodosLosSocios();
            Limpiar();
            SelectedSocio = null; // limpia selección para evitar confusiones
        }

        // Eliminar socio existente
        private void EliminarSocioExistente()
        {
            if (SelectedSocio == null || Id <= 0)
            {
                MessageBox.Show("Seleccione un socio de la lista para eliminar.", "Atención", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var socioExistente = repo.SelecionarPorId(Id);
            if (socioExistente == null)
            {
                MessageBox.Show("No se encontró el socio seleccionado.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            repo.Borrar(socioExistente);

            ListarTodosLosSocios();
            Limpiar();
            SelectedSocio = null;
        }

        // Métodos auxiliares
        private void Limpiar()
        {
            Id = 0;
            Nombre = string.Empty;
            Email = string.Empty;
            Activo = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
