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

        public async Task<(bool,string)> addHistorian(Tag tag)
        {
            // converte a tag para tagBigDate
            var tagBigTable = ConvertTagInTagBigDate(tag);

            // Verifica se a coluna existe
            await _structureDbService.addColumn(tagBigTable.column);

            // var returnDb = GetHistorian(tagBigTable.thingId,tagBigTable.date,tagBigTable.date);

            // if(returnDb != null)
            // {

            // }
            // else
            // {

            // }



            return(true,"");
        }

        private TagBigDate ConvertTagInTagBigDate(Tag tag)
        {
            TagBigDate tagBigDate = new TagBigDate();
            tagBigDate.date = tag.date;
            tagBigDate.column = tag.group + tag.tag;
            tagBigDate.thingId = tag.thingId;
            tagBigDate.value = tag.value;
            return tagBigDate;
        }

        
    }
}