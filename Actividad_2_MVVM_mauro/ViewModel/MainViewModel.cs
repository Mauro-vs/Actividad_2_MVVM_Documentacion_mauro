using Actividad_2_MVVM_mauro.Infrastructure;
using Actividad_2_MVVM_mauro.View;
using System.ComponentModel;
using System.Windows.Input;

namespace Actividad_2_MVVM_mauro.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ICommand AbrirSocio { get; }
        public ICommand AbrirActividades { get; }
        public ICommand AbrirReservas { get; }

        public MainViewModel()
        {
            AbrirSocio = new RelayCommand(AbrirVentanaSocio);
            AbrirActividades = new RelayCommand(AbrirVentanaActividades);
            AbrirReservas = new RelayCommand(AbrirVentanaReservas);
        }

        private void AbrirVentanaSocio()
        {
            SociosWin winS = new SociosWin();
            winS.ShowDialog();
        }

        private void AbrirVentanaActividades()
        {
            ActividadesWin winA = new ActividadesWin();
            winA.ShowDialog();
        }

        private void AbrirVentanaReservas()
        {
            ReservasWin winR = new ReservasWin();
            winR.ShowDialog();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
