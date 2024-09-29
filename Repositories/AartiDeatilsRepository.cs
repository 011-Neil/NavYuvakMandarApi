using NavYuvakMandarApi.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;

namespace NavYuvakMandarApi.Repositories
{
    public class AartiDeatilsRepository : IAartiDeatilsRepository
    {
        private readonly navYuvakMandarDBContext _dbContext;

        public AartiDeatilsRepository(navYuvakMandarDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task addAartiDetails(AartiDetails aartiDetails)
        {
            _dbContext.AartiData.Add(aartiDetails);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<AartiDetails>> GetAartiDetails()
        {
            return await _dbContext.AartiData.ToListAsync();

        }
    }
}
