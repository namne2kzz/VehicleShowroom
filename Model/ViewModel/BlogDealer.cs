using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class BlogDealer
    {
         public long ID { get; set; }

        public long? DealerID { get; set; }

        public string DealerName { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage ="Blog Name mustn't empty")]
        [Display(Name ="Blog Name")]
        public string Name { get; set; }

        [Column(TypeName = "ntext")]
        [Required(ErrorMessage = "Description mustn't empty")]
        public string Description { get; set; }    

        [StringLength(50)]       
        public string Image { get; set; }      

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }

        public int Status { get; set; }
    }
}
