using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Discord.OAuth;

/// <summary>
/// A representation of an oauth scope collection.
/// </summary>
public interface IOAuthScopeCollection :
    ICollection<string>,
    ICollection,
    IReadOnlyCollection<string>,
    IEnumerable<string>,
    IEnumerable
{
    /// <summary>
    /// Returns whether or not this oauth scope collection
    /// contains the specified scope.
    /// </summary>
    /// <remarks>
    /// Comparisons are made with
    /// <see cref="StringComparison.OrdinalIgnoreCase"/>.
    /// </remarks>
    /// <param name="scope">
    /// The scope to check.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if this oauth scope collection
    /// contains <paramref name="scope"/>; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    new bool Contains([NotNullWhen(true)] string? scope);

    /// <summary>
    /// Returns whether or not this oauth scope collection
    /// contains the specified scope.
    /// </summary>
    /// <remarks>
    /// Comparisons are made with
    /// <see cref="StringComparison.OrdinalIgnoreCase"/>.
    /// </remarks>
    /// <param name="scope">
    /// The scope to check.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if this oauth scope collection
    /// contains <paramref name="scope"/>; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    bool Contains(ReadOnlySpan<char> scope);

    /// <summary>
    /// The count of defined/known scopes.
    /// </summary>
    int DefinedCount { get; }

    /// <inheritdoc cref="OAuthScopes.ReadActivities"/>
    bool ReadActivities { get; }

    /// <inheritdoc cref="OAuthScopes.WriteActivities"/>
    bool WriteActivities { get; }

    /// <inheritdoc cref="OAuthScopes.ReadApplicationBuilds"/>
    bool ReadApplicationBuilds { get; }

    /// <inheritdoc cref="OAuthScopes.UploadApplicationBuilds"/>
    bool UploadApplicationBuilds { get; }

    /// <inheritdoc cref="OAuthScopes.ApplicationCommands"/>
    bool ApplicationCommands { get; }

    /// <inheritdoc cref="OAuthScopes.UpdateApplicationCommands"/>
    bool UpdateApplicationCommands { get; }

    /// <inheritdoc cref="OAuthScopes.UpdateApplicationCommandPermissions"/>
    bool UpdateApplicationCommandPermissions { get; }

    /// <inheritdoc cref="OAuthScopes.ApplicationEntitlements"/>
    bool ApplicationEntitlements { get; }

    /// <inheritdoc cref="OAuthScopes.UpdateApplicationStore"/>
    bool UpdateApplicationStore { get; }

    /// <inheritdoc cref="OAuthScopes.Bot"/>
    bool Bot { get; }

    /// <inheritdoc cref="OAuthScopes.Connections"/>
    bool Connections { get; }

    /// <inheritdoc cref="OAuthScopes.ReadDMChannels"/>
    bool ReadDMChannels { get; }

    /// <inheritdoc cref="OAuthScopes.Email"/>
    bool Email { get; }

    /// <inheritdoc cref="OAuthScopes.JoinGroupDM"/>
    bool JoinGroupDM { get; }

    /// <inheritdoc cref="OAuthScopes.Guilds"/>
    bool Guilds { get; }

    /// <inheritdoc cref="OAuthScopes.JoinGuilds"/>
    bool JoinGuilds { get; }

    /// <inheritdoc cref="OAuthScopes.ReadGuildMembers"/>
    bool ReadGuildMembers { get; }

    /// <inheritdoc cref="OAuthScopes.Identify"/>
    bool Identify { get; }

    /// <inheritdoc cref="OAuthScopes.ReadMessages"/>
    bool ReadMessages { get; }

    /// <inheritdoc cref="OAuthScopes.ReadRelationships"/>
    bool ReadRelationships { get; }

    /// <inheritdoc cref="OAuthScopes.WriteRoleConnections"/>
    bool WriteRoleConnections { get; }

    /// <inheritdoc cref="OAuthScopes.Rpc"/>
    bool Rpc { get; }

    /// <inheritdoc cref="OAuthScopes.WriteRpcActivities"/>
    bool WriteRpcActivities { get; }

    /// <inheritdoc cref="OAuthScopes.ReadRpcNotifications"/>
    bool ReadRpcNotifications { get; }

    /// <inheritdoc cref="OAuthScopes.ReadRpcVoice"/>
    bool ReadRpcVoice { get; }

    /// <inheritdoc cref="OAuthScopes.WriteRpcVoice"/>
    bool WriteRpcVoice { get; }

    /// <inheritdoc cref="OAuthScopes.Voice"/>
    bool Voice { get; }

    /// <inheritdoc cref="OAuthScopes.IncomingWebhook"/>
    bool IncomingWebhook { get; }

    /// <summary>
    /// The unknown scopes.
    /// </summary>
    IOAuthUnknownScopeCollection UnknownScopes { get; }
}

