using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OmbSpa.Models
{
    public class ProductContext
    {
        static IList<Product> productlist = CreateProductList();

        static IList<Product> CreateProductList()
        {
            return new List<Product>()
            {
                new Product()
                {
                    ID = 1,
                    Name = "Mountain Bike",
                    Price = 1995.99M
                },
                new Product()
                {
                    ID = 2,
                    Name = "Road Bike",
                    Price = 1549.95M
                },
                new Product()
                {
                    ID = 3,
                    Name = "Helmet",
                    Price = 29.95M
                },
                new Product()
                {
                    ID = 4,
                    Name = "Shorts",
                    Price = 39.95M
                }
            };
        }

        public IList<Product> Products
        {
            get
            {
                return productlist;
            }
        }
    }
}