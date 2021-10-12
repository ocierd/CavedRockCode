using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using CavedRockCode.Api.ApiModels;
using CavedRockCode.Api.ApiModels.CavedRockCode.Api.ApiModels;
using System.Linq;

namespace CavedRockCode.Api.Repository
{
    public class CavedRepository : ICavedRepository
    {
        private readonly IDbConnection DbConnection;
        public CavedRepository(IDbConnection dbConnection)
        {
            DbConnection = dbConnection;
        }

        public async Task<List<Product>> GetProducts(string category)
        {
            string sql = "SELECT * FROM Product WHERE Category = COALESCE(@category,Category) OR @category='all'";
            IEnumerable<Product> productos = await DbConnection.QueryAsync<Product>(sql, new { category });
            return productos.ToList();
        }

        public async Task SubmitProduct(QuickOrder order, int customerId, Guid orderId)
        {
            string sqlStatement = @"INSERT INTO [Order](OrderId, CustomerId, ProductId, QuantityOrdered, OrderTotal)
                                    SELECT @OrderId, @CustomerId, @ProductId, @Quantity, @Quantity * P.Price FROM Product P
                                    WHERE P.Id=@ProductId";
            await DbConnection.ExecuteReaderAsync(sqlStatement, new
            {
                orderId,
                customerId,
                order.ProductId,
                order.Quantity
            });
        }
    }

}