using System;

using Model = Discord.API.Entitlement;

namespace Discord.Rest;

public class RestEntitlement : RestEntity<ulong>, IEntitlement
{
    /// <inheritdoc/>
    public DateTimeOffset CreatedAt { get; private set; }

    /// <inheritdoc/>
    public ulong SkuId { get; private set; }

    /// <inheritdoc/>
    public ulong? UserId { get; private set; }

    /// <inheritdoc/>
    public ulong? GuildId { get; private set; }

    /// <inheritdoc/>
    public ulong ApplicationId { get; private set; }

    /// <inheritdoc/>
    public EntitlementType Type { get; private set; }

    /// <inheritdoc/>
    public bool IsConsumed { get; private set; }

    /// <inheritdoc/>
    public DateTimeOffset? StartsAt { get; private set; }

    /// <inheritdoc/>
    public DateTimeOffset? EndsAt { get; private set; }

    internal RestEntitlement(BaseDiscordClient discord, ulong id) : base(discord, id)
    {
    }

    internal static RestEntitlement Create(BaseDiscordClient discord, Model model)
    {
        var entity = new RestEntitlement(discord, model.Id);
        entity.Update(model);
        return entity;
    }

    internal void Update(Model model)
    {
        this.SkuId = model.SkuId;
        this.UserId = model.UserId.ToNullable();
        this.GuildId = model.GuildId.ToNullable();
        this.ApplicationId = model.ApplicationId;
        this.Type = model.Type;
        this.IsConsumed = model.IsConsumed;
        this.StartsAt = model.StartsAt.ToNullable();
        this.EndsAt = model.EndsAt.ToNullable();
    }
}
