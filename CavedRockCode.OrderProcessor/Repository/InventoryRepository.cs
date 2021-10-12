using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace CavedRockCode.OrderProcessor.Repository
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly IDbConnection DbConnection;

        public InventoryRepository(IDbConnection dbConnection)
        {
            DbConnection = dbConnection;
        }

        public async Task<int> GetInventoryForProduct(int productId)
        {
            return await DbConnection.ExecuteScalarAsync<int>(
                "SELECT QuantityOnHand FROM dbo.Inventory WHERE ProductId = @ProductId",
                new { productId });
        }

        public async Task UpdateInventoryForProduct(int productId, int newInventory)
        {
            await DbConnection.ExecuteAsync("UPDATE Inventory SET QuantityOnHand = @newInventory WHERE ProductId = @productId"
            , new { productId, newInventory });
        }
    }

}