using System.Collections.Generic;
using System.Linq;
using Marketplace.Data;
using Marketplace.Models;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Services
{
    public static class CartService
    {
        public static List<CartItem> GetItems(int userId)
        {
            using MarketplaceContext context = new MarketplaceContext();
            return context.CartItems
                .AsNoTracking()
                .Include(item => item.Product)
                .Where(item => item.UserId == userId)
                .OrderBy(item => item.Id)
                .ToList();
        }

        public static decimal GetTotal(IEnumerable<CartItem> items)
        {
            return items.Sum(item => item.Product!.Price * item.Quantity);
        }

        /// <summary>
        /// Добавляет товар в корзину. Если он уже есть, увеличивает количество.
        /// </summary>
        public static string? AddProduct(int userId, int productId, int quantity)
        {
            using MarketplaceContext context = new MarketplaceContext();
            Product? product = context.Products.FirstOrDefault(item => item.Id == productId);
            if (product == null)
            {
                return $"Товар с номером {productId} не найден.";
            }

            CartItem? existing = context.CartItems
                .FirstOrDefault(item => item.UserId == userId && item.ProductId == productId);
            int newQuantity = (existing?.Quantity ?? 0) + quantity;
            if (newQuantity > product.StockQuantity)
            {
                return $"Недостаточно товара: в наличии {product.StockQuantity} шт., в корзине уже {existing?.Quantity ?? 0} шт.";
            }

            if (existing == null)
            {
                context.CartItems.Add(new CartItem { UserId = userId, ProductId = productId, Quantity = quantity });
            }
            else
            {
                existing.Quantity = newQuantity;
            }
            context.SaveChanges();
            return null;
        }

        public static bool RemoveItem(int userId, int cartItemId)
        {
            using MarketplaceContext context = new MarketplaceContext();
            CartItem? item = context.CartItems.FirstOrDefault(cart => cart.Id == cartItemId && cart.UserId == userId);
            if (item == null)
            {
                return false;
            }
            context.CartItems.Remove(item);
            context.SaveChanges();
            return true;
        }
    }
}
