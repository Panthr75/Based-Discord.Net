using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Shims
{

    /// <summary>
    /// Represents an shimmable snowflake entity, which will always
    /// have a Client property.
    /// </summary>
    public interface ISnowflakeShimEntity : ISnowflakeEntity, IShimEntity<ulong>
    {
    }
}
