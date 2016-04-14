namespace OnlineHotel.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LOAIPHONG")]
    public partial class LOAIPHONG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LOAIPHONG()
        {
            BANGDUYETPHONGs = new HashSet<BANGDUYETPHONG>();
            PHONGs = new HashSet<PHONG>();
        }

        [Key]
        public int MaLP { get; set; }

        [StringLength(50)]
        public string TenLP { get; set; }

        public int? DonGia { get; set; }

        [StringLength(500)]
        public string GioiThieu { get; set; }

        [StringLength(250)]
        public string HinhAnh { get; set; }

        [StringLength(1000)]
        public string ChiTiet { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BANGDUYETPHONG> BANGDUYETPHONGs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PHONG> PHONGs { get; set; }
    }
}
