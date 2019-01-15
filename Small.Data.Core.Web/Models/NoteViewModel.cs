using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Small.Data.Core.Web.Models
{
    public class NoteViewModel
    {
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Detail")]
        public string Detail { get; set; }


        [Display(Name = "Category")]
        public string Category { get; set; }

        public int NoteID { get; set; }
    }
}