/// <summary>
/// An immutable collection of OAuth Scopes.
/// </summary>
public sealed partial class OAuthScopeCollection : IOAuthScopeCollection
{
    internal OAuthScopeCollection(
        string underlyingString,
        int definedCount,
        bool readActivities = false,
        bool writeActivities = false,
        bool readApplicationBuilds = false,
        bool uploadApplicationBuilds = false,
        bool applicationCommands = false,
        bool updateApplicationCommands = false,
        bool updateApplicationCommandPermissions = false,
        bool applicationEntitlements = false,
        bool updateApplicationStore = false,
        bool bot = false,
        bool connections = false,
        bool readDMChannels = false,
        bool email = false,
        bool joinGroupDM = false,
        bool guilds = false,
        bool joinGuilds = false,
        bool readGuildMembers = false,
        bool identify = false,
        bool readMessages = false,
        bool readRelationships = false,
        bool writeRoleConnections = false,
        bool rpc = false,
        bool writeRpcActivities = false,
        bool readRpcNotifications = false,
        bool readRpcVoice = false,
        bool writeRpcVoice = false,
        bool voice = false,
        bool incomingWebhook = false,
        UnknownScopeCollection? unknownScopes = null)
    {
        this.StringValue = underlyingString;
        this.DefinedCount = definedCount;

        this.RawValue = OAuthKnownScopes.GetRawValue(
            readActivities: readActivities,
            writeActivities: writeActivities,
            readApplicationBuilds: readApplicationBuilds,
            uploadApplicationBuilds: uploadApplicationBuilds,
            applicationCommands: applicationCommands,
            updateApplicationCommands: updateApplicationCommands,
            updateApplicationCommandPermissions: updateApplicationCommandPermissions,
            applicationEntitlements: applicationEntitlements,
            updateApplicationStore: updateApplicationStore,
            bot: bot,
            connections: connections,
            readDMChannels: readDMChannels,
            email: email,
            joinGroupDM: joinGroupDM,
            guilds: guilds,
            joinGuilds: joinGuilds,
            readGuildMembers: readGuildMembers,
            identify: identify,
            readMessages: readMessages,
            readRelationships: readRelationships,
            writeRoleConnections: writeRoleConnections,
            rpc: rpc,
            writeRpcActivities: writeRpcActivities,
            readRpcNotifications: readRpcNotifications,
            readRpcVoice: readRpcVoice,
            writeRpcVoice: writeRpcVoice,
            voice: voice,
            incomingWebhook: incomingWebhook);

        this.UnknownScopes = unknownScopes ?? UnknownScopeCollection.Empty;
        this.Count = this.DefinedCount + this.UnknownScopes.Count;
    }

    internal OAuthScopeCollection(
        string underlyingString,
        int definedCount,
        uint rawValue,
        UnknownScopeCollection? unknownScopes = null)
    {
        this.StringValue = underlyingString;
        this.DefinedCount = definedCount;

        this.RawValue = rawValue;

        this.UnknownScopes = unknownScopes ?? UnknownScopeCollection.Empty;
        this.Count = this.DefinedCount + this.UnknownScopes.Count;
    }

    /// <summary>
    /// The number of scopes in this oauth scope collection.
    /// </summary>
    public int Count { get; }

    /// <inheritdoc cref="IOAuthScopeCollection.DefinedCount"/>
    public int DefinedCount { get; }

    internal uint RawValue { get; }

    /// <summary>
    /// The stringified version of this scope collection.
    /// </summary>
    public string StringValue { get; }

    /// <inheritdoc cref="IOAuthScopeCollection.ReadActivities"/>
    public bool ReadActivities => (this.RawValue & OAuthKnownScopes.ReadActivities) == OAuthKnownScopes.ReadActivities;

    /// <inheritdoc cref="IOAuthScopeCollection.WriteActivities"/>
    public bool WriteActivities => (this.RawValue & OAuthKnownScopes.WriteActivities) == OAuthKnownScopes.WriteActivities;

    /// <inheritdoc cref="IOAuthScopeCollection.ReadApplicationBuilds"/>
    public bool ReadApplicationBuilds => (this.RawValue & OAuthKnownScopes.ReadApplicationBuilds) == OAuthKnownScopes.ReadApplicationBuilds;

    /// <inheritdoc cref="IOAuthScopeCollection.UploadApplicationBuilds"/>
    public bool UploadApplicationBuilds => (this.RawValue & OAuthKnownScopes.UploadApplicationBuilds) == OAuthKnownScopes.UploadApplicationBuilds;

    /// <inheritdoc cref="IOAuthScopeCollection.ApplicationCommands"/>
    public bool ApplicationCommands => (this.RawValue & OAuthKnownScopes.ApplicationCommands) == OAuthKnownScopes.ApplicationCommands;

    /// <inheritdoc cref="IOAuthScopeCollection.UpdateApplicationCommands"/>
    public bool UpdateApplicationCommands => (this.RawValue & OAuthKnownScopes.UpdateApplicationCommands) == OAuthKnownScopes.UpdateApplicationCommands;

    /// <inheritdoc cref="IOAuthScopeCollection.UpdateApplicationCommandPermissions"/>
    public bool UpdateApplicationCommandPermissions => (this.RawValue & OAuthKnownScopes.UpdateApplicationCommandPermissions) == OAuthKnownScopes.UpdateApplicationCommandPermissions;

    /// <inheritdoc cref="IOAuthScopeCollection.ApplicationEntitlements"/>
    public bool ApplicationEntitlements => (this.RawValue & OAuthKnownScopes.ApplicationEntitlements) == OAuthKnownScopes.ApplicationEntitlements;

    /// <inheritdoc cref="IOAuthScopeCollection.UpdateApplicationStore"/>
    public bool UpdateApplicationStore => (this.RawValue & OAuthKnownScopes.UpdateApplicationStore) == OAuthKnownScopes.UpdateApplicationStore;

    /// <inheritdoc cref="IOAuthScopeCollection.Bot"/>
    public bool Bot => (this.RawValue & OAuthKnownScopes.Bot) == OAuthKnownScopes.Bot;

    /// <inheritdoc cref="IOAuthScopeCollection.Connections"/>
    public bool Connections => (this.RawValue & OAuthKnownScopes.Connections) == OAuthKnownScopes.Connections;

    /// <inheritdoc cref="IOAuthScopeCollection.ReadDMChannels"/>
    public bool ReadDMChannels => (this.RawValue & OAuthKnownScopes.ReadDMChannels) == OAuthKnownScopes.ReadDMChannels;

