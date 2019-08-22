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
using Task01.Filters;
using Task01.Models;

namespace Task01.Controllers
{
    [RoutePrefix("Clients")]
    [APIErrorException]
    public class ClientsController : ApiController
    {
        private FabricsEntities db = new FabricsEntities();

        public ClientsController() {
            // 因為EF內有導覽屬性問題會導致LOOP參考的問題，因此需先關閉延遲載入的特性
            db.Configuration.LazyLoadingEnabled = false;
        }

        // GET: api/Clients
        [Route("")]
        //public IQueryable<Client> GetClient()
        public HttpResponseMessage GetClient()
        {
            return new HttpResponseMessage() {
                StatusCode = HttpStatusCode.OK,
                Content = new ObjectContent<IQueryable<Client>>(db.Client.Take(10), GlobalConfiguration.Configuration.Formatters.JsonFormatter)
            };
        }

        // GET: api/Clients/5
        [Route("{id}", Name = nameof(GetClientByID))]
        [ResponseType(typeof(Client))]
        public IHttpActionResult GetClientByID(int id)
        {
            Client client = db.Client.Find(id);

            //if (client == null)
            //{
            //    return NotFound();
            //}

            throw new Exception("未知錯誤");

            return Ok(client);
        }


        [Route("{id}/orders")]
        [ResponseType(typeof(Client))]
        public IHttpActionResult GetClientOrders(int id) {
            var orders = db.Order.Where(o => o.ClientId == id);
            //if (orders.Any() == false) {
            //    return NotFound();
            //}

            return Ok(orders.ToList());
        }

        [Route("{id}/orders/{date:datetime}")]
        [ResponseType(typeof(Client))]
        public IHttpActionResult GetClientOrders1(int id, DateTime date) {
            DateTime _dt = date.AddDays(7);
            var orders = db.Order.Where(o => o.ClientId == id && o.OrderDate >= date && o.OrderDate <= _dt);
            if (orders.Any() == false) {
                return NotFound();
            }

            return Ok(orders.ToList());
        }


        [Route("{id}/orders/{*date:datetime}")]
        [ResponseType(typeof(Client))]
        public IHttpActionResult GetClientOrders2(int id, DateTime date) {
            DateTime _dt = date.AddDays(7);
            var orders = db.Order.Where(o => o.ClientId == id && o.OrderDate >= date && o.OrderDate <= _dt);
            if (orders.Any() == false) {
                return NotFound();
            }

            return Ok(orders.ToList());
        }

        // PUT: api/Clients/5
        [Route("{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutClient(int id, Client client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != client.ClientId)
            {
                return BadRequest();
            }

            db.Entry(client).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id))
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

        // POST: api/Clients
        [Route("")]
        [ResponseType(typeof(Client))]
        public IHttpActionResult PostClient(Client client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Client.Add(client);
            db.SaveChanges();

            return CreatedAtRoute(nameof(GetClientByID), new { id = client.ClientId }, client);
        }

        // DELETE: api/Clients/5
        [Route("{id}")]
        [ResponseType(typeof(Client))]
        public IHttpActionResult DeleteClient(int id)
        {
            Client client = db.Client.Find(id);
            if (client == null)
            {
                return NotFound();
            }

            db.Client.Remove(client);
            db.SaveChanges();

            return Ok(client);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ClientExists(int id)
        {
            return db.Client.Count(e => e.ClientId == id) > 0;
        }
    }
}