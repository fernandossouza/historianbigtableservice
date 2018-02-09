using System.Threading.Tasks;
using System.Collections.Generic;
using historianbigtableservice.Model;

namespace historianbigtableservice.Service.Interface
{
    public interface IStructureBigTable
    {
       List<string> GetColumn();
       Task<(bool,string)> AddColumn(string columnName);

       Task<IEnumerable<dynamic>> ExecuteCommandSelect(string commandSQL);

       Task<int> ExecuteCommandInsertUpdate(string commandSQL);
    }
}