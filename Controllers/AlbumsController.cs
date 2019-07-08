using System.Collections.Generic;
using CourseApi.Models;
using CourseApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumsController : ControllerBase
    {
        private readonly AlbumService _albumService;
        public AlbumsController(AlbumService albumsService)
        {
            _albumService = albumsService;
        }
        [HttpGet]
        public ActionResult<List<Album>> Get() =>
            _albumService.Get();

        [HttpGet("{id:length(24)}", Name="GetAlbum")]
        public ActionResult<Album> Get(string id)
        {   
            var book = _albumService.Get(id);
            if(book == null)
                return NotFound();
            return book;
        }
        [HttpPost]
        public ActionResult<Album> Create(Album album)
        {
            _albumService.Create(album);
            return CreatedAtRoute("GetAlbum", new { id = album.Id.ToString() }, album);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Album albumIn)
        {
            var album = _albumService.Get(id);
            if(album == null)
                return NotFound();
            _albumService.Update(id, albumIn);
            return NoContent();
        }
        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var album = _albumService.Get(id);
            if(album == null)
                return NotFound();
            _albumService.Remove(album.Id);
            return NoContent();
        }

    }
}