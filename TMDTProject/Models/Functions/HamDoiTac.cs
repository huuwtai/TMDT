using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TMDTProject.Models;

namespace TMDTProject.Models.Functions
{
    public class HamDoiTac
    {
        private TMDTEntities2 db;
        
        public HamDoiTac()
        {
            db = new TMDTEntities2();
        }
        public IQueryable<DoiTac> DoiTacs { get { return db.DoiTacs; } }

        public string Update(DoiTac model)
        {
            DoiTac dbEntry = db.DoiTacs.Find(model.MaDT);
            if (dbEntry == null)
            {
                return null;
            }
            dbEntry.MaDT = model.MaDT;
            dbEntry.TenDT = model.TenDT;
            dbEntry.MaLoaiDT = model.MaLoaiDT;
            dbEntry.MaLoaiDV = model.MaLoaiDV;
            dbEntry.AnhLogo = model.AnhLogo;
            dbEntry.DiaChi = model.DiaChi;
            dbEntry.ThongTinChiTiet = model.ThongTinChiTiet;
            dbEntry.ThongTinLienHe = model.ThongTinLienHe;

            db.SaveChanges();
            return model.MaDT.ToString();
        }
        public string Delete(int id)
        {
            DoiTac dbEntry =  db.DoiTacs.Find(id);
            if(dbEntry == null)
            {
                return null;
            }
            db.DoiTacs.Remove(dbEntry);
            db.SaveChanges();
            return id.ToString();
        }
    }
}