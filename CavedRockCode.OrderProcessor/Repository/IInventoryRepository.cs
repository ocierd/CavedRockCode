using System.Threading.Tasks;

namespace CavedRockCode.OrderProcessor.Repository
{
    public interface IInventoryRepository
    {
        Task<int> GetInventoryForProduct(int productId);

        Task UpdateInventoryForProduct(int productId, int newInventory);
    }

}