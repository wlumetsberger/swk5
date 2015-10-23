using System;
using System.Configuration;
using System.Reflection;
using PhoneTariff.DAL.Common.Dao;

namespace PhoneTariff.DAL.Common {

  public class DALFactory {

    private static string assemblyName;
    private static Assembly dalAssembly;

    static DALFactory() {
      assemblyName = ConfigurationManager.AppSettings["DALAssembly"];
      dalAssembly = Assembly.Load(assemblyName);
    }

    public static IDatabase CreateDatabase() {
      string connectionString =
        ConfigurationManager.ConnectionStrings["PhoneTariffDb"].ConnectionString;
      return CreateDatabase(connectionString);
    }

    public static IDatabase CreateDatabase(string connectionString) {
      string databaseClassName = assemblyName + ".Database";
      Type dbClass = dalAssembly.GetType(databaseClassName);

      return Activator.CreateInstance(dbClass,
        new object[] { connectionString }) as IDatabase;
    }

    public static IZoneDao CreateZoneDao(IDatabase database) {
      Type zoneType = dalAssembly.GetType(assemblyName + ".Dao.ZoneDao");
      return Activator.CreateInstance(zoneType, new object[] { database }) 
               as IZoneDao;
    }

    public static ITariffDao CreateTariffDao(IDatabase database) {
      Type zoneType = dalAssembly.GetType(assemblyName + ".Dao.TariffDao");
      return Activator.CreateInstance(zoneType, new object[] { database }) 
               as ITariffDao;
    }

    public static IRateDao CreateRateDao(IDatabase database) {
      Type zoneType = dalAssembly.GetType(assemblyName + ".Dao.RateDao");
      return Activator.CreateInstance(zoneType, new object[] { database }) 
               as IRateDao;
    }

//    public static IPhoneTariffDao CreatePhoneTariffDao(IDatabase database)
//    {
//        Type accessorType = dalAssembly.GetType(assemblyName + ".PhoneTariffDao");
//        return Activator.CreateInstance(accessorType, new object[] { database })
//                 as IPhoneTariffDao;
//    }
  }
}
