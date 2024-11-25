using Google.Cloud.Firestore;


namespace Firestore_Library {
    public class Database {
        private static readonly string projectName = "csharpfirebase-8de5b";
        public static async Task getCollectionPlayers(string collectionName) {

            FirestoreDb db = FirestoreDb.Create(projectName);

            Query docRef = db.Collection("Games").Document(collectionName).Collection("Players").OrderBy("score");

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
        public static async Task getCollectionPlayersRealtime(string collectionName, CancellationToken token) {
            FirestoreDb db = FirestoreDb.Create(projectName);

            Query docRef = db.Collection("Games").Document("Super mario").Collection("Players").OrderByDescending("score");

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

        private static string getTimePart(string timestamp) {
            return timestamp.Substring(22, 8);
        }
    }
}
