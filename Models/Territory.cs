using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ConsultorioUI.Models
{
    [Table("Territories")]
    public partial class Territory
    {
        [Key]
        public string TerritoryID
        {
            get;
            set;
        }


        [InverseProperty("Territory")]
        public ICollection<EmployeeTerritory> EmployeeTerritories { get; set; }
        public string TerritoryDescription
        {
            get;
            set;
        }
        public int RegionID
        {
            get;
            set;
        }

        //[ForeignKey("RegionID")]
        //public Region Region { get; set; }
    }
}
