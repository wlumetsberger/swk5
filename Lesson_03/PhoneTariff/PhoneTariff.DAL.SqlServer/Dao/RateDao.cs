using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using PhoneTariff.DAL.Common;
using PhoneTariff.DAL.Common.Dao;
using PhoneTariff.DAL.Common.Domain;

namespace PhoneTariff.DAL.SqlServer.Dao
{

    public class RateDao : IRateDao
    {

        const string SQL_FIND_BY_ID =
          @"SELECT * FROM Rate WHERE TariffId = @tariffId AND ZoneId = @zoneId";
        const string SQL_FIND_ALL =
          @"SELECT Rate.TariffId, Rate.ZoneId, Rate.PeakRate, Rate.OffPeakRate, 
                   Tariff.Name AS TariffName, Tariff.BasicFee AS BasicFee, 
                   TZone.Name AS ZoneName
                FROM Rate 
                JOIN TZone ON Rate.ZoneId = TZone.Id
                JOIN Tariff ON Rate.TariffId = Tariff.Id";
        const string SQL_UPDATE_BY_ID =
          @"UPDATE Rate 
                SET PeakRate=@PeakRate, OffPeakRate=@OffPeakRate
                WHERE TariffId = @tariffId AND ZoneId = @zoneId";
        const string SQL_INSERT =
          @"INSERT INTO Rate
            VALUES (@tariffId, @zoneId, @peakRate, @offPeakRate)";

        private IDatabase database;

        public RateDao(IDatabase database)
        {
            this.database = database;
        }

        protected DbCommand CreateFindByIdCmd(string tariffId, string zoneId)
        {
            DbCommand findByIdCmd = database.CreateCommand(SQL_FIND_BY_ID);
            database.DefineParameter(findByIdCmd, "tariffId", DbType.String, tariffId);
            database.DefineParameter(findByIdCmd, "zoneId", DbType.String, zoneId);
            return findByIdCmd;
        }

        protected DbCommand CreateFindAllCmd()
        {
            return database.CreateCommand(SQL_FIND_ALL);
        }

        protected DbCommand CreateUpdateByIdCmd(string tariffId, string zoneId,
                                                double peakRate, double offPeakRate)
        {
            DbCommand updateByIdCmd = database.CreateCommand(SQL_UPDATE_BY_ID);
            database.DefineParameter(updateByIdCmd, "peakRate", DbType.Double, peakRate);
            database.DefineParameter(updateByIdCmd, "offPeakRate", DbType.Double, offPeakRate);
            database.DefineParameter(updateByIdCmd, "tariffId", DbType.String, tariffId);
            database.DefineParameter(updateByIdCmd, "zoneId", DbType.String, zoneId);
            return updateByIdCmd;
        }

        protected DbCommand CreateInsertCmd(string tariffId, string zoneId,
                                        double peakRate, double offPeakRate)
        {
            DbCommand insertCmd = database.CreateCommand(SQL_INSERT);
            database.DefineParameter(insertCmd, "tariffId", DbType.String, tariffId);
            database.DefineParameter(insertCmd, "zoneId", DbType.String, zoneId);
            database.DefineParameter(insertCmd, "peakRate", DbType.Double, peakRate);
            database.DefineParameter(insertCmd, "offPeakRate", DbType.Double, offPeakRate);
            return insertCmd;
        }

        public Rate FindById(string tariffId, string zoneId)
        {
            Tariff tariffData = new TariffDao(database).FindById(tariffId);
            Zone zoneData = new ZoneDao(database).FindById(zoneId);

            if (tariffData == null || zoneData == null)
                return null;

            using (IDataReader reader = database.ExecuteReader(CreateFindByIdCmd(tariffId, zoneId)))
            {
                if (reader.Read())
                    return new Rate(
                        tariffData, zoneData,
                        (double)reader["PeakRate"], (double)reader["OffPeakRate"]);
                else
                    return null;
            }
        }

        public IList<Rate> FindAll()
        {
            using (IDataReader reader = database.ExecuteReader(CreateFindAllCmd()))
            {
                IList<Rate> result = new List<Rate>();
                while (reader.Read())
                    result.Add(new Rate(
                        new Tariff((string)reader["TariffId"],
                                   (string)reader["TariffName"],
                                   (double)reader["BasicFee"]),
                        new Zone(reader["ZoneId"].ToString(),
                                 reader["ZoneName"].ToString()),
                                 (double)reader["PeakRate"],
                                 (double)reader["OffPeakRate"]));
                return result;
            }
        }

        public bool Update(Rate rate)
        {
            return database.ExecuteNonQuery(CreateUpdateByIdCmd(rate.Tariff.Id, rate.Zone.Id,
                                                                rate.PeakRate, rate.OffPeakRate)) == 1;
        }

        public bool Insert(Rate rate)
        {
            return database.ExecuteNonQuery(
                CreateInsertCmd(rate.Tariff.Id, rate.Zone.Id, rate.PeakRate, rate.OffPeakRate)) == 1;
        }
    }
}
