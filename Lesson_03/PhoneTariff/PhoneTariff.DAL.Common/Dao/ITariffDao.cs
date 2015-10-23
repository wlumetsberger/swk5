using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhoneTariff.DAL.Common.Domain;

namespace PhoneTariff.DAL.Common.Dao
{
    public interface ITariffDao
    {
        Tariff FindById(string tariffId);
        IList<Tariff> FindAll();
        bool Update(Tariff tariff);
    }
}
