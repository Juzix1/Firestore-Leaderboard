using Firestore_Library;
using Google.Cloud.Firestore;
using GUI.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.MVVM.ViewModel {
    /// <summary>
    /// ViewModel dla widoku domowego aplikacji. Odpowiada za zarządzanie graczami i ich danymi.
    /// </summary>
    public class HomeViewModel : ObservableObject {
        /// <summary>
        /// Kolekcja graczy widoczna w interfejsie użytkownika.
        /// </summary>
        public ObservableCollection<Player> Players { get; set; }

        private readonly string projectName = "csharpfirebase-8de5b";
        /// <summary>
        /// Konstruktor inicjalizujący ViewModel i ładujący graczy z bazy danych.
        /// </summary>
        public HomeViewModel() {
            Players = new ObservableCollection<Player>();
            LoadPlayers();
        }
        /// <summary>
        /// Asynchronicznie ładuje dane graczy z kolekcji Firestore i nasłuchuje zmian w czasie rzeczywistym.
        /// </summary>
        private async void LoadPlayers() {
            try {
                FirestoreDb db = FirestoreDb.Create(projectName);
                Query docRef = db.Collection("Games").Document("Super mario").Collection("Players").OrderByDescending("score");
                FirestoreChangeListener listener = null;

                listener = docRef.Listen(async snapshot => {
                    App.Current.Dispatcher.Invoke(() => {
                        Players.Clear();
                        foreach (DocumentSnapshot docsnap in snapshot.Documents) {
                            Player player = docsnap.ConvertTo<Player>();
                            player.timeString = getTimePart(player.time.ToString());
                            App.Current.Dispatcher.Invoke(() => Players.Add(player));
                        }
                    });
                });


            } catch (Exception ex) {
                Debug.WriteLine($"Error loading players: {ex.Message}");
            }
        }
        // <summary>
        /// Wydobywa część czasu z ciągu znaków Timestamp.
        /// </summary>
        /// <param name="timestamp">Ciąg znaków reprezentujący Timestamp.</param>
        /// <returns>Fragment czasu w formacie "HH:mm:ss".</returns>
        private static string getTimePart(string timestamp) {
            return timestamp.Substring(22, 8);
        }

    }
}
