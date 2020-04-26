using NorthwindBlazor.Database;

namespace NorthwindBlazor.Server.Services
{
    public interface INorthwindDbService
    {
        NorthwindDbContext GetNorthwindDb();
    }
}