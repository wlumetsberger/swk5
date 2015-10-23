using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhoneTariff.DAL.Common.Domain;

namespace PhoneTariff.DAL.Common.Dao
{
    public interface IZoneDao
    {
        Zone FindById(string zoneId);
        IList<Zone> FindAll();
        bool Update(Zone zone);
    }
}
