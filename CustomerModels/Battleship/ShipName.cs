using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CustomerModels.Battleship
{
    public enum ShipName
    {
        [Display(Name = "Carrier" )]
        Carrier,

        [Display(Name = "Battleship")]
        Battleship,

        [Display(Name = "Cruiser")]
        Cruiser,

        [Display(Name = "Submarine")]
        Submarine,

        [Display(Name = "Destroyer")]
        Destroyer
    }
}
