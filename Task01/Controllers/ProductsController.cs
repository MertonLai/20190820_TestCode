using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Task01.Models;

namespace Task01.Controllers
{

    /// <summary>
    /// 產品相關API服務
    /// </summary>
    [RoutePrefix("Products")]
    public class ProductsController : ApiController
    {
        private FabricsEntities db = new FabricsEntities();

        /// <summary>
        /// 產品相關服務API類別
        /// </summary>

        public ProductsController() {
            // 因為EF內有導覽屬性問題會導致LOOP參考的問題，因此需先關閉延遲載入的特性
             db.Configuration.LazyLoadingEnabled = false;
        }

        /// <summary>
        /// 取得所有產品資訊
        /// </summary>
        /// <returns></returns>
        [Route("")]
        // GET: api/Products
        //public IQueryable<Product> GetProduct()
        public IHttpActionResult GetProduct()
        {
            return Ok(db.Product);
        }

        // GET: api/Products/5
        /// <summary>
        /// 取得單一品項產品資訊
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}", Name = nameof(GetProduct) + "ByID")]
        [ResponseType(typeof(Product))]
        public IHttpActionResult GetProduct(int id)
        {
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT: api/Products/5
        /// <summary>
        /// 修改單一產品資訊
        /// </summary>
        /// <param name="id"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        [Route("{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.ProductId)
            {
                return BadRequest();
            }

            db.Entry(product).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Products
        /// <summary>
        /// 新增產品資訊
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [Route("")]
        [ResponseType(typeof(Product))]
        public IHttpActionResult PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Product.Add(product);
            db.SaveChanges();

            return CreatedAtRoute(nameof(GetProduct) + "ByID", new { id = product.ProductId }, product);
        }

        // DELETE: api/Products/5
        /// <summary>
        /// 刪除產品資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        [ResponseType(typeof(Product))]  //  <-- 給文件產生器使用的
        public IHttpActionResult DeleteProduct(int id)
        {
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            db.Product.Remove(product);
            db.SaveChanges();

            return Ok(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return db.Product.Count(e => e.ProductId == id) > 0;
        }
    }
}