using Dapper;
using FreeCourse.Shared.Dtos;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Services.Discount.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IConfiguration _configuration;
        private readonly IDbConnection _dbConnection;

        public DiscountService(IConfiguration configuration)
        {
            _configuration = configuration;
            _dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("PostgreSql"));
        }

        public async Task<Response<NoContent>> Delete(int id)
        {
            var deleteStatus = await _dbConnection.ExecuteAsync($"delete from discount where id={id}");

            return deleteStatus > 0 ? Response<NoContent>.Success(204) : Response<NoContent>.Fail("discount not found", 500);


        }

        public async Task<Response<List<Model.Discount>>> GetAll()
        {
            var discounts = await _dbConnection.QueryAsync<Model.Discount>("Select * from discount");

            return Response<List<Model.Discount>>.Success(discounts.ToList(),200);

        }

        public async Task<Response<Model.Discount>> GetByCodeAndUserId(string code, string userId)
        {
            var discounts = await _dbConnection.QueryAsync<Model.Discount>($"select * from discount where code='{code}' and userid='{userId}'");

            return discounts.FirstOrDefault()==null ? Response<Model.Discount>.Fail("Discount not found",500):Response<Model.Discount>.Success(discounts.FirstOrDefault(),200); 
        }

        public async Task<Response<Model.Discount>> GetById(int id)
        {
            var discount = (await _dbConnection.QueryAsync<Model.Discount>($"Select * from discount where id={id}")).SingleOrDefault();

            if (discount==null)
            {
                return Response<Model.Discount>.Fail("Discount not found", 404);
            }

            return Response<Model.Discount>.Success(discount, 200);
        }

        public async Task<Response<NoContent>> Save(Model.Discount discount)
        {
            var saveStatus = await _dbConnection.ExecuteAsync("insert into discount (userid,rate,code) values(@UserId,@Rate,@Code)",discount);

            if (saveStatus>0)
            {
                return Response<NoContent>.Success(204);
            }
            return Response<NoContent>.Fail("An  error occured While adding", 500);

        }

        public async Task<Response<NoContent>> Update(Model.Discount discount)
        {
            var updateStatus = await _dbConnection.ExecuteAsync("update  discount set userid=@UserId,rate=@Rate,code=@Code where id=@Id",new
            {
                Id=discount.Id,
                UserId=discount.UserId,
                Code=discount.Code,
                Rate=discount.Rate,

            });

            if (updateStatus > 0)
            {
                return Response<NoContent>.Success(204);
            }
            return Response<NoContent>.Fail("Discount not found", 404);
        }
    }
}
