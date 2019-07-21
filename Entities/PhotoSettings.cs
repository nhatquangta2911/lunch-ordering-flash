using System.IO;
using System.Linq;

namespace CourseApi.Entities
{
    public class PhotoSettings
    {
        public int MaxBytes { get; set; }
        public string[] AcceptedFileTypes { get; set; }
        
        public bool IsSupported(string fileName)
        {
            return AcceptedFileTypes.Any(type => type == Path.GetExtension(fileName).ToLower());
        }
    }
}