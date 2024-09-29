using NavYuvakMandarApi.Models;

namespace NavYuvakMandarApi.Repositories
{
    public interface IAartiDeatilsRepository
    {
        Task<List<AartiDetails>> GetAartiDetails();
        Task addAartiDetails(AartiDetails aartiDetails);
    }
}
