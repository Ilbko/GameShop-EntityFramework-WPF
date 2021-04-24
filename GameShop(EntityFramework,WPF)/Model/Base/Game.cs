namespace GameShop_EntityFramework_WPF_.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Game")]
    public partial class Game
    {
        [Key]
        public int Game_Id { get; set; }

        public int Game_StyleId { get; set; }

        [Required]
        [StringLength(50)]
        public string Game_Name { get; set; }

        [Required]
        [StringLength(30)]
        public string Game_Studio { get; set; }

        public int? Game_SoldAmount { get; set; }

        public bool Game_IsMultiplayer { get; set; }

        [Column(TypeName = "date")]
        public DateTime Game_ReleaseDate { get; set; }

        public virtual Style Style { get; set; }
    }
}
