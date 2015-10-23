using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using PhoneTariff.DAL.Common;
using PhoneTariff.DAL.Common.Dao;
using PhoneTariff.DAL.Common.Domain;

namespace PhoneTariff.DAL.SqlServer.Dao
{

    public class ZoneDao : IZoneDao
    {

        private const string SQL_FIND_BY_ID = "SELECT * FROM TZone WHERE Id = @Id";
        private const string SQL_FIND_ALL = "SELECT * FROM TZone";
        private const string SQL_UPDATE = "UPDATE TZone SET Name=@Name WHERE Id=@Id";

        private IDatabase database;

        public ZoneDao(IDatabase database)
        {
            this.database = database;
        }

        protected DbCommand CreateFindByIdCommand(string zoneId)
        {
            DbCommand findByIdCommand = database.CreateCommand(SQL_FIND_BY_ID);
            database.DefineParameter(findByIdCommand, "@Id", DbType.String, zoneId);
            return findByIdCommand;
        }

        protected DbCommand CreateFindAllCommand()
        {
            return database.CreateCommand(SQL_FIND_ALL);
        }

        protected DbCommand CreateUpdateCommand(string id, string name)
        {
            DbCommand updateCommand = database.CreateCommand(SQL_UPDATE);
            database.DefineParameter(updateCommand, "@Id", DbType.String, id);
            database.DefineParameter(updateCommand, "@Name", DbType.String, name);
            return updateCommand;
        }

        public Zone FindById(string zoneId)
        {
            using (IDataReader reader = database.ExecuteReader(CreateFindByIdCommand(zoneId)))
            {
                if (reader.Read())
                    return new Zone((string)reader["Id"], (string)reader["Name"]);
                else
                    return null;
            }
        }

        public IList<Zone> FindAll()
        {
            using (IDataReader reader = database.ExecuteReader(CreateFindAllCommand()))
            {
                List<Zone> zones = new List<Zone>();
                while (reader.Read())
                    zones.Add(new Zone((string)reader["Id"], (string)reader["Name"]));
                return zones;
            }
        }

        public bool Update(Zone zone)
        {
            return database.ExecuteNonQuery(CreateUpdateCommand(zone.Id, zone.Name)) == 1;
        }
    }
}
