using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebSite.Models
{
    public class Blog 
    {

        [Key]
        public int? BlogID { get; set; }

        [StringLength(100)]       
        [Display(Name = "Etiket")]
        public string Tag { get; set; }

        [StringLength(200)]
        [Display(Name = "Başlık")]
        public string Title { get; set; }

        
        [Display(Name = "İçerik")]
        public string Description { get; set; }  

        [Display(Name = "Fotoğraf")]
        public string Photo { get; set; }

    }
}
