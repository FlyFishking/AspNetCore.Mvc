using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Kernal.Extension
{
    public static class OracleDatabaseFacadeExtensions
    {
        /// <summary>
        ///     <para>
        ///         Returns true if the database provider currently in use is the Oracle provider.
        ///     </para>
        ///     <para>
        ///         This method can only be used after the <see cref="DbContext" /> has been configured because
        ///         it is only then that the provider is known. This means that this method cannot be used
        ///         in <see cref="DbContext.OnConfiguring" /> because this is where application code sets the
        ///         provider to use as part of configuring the context.
        ///     </para>
        /// </summary>
        /// <param name="database"> The facade from <see cref="DbContext.Database" />. </param>
        /// <returns> True if Oracle is being used; false otherwise. </returns>
        public static bool IsOracle(this DatabaseFacade database)
            => database.ProviderName.Equals(
                typeof(OracleDatabaseFacadeExtensions).GetTypeInfo().Assembly.GetName().Name,
                StringComparison.Ordinal);
    }
}