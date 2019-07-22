using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using CourseApi.Entities;
using CourseApi.Repositories;
using CourseApi.Services.Dishes;
using CourseApi.Services.PhotoServices;
using CourseApi.Services.PhotoServices.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CourseApi.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("/api/dishes/{dishId}/photos")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IHostingEnvironment _host;
        private readonly IDishRepository _dishRepository;
        private readonly IphotoRepository _photoRepository;
        private readonly IMapper _mapper;
        private readonly PhotoSettings _photoSettings;
        private readonly PhotoService _photoService;
        private readonly DishService _dishService;
        public PhotosController(IHostingEnvironment host,
                                IDishRepository dishRepository,
                                IphotoRepository photoRepository,
                                IMapper mapper,
                                PhotoSettings photoSettings,
                                PhotoService photoService,
                                DishService dishService)
        {
            _host = host;
            _dishRepository = dishRepository;
            _photoRepository = photoRepository;
            _mapper = mapper;
            _photoSettings = photoSettings;
            _photoService = photoService;
            _dishService = dishService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<PhotoDto> GetPhoto(string dishId)
        {
            var photo = await _dishService.GetPhoto(dishId);
            return _mapper.Map<PhotoDto>(photo);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Upload(string dishId, IFormFile file)
        {
            var dish = await _dishRepository.GetDish(dishId, includeRelated: false);
            if(dish == null)
                return NotFound();
            
            if(file == null) return BadRequest("Null file.");
            if(file.Length == 0) return BadRequest("Empty file");
            if(file.Length > _photoSettings.MaxBytes) return BadRequest("Max file size exceeded");
            if(!_photoSettings.IsSupported(file.FileName)) return BadRequest("Invalid file type.");

            var uploadsFolderPath = Path.Combine(_host.WebRootPath, "uploads");
            var photo = await _photoService.UploadPhoto(dish, file, uploadsFolderPath);

            return Ok(Mapper.Map<PhotoDto>(photo));

        }
    }
}