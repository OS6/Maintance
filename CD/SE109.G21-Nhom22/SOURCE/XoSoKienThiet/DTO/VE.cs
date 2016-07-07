namespace XoSoKienThiet.DTO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VE")]
    public partial class VE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VE()
        {
            CT_KETQUAXOSO = new HashSet<CT_KETQUAXOSO>();
        }

        [Key]
        [StringLength(10)]
        public string MaVe { get; set; }

        [StringLength(10)]
        public string MaLoaiVe { get; set; }

        [StringLength(10)]
        public string SoVe { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CT_KETQUAXOSO> CT_KETQUAXOSO { get; set; }

        public virtual LOAIVE LOAIVE { get; set; }
    }
}
