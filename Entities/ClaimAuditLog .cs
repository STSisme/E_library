using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_Library.Model
{
    public class ClaimAuditLog
    {
        [Key]
        public Guid ClaimAuditLog_Id { get; set; } = Guid.NewGuid();

        public string Staff_Id { get; set; }
        public Guid Order_Id { get; set; }

        public DateTime ClaimedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(Staff_Id))]
        public ApplicationUser Staff { get; set; }

        [ForeignKey(nameof(Order_Id))]
        public Order Order { get; set; }
    }

}
