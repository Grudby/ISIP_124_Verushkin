using System;
using System.Collections.Generic;

namespace Marketplace.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PickupPointId { get; set; }
        public DateTime CreatedAt { get; set; }

        public User? User { get; set; }
        public PickupPoint? PickupPoint { get; set; }
        public List<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
    }
}
