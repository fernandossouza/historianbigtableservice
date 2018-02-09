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
    public class HistorianService : IHistorianService
    {
        private readonly IConfiguration _configuration;
        private IStructureBigTable _structureDbService;

        public HistorianService(IConfiguration configuration,IStructureBigTable structureDbService)
        {
            _structureDbService = structureDbService;
            _configuration = configuration;
        }

        public async Task<(bool,string)> AddHistorian(TagInput tag)
        {
            // converte a tag para tagBigDate
            var tagBigTable = ConvertTagInTagBigDate(tag);

            // Verifica se a coluna existe
            await _structureDbService.AddColumn(tagBigTable.column);

            var historianDb = await GetHistorianDb(tagBigTable.thingId,tagBigTable.date,tagBigTable.date);

            if(historianDb.FirstOrDefault() == null)
            {
                var result = await AddHistorianInDb(tagBigTable);
                if(result < 1)
                {
                    return(false,"Erro in insert tag DB");
                }
            }
            else
            {
                // pega o id da linha
                var row = (IDictionary<string,object>)historianDb.FirstOrDefault();
                var resultUpdate = await UpdateHistorianInDb(Convert.ToInt32(row["id"].ToString()),tagBigTable);
                if(resultUpdate < 1)
                {
                    return(false,"Erro in update tag DB, id row table " + row["id"].ToString());
                }

            }
            return(true,string.Empty);
        }

        public async Task<(ThingOutput,string)> GetHistorian(int thingId, long startDate, long endDate)
        {
            if(thingId<=0)
            {
                return(null,"thingId invalid");
            }

            var returnDb =await GetHistorianDb(thingId,startDate,endDate);

            if(returnDb.Count() <=0 )
            {
                return(null,string.Empty);
            }

            ThingOutput thingHistorian = new ThingOutput();
            thingHistorian.thingId = thingId;
            thingHistorian.tags = new List<TagOutput>();

            foreach(var column in _structureDbService.GetColumn())
            {
                if(column.ToLower() == "id" || column.ToLower() == "thingid" || column.ToLower() == "date")
                    continue;

                List<string> listValue = new List<string>();
                List<long> listdate = new List<long>();

                // Pega os valores das colunas que não são nulas
                foreach(var rowDb in returnDb)
                {
                    var row = (IDictionary<string,object>)rowDb;
                    if(row[column] != null)
                    {
                        listValue.Add(row[column].ToString());
                        listdate.Add(Convert.ToInt64(row["date"].ToString()));
                    }
                }

                if(listValue.Count()>0)
                {
                    if(listValue.Count()!= listdate.Count())
                    {
                        return(null,"erro number the value is different the number date");
                    }

                    
                    TagOutput tagOutput = new TagOutput();
                    tagOutput.name = column.Split(".")[1];
                    tagOutput.group = column.Split(".")[0];

                    tagOutput.timestamp = listdate;
                    tagOutput.value = listValue;

                    thingHistorian.tags.Add(tagOutput);
                }
            }

            return (thingHistorian,string.Empty);

        }

        private TagBigTable ConvertTagInTagBigDate(TagInput tag)
        {
            TagBigTable tagBigDate = new TagBigTable();
            tagBigDate.date = tag.date;
            tagBigDate.column = tag.group +"."+ tag.tag;
            tagBigDate.thingId = tag.thingId;
            tagBigDate.value = tag.value;
            return tagBigDate;
        }

        private async Task<IEnumerable<dynamic>> GetHistorianDb(int thingId, long startDate, long endDate)
        {
            string commandSql = "SELECT * FROM public.\"Historian\" where \"thingId\" = "+ thingId;
            commandSql = commandSql + " and \"date\" >=" + startDate +" and \"date\" <=" + endDate; 

            var result = await _structureDbService.ExecuteCommandSelect(commandSql);

            return result;
        }

        private async Task<int> AddHistorianInDb(TagBigTable tagBigTable)
        {
            string commandSql = "INSERT INTO public.\"Historian\"(\"thingId\", date, \""+ tagBigTable.column+"\") ";
            commandSql = commandSql + " VALUES ("+tagBigTable.thingId+","+tagBigTable.date+",'"+tagBigTable.value+"')";

            var result = await _structureDbService.ExecuteCommandInsertUpdate(commandSql);

            return result;
        }

        private async Task<int> UpdateHistorianInDb(long rowId, TagBigTable tagBigTable)
        {
            if(rowId > 0)
            {
            string commandSql = "UPDATE public.\"Historian\" SET \""+ tagBigTable.column+"\" =  '"+ tagBigTable.value+"'";
            commandSql = commandSql + " WHERE \"id\" = " + rowId.ToString();

            var result = await _structureDbService.ExecuteCommandInsertUpdate(commandSql);

            return result;
            }
            return 0;
        }

        
    }
}