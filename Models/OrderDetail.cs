﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ConsultorioUI.Models
{
    [Table("OrderDetails")]
    public partial class OrderDetail
    {
        [Key]
        public int OrderID
        {
            get;
            set;
        }

        [ForeignKey("OrderID")]
        public Order Order { get; set; }
        public int ProductID
        {
            get;
            set;
        }

        //[ForeignKey("ProductID")]
        //public Product Product { get; set; }
        public double? UnitPrice
        {
            get;
            set;
        }
        public Int16? Quantity
        {
            get;
            set;
        }
        public float? Discount
        {
            get;
            set;
        }
    }
}
