using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Shims
{
    /// <summary>
    /// A shim for snowflake entities.
    /// </summary>
    public abstract class SnowflakeShimEntity : ShimEntity<ulong>, ISnowflakeShimEntity
    {
        /// <inheritdoc cref="ISnowflakeEntity.CreatedAt"/>
        public DateTimeOffset CreatedAt => SnowflakeUtils.FromSnowflake(this.Id);

        protected SnowflakeShimEntity(IDiscordClientShim client) : base(client, ShimUtils.GenerateSnowflake())
        { }
        protected SnowflakeShimEntity(IDiscordClientShim client, ulong id) : base(client, id)
        { }
    }
}
