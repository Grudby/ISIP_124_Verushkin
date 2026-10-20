using System.Collections.Generic;
using System.Linq;
using Marketplace.Data;
using Marketplace.Models;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Services
{
    public static class CatalogService
    {
        public static List<Product> GetProducts()
        {
            using MarketplaceContext context = new MarketplaceContext();
            return context.Products
                .AsNoTracking()
                .Include(product => product.Category)
                .OrderBy(product => product.Category!.Name)
                .ThenBy(product => product.Name)
                .ToList();
        }

        public static Product? FindProduct(int productId)
        {
            using MarketplaceContext context = new MarketplaceContext();
            return context.Products
                .AsNoTracking()
                .Include(product => product.Category)
                .FirstOrDefault(product => product.Id == productId);
        }

        public static List<PickupPoint> GetPickupPoints()
        {
            using MarketplaceContext context = new MarketplaceContext();
            return context.PickupPoints.AsNoTracking().OrderBy(point => point.Id).ToList();
        }
    }
}