    /// <inheritdoc cref="IOAuthScopeCollection.Email"/>
    public bool Email => (this.RawValue & OAuthKnownScopes.Email) == OAuthKnownScopes.Email;

    /// <inheritdoc cref="IOAuthScopeCollection.JoinGroupDM"/>
    public bool JoinGroupDM => (this.RawValue & OAuthKnownScopes.JoinGroupDM) == OAuthKnownScopes.JoinGroupDM;

    /// <inheritdoc cref="IOAuthScopeCollection.Guilds"/>
    public bool Guilds => (this.RawValue & OAuthKnownScopes.Guilds) == OAuthKnownScopes.Guilds;

    /// <inheritdoc cref="IOAuthScopeCollection.JoinGuilds"/>
    public bool JoinGuilds => (this.RawValue & OAuthKnownScopes.JoinGuilds) == OAuthKnownScopes.JoinGuilds;

    /// <inheritdoc cref="IOAuthScopeCollection.ReadGuildMembers"/>
    public bool ReadGuildMembers => (this.RawValue & OAuthKnownScopes.ReadGuildMembers) == OAuthKnownScopes.ReadGuildMembers;

    /// <inheritdoc cref="IOAuthScopeCollection.Identify"/>
    public bool Identify => (this.RawValue & OAuthKnownScopes.Identify) == OAuthKnownScopes.Identify;

    /// <inheritdoc cref="IOAuthScopeCollection.ReadMessages"/>
    public bool ReadMessages => (this.RawValue & OAuthKnownScopes.ReadMessages) == OAuthKnownScopes.ReadMessages;

    /// <inheritdoc cref="IOAuthScopeCollection.ReadRelationships"/>
    public bool ReadRelationships => (this.RawValue & OAuthKnownScopes.ReadRelationships) == OAuthKnownScopes.ReadRelationships;

    /// <inheritdoc cref="IOAuthScopeCollection.WriteRoleConnections"/>
    public bool WriteRoleConnections => (this.RawValue & OAuthKnownScopes.WriteRoleConnections) == OAuthKnownScopes.WriteRoleConnections;

    /// <inheritdoc cref="IOAuthScopeCollection.Rpc"/>
    public bool Rpc => (this.RawValue & OAuthKnownScopes.Rpc) == OAuthKnownScopes.Rpc;

    /// <inheritdoc cref="IOAuthScopeCollection.WriteRpcActivities"/>
    public bool WriteRpcActivities => (this.RawValue & OAuthKnownScopes.WriteRpcActivities) == OAuthKnownScopes.WriteRpcActivities;

    /// <inheritdoc cref="IOAuthScopeCollection.ReadRpcNotifications"/>
    public bool ReadRpcNotifications => (this.RawValue & OAuthKnownScopes.ReadRpcNotifications) == OAuthKnownScopes.ReadRpcNotifications;

    /// <inheritdoc cref="IOAuthScopeCollection.ReadRpcVoice"/>
    public bool ReadRpcVoice => (this.RawValue & OAuthKnownScopes.ReadRpcVoice) == OAuthKnownScopes.ReadRpcVoice;

    /// <inheritdoc cref="IOAuthScopeCollection.WriteRpcVoice"/>
    public bool WriteRpcVoice => (this.RawValue & OAuthKnownScopes.WriteRpcVoice) == OAuthKnownScopes.WriteRpcVoice;

    /// <inheritdoc cref="IOAuthScopeCollection.Voice"/>
    public bool Voice => (this.RawValue & OAuthKnownScopes.Voice) == OAuthKnownScopes.Voice;

    /// <inheritdoc cref="IOAuthScopeCollection.IncomingWebhook"/>
    public bool IncomingWebhook => (this.RawValue & OAuthKnownScopes.IncomingWebhook) == OAuthKnownScopes.IncomingWebhook;

    /// <inheritdoc cref="IOAuthScopeCollection.UnknownScopes"/>
    public UnknownScopeCollection UnknownScopes { get; }

    /// <summary>
    /// Copies the elements of this oauth scope collection to
    /// the specified array.
    /// </summary>
    /// <param name="array">
    /// The zero-index-based one-dimensional string array to
    /// copy the oauth scopes to.
    /// </param>
    /// <param name="arrayIndex">
    /// The zero-based index in <paramref name="array"/> at
    /// which copying begins.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="array"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="arrayIndex"/> is less than 0.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// There's not enough space in <paramref name="array"/>
    /// starting at <paramref name="arrayIndex"/> to fit this
    /// oauth scope collection.
    /// </exception>
    public void CopyTo(string[] array, int arrayIndex)
    {
        if (array is null)
            throw new ArgumentNullException(nameof(array));
        if (arrayIndex < 0)
            throw new ArgumentOutOfRangeException(nameof(arrayIndex));
        if (arrayIndex + this.Count > array.Length)
            throw new ArgumentException("Not enough space to copy items into array.", nameof(array));

        Enumerator enumerator = new(this);
        while (enumerator.MoveNext())
        {
            array.SetValue(enumerator.Current, arrayIndex++);
        }
    }

