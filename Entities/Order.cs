using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Library.Model
{
    [Table("Order")]
    public class Order
    {
        [Key]
        public Guid Order_Id { get; set; } = Guid.NewGuid();

        [Required]
        public string User_Id { get; set; }

        [ForeignKey(nameof(User_Id))]
        public ApplicationUser User { get; set; } // The member who placed the order

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        public decimal DiscountAmount { get; set; } = 0;

        public string ClaimCode { get; set; }
        public bool IsClaimed { get; set; } = false;
        public DateTime? ClaimedAt { get; set; }

        public string? ClaimedByStaffId { get; set; }  // Nullable

        [ForeignKey(nameof(ClaimedByStaffId))]
        public ApplicationUser? ClaimedByStaff { get; set; }  // The staff who fulfilled the claim

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Placed";

        public DateTime? CancelledAt { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
