using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class ACCOUNT_DAO
    {
        OnlineHotelDbContext db = null;

        public ACCOUNT_DAO()
        {
            db = new OnlineHotelDbContext();
        }
        public int Insert(ACCOUNT entity)
        {
            db.ACCOUNTs.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }
    }
}
