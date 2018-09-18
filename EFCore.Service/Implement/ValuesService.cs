using System.Collections.Generic;
using EFCore.Repository;
using EFCore.Service.Contract;
using Microsoft.Extensions.Logging;

namespace EFCore.Service.Implement
{
    public class ValuesService : IValuesService
    {
        private readonly ILogger<ValuesService> logger;
        public IStudentRepository repoStudent { get; set; }

        public ValuesService(ILogger<ValuesService> logger)
        {
            this.logger = logger;
        }

        public IEnumerable<string> FindAll()
        {
            logger.LogDebug("{method} called", nameof(FindAll));

            return new[] { "value1", "value2" };
        }

        public string Find(int id)
        {
            logger.LogDebug("{method} called with {id}", nameof(Find), id);

            return $"value{id}";
        }
    }
}