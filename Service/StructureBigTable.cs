using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Microsoft.Extensions.Configuration;
using historianbigtableservice.Model;
using historianbigtableservice.Service.Interface;
using Dapper;
using Npgsql;
using Newtonsoft;
using Newtonsoft.Json;

namespace historianbigtableservice.Service
{
    public class StructureBigTable :IStructureBigTable
    {
        private readonly IConfiguration _configuration;
        private StructureDb _structureDb;

        public StructureBigTable(IConfiguration configuration)
        {
            _structureDb = new StructureDb();
            _structureDb.name = "HistorianBigTable";
            _configuration = configuration;

            SynchronizeColumn();
        }

        public async Task<(bool,string)> addColumn(string columnName)
        {
            if(!_structureDb.column.Contains(columnName))
            {

            }

            return(true,"");
        }



        // private async Task<dynamic> GetHistorian(int thingId, long startDate, long endDate)
        // {

        // }

        // private async Task<TagBigDate> UpdateHistorian(TagBigDate tagBigDate)
        // {

        // }

        private void SynchronizeColumn()
        {
            string commandSql = string.Empty;

            commandSql = "SELECT column_name FROM information_schema.columns WHERE table_name ='Historian'";

            var result = ExecuteCommand(commandSql).Result;

            List<string> listString = new List<string>();
            foreach(var column in result)
            {
                var row = (IDictionary<string,object>)column;
                listString.Add(row["column_name"].ToString());
            }

            _structureDb.column = listString;

        }

        private async Task<IEnumerable<dynamic>> ExecuteCommand(string commandSQL)
        {
          
            IEnumerable<dynamic> dbResult;
            using(IDbConnection dbConnection = new NpgsqlConnection(_configuration["stringHistorianBigTable"]))
            {
                dbConnection.Open();
                dbResult = await dbConnection.QueryAsync<dynamic>(commandSQL);
            }

            return dbResult;
           
            
        }

        
    }
}