
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Firestore_Library;
using Google.Cloud.Firestore.V1;


public class Program {
    private static async Task Main(string[] args) {
        var cts = new CancellationTokenSource();
        while (true) {
        Console.Clear();
        Console.WriteLine("Leaderboard");
        Console.WriteLine("1. Add Game");
        Console.WriteLine("2. Add Player to the Game");
        Console.WriteLine("3. Show Players For Game");
        Console.WriteLine("4. Delete Player by Game");
        Console.WriteLine("5. Update Player by Game");

        Timestamp timestamp = Timestamp.FromDateTime(DateTime.UtcNow);

        int wybor = Convert.ToInt32(Console.ReadLine());


            switch (wybor)
            {
                case 1:
                    Console.WriteLine("Podaj nazwe gry:");
                    string gameName = Console.ReadLine();
                    while (gameName == null || gameName == "")
                    {
                        gameName = Console.ReadLine();
                    }
                    if (gameName != null && gameName != "") { await Database.addGame(gameName); }
                    break;
                case 2:
                    Console.WriteLine("Podaj nazwe gry:");
                    string gameName1 = Console.ReadLine();
                    while (gameName1 == null || gameName1 == "")
                    {
                        Console.WriteLine("Podaj nazwe gry:");
                        gameName = Console.ReadLine();
                    }
                    if (gameName1 != null && gameName1 != "")
                    {
                        Console.WriteLine("Podaj nick gracza: ");
                        string playerNick = Console.ReadLine();
                        Console.WriteLine("Podaj wynik gracza: ");
                        int playerScore = Convert.ToInt32(Console.ReadLine());
                        while (playerNick == null || playerNick == "" || playerScore == null)
                        {
                            Console.WriteLine("Podaj nick gracza: ");
                            playerNick = Console.ReadLine();
                            Console.WriteLine("Podaj wynik gracza: ");
                            playerScore = Convert.ToInt32(Console.ReadLine());
                        }
                        Player player1 = new Player
                        {

                            nick = playerNick,
                            score = playerScore,
                            time = timestamp
                        };
                        await Database.addPlayerByGame(gameName1, player1);
                    }
                    break;
                case 3:
                    var listenerTask = Task.Run(() => Database.getPlayersByGameRealtime("Super mario", cts.Token));

                    while (true)
                    {
                        var input = Console.ReadLine();
                        if (input == "exit")
                        {
                            cts.Cancel();
                            break;
                        }
                        Console.Write("you typed: " + input);
                    }
                    await listenerTask;
                    break;
                case 4:
                    Console.WriteLine("Podaj nazwe gry:");
                    string gameName2 = Console.ReadLine();
                    Console.WriteLine("Podaj nick gracza do usuniecia: ");
                    string playerName1 = Console.ReadLine();
                    while (playerName1 == null || playerName1 == "" || gameName2 == null || gameName2 == "")
                    {
                        Console.WriteLine("Podaj nazwe gry:");
                        gameName2 = Console.ReadLine();
                        Console.WriteLine("Podaj nick gracza do usuniecia: ");
                        playerName1 = Console.ReadLine();
                    }
                    await Database.deletePlayerByGame(gameName2, playerName1);
                    break;
                case 5:
                    Console.WriteLine("Podaj nazwe gry:");
                    string gameName3 = Console.ReadLine();
                    Console.WriteLine("Podaj nazwe gracza: ");
                    string playerName2 = Console.ReadLine();
                    while (gameName3 == null || gameName3 == "" || playerName2 == null || playerName2 == "")
                    {
                        Console.WriteLine("Podaj nazwe gry:");
                        gameName3 = Console.ReadLine();
                        Console.WriteLine("Podaj nazwe gracza: ");
                        playerName2 = Console.ReadLine();
                    }
                    Console.WriteLine("1. Score");
                    Console.WriteLine("2. Time");
                    int wybor3 = Convert.ToInt32(Console.ReadLine());
                    switch(wybor3)
                    {
                        case 1:
                            Console.WriteLine("Podaj wynik:");
                            int score = Convert.ToInt32(Console.ReadLine());
                            await Database.updatePlayerScoreByGame(gameName3, playerName2, score);
                            break;
                        case 2:
                            await Database.updatePlayerTimeByGame(gameName3, playerName2, timestamp);
                            break;
                    }
                    break;
            }
        }
    }

    public static void getRealTime(string project) {

       /* FirestoreDb db = FirestoreDb.Create(project);

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

        });*/

    }
}
    