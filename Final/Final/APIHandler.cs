using Newtonsoft.Json;
using SQLite.Net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Syndication;

namespace Final
{
    class APIHandler
    {
        private String APIResult;
        private DatabaseHandler DBH = new DatabaseHandler();
        private HttpClient client = new HttpClient();
        private Uri gatherer = new Uri("https://api.magicthegathering.io/v1/cards");



        public void PopulateDatabaseFromApi()
        {
            var DB = DBH.DbConnection;
            //Card card;
            var temp = client.GetAsync(gatherer);

            //Debug.WriteLine(temp);
            //List<Card> cards = (from p in DB.Table<Card>() select p).ToList();
            CallApi();


        }

        public async void CallApi()
        {

            HttpClient webClient = new HttpClient();

            var http = new HttpClient();
            var response = await http.GetAsync(gatherer);
            APIResult = await response.Content.ReadAsStringAsync();

            Object temp = JsonConvert.DeserializeObject<Card>(APIResult);

            Debug.WriteLine(temp);

            //Object p2 = (Object)ser.ReadObject(response);

        }



    }
}

