
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SalesWebMvc.Models.Enums
{
    public enum SalesStatus : int
    {
        [Display(Name = "Pending")]
        Pending = 0,
        [Display(Name = "Billed")]
        Billed = 1,
        [Display(Name = "Canceled")]
        Canceled = 2,
    }

}
