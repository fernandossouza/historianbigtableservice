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

        public List<string> GetColumn()
        {
            return _structureDb.column;
        }

        public async Task<(bool,string)> AddColumn(string columnName)
        {
            try
            {
                // Verifica se a coluna existe na structure memory
                // caso não exista  sincroni
                if(!_structureDb.column.Contains(columnName))
                {                    
                    SynchronizeColumn();
                     if(!_structureDb.column.Contains(columnName))
                    {            
                    string commandSql = "ALTER TABLE \"Historian\" ADD COLUMN \""+ columnName+"\" character varying(50)";
                    await ExecuteCommandInsertUpdate(commandSql);
                    SynchronizeColumn();
                    }
                }

                return(true,string.Empty);
            }
            catch (Exception ex)
            {
                return(false,ex.ToString());
            }
        }



        

        // private async Task<TagBigDate> UpdateHistorian(TagBigDate tagBigDate)
        // {

        // }

        private void SynchronizeColumn()
        {
            string commandSql = string.Empty;

            commandSql = "SELECT column_name FROM information_schema.columns WHERE table_name ='Historian'";

            var result = ExecuteCommandSelect(commandSql).Result;

            List<string> listString = new List<string>();
            foreach(var column in result)
            {
                var row = (IDictionary<string,object>)column;
                listString.Add(row["column_name"].ToString());
            }

            _structureDb.column = listString;

        }

        public async Task<IEnumerable<dynamic>> ExecuteCommandSelect(string commandSQL)
        {
          
            IEnumerable<dynamic> dbResult;
            using(IDbConnection dbConnection = new NpgsqlConnection(_configuration["stringHistorianBigTable"]))
            {
                dbConnection.Open();
                dbResult = await dbConnection.QueryAsync<dynamic>(commandSQL);
            }

            return dbResult;
           
            
        }

        public async Task<int> ExecuteCommandInsertUpdate(string commandSQL)
        {
          
            int dbResult;
            using(IDbConnection dbConnection = new NpgsqlConnection(_configuration["stringHistorianBigTable"]))
            {
                dbConnection.Open();
                dbResult = await dbConnection.ExecuteAsync(commandSQL);
            }

            return dbResult;
           
            
        }

        
    }
}