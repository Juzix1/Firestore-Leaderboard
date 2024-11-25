
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Firestore_Library;


public class Program {
    private static async Task Main(string[] args) {
        var cts = new CancellationTokenSource();
        
        
        var listenerTask = Task.Run(() => Database.getCollectionPlayersRealtime("Super mario",cts.Token));

        while (true) {
            var input = Console.ReadLine();
            if (input == "exit") {
                cts.Cancel();
                break;
            }
            Console.Write("you typed: " + input);
        }
        await listenerTask;

    }

    public static void getRealTime(string project) {

        FirestoreDb db = FirestoreDb.Create(project);

        Query query = db.Collection("cities").WhereEqualTo("State", "CA");

        FirestoreChangeListener listener = query.Listen(snapshot => {
            Console.Clear();
            foreach (DocumentChange change in snapshot.Changes) {
                if (change.ChangeType.ToString() == "Added") {
                    Console.WriteLine("New City: {0}", change.Document.Id);

                } else if (change.ChangeType.ToString() == "Modified") {
                    Console.WriteLine("Modified City: {0}", change.Document.Id);

                } else if (change.ChangeType.ToString() == "Removed") {
                    Console.WriteLine("Removed City: {0}", change.Document.Id);
                }
            }

        });

    }
}
    