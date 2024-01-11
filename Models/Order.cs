using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ConsultorioUI.Models
{
    [Table("Orders")]
    public partial class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderID
        {
            get;
            set;
        }


        [InverseProperty("Order")]
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public string CustomerID
        {
            get;
            set;
        }

       // [ForeignKey("CustomerID")]
      //  public Customer Customer { get; set; }
        public int? EmployeeID
        {
            get;
            set;
        }

        [ForeignKey("EmployeeID")]
        public Employee Employee { get; set; }
        public DateTime? OrderDate
        {
            get;
            set;
        }
        public DateTime? RequiredDate
        {
            get;
            set;
        }
        public DateTime? ShippedDate
        {
            get;
            set;
        }
        public decimal? Freight
        {
            get;
            set;
        }
        public string ShipName
        {
            get;
            set;
        }
        public string ShipAddress
        {
            get;
            set;
        }
        public string ShipCity
        {
            get;
            set;
        }
        public string ShipRegion
        {
            get;
            set;
        }
        public string ShipPostalCode
        {
            get;
            set;
        }
        public string ShipCountry
        {
            get;
            set;
        }
    }
}

