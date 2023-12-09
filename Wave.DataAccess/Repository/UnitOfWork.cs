﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wave.DataAccess.Data;
using Wave.DataAccess.Repository.IRepository;

namespace Wave.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public ApplicationDbContext _db;
        public ICategoryRepository Category { get; set; }
        public IProductRepository Product { get; set; }
        public IProductImageRepository ProductImage { get; set; }
        public IShoppingCartRepository ShoppingCart { get; set; }
        public IApplicationUserRepository ApplicationUser { get; set; }
        public IOrderHeaderRepository OrderHeader { get; set; }
        public IOrderDetailRepository OrderDetail { get; set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Product = new ProductRepository(_db);
            ShoppingCart = new ShoppingCartRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);
            OrderHeader = new OrderHeaderRepository(_db);
            OrderDetail = new OrderDetailRepository(_db);
            ProductImage = new ProductImageRepository(_db);


        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
