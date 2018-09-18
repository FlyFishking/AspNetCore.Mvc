using System.Collections.Generic;

namespace EFCore.Service.Contract
{
    public interface IValuesService
    {
        IEnumerable<string> FindAll();

        string Find(int id);
    }
}