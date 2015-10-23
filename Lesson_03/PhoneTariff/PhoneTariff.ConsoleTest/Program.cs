using PhoneTariff.DAL.Common;
using PhoneTariff.DAL.Common.Dao;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneTariff.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            /*  string connectionString = ConfigurationManager.ConnectionStrings["PhoneTariffDb"].ConnectionString;
              IDatabase database = new PhoneTariff.DAL.SqlServer.Database(connectionString);

              ITariffDao tariffDao = new PhoneTariff.DAL.SqlServer.Dao.TariffDao(database);*/

            IDatabase database = DALFactory.CreateDatabase();
            ITariffDao tariffDao = DALFactory.CreateTariffDao(database);


            var tariff = tariffDao.FindById("A1");

            Console.WriteLine(tariff.BasicFee);

            Console.ReadKey();
        }
    }
}
