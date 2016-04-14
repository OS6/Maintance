namespace OnlineHotel.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BANGDUYETPHONG")]
    public partial class BANGDUYETPHONG
    {
        [Key]
        public int MaDuyetPhieu { get; set; }

        public int? MaKH { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? NgayBatDauThue { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? NgayKetThucThue { get; set; }

        public int? SoNguoi { get; set; }

        public int? MaLP { get; set; }

        public virtual LOAIPHONG LOAIPHONG { get; set; }
    }
}
