using OnlineHotel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineHotel.Dao
{
    public class CONTACT_DAO
    {
        OnlineHotelDbContext db = null;
        public CONTACT_DAO()
        {
            db = new OnlineHotelDbContext();
        }

        public int Insert(CONTACT contact)
        {
            db.CONTACTs.Add(contact);
            return db.SaveChanges();
        }
    }
}