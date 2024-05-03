using Buffet_Galina_API.DTO;
using Galina;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Extensions.Msal;
using System.Linq;

namespace Buffet_Galina_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly User01Context _context;
        private readonly OrderNumbers orderNumbers;

        public AdminController(User01Context context, OrderNumbers orderNumbers)
        {
            _context = context;
            this.orderNumbers = orderNumbers;
        }

        [HttpGet("GetAdmin")]
        public async Task<ActionResult<Admin>> GetAdmin(int id)
        {
            var s = _context.Admins.FirstOrDefault(s => s.Id == id);
            if (s == null)
            {
                return NotFound();

            }
            return Ok(new Admin
            {
                Id = s.Id,
                Password = s.Password,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt
            });
        }



        [HttpPost("LoginAdmin")]
        public ActionResult<Admin> LoginAdmin(Admin loginAdmin)
        {

            Admin admin = _context.Admins.FirstOrDefault(a => a.Password == loginAdmin.Password);
            if (admin != null)
            {
                return new Admin
                {
                    Id = admin.Id,
                    Password = admin.Password,
                    CreatedAt = admin.CreatedAt,
                    UpdatedAt = admin.UpdatedAt
                };
            }
            else
            {
                return BadRequest("нЕПРАВИЛЬНЫЙ пароль");
            }

        }

        [HttpPost("AddDish")]
        public async void AddDish(DishDTO dish)
        {
            var dish1 = new Dish1
            {
                Title = dish.Title,
                CategoryId = dish.CategoryId,
                Price = dish.Price,
                Image = dish.Image
            };
            List<Product> tvar = new List<Product>();
            foreach (var product in dish.Products)
            {
                var p = _context.Products.FirstOrDefault(s => s.Id == product.Id);
                if (p != null)
                    tvar.Add(p);
                else
                    tvar.Add(new Product { CreatedAt = p.CreatedAt, Title = p.Title, UpdatedAt = p.UpdatedAt });
            }
            dish1.DishProducts = tvar.Select(p => new DishProduct { Product = p }).ToList();
            _context.Add(dish1);
            _context.SaveChanges();
        }

        [HttpGet("GetDish")]
        public async Task<ActionResult<List<DishDTO>>> GetDish()
        {
            var h = _context.DishProducts.Include(s => s.Product).Include(s => s.Dish).
                ThenInclude(s => s.Category).OrderBy(s => s.DishId).ToList().GroupBy(s => s.Dish);


            var hz = h.Select(s => new DishDTO { Category = s.Key.Category.Title, CategoryId = s.Key.CategoryId,
                Price = s.Key.Price, Image = s.Key.Image, Title = s.Key.Title, 
                Id = s.Key.Id, Products = s.Select(d => new ProductDTO { CreatedAt = d.CreatedAt,
                    Id = d.ProductId, Title = d.Product.Title, UpdatedAt = d.UpdatedAt }).ToList() });

            return hz.ToList();
            //var t = _context.Dish1s.Include(s => s.Category).Include(s => s.DishProducts).ThenInclude(s => s.Product).ToList(); 
            //List<DishDTO> dishes = t.Select(s => new DishDTO
            //{
            //    Id = s.Id,
            //    Title = s.Title,
            //    CategoryId = s.CategoryId,
            //    Category = s.Category.Title,
            //    Price = s.Price,
            //    Products = s.DishProducts?.Select(d => d.Product).ToList()

            //}).ToList();
            //return dishes;
        }

        [HttpGet("GetDishByCategory")]
        public async Task<ActionResult<List<DishDTO>>> GetDish(int category)
        {
            var h = _context.DishProducts.Include(s => s.Product).Include(s => s.Dish).
                ThenInclude(s => s.Category).Where(s => s.Dish.CategoryId == category).OrderBy(s => s.DishId).ToList().GroupBy(s => s.Dish);


            var hz = h.Select(s => new DishDTO
            {
                Category = s.Key.Title,
                CategoryId = s.Key.CategoryId,
                Price = s.Key.Price,
                Image = s.Key.Image,
                Title = s.Key.Title,
                Id = s.Key.Id,
                Products = s.Select(d => new ProductDTO
                {
                    CreatedAt = d.CreatedAt,
                    Id = d.ProductId,
                    Title = d.Product.Title,
                    UpdatedAt = d.UpdatedAt
                }).ToList()
            });

            return hz.ToList();
        }

        [HttpGet("GetCategories")]
        public async Task<ActionResult<List<CategoryDTO>>> GetCategories()
        {
            List<CategoryDTO> categories = _context.Categories.ToList().Select(s => new CategoryDTO { Id = s.Id, Title = s.Title, CreatedAt = s.CreatedAt, UpdateAt = s.UpdateAt }).ToList();
            return categories;
        }

        [HttpGet("GetProducts")]
        public async Task<ActionResult<List<ProductDTO>>> GetProducts()
        {
            List<ProductDTO> products = _context.Products.ToList().Select(s => new ProductDTO { Id = s.Id, Title = s.Title, CreatedAt = s.CreatedAt, UpdatedAt = s.UpdatedAt }).ToList();
            return products;
        }


        [HttpGet("AddOrder")]
        public async Task<ActionResult<int>> AddOrder()
        {
            Order order = new Order() { Number = orderNumbers.GetNextNumber(), CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
            _context.Orders.Add(order);
            _context.SaveChanges();

            return Ok(order.Id);

        }

        [HttpPut("{orderid}/{dishid}/{count}")]
        public async Task<ActionResult> AddDishToOrder(int orderid, int dishid, int count)
        {
            var dish = await _context.Dish1s.FindAsync(dishid);
            var order = await _context.Orders.Include(s=>s.OrderDishes).FirstOrDefaultAsync(s=>s.Id==orderid);
            if (dish == null || order == null)
            {
                return NotFound();
            }
            var addeddish = order.OrderDishes.FirstOrDefault(s => s.DishId == dishid);
            if (addeddish == null)
            {
                order.OrderDishes.Add(new OrderDish
                {
                    OrderId = orderid,
                    DishId = dishid,
                    Order = order,
                    Dish = dish,
                    Value = count,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
            }
            else
            {
                addeddish.Value++;
            }
            _context.Entry(order).State = EntityState.Modified; 
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete("DeleteOrder/{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            var orderdish = _context.OrderDishes.Where(s => s.DishId == id).ToList();
            _context.OrderDishes.RemoveRange(orderdish);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("DeleteDishInOrder/{id}")]
        public async Task<IActionResult> DeleteDishInOrder(int id)
        {
            if (_context.Dish1s == null) 
            {
                return NotFound();
            }
            var dish = await _context.Dish1s.FindAsync(id);
            if (dish == null) 
            {
                return NotFound();
            }
            var fig = _context.OrderDishes.Where(s =>s.DishId== id).ToList();
            _context.OrderDishes.RemoveRange(fig);
            _context.RemoveRange(fig);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDishes(int id)
        {
            if (_context.Dish1s == null)
            {
                return NotFound();
            }
            var dish = await _context.Dish1s.FindAsync(id);
            if (dish == null)
            {
                return NotFound();
            }
            var hren = _context.DishProducts.Where(s => s.DishId == id).ToList();
            _context.DishProducts.RemoveRange(hren);
            _context.Dish1s.Remove(dish);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Dish1>> EditDish(int id, DishDTO dishDTO)
        {
            // Проверка, что переданные данные валидны
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Поиск существующего товаров в базе данных
            var existingDish = await _context.Dish1s.Include(s=>s.Category).Include(s=>s.DishProducts).FirstOrDefaultAsync(s => s.Id == id);
            if (existingDish == null)
            {
                return NotFound();
            }

            // Обновление свойств существующего товара
            existingDish.Title = dishDTO.Title;
            existingDish.CategoryId = dishDTO.CategoryId;
            existingDish.Price = dishDTO.Price;
            existingDish.Image = dishDTO.Image;
            _context.DishProducts.RemoveRange(existingDish.DishProducts);

            existingDish.DishProducts.Clear();
            existingDish.DishProducts = dishDTO.Products.Select(p => new DishProduct
            {
                DishId = id,
                Product = _context.Products.Find(p.Id)
            }).ToList();


           

            try
            { 
                _context.Entry(existingDish).State = EntityState.Modified;
                // Сохранение изменений в базе данных
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                if (!DishExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Возвращение обновленного объекта
            return NoContent();
        }


        private bool DishExists(int id)
        {
            return (_context.Dish1s?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        
    }
}
