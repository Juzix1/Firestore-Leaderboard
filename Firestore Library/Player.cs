using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firestore_Library {
    [FirestoreData]
    public class Player {
        [FirestoreDocumentId]
        public string Id { get; set; }
        [FirestoreProperty(nameof(nick))]
        public string nick {  get; set; }
        [FirestoreProperty(nameof(score))]
        public int score { get; set; }
        [FirestoreProperty(nameof(time))]
        public Timestamp time { get; set; }

    }
}
