namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTACT")]
    public partial class CONTACT
    {
        [Key]
        public int MaCT { get; set; }

        [StringLength(50)]
        public string TenKH { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(15)]
        public string Phone { get; set; }

        [StringLength(500)]
        public string Message { get; set; }
    }
}
