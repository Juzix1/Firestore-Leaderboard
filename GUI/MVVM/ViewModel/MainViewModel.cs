using Firestore_Library;
using Google.Cloud.Firestore;
using GUI.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace GUI.MVVM.ViewModel {
    /// <summary>
    /// Główny ViewModel aplikacji, zarządzający przełączaniem widoków i podstawowymi danymi.
    /// </summary>
    class MainViewModel : ObservableObject, INotifyPropertyChanged {
        /// <summary>
        /// Kolekcja graczy widoczna w całej aplikacji.
        /// </summary>
        public ObservableCollection<Player> Players { get; set; }
        /// <summary>
        /// Komenda do przełączania widoku na widok domowy.
        /// </summary>
        public RelayCommand HomeViewCommand { get; set; }
        /// <summary>
        /// Komenda do przełączania widoku na widok funkcjonalności.
        /// </summary>
        public RelayCommand FeatureViewCommand { get; set; }



        /// <summary>
        /// ViewModel dla widoku domowego.
        /// </summary>
        public HomeViewModel HomeVm { get; set; }
        /// <summary>
        /// ViewModel dla widoku funkcjonalności.
        /// </summary>
        public FeatureViewModel FeatureVM { get; set; }
        
        private object _currentView;
        /// <summary>
        /// Aktualnie wyświetlany widok w aplikacji.
        /// </summary>
        public object CurrentView {
            get => _currentView;
            set {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }
        /// <summary>
        /// Konstruktor inicjalizujący główny ViewModel.
        /// Ustawia widoki, kolekcje oraz przypisuje komendy.
        /// </summary>
        public MainViewModel() {
            Players = new ObservableCollection<Player>();
            HomeVm = new HomeViewModel();
            FeatureVM = new FeatureViewModel();

            CurrentView = HomeVm;

            HomeViewCommand = new RelayCommand(o => {
                CurrentView = HomeVm;

            });

            FeatureViewCommand = new RelayCommand(o => {
                CurrentView = FeatureVM;
            });
        }

        /// <summary>
        /// Zdarzenie powiadamiające o zmianie właściwości.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Wywołuje zdarzenie PropertyChanged dla podanej nazwy właściwości.
        /// </summary>
        /// <param name="propertyName">Nazwa właściwości, która uległa zmianie.</param>
        protected void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