    /// <inheritdoc cref="IOAuthScopeCollection.Contains(string?)"/>
    public bool Contains([NotNullWhen(true)] string? scope)
    {
        if (string.IsNullOrEmpty(scope))
            return false;

        if (scope.Equals(OAuthScopes.ReadActivities, StringComparison.OrdinalIgnoreCase))
            return this.ReadActivities;
        if (scope.Equals(OAuthScopes.WriteActivities, StringComparison.OrdinalIgnoreCase))
            return this.WriteActivities;
        if (scope.Equals(OAuthScopes.ReadApplicationBuilds, StringComparison.OrdinalIgnoreCase))
            return this.ReadApplicationBuilds;
        if (scope.Equals(OAuthScopes.UploadApplicationBuilds, StringComparison.OrdinalIgnoreCase))
            return this.UploadApplicationBuilds;
        if (scope.Equals(OAuthScopes.ApplicationCommands, StringComparison.OrdinalIgnoreCase))
            return this.ApplicationCommands;
        if (scope.Equals(OAuthScopes.UpdateApplicationCommands, StringComparison.OrdinalIgnoreCase))
            return this.UpdateApplicationCommands;
        if (scope.Equals(OAuthScopes.UpdateApplicationCommandPermissions, StringComparison.OrdinalIgnoreCase))
            return this.UpdateApplicationCommandPermissions;
        if (scope.Equals(OAuthScopes.ApplicationEntitlements, StringComparison.OrdinalIgnoreCase))
            return this.ApplicationEntitlements;
        if (scope.Equals(OAuthScopes.UpdateApplicationStore, StringComparison.OrdinalIgnoreCase))
            return this.UpdateApplicationStore;
        if (scope.Equals(OAuthScopes.Bot, StringComparison.OrdinalIgnoreCase))
            return this.Bot;
        if (scope.Equals(OAuthScopes.Connections, StringComparison.OrdinalIgnoreCase))
            return this.Connections;
        if (scope.Equals(OAuthScopes.ReadDMChannels, StringComparison.OrdinalIgnoreCase))
            return this.ReadDMChannels;
        if (scope.Equals(OAuthScopes.Email, StringComparison.OrdinalIgnoreCase))
            return this.Email;
        if (scope.Equals(OAuthScopes.JoinGroupDM, StringComparison.OrdinalIgnoreCase))
            return this.JoinGroupDM;
        if (scope.Equals(OAuthScopes.Guilds, StringComparison.OrdinalIgnoreCase))
            return this.Guilds;
        if (scope.Equals(OAuthScopes.JoinGuilds, StringComparison.OrdinalIgnoreCase))
            return this.JoinGuilds;
        if (scope.Equals(OAuthScopes.ReadGuildMembers, StringComparison.OrdinalIgnoreCase))
            return this.ReadGuildMembers;
        if (scope.Equals(OAuthScopes.Identify, StringComparison.OrdinalIgnoreCase))
            return this.Identify;
        if (scope.Equals(OAuthScopes.ReadMessages, StringComparison.OrdinalIgnoreCase))
            return this.ReadMessages;
        if (scope.Equals(OAuthScopes.ReadRelationships, StringComparison.OrdinalIgnoreCase))
            return this.ReadRelationships;
        if (scope.Equals(OAuthScopes.WriteRoleConnections, StringComparison.OrdinalIgnoreCase))
            return this.WriteRoleConnections;
        if (scope.Equals(OAuthScopes.Rpc, StringComparison.OrdinalIgnoreCase))
            return this.Rpc;
        if (scope.Equals(OAuthScopes.WriteRpcActivities, StringComparison.OrdinalIgnoreCase))
            return this.WriteRpcActivities;
        if (scope.Equals(OAuthScopes.ReadRpcNotifications, StringComparison.OrdinalIgnoreCase))
            return this.ReadRpcNotifications;
        if (scope.Equals(OAuthScopes.ReadRpcVoice, StringComparison.OrdinalIgnoreCase))
            return this.ReadRpcVoice;
        if (scope.Equals(OAuthScopes.WriteRpcVoice, StringComparison.OrdinalIgnoreCase))
            return this.WriteRpcVoice;
        if (scope.Equals(OAuthScopes.Voice, StringComparison.OrdinalIgnoreCase))
            return this.Voice;
        if (scope.Equals(OAuthScopes.IncomingWebhook, StringComparison.OrdinalIgnoreCase))
            return this.IncomingWebhook;
        
        return this.UnknownScopes.Contains(scope);
    }

