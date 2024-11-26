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
    class MainViewModel : ObservableObject, INotifyPropertyChanged {
        private readonly string projectName = "csharpfirebase-8de5b";
        public ObservableCollection<Player> Players { get; set; }
        public RelayCommand HomeViewCommand { get; set; }

        public RelayCommand FeatureViewCommand { get; set; }




        public HomeViewModel HomeVm { get; set; }
        public FeatureViewModel FeatureVM { get; set; }

        private object _currentView;

        public object CurrentView {
            get => _currentView;
            set {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }
        public MainViewModel() {
            Players = new ObservableCollection<Player>();
            //LoadPlayers();
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

        private async void LoadPlayers() {
            try {
                FirestoreDb db = FirestoreDb.Create(projectName);
                Query docRef = db.Collection("Games").Document("Super mario").Collection("Players").OrderByDescending("score");
                QuerySnapshot snapshot = await docRef.GetSnapshotAsync();
                Debug.WriteLine("Loading Players...");
                foreach (DocumentSnapshot docsnap in snapshot.Documents) {
                    Dictionary<string, object> dict = docsnap.ToDictionary();
                    foreach (KeyValuePair<string, object> pair in dict) {
                        Player player = docsnap.ConvertTo<Player>();
                        App.Current.Dispatcher.Invoke(() => {
                            Players.Add(player);
                        });

                    }
                }
            } catch (Exception ex) {
                Debug.WriteLine($"Error loading players: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
