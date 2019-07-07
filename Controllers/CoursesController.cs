using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseApi.Controllers {
    [Route ("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase {
        private readonly CourseContext _context;

        public CoursesController (CourseContext context) {
            _context = context;
            if (_context.Courses.Count () == 0) {
                _context.Courses.Add (new Course { Name = "Course1" });
                _context.SaveChanges ();
            }
        }

        //TODO: api/course
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses () {
            return await _context.Courses.ToListAsync ();
        }
        //TODO: api/courses/1
        [HttpGet ("{id}")]
        public async Task<ActionResult<Course>> GetCourse (long id) {
            var course = await _context.Courses.FindAsync (id);
            if (course == null) {
                return NotFound ();
            }
            return course;
        }
        //TODO: api/courses
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, course);
        }
        //TODO: api/courses/1
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(long id, Course course)
        {
            if(id != course.Id)
            {
                return BadRequest();
            }
            _context.Entry(course).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        //TODO: api/courses/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(long id)
        {
            var course = await _context.Courses.FindAsync(id);
            if(course == null)
            {
                return NotFound();
            }
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}