    /// <inheritdoc cref="IOAuthScopeCollection.Contains(ReadOnlySpan{char})"/>
    public bool Contains(ReadOnlySpan<char> scope)
    {
        if (scope.IsEmpty)
            return false;

        if (scope.Equals(OAuthScopes.ReadActivities, StringComparison.OrdinalIgnoreCase))
            return this.ReadActivities;
        if (scope.Equals(OAuthScopes.WriteActivities, StringComparison.OrdinalIgnoreCase))
            return this.WriteActivities;
        if (scope.Equals(OAuthScopes.ReadApplicationBuilds, StringComparison.OrdinalIgnoreCase))
            return this.ReadApplicationBuilds;
        if (scope.Equals(OAuthScopes.UploadApplicationBuilds, StringComparison.OrdinalIgnoreCase))
            return this.UploadApplicationBuilds;
        if (scope.Equals(OAuthScopes.ApplicationCommands, StringComparison.OrdinalIgnoreCase))
            return this.ApplicationCommands;
        if (scope.Equals(OAuthScopes.UpdateApplicationCommands, StringComparison.OrdinalIgnoreCase))
            return this.UpdateApplicationCommands;
        if (scope.Equals(OAuthScopes.UpdateApplicationCommandPermissions, StringComparison.OrdinalIgnoreCase))
            return this.UpdateApplicationCommandPermissions;
        if (scope.Equals(OAuthScopes.ApplicationEntitlements, StringComparison.OrdinalIgnoreCase))
            return this.ApplicationEntitlements;
        if (scope.Equals(OAuthScopes.UpdateApplicationStore, StringComparison.OrdinalIgnoreCase))
            return this.UpdateApplicationStore;
        if (scope.Equals(OAuthScopes.Bot, StringComparison.OrdinalIgnoreCase))
            return this.Bot;
        if (scope.Equals(OAuthScopes.Connections, StringComparison.OrdinalIgnoreCase))
            return this.Connections;
        if (scope.Equals(OAuthScopes.ReadDMChannels, StringComparison.OrdinalIgnoreCase))
            return this.ReadDMChannels;
        if (scope.Equals(OAuthScopes.Email, StringComparison.OrdinalIgnoreCase))
            return this.Email;
        if (scope.Equals(OAuthScopes.JoinGroupDM, StringComparison.OrdinalIgnoreCase))
            return this.JoinGroupDM;
        if (scope.Equals(OAuthScopes.Guilds, StringComparison.OrdinalIgnoreCase))
            return this.Guilds;
        if (scope.Equals(OAuthScopes.JoinGuilds, StringComparison.OrdinalIgnoreCase))
            return this.JoinGuilds;
        if (scope.Equals(OAuthScopes.ReadGuildMembers, StringComparison.OrdinalIgnoreCase))
            return this.ReadGuildMembers;
        if (scope.Equals(OAuthScopes.Identify, StringComparison.OrdinalIgnoreCase))
            return this.Identify;
        if (scope.Equals(OAuthScopes.ReadMessages, StringComparison.OrdinalIgnoreCase))
            return this.ReadMessages;
        if (scope.Equals(OAuthScopes.ReadRelationships, StringComparison.OrdinalIgnoreCase))
            return this.ReadRelationships;
        if (scope.Equals(OAuthScopes.WriteRoleConnections, StringComparison.OrdinalIgnoreCase))
            return this.WriteRoleConnections;
        if (scope.Equals(OAuthScopes.Rpc, StringComparison.OrdinalIgnoreCase))
            return this.Rpc;
        if (scope.Equals(OAuthScopes.WriteRpcActivities, StringComparison.OrdinalIgnoreCase))
            return this.WriteRpcActivities;
        if (scope.Equals(OAuthScopes.ReadRpcNotifications, StringComparison.OrdinalIgnoreCase))
            return this.ReadRpcNotifications;
        if (scope.Equals(OAuthScopes.ReadRpcVoice, StringComparison.OrdinalIgnoreCase))
            return this.ReadRpcVoice;
        if (scope.Equals(OAuthScopes.WriteRpcVoice, StringComparison.OrdinalIgnoreCase))
            return this.WriteRpcVoice;
        if (scope.Equals(OAuthScopes.Voice, StringComparison.OrdinalIgnoreCase))
            return this.Voice;
        if (scope.Equals(OAuthScopes.IncomingWebhook, StringComparison.OrdinalIgnoreCase))
            return this.IncomingWebhook;
        
        return this.UnknownScopes.Contains(scope);
    }

    /// <summary>
    /// Gets an enumerator that returns
    /// ReadOnlySpan&lt;<see cref="char"/>&gt;
    /// instead of <see cref="string"/>, reducing
    /// allocations from substringing the underlying
    /// scope string value.
    /// </summary>
    /// <returns>
    /// A span enumerator.
    /// </returns>
    public SpanEnumerator GetSpanEnumerator()
    {
        return new SpanEnumerator(this);
    }

    /// <summary>
    /// Gets an enumerator that returns
    /// <see cref="string"/>. Each enumeration requires
    /// substringing the underlying scope string value. 
    /// </summary>
    /// <returns>
    /// A string enumerator.
    /// </returns>
    public Enumerator GetEnumerator()
    {
        return new Enumerator(this);
    }

    /// <summary>
    /// Returns the stringified version of this oauth scope
    /// collection.
    /// </summary>
    /// <returns>
    /// <see cref="StringValue"/>
    /// </returns>
    public sealed override string ToString()
    {
        return this.StringValue;
    }

    IOAuthUnknownScopeCollection IOAuthScopeCollection.UnknownScopes => this.UnknownScopes;
    bool ICollection<string>.IsReadOnly => true;
    bool ICollection.IsSynchronized => true;
    object ICollection.SyncRoot => this;

    void ICollection<string>.Add(string item)
        => throw new NotSupportedException();

    void ICollection<string>.Clear()
        => throw new NotSupportedException();

    bool ICollection<string>.Remove(string item)
        => throw new NotSupportedException();
    
    void ICollection.CopyTo(Array array, int index)
    {
        if (array is null)
            throw new ArgumentNullException(nameof(array));
        if (index < 0)
            throw new ArgumentOutOfRangeException(nameof(index));
        if (array.Rank != 1)
            throw new ArgumentException("Multi-Dimensional arrays aren't supported", nameof(array));
        if (index + this.Count > array.Length)
            throw new ArgumentException("Not enough space to copy items into array.", nameof(array));

        Enumerator enumerator = new(this);
        while (enumerator.MoveNext())
        {
            array.SetValue(enumerator.Current, index++);
        }
    }

