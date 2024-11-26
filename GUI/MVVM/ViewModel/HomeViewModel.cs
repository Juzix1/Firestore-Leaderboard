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
    public class HomeViewModel : ObservableObject {
        public ObservableCollection<Player> Players { get; set; }

        private readonly string projectName = "csharpfirebase-8de5b";

        public HomeViewModel() {
            Players = new ObservableCollection<Player>();
            LoadPlayers();
        }

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
                            Debug.WriteLine(player.time);
                            App.Current.Dispatcher.Invoke(() => Players.Add(player));
                        }
                    });
                });


            } catch (Exception ex) {
                Debug.WriteLine($"Error loading players: {ex.Message}");
            }
        }
        private static string getTimePart(string timestamp) {
            return timestamp.Substring(22, 8);
        }

    }
}
