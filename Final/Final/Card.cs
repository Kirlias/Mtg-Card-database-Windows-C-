using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLiteNetExtensions.Attributes;

namespace Final
{
    public class Legality
    {
        public string format { get; set; }
        public string legality { get; set; }
    }

    public class Ruling
    {
        public string date { get; set; }
        public string text { get; set; }
    }

    public class Card
    {
        public string name { get; set; }

        public string manaCost { get; set; }
        public int cmc { get; set; }

        [TextBlob("colorsBlobbed")]
        public List<string> colors { get; set; }
        public string colorsBlobbed { get; set; }

        [TextBlob("colorIdentityBlobbed")]
        public List<string> colorIdentity { get; set; }
        public string colorIdentityBlobbed { get; set; }

        public string type { get; set; }

        [TextBlob("typesBlobbed")]
        public List<string> types { get; set; }
        public string typesBlobbed { get; set; }

        [TextBlob("subtypesBlobbed")]
        public List<string> subtypes { get; set; }
        public string subtypesBlobbed { get; set; }

        public string rarity { get; set; }
        public string set { get; set; }
        public string setName { get; set; }
        public string text { get; set; }
        public string flavor { get; set; }
        public string artist { get; set; }
        public string power { get; set; }
        public string toughness { get; set; }
        public string layout { get; set; }
        public int multiverseid { get; set; }
        public string imageUrl { get; set; }

        



        [TextBlob("printingsBlobbed")]
        public List<string> printings { get; set; }
        public string printingsBlobbed { get; set; }

        public string originalText { get; set; }
        public string originalType { get; set; }

        [TextBlob("legalitiesBlobbed")]
        public List<Legality> legalities { get; set; }
        public string legalitiesBlobbed;

        [PrimaryKey]
        public string id { get; set; }

        public bool? reserved { get; set; }

        [TextBlob("rulingsBlobbed")]
        public List<Ruling> rulings { get; set; }
        public string rulingsBlobbed { get; set; }

        [TextBlob("supertypesBlobbed")]
        public List<string> supertypes { get; set; }
        public string supertypesBlobbed { get; set; }

        [TextBlob("variationsBlobbed")]
        public List<int?> variations { get; set; }
        public int? variationsBlobbed { get; set; }

        public string cardDataString { get; set; }


    }


    public class RootObject
    {
        public List<Card> cards { get; set; }
    }
}