    IEnumerator<string> IEnumerable<string>.GetEnumerator()
    {
        return new SlowEnumerator(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return new SlowEnumerator(this);
    }

    /// <summary>
    /// An empty OAuth Scope Collection.
    /// </summary>
    public static readonly OAuthScopeCollection None = new(string.Empty, 0);

    public static OAuthScopeCollection Parse(string value)
    {
        if (value is null)
            throw new ArgumentNullException(nameof(value));

        if (value.Length == 0)
            return OAuthScopeCollection.None;

        return OAuthScopeCollection.Parse(value.AsSpan(), value);
    }

    public static OAuthScopeCollection Parse(ReadOnlySpan<char> value)
    {
        if (value.Length == 0)
            return OAuthScopeCollection.None;

        return OAuthScopeCollection.Parse(value, null);
    }

    private static OAuthScopeCollection Parse(ReadOnlySpan<char> value, string? stringValue)
    {
        ReadOnlySpan<char> fullSpan = value;

        uint rawValue = 0;
        List<ScopeStringSegment> unknownScopes = new();
        int definedCount = 0;

        int currentIndex = 0;
        int spaceIndex;
        ReadOnlySpan<char> scope;

        while (true)
        {
            if ((spaceIndex = value.IndexOf(' ')) >= 0)
                scope = value.Slice(0, spaceIndex);
            else
                scope = value;

            if (scope.Length == 0)
                throw new ArgumentException($"Scope cannot be empty at index {currentIndex}", nameof(value));

            if (scope.Equals(OAuthScopes.ReadActivities, StringComparison.OrdinalIgnoreCase))
            {
                if ((rawValue & OAuthKnownScopes.ReadActivities) == 0)
                {
                    rawValue |= OAuthKnownScopes.ReadActivities;
                    definedCount++;
                }
            }
            else if (scope.Equals(OAuthScopes.WriteActivities, StringComparison.OrdinalIgnoreCase))
            {
                if ((rawValue & OAuthKnownScopes.WriteActivities) == 0)
                {
                    rawValue |= OAuthKnownScopes.WriteActivities;
                    definedCount++;
                }
            }
            else if (scope.Equals(OAuthScopes.ReadApplicationBuilds, StringComparison.OrdinalIgnoreCase))
            {
                if ((rawValue & OAuthKnownScopes.ReadApplicationBuilds) == 0)
                {
                    rawValue |= OAuthKnownScopes.ReadApplicationBuilds;
                    definedCount++;
                }
            }
            else if (scope.Equals(OAuthScopes.UploadApplicationBuilds, StringComparison.OrdinalIgnoreCase))
            {
                if ((rawValue & OAuthKnownScopes.UploadApplicationBuilds) == 0)
                {
                    rawValue |= OAuthKnownScopes.UploadApplicationBuilds;
                    definedCount++;
                }
            }
            else if (scope.Equals(OAuthScopes.ApplicationCommands, StringComparison.OrdinalIgnoreCase))
            {
                if ((rawValue & OAuthKnownScopes.ApplicationCommands) == 0)
                {
                    rawValue |= OAuthKnownScopes.ApplicationCommands;
                    definedCount++;
                }
            }
            else if (scope.Equals(OAuthScopes.UpdateApplicationCommands, StringComparison.OrdinalIgnoreCase))
            {
                if ((rawValue & OAuthKnownScopes.UpdateApplicationCommands) == 0)
                {
                    rawValue |= OAuthKnownScopes.UpdateApplicationCommands;
                    definedCount++;
                }
            }
            else if (scope.Equals(OAuthScopes.UpdateApplicationCommandPermissions, StringComparison.OrdinalIgnoreCase))
            {
                if ((rawValue & OAuthKnownScopes.UpdateApplicationCommandPermissions) == 0)
                {
                    rawValue |= OAuthKnownScopes.UpdateApplicationCommandPermissions;
                    definedCount++;
                }
            }
            else if (scope.Equals(OAuthScopes.ApplicationEntitlements, StringComparison.OrdinalIgnoreCase))
            {
                if ((rawValue & OAuthKnownScopes.ApplicationEntitlements) == 0)
                {
                    rawValue |= OAuthKnownScopes.ApplicationEntitlements;
                    definedCount++;
                }
            }
            else if (scope.Equals(OAuthScopes.UpdateApplicationStore, StringComparison.OrdinalIgnoreCase))
            {
                if ((rawValue & OAuthKnownScopes.UpdateApplicationStore) == 0)
                {
                    rawValue |= OAuthKnownScopes.UpdateApplicationStore;
                    definedCount++;
                }
            }
            else if (scope.Equals(OAuthScopes.Bot, StringComparison.OrdinalIgnoreCase))
            {
                if ((rawValue & OAuthKnownScopes.Bot) == 0)
                {
                    rawValue |= OAuthKnownScopes.Bot;
                    definedCount++;
                }
            }
            else if (scope.Equals(OAuthScopes.Connections, StringComparison.OrdinalIgnoreCase))
            {
                if ((rawValue & OAuthKnownScopes.Connections) == 0)
                {
                    rawValue |= OAuthKnownScopes.Connections;
                    definedCount++;
                }
            }
            else if (scope.Equals(OAuthScopes.ReadDMChannels, StringComparison.OrdinalIgnoreCase))
            {
                if ((rawValue & OAuthKnownScopes.ReadDMChannels) == 0)
                {
                    rawValue |= OAuthKnownScopes.ReadDMChannels;
                    definedCount++;
                }
            }
            else if (scope.Equals(OAuthScopes.Email, StringComparison.OrdinalIgnoreCase))
            {
                if ((rawValue & OAuthKnownScopes.Email) == 0)
                {
                    rawValue |= OAuthKnownScopes.Email;
                    definedCount++;
                }
            }
            else if (scope.Equals(OAuthScopes.JoinGroupDM, StringComparison.OrdinalIgnoreCase))
            {
                if ((rawValue & OAuthKnownScopes.JoinGroupDM) == 0)
                {
                    rawValue |= OAuthKnownScopes.JoinGroupDM;
                    definedCount++;
                }
            }
            else if (scope.Equals(OAuthScopes.Guilds, StringComparison.OrdinalIgnoreCase))
            {
                if ((rawValue & OAuthKnownScopes.Guilds) == 0)
                {
                    rawValue |= OAuthKnownScopes.Guilds;
                    definedCount++;
                }
            }
            else if (scope.Equals(OAuthScopes.JoinGuilds, StringComparison.OrdinalIgnoreCase))
            {
                if ((rawValue & OAuthKnownScopes.JoinGuilds) == 0)
                {
                    rawValue |= OAuthKnownScopes.JoinGuilds;
                    definedCount++;
                }
            }
            else if (scope.Equals(OAuthScopes.ReadGuildMembers, StringComparison.OrdinalIgnoreCase))
            {
                if ((rawValue & OAuthKnownScopes.ReadGuildMembers) == 0)
                {
                    rawValue |= OAuthKnownScopes.ReadGuildMembers;
                    definedCount++;
                }
            }
            else if (scope.Equals(OAuthScopes.Identify, StringComparison.OrdinalIgnoreCase))
            {
                if ((rawValue & OAuthKnownScopes.Identify) == 0)
                {
                    rawValue |= OAuthKnownScopes.Identify;
                    definedCount++;
                }
            }
            else if (scope.Equals(OAuthScopes.ReadMessages, StringComparison.OrdinalIgnoreCase))
            {
                if ((rawValue & OAuthKnownScopes.ReadMessages) == 0)
                {
                    rawValue |= OAuthKnownScopes.ReadMessages;
                    definedCount++;
                }
            }
            else if (scope.Equals(OAuthScopes.ReadRelationships, StringComparison.OrdinalIgnoreCase))
            {
                if ((rawValue & OAuthKnownScopes.ReadRelationships) == 0)
                {
                    rawValue |= OAuthKnownScopes.ReadRelationships;
                    definedCount++;
                }
            }
            else if (scope.Equals(OAuthScopes.WriteRoleConnections, StringComparison.OrdinalIgnoreCase))
            {
                if ((rawValue & OAuthKnownScopes.WriteRoleConnections) == 0)
                {
                    rawValue |= OAuthKnownScopes.WriteRoleConnections;
                    definedCount++;
                }
            }
            else if (scope.Equals(OAuthScopes.Rpc, StringComparison.OrdinalIgnoreCase))
            {
                if ((rawValue & OAuthKnownScopes.Rpc) == 0)
                {
                    rawValue |= OAuthKnownScopes.Rpc;
                    definedCount++;
                }
            }
            else if (scope.Equals(OAuthScopes.WriteRpcActivities, StringComparison.OrdinalIgnoreCase))
            {
                if ((rawValue & OAuthKnownScopes.WriteRpcActivities) == 0)
                {
                    rawValue |= OAuthKnownScopes.WriteRpcActivities;
                    definedCount++;
                }
            }
            else if (scope.Equals(OAuthScopes.ReadRpcNotifications, StringComparison.OrdinalIgnoreCase))
            {
                if ((rawValue & OAuthKnownScopes.ReadRpcNotifications) == 0)
                {
                    rawValue |= OAuthKnownScopes.ReadRpcNotifications;
                    definedCount++;
                }
            }
            else if (scope.Equals(OAuthScopes.ReadRpcVoice, StringComparison.OrdinalIgnoreCase))
            {
                if ((rawValue & OAuthKnownScopes.ReadRpcVoice) == 0)
                {
                    rawValue |= OAuthKnownScopes.ReadRpcVoice;
                    definedCount++;
                }
            }
            else if (scope.Equals(OAuthScopes.WriteRpcVoice, StringComparison.OrdinalIgnoreCase))
            {
                if ((rawValue & OAuthKnownScopes.WriteRpcVoice) == 0)
                {
                    rawValue |= OAuthKnownScopes.WriteRpcVoice;
                    definedCount++;
                }
            }
            else if (scope.Equals(OAuthScopes.Voice, StringComparison.OrdinalIgnoreCase))
            {
                if ((rawValue & OAuthKnownScopes.Voice) == 0)
                {
                    rawValue |= OAuthKnownScopes.Voice;
                    definedCount++;
                }
            }
            else if (scope.Equals(OAuthScopes.IncomingWebhook, StringComparison.OrdinalIgnoreCase))
            {
                if ((rawValue & OAuthKnownScopes.IncomingWebhook) == 0)
                {
                    rawValue |= OAuthKnownScopes.IncomingWebhook;
                    definedCount++;
                }
            }
            else
            {
                bool hasScope = false;
                foreach (ScopeStringSegment segment in unknownScopes)
                {
                    if (fullSpan.Slice(segment.startIndex, segment.length).Equals(scope, StringComparison.OrdinalIgnoreCase))
                    {
                        hasScope = true;
                        break;
                    }
                }

                if (!hasScope)
                    unknownScopes.Add(new ScopeStringSegment(currentIndex, scope.Length));
            }
            currentIndex += spaceIndex + 1;

            if (spaceIndex >= 0)
                value = value.Slice(spaceIndex + 1);
            else
                break;
        }
        
        stringValue ??= fullSpan.ToString();

        return new OAuthScopeCollection(
            underlyingString: stringValue,
            definedCount: definedCount,
            rawValue: rawValue,
            unknownScopes.Count == 0 ?
                null :
                new(stringValue, unknownScopes.ToArray()));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string? GetNameFromIndex(int index, OAuthScopeCollection collection)
    {
        if (index < 0 || index >= OAuthKnownScopes.Count)
            return null;

        uint mask = 1U << index;
        if ((collection.RawValue & mask) != mask)
            return null;
        
        return OAuthScopes.GetNameFromIndex(index);
    }

    internal readonly struct ScopeStringSegment
    {
        public readonly int startIndex;
        public readonly int length;
        public readonly string? stringValue;

        public ScopeStringSegment(int startIndex, int length, string? stringValue = null)
        {
            this.startIndex = startIndex;
            this.length = length;
            this.stringValue = stringValue;
        }

        public ScopeStringSegment WithStringValue(string? stringValue)
        {
            return new ScopeStringSegment(startIndex, length, stringValue);
        }
    }

    /// <summary>
    /// An enumerator that enumerates through a collection of
    /// oauth scopes, but may allocate a new string for each
    /// unknown scope if one wasn't allocated already by
    /// substringing the underlying string.
    /// </summary>
    public struct Enumerator
    {
        private readonly OAuthScopeCollection collection;
        private readonly UnknownScopeCollection.Enumerator unknownScopeEnumerator;
        private int index;
        private string current;

        internal Enumerator(OAuthScopeCollection collection)
        {
            this.collection = collection;
            this.unknownScopeEnumerator = collection.UnknownScopes.GetEnumerator();
            this.index = -1;
            this.current = null!;
        }

        /// <summary>
        /// The current scope in the enumerator.
        /// </summary>
        public readonly string Current => this.current;

        /// <summary>
        /// Advances to the next oauth scope in the oauth scope
        /// collection.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if another oauth scope
        /// exists in the oauth scope collection; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        public bool MoveNext()
        {
            if (this.index > OAuthKnownScopes.Count)
                return false;
            else if (this.index == OAuthKnownScopes.Count)
            {
                if (this.unknownScopeEnumerator.MoveNext())
                {
                    this.current = this.unknownScopeEnumerator.Current;
                    return true;
                }
                else
                {
                    this.index = OAuthKnownScopes.Count + 1;
                    return false;
                }
            }

            do
            {
                this.current = OAuthScopeCollection.GetNameFromIndex(++this.index, this.collection)!;
            }
            while (this.current is null && this.index < OAuthKnownScopes.Count);

            if (this.current is not null)
                return true;

            if (!this.unknownScopeEnumerator.MoveNext())
            {
                this.index = OAuthKnownScopes.Count + 1;
                return false;
            }

            this.current = this.unknownScopeEnumerator.Current;
            return true;
        }
    }

    
    /// <summary>
    /// An enumerator that enumerates through a collection of
    /// oauth scopes that returns spans instead of strings,
    /// reducing allocations as an element may need to be
    /// allocated by substringing the underlying string.
    /// </summary>
    public ref struct SpanEnumerator
    {
        private readonly OAuthScopeCollection collection;
        private readonly UnknownScopeCollection.SpanEnumerator unknownScopeEnumerator;
        private int index;
        private ReadOnlySpan<char> current;

        internal SpanEnumerator(OAuthScopeCollection collection)
        {
            this.collection = collection;
            this.unknownScopeEnumerator = collection.UnknownScopes.GetSpanEnumerator();
            this.index = -1;
        }

        /// <summary>
        /// The current scope in the enumerator.
        /// </summary>
        public readonly ReadOnlySpan<char> Current => this.current;

        /// <summary>
        /// Advances to the next oauth scope in the oauth scope
        /// collection.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if another oauth scope
        /// exists in the oauth scope collection; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        public bool MoveNext()
        {
            if (this.index > OAuthKnownScopes.Count)
                return false;
            else if (this.index == OAuthKnownScopes.Count)
            {
                if (this.unknownScopeEnumerator.MoveNext())
                {
                    this.current = this.unknownScopeEnumerator.Current;
                    return true;
                }
                else
                {
                    this.index = OAuthKnownScopes.Count + 1;
                    return false;
                }
            }

            string? name;
            do
            {
                name = OAuthScopeCollection.GetNameFromIndex(++this.index, this.collection);
            }
            while (name is null && this.index < OAuthKnownScopes.Count);

            if (name is not null)
            {
                this.current = name.AsSpan();
                return true;
            }

            if (!this.unknownScopeEnumerator.MoveNext())
            {
                this.index = OAuthKnownScopes.Count + 1;
                return false;
            }

            this.current = this.unknownScopeEnumerator.Current;
            return true;
        }
    }

    internal sealed class SlowEnumerator : IEnumerator<string>
    {
        private readonly OAuthScopeCollection collection;
        private readonly UnknownScopeCollection.SlowEnumerator unknownScopeEnumerator;
        private int index;
        private string current;

        internal SlowEnumerator(OAuthScopeCollection collection)
        {
            this.collection = collection;
            this.unknownScopeEnumerator = collection.UnknownScopes.GetSlowEnumerator();
            this.index = -1;
            this.current = null!;
        }

        public string Current => this.current;
        object? IEnumerator.Current => this.current;

        public bool MoveNext()
        {
            if (this.index > OAuthKnownScopes.Count)
                return false;
            else if (this.index == OAuthKnownScopes.Count)
            {
                if (this.unknownScopeEnumerator.MoveNext())
                {
                    this.current = this.unknownScopeEnumerator.Current;
                    return true;
                }
                else
                {
                    this.index = OAuthKnownScopes.Count + 1;
                    return false;
                }
            }

            do
            {
                this.current = OAuthScopeCollection.GetNameFromIndex(++this.index, this.collection)!;
            }
            while (this.current is null && this.index < OAuthKnownScopes.Count);

            if (this.current is not null)
                return true;

            if (!this.unknownScopeEnumerator.MoveNext())
            {
                this.index = OAuthKnownScopes.Count + 1;
                return false;
            }

            this.current = this.unknownScopeEnumerator.Current;
            return true;
        }

        public void Dispose()
        {
            this.unknownScopeEnumerator.Dispose();
        }

        public void Reset()
        {
            this.index = -1;
            this.current = null!;
            this.unknownScopeEnumerator.Reset();
        }
    }
}
