using System;
using System.Collections.Generic;
using System.Linq;
using Marketplace.Data;
using Marketplace.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Marketplace.Services
{
    public static class OrderService
    {
        /// <summary>
        /// Оформляет заказ из выбранных позиций корзины: списывает остатки и очищает эти позиции.
        /// Всё выполняется в одной транзакции.
        /// </summary>
        public static Order? Checkout(int userId, IReadOnlyCollection<int> cartItemIds, int pickupPointId, out string error)
        {
            error = string.Empty;
            using MarketplaceContext context = new MarketplaceContext();
            using IDbContextTransaction transaction = context.Database.BeginTransaction();

            if (!context.PickupPoints.Any(point => point.Id == pickupPointId))
            {
                error = "Пункт выдачи не найден.";
                return null;
            }

            List<CartItem> items = context.CartItems
                .Include(item => item.Product)
                .Where(item => item.UserId == userId && cartItemIds.Contains(item.Id))
                .ToList();
            if (items.Count == 0)
            {
                error = "Нет товаров для оформления.";
                return null;
            }

            foreach (CartItem item in items)
            {
                if (item.Quantity > item.Product!.StockQuantity)
                {
                    error = $"Товара \"{item.Product.Name}\" осталось только {item.Product.StockQuantity} шт.";
                    return null;
                }
            }

            Order order = new Order
            {
                UserId = userId,
                PickupPointId = pickupPointId,
                CreatedAt = DateTime.Now
            };

            foreach (CartItem item in items)
            {
                item.Product!.StockQuantity -= item.Quantity;
                order.OrderProducts.Add(new OrderProduct
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Product.Price
                });
            }

            context.Orders.Add(order);
            context.CartItems.RemoveRange(items);
            context.SaveChanges();
            transaction.Commit();
            return order;
        }

        public static List<Order> GetOrders(int userId, bool newestFirst)
        {
            using MarketplaceContext context = new MarketplaceContext();
            IQueryable<Order> query = context.Orders
                .AsNoTracking()
                .Include(order => order.PickupPoint)
                .Include(order => order.OrderProducts)
                .ThenInclude(item => item.Product)
                .Where(order => order.UserId == userId);

            query = newestFirst
                ? query.OrderByDescending(order => order.CreatedAt).ThenByDescending(order => order.Id)
                : query.OrderBy(order => order.CreatedAt).ThenBy(order => order.Id);
            return query.ToList();
        }

        public static decimal GetTotal(Order order)
        {
            return order.OrderProducts.Sum(item => item.Price * item.Quantity);
        }
    }
}
