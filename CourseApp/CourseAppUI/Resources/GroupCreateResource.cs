using System.ComponentModel.DataAnnotations;

namespace CourseAppUI.Resources
{
    public class GroupCreateResource
    {
        [MaxLength(5)]
        [MinLength(4)]
        public string No { get; set; }
        [Range(5, 18)]
        public byte Limit { get; set; }
        //public IFormFile File { get; set; }
    }
}
