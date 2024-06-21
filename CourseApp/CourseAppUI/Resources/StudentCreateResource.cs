namespace CourseAppUI.Resources
{
    public class StudentCreateResource
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime BirthDay { get; set; }
        public int GroupId { get; set; }
        public IFormFile FormFile { get; set; }
    }
}
