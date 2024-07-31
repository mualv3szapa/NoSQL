using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using minimalApiMongo.Domains;
using minimalApiMongo.Services;
using minimalApiMongo.ViewModel;
using MongoDB.Driver;

namespace minimalApiMongo.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class OrderController : ControllerBase
    {
        private readonly IMongoCollection<Order> _order;
        private readonly IMongoCollection<Client> _client;
        private readonly IMongoCollection<Product> _product;

        public OrderController(MongoDbService mongoDbService)
        {
            _order = mongoDbService.GetDatabase.GetCollection<Order>("order");
            _client = mongoDbService.GetDatabase.GetCollection<Client>("client");
            _product = mongoDbService.GetDatabase.GetCollection<Product>("product");
        }

        [HttpPost]
        public async Task<ActionResult<Order>> Create(OrderViewModel orderViewModel)
        {
            try
            {
                Order order = new Order();

                order.Id = orderViewModel.Id;
                order.Date = orderViewModel.Date;
                order.Status = orderViewModel.Status;
                order.ProductId = orderViewModel.ProductId;
                order.ClientId = orderViewModel.ClientId;

                var client = await _client.Find(x => x.Id == order.ClientId).FirstOrDefaultAsync();

                if (client == null)
                {
                    return NotFound("Cliente não existe");
                }

                await _order.InsertOneAsync(order);

                return StatusCode(201, order);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]

        public async Task<ActionResult<List<Order>>> Get()
        {
            try
            {
                var orders = await _order.Find(FilterDefinition<Order>.Empty).ToListAsync();

                foreach (var order in orders)
                {
                    if (order.ProductId != null)
                    {
                        var filter = Builders<Product>.Filter.In(p => p.Id, order.ProductId);

                        order.Products = await _product.Find(filter).ToListAsync();
                    }
                    if (order.ClientId != null)
                    {
                        order.Client = await _client.Find(x => x.Id == order.ClientId).FirstOrDefaultAsync();

                    }
                }



                return Ok(orders);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);

            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetById(string id)
        {
            try
            {
                var filter = Builders<Order>.Filter.Eq(x => x.Id, id);
                var Order = await _order.Find(filter).FirstOrDefaultAsync(); 
                return Order == null ? NotFound() : Ok(Order);

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Order>> UpdateOrder(OrderViewModel Order, string id)
        {
            try
            {
                var filter = Builders<Order>.Filter.Eq(x => x.Id, Order.Id);
                Order o = new Order()
                {
                    Id = id,
                    Date = Order.Date,
                    Status = Order.Status,
                    Client = await _client.Find(x => x.Id == Order.ClientId).FirstOrDefaultAsync(),
                    Products = new List<Product>(),
                };
                foreach (var item in Order.ProductId)
                {
                    o.Products.Add(await _product.Find(x => x.Id == item).FirstOrDefaultAsync());
                }
                await _order.ReplaceOneAsync(filter, o);
                return Order == null ? NotFound() : Ok(o);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpDelete("{id}")]

        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var filter = Builders<Order>.Filter.Eq(x => x.Id, id);

                if (filter != null)
                {
                    await _order.DeleteOneAsync(filter);
                    return Ok(filter);
                }
                return NotFound();
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }






    }
}
