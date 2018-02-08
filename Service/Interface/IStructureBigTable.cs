using System.Threading.Tasks;
using historianbigtableservice.Model;

namespace historianbigtableservice.Service.Interface
{
    public interface IStructureBigTable
    {
       Task<(bool,string)> addColumn(string columnName);
    }
}