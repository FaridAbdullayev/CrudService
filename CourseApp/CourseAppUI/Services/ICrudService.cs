using CourseAppUI.Resources;

namespace UniversityApp.UI.Services
{
    public interface ICrudService
    {
        Task<PaginatedResponseResource<TResponse>> GetAllPaginated<TResponse>(string path, int page);
        Task<TResponse> Get<TResponse>(string path);
    }
}
