using Google.Cloud.Firestore;
using Grpc.Core;
using System.Numerics;


namespace Firestore_Library {
    public class Database {

        private static readonly string projectName = "csharpfirebase-8de5b";
        /// <summary>
        /// Wyświetla Wszystkie rekordy graczy z bazy danych
        /// </summary>
        /// <param name="gameName"></param>
        /// <returns></returns>
        public static async Task getCollectionPlayers(string gameName) {

            FirestoreDb db = FirestoreDb.Create(projectName);


            Query docRef = db.Collection("Games").Document(gameName).Collection("Players").OrderBy("score");


            QuerySnapshot snapshot = await docRef.GetSnapshotAsync();
            foreach (DocumentSnapshot docsnap in snapshot.Documents) {

                Dictionary<string, object> dict = docsnap.ToDictionary();
                foreach (KeyValuePair<string, object> obj in dict) {
                    if (obj.Key == "time") {
                        Console.WriteLine("Time: {0}", getTimePart(obj.Value.ToString()));

                    } else {
                        Console.WriteLine("{0}: {1}", obj.Key, obj.Value);
                    }
                }


            }


        }
        /// <summary>
        /// Implementuje Listener który nasłuchuje zmian w bazie danych na bieżąco.
        /// </summary>
        /// <param name="gameName"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task getPlayersByGameRealtime(string gameName, CancellationToken token) {
            FirestoreDb db = FirestoreDb.Create(projectName);

            Query docRef = db.Collection("Games").Document(gameName).Collection("Players").OrderByDescending("score");

            FirestoreChangeListener listener = null;

            listener = docRef.Listen(async snapshot => {
                if (token.IsCancellationRequested) {
                    await listener.StopAsync(); return;
                }

                var counter = 0;
                Console.Clear();
                foreach (DocumentSnapshot docsnap in snapshot.Documents) {


                    counter += 1;
                    Player player = docsnap.ConvertTo<Player>();
                    Console.WriteLine($"{counter}.{player.nick}:");
                    Console.WriteLine("Score: {0}", player.score);
                    Console.WriteLine("Time: {0}", getTimePart(player.time.ToString()));
                    Console.WriteLine("");

                }
                while (!token.IsCancellationRequested) {
                    await Task.Delay(1000);
                }

            });
        }
        /// <summary>
        /// Dodaje gracza do istniejącej gry
        /// </summary>
        /// <param name="gameName"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public static async Task addPlayerByGame(string gameName, Player player)
        {
            FirestoreDb db = FirestoreDb.Create(projectName);

            try
            {
                DocumentReference docRef = db.Collection("Games").Document(gameName).Collection("Players").Document(player.nick);
                await docRef.SetAsync(player);
                Console.WriteLine($"Dodano wynik gracza {player.nick} do gry {gameName}");
            }
            catch (Exception ex) 
            { 
                Console.WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// Metoda dodająca Kategorię gry, wprowadza się jej nazwę
        /// </summary>
        /// <param name="gameName"></param>
        /// <returns></returns>
        public static async Task addGame(string gameName)
        {
            FirestoreDb db = FirestoreDb.Create(projectName);

            try
            {
                DocumentReference docRef = db.Collection("Games").Document(gameName);
                await docRef.SetAsync(gameName);
                Console.WriteLine($"Dodano gre {gameName} do bazy danych");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Dodanie gry do bazy danych nie powiodlo sie pomyslnie. Prawdopodobnie taka gra już istnieje");
            }
        }
        /// <summary>
        /// Aktualizacja wartości wyniku dla gracza
        /// </summary>
        /// <param name="gameName"></param>
        /// <param name="playerID"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public static async Task updatePlayerScoreByGame(string gameName, string playerID, int score)
        {
            FirestoreDb db = FirestoreDb.Create(projectName);

            try
            {
                DocumentReference docRef = db.Collection("Games").Document(gameName).Collection("Players").Document(playerID);
                await docRef.UpdateAsync("score", score);
                Console.WriteLine($"Zmieniono wynik gracza {playerID} na {score}");
            }
            catch (Exception ex) 
            {
                Console.WriteLine("Zmiana wyniku gracza nie powiodla sie pomyslnie");
            }
        }
        /// <summary>
        /// Aktualizacja wartości czasu timera dla gracza
        /// </summary>
        /// <param name="gameName"></param>
        /// <param name="playerID"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static async Task updatePlayerTimeByGame(string gameName, string playerID, Timestamp timestamp)
        {
            FirestoreDb db = FirestoreDb.Create(projectName);

            try
            {
                DocumentReference docRef = db.Collection("Games").Document(gameName).Collection("Players").Document(playerID);
                await docRef.UpdateAsync("time", timestamp);
                Console.WriteLine($"Zmieniono czas gracza {playerID} na {timestamp}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Zmiana czasu gracza nie powiodla sie pomyslnie");
            }
        }

        /// <summary>
        /// Metoda usuwająca gracza z bazy danych
        /// </summary>
        /// <param name="gameName"></param>
        /// <param name="playerID"></param>
        /// <returns></returns>
        public static async Task deletePlayerByGame(string gameName, string playerID)
        {
            FirestoreDb db = FirestoreDb.Create(projectName);

            DocumentReference docRef = db.Collection("Games").Document(gameName).Collection("Players").Document(playerID);
            await docRef.DeleteAsync();
        }
        /// <summary>
        /// Metoda usuwająca kategorię gry z bazy danych
        /// </summary>
        /// <param name="gameName"></param>
        /// <returns></returns>
        public static async Task deleteGame(string gameName)
        {
            FirestoreDb db = FirestoreDb.Create(projectName);

            DocumentReference docRef = db.Collection("Games").Document(gameName);
            await docRef.DeleteAsync();
        }

        private static string getTimePart(string timestamp) {
            return timestamp.Substring(22, 8);
        }
    }
}
