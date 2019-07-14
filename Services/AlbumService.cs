using System.Collections.Generic;
using CourseApi.Entities;
using MongoDB.Driver;
using System.Linq;
using CourseApi.Models;

namespace CourseApi.Services
{
    public class AlbumService
    {
        private readonly IMongoCollection<Album> _albums;
        public AlbumService(IAlbumstoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _albums = database.GetCollection<Album>(settings.AlbumsCollectionName);
        } 
        public List<Album> Get() => 
            _albums.Find(album => true).ToList();
    
        public Album Get(string id) => 
            _albums.Find<Album>(album => album.Id == id).FirstOrDefault();

        public Album Create(Album album)
        {
            _albums.InsertOne(album);
            return album;
        }
        public void Update(string id, Album albumIn) => 
            _albums.ReplaceOne(album => album.Id == id, albumIn);
        public void Remove(Album albumIn) =>
            _albums.DeleteOne(album => album.Id == albumIn.Id);
        public void Remove(string id) => 
            _albums.DeleteOne(album => album.Id == id);
    }
}