using Newtonsoft.Json;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Final
{
    public class MainData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public LoadBtnCmd LoadBtnCmd { get; }
        public Card CurrentCard { get; set; }
        public ObservableCollection<Card> Cards { get; set; }
        public string CardName { get; set;}

        private string _filter;
        private List<Card> _AllCards = new List<Card>();
        private String APIResult;
        private HttpClient client = new HttpClient();
        private Uri gatherer = new Uri("https://api.magicthegathering.io/v1/cards");
        private Card _selectedCard;
        private String _cardData;
        public string CardDataString;


        public MainData()
        {
            LoadBtnCmd = new LoadBtnCmd(this);
            Cards = new ObservableCollection<Card>();

            DbConnection.CreateTable<Card>();
            //var info = DBH.DbConnection.GetMapping(typeof(Card));

            LoadCards();
            //CallApi();
        }
        

        public Card SelectedCard
        {
            get
            { return _selectedCard; }
            set
            {
                _selectedCard = value;
                if (value == null)
                {
                    CurrentCard = null;
                    CardDataString = null;
                }
                else
                {
                    CurrentCard = value;
                    CardDataString = "Name: " + CurrentCard.name + 
                        "\nType: " + CurrentCard.type + 
                        "\nColor: " + CurrentCard.colorsBlobbed + 
                        "\nRarity: " + CurrentCard.rarity + 
                        "\nManaCost: " + CurrentCard.manaCost + 
                        "\nCMC: " + CurrentCard.cmc + 
                        "\nText: " + CurrentCard.text;

                    CurrentCard.cardDataString = CardDataString;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentCard"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CardDataString"));
            }
        }


        public string Filter
        {
            get
            {
                return _filter;
            }
            set
            {
                if (value == _filter)
                {
                    return;
                }
                _filter = value;
                PropertyChanged?.Invoke(this,
                    new PropertyChangedEventArgs(nameof(Filter)));
                PerformFiltering();
            }
        }

        private void PerformFiltering()
        {
            if (_filter == null)
            {
                _filter = "";
            }
            var lowerCaseFilter = Filter.ToLowerInvariant().Trim();
            var result = _AllCards.Where(
                d => d.name.ToLowerInvariant()
                        .Contains(lowerCaseFilter)).ToList();
            var toRemove = Cards.Except(result).ToList();
            foreach (var x in toRemove)
            {
                Cards.Remove(x);
            }
            // Add back in the correct order.
            var resultcount = result.Count;
            for (int i = 0; i < resultcount; i++)
            {
                var resultItem = result[i];
                if (i + 1 > Cards.Count || !Cards[i].Equals(resultItem))
                {
                    Cards.Insert(i, resultItem);
                }
            }
        }



        public void LoadCards()
        {
            List<Card> tempCards = (from p in DbConnection.Table<Card>() select p).ToList();

            foreach(var card in tempCards)
            {
                _AllCards.Add(card);
                Cards.Add(card);
                
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LoadCards)));

        }

        public void PopulateDatabase()
        {
            
            foreach (var card in _AllCards)
            {
                try
                {
                    foreach (var color in card.colors)
                    {
                        card.colorsBlobbed += (color + " ");
                    }
                    foreach (var colorIdentity in card.colorIdentity)
                    {
                        card.colorIdentityBlobbed += (colorIdentity + " ");
                    }
                    foreach (var type in card.types)
                    {
                        card.typesBlobbed += (type + " ");
                    }
                    foreach (var subtype in card.subtypes)
                    {
                        card.subtypesBlobbed += (subtype + " ");
                    }
                    foreach (var printing in card.printings)
                    {
                        card.printingsBlobbed += (card.printings + " ");
                    }
                    foreach (var legality in card.legalities)
                    {
                        card.legalitiesBlobbed += (card.legalities + " ");
                    }
                    foreach (var ruling in card.rulings)
                    {
                        card.rulingsBlobbed += (card.rulings + " ");
                    }
                    foreach (var supertype in card.supertypes)
                    {
                        card.supertypesBlobbed += (card.supertypes + " ");
                    }
                }
                catch { }
                
              
                DbConnection.InsertOrReplace(card);
            }

            LoadCards(); 
        }

        public async void CallApi()
        {
            _AllCards = new List<Card>();
            HttpClient webClient = new HttpClient();

            var http = new HttpClient();
            var response = await http.GetAsync(gatherer);
            APIResult = await response.Content.ReadAsStringAsync();

            var temp = JsonConvert.DeserializeObject<RootObject>(APIResult);

            foreach (var card in temp.cards)
            {
                _AllCards.Add(card);
            }

            PopulateDatabase();

        }

        public SQLiteConnection DbConnection
        {
            get
            {
                return new SQLiteConnection(
                    new SQLitePlatformWinRT(),
                    Path.Combine(ApplicationData.Current.LocalFolder.Path, "Storage.sqlite"));
            }
        }
    }
}
