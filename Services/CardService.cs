using System.Collections.Generic;
using CourseApi.Models;
using MongoDB.Driver;

namespace CourseApi.Services
{
    public class CardService
    {
        private readonly IMongoCollection<Card> _cards;
        public CardService(ICardstoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _cards = database.GetCollection<Card>(settings.CardsCollectionName);
        }

        public List<Card> Get() => 
            _cards.Find(card => true).ToList();

        public Card Get(string id) =>
            _cards.Find<Card>(card => card.Id == id).FirstOrDefault();

        // public Card GetLatest() =>
            // _cards.Find(card => true);

        public Card Create(Card card)
        {
            _cards.InsertOne(card);
            return card;
        }

        public void Update(string id, Card cardIn)
        {
            _cards.ReplaceOne(card => card.Id == id, cardIn );
        }

        public void Delete(string id)
        {
            _cards.DeleteOne(card => card.Id == id);
        }
    }
}