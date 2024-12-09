using Google.Cloud.Firestore;
using System;

namespace Firestore_Library {
    /// <summary>
    /// Reprezentuje gracza w bazie danych Firestore.
    /// </summary>
    [FirestoreData]
    public class Player {
        /// <summary>
        /// Unikalny identyfikator dokumentu w kolekcji Firestore.
        /// </summary>
        [FirestoreDocumentId]
        public string Id { get; set; }

        /// <summary>
        /// Nick gracza.
        /// </summary>
        /// <remarks>
        /// Nick to unikalna nazwa, pod którą gracz jest identyfikowany w systemie.
        /// </remarks>
        [FirestoreProperty(nameof(nick))]
        public string nick { get; set; }

        /// <summary>
        /// Wynik gracza.
        /// </summary>
        /// <remarks>
        /// Wynik odzwierciedla osiągnięcia gracza w grze.
        /// </remarks>
        [FirestoreProperty(nameof(score))]
        public int score { get; set; }

        /// <summary>
        /// Czas w formacie <see cref="Timestamp"/>, zapisany w Firestore.
        /// </summary>
        /// <remarks>
        /// Reprezentuje czas ostatniej aktualizacji lub zapisu rekordu gracza.
        /// </remarks>
        [FirestoreProperty(nameof(time))]
        public Timestamp time { get; set; }

        /// <summary>
        /// Stringowa reprezentacja czasu.
        /// </summary>
        /// <remarks>
        /// Może być używana do formatowania czasu w czytelnej formie.
        /// </remarks>
        public string timeString { get; set; }
    }
}
