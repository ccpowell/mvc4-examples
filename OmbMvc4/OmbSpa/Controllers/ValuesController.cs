using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OmbSpa.Models;

namespace OmbSpa.Controllers
{
   public class ValuesController : ApiController
    {
        ProductContext context;

        public ValuesController()
        {
            context = new ProductContext();
        }

        // This is Primary Get (using GET)
        public IQueryable<Product> GetProducts()
        {
            var query = context.Products.AsQueryable<Product>();
            return query;
        }

        // This is Single Get (using GET)
        public HttpResponseMessage GetProduct(int id)
        {
            var prod = context.Products.Where(p => p.ID == id).FirstOrDefault();

            if (prod == null)
            {
                Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, prod);
        }

        // This is Delete (using DELETE)
        public HttpResponseMessage DeleteProduct(int id)
        {
            var prod = context.Products.Where(p => p.ID == id).FirstOrDefault();

            if (prod == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            context.Products.Remove(prod);

            return Request.CreateResponse(HttpStatusCode.OK, prod);
        }

        // This is Add (using POST)
        public HttpResponseMessage PostProduct(Product add)
        {
            add.ID = context.Products.Max(p => p.ID) + 1;
            context.Products.Add(add);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, add);
            response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = add.ID, controller = "Values" }));
            return response;
        }

        // This is Update (using PUT)
        public HttpResponseMessage PutProduct(int id, Product update)
        {
            var prod = context.Products.Where(p => p.ID == id).FirstOrDefault();

            if (prod == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            prod.ID = update.ID;
            prod.Name = update.Name;
            prod.Price = update.Price;

            return Request.CreateResponse(HttpStatusCode.OK, prod);
        }
    }
}