using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

namespace Discord.OAuth;

#if NET8_0_OR_GREATER
[CollectionBuilderAttribute(typeof(OAuthScopeCollection.Builder), nameof(OAuthScopeCollection.Builder.Build))]
#endif
partial class OAuthScopeCollection
{
    /// <summary>
    /// A builder that builds oauth scope collections.
    /// </summary>
    public sealed class Builder : IOAuthScopeCollection
    {
        private readonly UnknownScopeCollection unknownScopes;
        private uint rawValue;
        private int definedCount;

        /// <summary>
        /// Initializes this builder instance.
        /// </summary>
        public Builder()
        {
            this.unknownScopes = new(this);
            this.rawValue = 0;
            this.definedCount = 0;
        }

        /// <inheritdoc cref="IOAuthScopeCollection.DefinedCount"/>
        public int DefinedCount => this.definedCount;

        /// <summary>
        /// The number of oauth scopes in this oauth scope
        /// collection builder.
        /// </summary>
        public int Count => this.definedCount + this.unknownScopes.Count;

        IOAuthUnknownScopeCollection IOAuthScopeCollection.UnknownScopes => this.unknownScopes;

        /// <inheritdoc cref="IOAuthScopeCollection.ReadActivities"/>
        public bool ReadActivities
        {
            get => (this.rawValue & OAuthKnownScopes.ReadActivities) == OAuthKnownScopes.ReadActivities;
            set
            {
                if (value)
                {
                    if ((this.rawValue & OAuthKnownScopes.ReadActivities) == OAuthKnownScopes.ReadActivities)
                        return;

                    this.rawValue |= OAuthKnownScopes.ReadActivities;
                    this.definedCount++;
                }
                else
                {
                    if ((this.rawValue & OAuthKnownScopes.ReadActivities) == 0)
                        return;

                    this.rawValue &= ~OAuthKnownScopes.ReadActivities;
                    this.definedCount--;
                }
            }
        }
        /// <inheritdoc cref="IOAuthScopeCollection.WriteActivities"/>
        public bool WriteActivities
        {
            get => (this.rawValue & OAuthKnownScopes.WriteActivities) == OAuthKnownScopes.WriteActivities;
            set
            {
                if (value)
                {
                    if ((this.rawValue & OAuthKnownScopes.WriteActivities) == OAuthKnownScopes.WriteActivities)
                        return;

                    this.rawValue |= OAuthKnownScopes.WriteActivities;
                    this.definedCount++;
                }
                else
                {
                    if ((this.rawValue & OAuthKnownScopes.WriteActivities) == 0)
                        return;

                    this.rawValue &= ~OAuthKnownScopes.WriteActivities;
                    this.definedCount--;
                }
            }
        }
        /// <inheritdoc cref="IOAuthScopeCollection.ReadApplicationBuilds"/>
        public bool ReadApplicationBuilds
        {
            get => (this.rawValue & OAuthKnownScopes.ReadApplicationBuilds) == OAuthKnownScopes.ReadApplicationBuilds;
            set
            {
                if (value)
                {
                    if ((this.rawValue & OAuthKnownScopes.ReadApplicationBuilds) == OAuthKnownScopes.ReadApplicationBuilds)
                        return;

                    this.rawValue |= OAuthKnownScopes.ReadApplicationBuilds;
                    this.definedCount++;
                }
                else
                {
                    if ((this.rawValue & OAuthKnownScopes.ReadApplicationBuilds) == 0)
                        return;

                    this.rawValue &= ~OAuthKnownScopes.ReadApplicationBuilds;
                    this.definedCount--;
                }
            }
        }
        /// <inheritdoc cref="IOAuthScopeCollection.UploadApplicationBuilds"/>
        public bool UploadApplicationBuilds
        {
            get => (this.rawValue & OAuthKnownScopes.UploadApplicationBuilds) == OAuthKnownScopes.UploadApplicationBuilds;
            set
            {
                if (value)
                {
                    if ((this.rawValue & OAuthKnownScopes.UploadApplicationBuilds) == OAuthKnownScopes.UploadApplicationBuilds)
                        return;

                    this.rawValue |= OAuthKnownScopes.UploadApplicationBuilds;
                    this.definedCount++;
                }
                else
                {
                    if ((this.rawValue & OAuthKnownScopes.UploadApplicationBuilds) == 0)
                        return;

                    this.rawValue &= ~OAuthKnownScopes.UploadApplicationBuilds;
                    this.definedCount--;
                }
            }
        }
        /// <inheritdoc cref="IOAuthScopeCollection.ApplicationCommands"/>
        public bool ApplicationCommands
        {
            get => (this.rawValue & OAuthKnownScopes.ApplicationCommands) == OAuthKnownScopes.ApplicationCommands;
            set
            {
                if (value)
                {
                    if ((this.rawValue & OAuthKnownScopes.ApplicationCommands) == OAuthKnownScopes.ApplicationCommands)
                        return;

                    this.rawValue |= OAuthKnownScopes.ApplicationCommands;
                    this.definedCount++;
                }
                else
                {
                    if ((this.rawValue & OAuthKnownScopes.ApplicationCommands) == 0)
                        return;

                    this.rawValue &= ~OAuthKnownScopes.ApplicationCommands;
                    this.definedCount--;
                }
            }
        }
        /// <inheritdoc cref="IOAuthScopeCollection.UpdateApplicationCommands"/>
        public bool UpdateApplicationCommands
        {
            get => (this.rawValue & OAuthKnownScopes.UpdateApplicationCommands) == OAuthKnownScopes.UpdateApplicationCommands;
            set
            {
                if (value)
                {
                    if ((this.rawValue & OAuthKnownScopes.UpdateApplicationCommands) == OAuthKnownScopes.UpdateApplicationCommands)
                        return;

                    this.rawValue |= OAuthKnownScopes.UpdateApplicationCommands;
                    this.definedCount++;
                }
                else
                {
                    if ((this.rawValue & OAuthKnownScopes.UpdateApplicationCommands) == 0)
                        return;

                    this.rawValue &= ~OAuthKnownScopes.UpdateApplicationCommands;
                    this.definedCount--;
                }
            }
        }
        /// <inheritdoc cref="IOAuthScopeCollection.UpdateApplicationCommandPermissions"/>
        public bool UpdateApplicationCommandPermissions
        {
            get => (this.rawValue & OAuthKnownScopes.UpdateApplicationCommandPermissions) == OAuthKnownScopes.UpdateApplicationCommandPermissions;
            set
            {
                if (value)
                {
                    if ((this.rawValue & OAuthKnownScopes.UpdateApplicationCommandPermissions) == OAuthKnownScopes.UpdateApplicationCommandPermissions)
                        return;

                    this.rawValue |= OAuthKnownScopes.UpdateApplicationCommandPermissions;
                    this.definedCount++;
                }
                else
                {
                    if ((this.rawValue & OAuthKnownScopes.UpdateApplicationCommandPermissions) == 0)
                        return;

                    this.rawValue &= ~OAuthKnownScopes.UpdateApplicationCommandPermissions;
                    this.definedCount--;
                }
            }
        }
        /// <inheritdoc cref="IOAuthScopeCollection.ApplicationEntitlements"/>
        public bool ApplicationEntitlements
        {
            get => (this.rawValue & OAuthKnownScopes.ApplicationEntitlements) == OAuthKnownScopes.ApplicationEntitlements;
            set
            {
                if (value)
                {
                    if ((this.rawValue & OAuthKnownScopes.ApplicationEntitlements) == OAuthKnownScopes.ApplicationEntitlements)
                        return;

                    this.rawValue |= OAuthKnownScopes.ApplicationEntitlements;
                    this.definedCount++;
                }
                else
                {
                    if ((this.rawValue & OAuthKnownScopes.ApplicationEntitlements) == 0)
                        return;

                    this.rawValue &= ~OAuthKnownScopes.ApplicationEntitlements;
                    this.definedCount--;
                }
            }
        }
        /// <inheritdoc cref="IOAuthScopeCollection.UpdateApplicationStore"/>
        public bool UpdateApplicationStore
        {
            get => (this.rawValue & OAuthKnownScopes.UpdateApplicationStore) == OAuthKnownScopes.UpdateApplicationStore;
            set
            {
                if (value)
                {
                    if ((this.rawValue & OAuthKnownScopes.UpdateApplicationStore) == OAuthKnownScopes.UpdateApplicationStore)
                        return;

                    this.rawValue |= OAuthKnownScopes.UpdateApplicationStore;
                    this.definedCount++;
                }
                else
                {
                    if ((this.rawValue & OAuthKnownScopes.UpdateApplicationStore) == 0)
                        return;

                    this.rawValue &= ~OAuthKnownScopes.UpdateApplicationStore;
                    this.definedCount--;
                }
            }
        }
        /// <inheritdoc cref="IOAuthScopeCollection.Bot"/>
        public bool Bot
        {
            get => (this.rawValue & OAuthKnownScopes.Bot) == OAuthKnownScopes.Bot;
            set
            {
                if (value)
                {
                    if ((this.rawValue & OAuthKnownScopes.Bot) == OAuthKnownScopes.Bot)
                        return;

                    this.rawValue |= OAuthKnownScopes.Bot;
                    this.definedCount++;
                }
                else
                {
                    if ((this.rawValue & OAuthKnownScopes.Bot) == 0)
                        return;

                    this.rawValue &= ~OAuthKnownScopes.Bot;
                    this.definedCount--;
                }
            }
        }
        /// <inheritdoc cref="IOAuthScopeCollection.Connections"/>
        public bool Connections
        {
            get => (this.rawValue & OAuthKnownScopes.Connections) == OAuthKnownScopes.Connections;
            set
            {
                if (value)
                {
                    if ((this.rawValue & OAuthKnownScopes.Connections) == OAuthKnownScopes.Connections)
                        return;

                    this.rawValue |= OAuthKnownScopes.Connections;
                    this.definedCount++;
                }
                else
                {
                    if ((this.rawValue & OAuthKnownScopes.Connections) == 0)
                        return;

                    this.rawValue &= ~OAuthKnownScopes.Connections;
                    this.definedCount--;
                }
            }
        }
        /// <inheritdoc cref="IOAuthScopeCollection.ReadDMChannels"/>
        public bool ReadDMChannels
        {
            get => (this.rawValue & OAuthKnownScopes.ReadDMChannels) == OAuthKnownScopes.ReadDMChannels;
            set
            {
                if (value)
                {
                    if ((this.rawValue & OAuthKnownScopes.ReadDMChannels) == OAuthKnownScopes.ReadDMChannels)
                        return;

                    this.rawValue |= OAuthKnownScopes.ReadDMChannels;
                    this.definedCount++;
                }
                else
                {
                    if ((this.rawValue & OAuthKnownScopes.ReadDMChannels) == 0)
                        return;

                    this.rawValue &= ~OAuthKnownScopes.ReadDMChannels;
                    this.definedCount--;
                }
            }
        }
        /// <inheritdoc cref="IOAuthScopeCollection.Email"/>
        public bool Email
        {
            get => (this.rawValue & OAuthKnownScopes.Email) == OAuthKnownScopes.Email;
            set
            {
                if (value)
                {
                    if ((this.rawValue & OAuthKnownScopes.Email) == OAuthKnownScopes.Email)
                        return;

                    this.rawValue |= OAuthKnownScopes.Email;
                    this.definedCount++;
                }
                else
                {
                    if ((this.rawValue & OAuthKnownScopes.Email) == 0)
                        return;

                    this.rawValue &= ~OAuthKnownScopes.Email;
                    this.definedCount--;
                }
            }
        }
        /// <inheritdoc cref="IOAuthScopeCollection.JoinGroupDM"/>
        public bool JoinGroupDM
        {
            get => (this.rawValue & OAuthKnownScopes.JoinGroupDM) == OAuthKnownScopes.JoinGroupDM;
            set
            {
                if (value)
                {
                    if ((this.rawValue & OAuthKnownScopes.JoinGroupDM) == OAuthKnownScopes.JoinGroupDM)
                        return;

                    this.rawValue |= OAuthKnownScopes.JoinGroupDM;
                    this.definedCount++;
                }
                else
                {
                    if ((this.rawValue & OAuthKnownScopes.JoinGroupDM) == 0)
                        return;

                    this.rawValue &= ~OAuthKnownScopes.JoinGroupDM;
                    this.definedCount--;
                }
            }
        }
        /// <inheritdoc cref="IOAuthScopeCollection.Guilds"/>
        public bool Guilds
        {
            get => (this.rawValue & OAuthKnownScopes.Guilds) == OAuthKnownScopes.Guilds;
            set
            {
                if (value)
                {
                    if ((this.rawValue & OAuthKnownScopes.Guilds) == OAuthKnownScopes.Guilds)
                        return;

                    this.rawValue |= OAuthKnownScopes.Guilds;
                    this.definedCount++;
                }
                else
                {
                    if ((this.rawValue & OAuthKnownScopes.Guilds) == 0)
                        return;

                    this.rawValue &= ~OAuthKnownScopes.Guilds;
                    this.definedCount--;
                }
            }
        }
        /// <inheritdoc cref="IOAuthScopeCollection.JoinGuilds"/>
        public bool JoinGuilds
        {
            get => (this.rawValue & OAuthKnownScopes.JoinGuilds) == OAuthKnownScopes.JoinGuilds;
            set
            {
                if (value)
                {
                    if ((this.rawValue & OAuthKnownScopes.JoinGuilds) == OAuthKnownScopes.JoinGuilds)
                        return;

                    this.rawValue |= OAuthKnownScopes.JoinGuilds;
                    this.definedCount++;
                }
                else
                {
                    if ((this.rawValue & OAuthKnownScopes.JoinGuilds) == 0)
                        return;

                    this.rawValue &= ~OAuthKnownScopes.JoinGuilds;
                    this.definedCount--;
                }
            }
        }
        /// <inheritdoc cref="IOAuthScopeCollection.ReadGuildMembers"/>
        public bool ReadGuildMembers
        {
            get => (this.rawValue & OAuthKnownScopes.ReadGuildMembers) == OAuthKnownScopes.ReadGuildMembers;
            set
            {
                if (value)
                {
                    if ((this.rawValue & OAuthKnownScopes.ReadGuildMembers) == OAuthKnownScopes.ReadGuildMembers)
                        return;

                    this.rawValue |= OAuthKnownScopes.ReadGuildMembers;
                    this.definedCount++;
                }
                else
                {
                    if ((this.rawValue & OAuthKnownScopes.ReadGuildMembers) == 0)
                        return;

                    this.rawValue &= ~OAuthKnownScopes.ReadGuildMembers;
                    this.definedCount--;
                }
            }
        }
        /// <inheritdoc cref="IOAuthScopeCollection.Identify"/>
        public bool Identify
        {
            get => (this.rawValue & OAuthKnownScopes.Identify) == OAuthKnownScopes.Identify;
            set
            {
                if (value)
                {
                    if ((this.rawValue & OAuthKnownScopes.Identify) == OAuthKnownScopes.Identify)
                        return;

                    this.rawValue |= OAuthKnownScopes.Identify;
                    this.definedCount++;
                }
                else
                {
                    if ((this.rawValue & OAuthKnownScopes.Identify) == 0)
                        return;

                    this.rawValue &= ~OAuthKnownScopes.Identify;
                    this.definedCount--;
                }
            }
        }
        /// <inheritdoc cref="IOAuthScopeCollection.ReadMessages"/>
        public bool ReadMessages
        {
            get => (this.rawValue & OAuthKnownScopes.ReadMessages) == OAuthKnownScopes.ReadMessages;
            set
            {
                if (value)
                {
                    if ((this.rawValue & OAuthKnownScopes.ReadMessages) == OAuthKnownScopes.ReadMessages)
                        return;

                    this.rawValue |= OAuthKnownScopes.ReadMessages;
                    this.definedCount++;
                }
                else
                {
                    if ((this.rawValue & OAuthKnownScopes.ReadMessages) == 0)
                        return;

                    this.rawValue &= ~OAuthKnownScopes.ReadMessages;
                    this.definedCount--;
                }
            }
        }
        /// <inheritdoc cref="IOAuthScopeCollection.ReadRelationships"/>
        public bool ReadRelationships
        {
            get => (this.rawValue & OAuthKnownScopes.ReadRelationships) == OAuthKnownScopes.ReadRelationships;
            set
            {
                if (value)
                {
                    if ((this.rawValue & OAuthKnownScopes.ReadRelationships) == OAuthKnownScopes.ReadRelationships)
                        return;

                    this.rawValue |= OAuthKnownScopes.ReadRelationships;
                    this.definedCount++;
                }
                else
                {
                    if ((this.rawValue & OAuthKnownScopes.ReadRelationships) == 0)
                        return;

                    this.rawValue &= ~OAuthKnownScopes.ReadRelationships;
                    this.definedCount--;
                }
            }
        }
        /// <inheritdoc cref="IOAuthScopeCollection.WriteRoleConnections"/>
        public bool WriteRoleConnections
        {
            get => (this.rawValue & OAuthKnownScopes.WriteRoleConnections) == OAuthKnownScopes.WriteRoleConnections;
            set
            {
                if (value)
                {
                    if ((this.rawValue & OAuthKnownScopes.WriteRoleConnections) == OAuthKnownScopes.WriteRoleConnections)
                        return;

                    this.rawValue |= OAuthKnownScopes.WriteRoleConnections;
                    this.definedCount++;
                }
                else
                {
                    if ((this.rawValue & OAuthKnownScopes.WriteRoleConnections) == 0)
                        return;

                    this.rawValue &= ~OAuthKnownScopes.WriteRoleConnections;
                    this.definedCount--;
                }
            }
        }
        /// <inheritdoc cref="IOAuthScopeCollection.Rpc"/>
        public bool Rpc
        {
            get => (this.rawValue & OAuthKnownScopes.Rpc) == OAuthKnownScopes.Rpc;
            set
            {
                if (value)
                {
                    if ((this.rawValue & OAuthKnownScopes.Rpc) == OAuthKnownScopes.Rpc)
                        return;

                    this.rawValue |= OAuthKnownScopes.Rpc;
                    this.definedCount++;
                }
                else
                {
                    if ((this.rawValue & OAuthKnownScopes.Rpc) == 0)
                        return;

                    this.rawValue &= ~OAuthKnownScopes.Rpc;
                    this.definedCount--;
                }
            }
        }
        /// <inheritdoc cref="IOAuthScopeCollection.WriteRpcActivities"/>
        public bool WriteRpcActivities
        {
            get => (this.rawValue & OAuthKnownScopes.WriteRpcActivities) == OAuthKnownScopes.WriteRpcActivities;
            set
            {
                if (value)
                {
                    if ((this.rawValue & OAuthKnownScopes.WriteRpcActivities) == OAuthKnownScopes.WriteRpcActivities)
                        return;

                    this.rawValue |= OAuthKnownScopes.WriteRpcActivities;
                    this.definedCount++;
                }
                else
                {
                    if ((this.rawValue & OAuthKnownScopes.WriteRpcActivities) == 0)
                        return;

                    this.rawValue &= ~OAuthKnownScopes.WriteRpcActivities;
                    this.definedCount--;
                }
            }
        }
        /// <inheritdoc cref="IOAuthScopeCollection.ReadRpcNotifications"/>
        public bool ReadRpcNotifications
        {
            get => (this.rawValue & OAuthKnownScopes.ReadRpcNotifications) == OAuthKnownScopes.ReadRpcNotifications;
            set
            {
                if (value)
                {
                    if ((this.rawValue & OAuthKnownScopes.ReadRpcNotifications) == OAuthKnownScopes.ReadRpcNotifications)
                        return;

                    this.rawValue |= OAuthKnownScopes.ReadRpcNotifications;
                    this.definedCount++;
                }
                else
                {
                    if ((this.rawValue & OAuthKnownScopes.ReadRpcNotifications) == 0)
                        return;

                    this.rawValue &= ~OAuthKnownScopes.ReadRpcNotifications;
                    this.definedCount--;
                }
            }
        }
        /// <inheritdoc cref="IOAuthScopeCollection.ReadRpcVoice"/>
        public bool ReadRpcVoice
        {
            get => (this.rawValue & OAuthKnownScopes.ReadRpcVoice) == OAuthKnownScopes.ReadRpcVoice;
            set
            {
                if (value)
                {
                    if ((this.rawValue & OAuthKnownScopes.ReadRpcVoice) == OAuthKnownScopes.ReadRpcVoice)
                        return;

                    this.rawValue |= OAuthKnownScopes.ReadRpcVoice;
                    this.definedCount++;
                }
                else
                {
                    if ((this.rawValue & OAuthKnownScopes.ReadRpcVoice) == 0)
                        return;

                    this.rawValue &= ~OAuthKnownScopes.ReadRpcVoice;
                    this.definedCount--;
                }
            }
        }
        /// <inheritdoc cref="IOAuthScopeCollection.WriteRpcVoice"/>
        public bool WriteRpcVoice
        {
            get => (this.rawValue & OAuthKnownScopes.WriteRpcVoice) == OAuthKnownScopes.WriteRpcVoice;
            set
            {
                if (value)
                {
                    if ((this.rawValue & OAuthKnownScopes.WriteRpcVoice) == OAuthKnownScopes.WriteRpcVoice)
                        return;

                    this.rawValue |= OAuthKnownScopes.WriteRpcVoice;
                    this.definedCount++;
                }
                else
                {
                    if ((this.rawValue & OAuthKnownScopes.WriteRpcVoice) == 0)
                        return;

                    this.rawValue &= ~OAuthKnownScopes.WriteRpcVoice;
                    this.definedCount--;
                }
            }
        }
        /// <inheritdoc cref="IOAuthScopeCollection.Voice"/>
        public bool Voice
        {
            get => (this.rawValue & OAuthKnownScopes.Voice) == OAuthKnownScopes.Voice;
            set
            {
                if (value)
                {
                    if ((this.rawValue & OAuthKnownScopes.Voice) == OAuthKnownScopes.Voice)
                        return;

                    this.rawValue |= OAuthKnownScopes.Voice;
                    this.definedCount++;
                }
                else
                {
                    if ((this.rawValue & OAuthKnownScopes.Voice) == 0)
                        return;

                    this.rawValue &= ~OAuthKnownScopes.Voice;
                    this.definedCount--;
                }
            }
        }
        /// <inheritdoc cref="IOAuthScopeCollection.IncomingWebhook"/>
        public bool IncomingWebhook
        {
            get => (this.rawValue & OAuthKnownScopes.IncomingWebhook) == OAuthKnownScopes.IncomingWebhook;
            set
            {
                if (value)
                {
                    if ((this.rawValue & OAuthKnownScopes.IncomingWebhook) == OAuthKnownScopes.IncomingWebhook)
                        return;

                    this.rawValue |= OAuthKnownScopes.IncomingWebhook;
                    this.definedCount++;
                }
                else
                {
                    if ((this.rawValue & OAuthKnownScopes.IncomingWebhook) == 0)
                        return;

                    this.rawValue &= ~OAuthKnownScopes.IncomingWebhook;
                    this.definedCount--;
                }
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
            
            return this.unknownScopes.Contains(scope);
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
            
            return this.unknownScopes.Contains(scope);
        }

        /// <summary>
        /// Adds the
        /// <c>activities.read</c>
        /// (<see cref="ReadActivities"/>)
        /// OAuth Scope if it wasn't already added.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithReadActivities()
        {
            if ((this.rawValue & OAuthKnownScopes.ReadActivities) == OAuthKnownScopes.ReadActivities)
                return this;

            this.rawValue |= OAuthKnownScopes.ReadActivities;
            this.definedCount++;
            return this;
        }

        /// <summary>
        /// Removes the
        /// <c>activities.read</c>
        /// (<see cref="ReadActivities"/>)
        /// OAuth Scope if is contained in this builder.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithoutReadActivities()
        {
            if ((this.rawValue & OAuthKnownScopes.ReadActivities) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.ReadActivities;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Sets whether or not the
        /// <c>activities.read</c>
        /// (<see cref="ReadActivities"/>)
        /// OAuth Scope if is included in this builder.
        /// </summary>
        /// <param name="value">
        /// Whether or not the scope should be
        /// included/excluded in this builder.
        /// </param>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder SetReadActivitiesValue(bool value)
        {
            if (value)
            {
                if ((this.rawValue & OAuthKnownScopes.ReadActivities) == OAuthKnownScopes.ReadActivities)
                    return this;

                this.rawValue |= OAuthKnownScopes.ReadActivities;
                this.definedCount++;
                return this;
            }

            if ((this.rawValue & OAuthKnownScopes.ReadActivities) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.ReadActivities;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Adds the
        /// <c>activities.write</c>
        /// (<see cref="WriteActivities"/>)
        /// OAuth Scope if it wasn't already added.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithWriteActivities()
        {
            if ((this.rawValue & OAuthKnownScopes.WriteActivities) == OAuthKnownScopes.WriteActivities)
                return this;

            this.rawValue |= OAuthKnownScopes.WriteActivities;
            this.definedCount++;
            return this;
        }

        /// <summary>
        /// Removes the
        /// <c>activities.write</c>
        /// (<see cref="WriteActivities"/>)
        /// OAuth Scope if is contained in this builder.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithoutWriteActivities()
        {
            if ((this.rawValue & OAuthKnownScopes.WriteActivities) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.WriteActivities;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Sets whether or not the
        /// <c>activities.write</c>
        /// (<see cref="WriteActivities"/>)
        /// OAuth Scope if is included in this builder.
        /// </summary>
        /// <param name="value">
        /// Whether or not the scope should be
        /// included/excluded in this builder.
        /// </param>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder SetWriteActivitiesValue(bool value)
        {
            if (value)
            {
                if ((this.rawValue & OAuthKnownScopes.WriteActivities) == OAuthKnownScopes.WriteActivities)
                    return this;

                this.rawValue |= OAuthKnownScopes.WriteActivities;
                this.definedCount++;
                return this;
            }

            if ((this.rawValue & OAuthKnownScopes.WriteActivities) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.WriteActivities;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Adds the
        /// <c>applications.builds.read</c>
        /// (<see cref="ReadApplicationBuilds"/>)
        /// OAuth Scope if it wasn't already added.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithReadApplicationBuilds()
        {
            if ((this.rawValue & OAuthKnownScopes.ReadApplicationBuilds) == OAuthKnownScopes.ReadApplicationBuilds)
                return this;

            this.rawValue |= OAuthKnownScopes.ReadApplicationBuilds;
            this.definedCount++;
            return this;
        }

        /// <summary>
        /// Removes the
        /// <c>applications.builds.read</c>
        /// (<see cref="ReadApplicationBuilds"/>)
        /// OAuth Scope if is contained in this builder.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithoutReadApplicationBuilds()
        {
            if ((this.rawValue & OAuthKnownScopes.ReadApplicationBuilds) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.ReadApplicationBuilds;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Sets whether or not the
        /// <c>applications.builds.read</c>
        /// (<see cref="ReadApplicationBuilds"/>)
        /// OAuth Scope if is included in this builder.
        /// </summary>
        /// <param name="value">
        /// Whether or not the scope should be
        /// included/excluded in this builder.
        /// </param>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder SetReadApplicationBuildsValue(bool value)
        {
            if (value)
            {
                if ((this.rawValue & OAuthKnownScopes.ReadApplicationBuilds) == OAuthKnownScopes.ReadApplicationBuilds)
                    return this;

                this.rawValue |= OAuthKnownScopes.ReadApplicationBuilds;
                this.definedCount++;
                return this;
            }

            if ((this.rawValue & OAuthKnownScopes.ReadApplicationBuilds) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.ReadApplicationBuilds;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Adds the
        /// <c>applications.builds.upload</c>
        /// (<see cref="UploadApplicationBuilds"/>)
        /// OAuth Scope if it wasn't already added.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithUploadApplicationBuilds()
        {
            if ((this.rawValue & OAuthKnownScopes.UploadApplicationBuilds) == OAuthKnownScopes.UploadApplicationBuilds)
                return this;

            this.rawValue |= OAuthKnownScopes.UploadApplicationBuilds;
            this.definedCount++;
            return this;
        }

        /// <summary>
        /// Removes the
        /// <c>applications.builds.upload</c>
        /// (<see cref="UploadApplicationBuilds"/>)
        /// OAuth Scope if is contained in this builder.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithoutUploadApplicationBuilds()
        {
            if ((this.rawValue & OAuthKnownScopes.UploadApplicationBuilds) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.UploadApplicationBuilds;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Sets whether or not the
        /// <c>applications.builds.upload</c>
        /// (<see cref="UploadApplicationBuilds"/>)
        /// OAuth Scope if is included in this builder.
        /// </summary>
        /// <param name="value">
        /// Whether or not the scope should be
        /// included/excluded in this builder.
        /// </param>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder SetUploadApplicationBuildsValue(bool value)
        {
            if (value)
            {
                if ((this.rawValue & OAuthKnownScopes.UploadApplicationBuilds) == OAuthKnownScopes.UploadApplicationBuilds)
                    return this;

                this.rawValue |= OAuthKnownScopes.UploadApplicationBuilds;
                this.definedCount++;
                return this;
            }

            if ((this.rawValue & OAuthKnownScopes.UploadApplicationBuilds) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.UploadApplicationBuilds;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Adds the
        /// <c>applications.commands</c>
        /// (<see cref="ApplicationCommands"/>)
        /// OAuth Scope if it wasn't already added.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithApplicationCommands()
        {
            if ((this.rawValue & OAuthKnownScopes.ApplicationCommands) == OAuthKnownScopes.ApplicationCommands)
                return this;

            this.rawValue |= OAuthKnownScopes.ApplicationCommands;
            this.definedCount++;
            return this;
        }

        /// <summary>
        /// Removes the
        /// <c>applications.commands</c>
        /// (<see cref="ApplicationCommands"/>)
        /// OAuth Scope if is contained in this builder.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithoutApplicationCommands()
        {
            if ((this.rawValue & OAuthKnownScopes.ApplicationCommands) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.ApplicationCommands;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Sets whether or not the
        /// <c>applications.commands</c>
        /// (<see cref="ApplicationCommands"/>)
        /// OAuth Scope if is included in this builder.
        /// </summary>
        /// <param name="value">
        /// Whether or not the scope should be
        /// included/excluded in this builder.
        /// </param>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder SetApplicationCommandsValue(bool value)
        {
            if (value)
            {
                if ((this.rawValue & OAuthKnownScopes.ApplicationCommands) == OAuthKnownScopes.ApplicationCommands)
                    return this;

                this.rawValue |= OAuthKnownScopes.ApplicationCommands;
                this.definedCount++;
                return this;
            }

            if ((this.rawValue & OAuthKnownScopes.ApplicationCommands) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.ApplicationCommands;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Adds the
        /// <c>applications.commands.update</c>
        /// (<see cref="UpdateApplicationCommands"/>)
        /// OAuth Scope if it wasn't already added.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithUpdateApplicationCommands()
        {
            if ((this.rawValue & OAuthKnownScopes.UpdateApplicationCommands) == OAuthKnownScopes.UpdateApplicationCommands)
                return this;

            this.rawValue |= OAuthKnownScopes.UpdateApplicationCommands;
            this.definedCount++;
            return this;
        }

        /// <summary>
        /// Removes the
        /// <c>applications.commands.update</c>
        /// (<see cref="UpdateApplicationCommands"/>)
        /// OAuth Scope if is contained in this builder.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithoutUpdateApplicationCommands()
        {
            if ((this.rawValue & OAuthKnownScopes.UpdateApplicationCommands) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.UpdateApplicationCommands;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Sets whether or not the
        /// <c>applications.commands.update</c>
        /// (<see cref="UpdateApplicationCommands"/>)
        /// OAuth Scope if is included in this builder.
        /// </summary>
        /// <param name="value">
        /// Whether or not the scope should be
        /// included/excluded in this builder.
        /// </param>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder SetUpdateApplicationCommandsValue(bool value)
        {
            if (value)
            {
                if ((this.rawValue & OAuthKnownScopes.UpdateApplicationCommands) == OAuthKnownScopes.UpdateApplicationCommands)
                    return this;

                this.rawValue |= OAuthKnownScopes.UpdateApplicationCommands;
                this.definedCount++;
                return this;
            }

            if ((this.rawValue & OAuthKnownScopes.UpdateApplicationCommands) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.UpdateApplicationCommands;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Adds the
        /// <c>applications.commands.permissions.update</c>
        /// (<see cref="UpdateApplicationCommandPermissions"/>)
        /// OAuth Scope if it wasn't already added.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithUpdateApplicationCommandPermissions()
        {
            if ((this.rawValue & OAuthKnownScopes.UpdateApplicationCommandPermissions) == OAuthKnownScopes.UpdateApplicationCommandPermissions)
                return this;

            this.rawValue |= OAuthKnownScopes.UpdateApplicationCommandPermissions;
            this.definedCount++;
            return this;
        }

        /// <summary>
        /// Removes the
        /// <c>applications.commands.permissions.update</c>
        /// (<see cref="UpdateApplicationCommandPermissions"/>)
        /// OAuth Scope if is contained in this builder.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithoutUpdateApplicationCommandPermissions()
        {
            if ((this.rawValue & OAuthKnownScopes.UpdateApplicationCommandPermissions) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.UpdateApplicationCommandPermissions;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Sets whether or not the
        /// <c>applications.commands.permissions.update</c>
        /// (<see cref="UpdateApplicationCommandPermissions"/>)
        /// OAuth Scope if is included in this builder.
        /// </summary>
        /// <param name="value">
        /// Whether or not the scope should be
        /// included/excluded in this builder.
        /// </param>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder SetUpdateApplicationCommandPermissionsValue(bool value)
        {
            if (value)
            {
                if ((this.rawValue & OAuthKnownScopes.UpdateApplicationCommandPermissions) == OAuthKnownScopes.UpdateApplicationCommandPermissions)
                    return this;

                this.rawValue |= OAuthKnownScopes.UpdateApplicationCommandPermissions;
                this.definedCount++;
                return this;
            }

            if ((this.rawValue & OAuthKnownScopes.UpdateApplicationCommandPermissions) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.UpdateApplicationCommandPermissions;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Adds the
        /// <c>applications.entitlements</c>
        /// (<see cref="ApplicationEntitlements"/>)
        /// OAuth Scope if it wasn't already added.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithApplicationEntitlements()
        {
            if ((this.rawValue & OAuthKnownScopes.ApplicationEntitlements) == OAuthKnownScopes.ApplicationEntitlements)
                return this;

            this.rawValue |= OAuthKnownScopes.ApplicationEntitlements;
            this.definedCount++;
            return this;
        }

        /// <summary>
        /// Removes the
        /// <c>applications.entitlements</c>
        /// (<see cref="ApplicationEntitlements"/>)
        /// OAuth Scope if is contained in this builder.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithoutApplicationEntitlements()
        {
            if ((this.rawValue & OAuthKnownScopes.ApplicationEntitlements) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.ApplicationEntitlements;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Sets whether or not the
        /// <c>applications.entitlements</c>
        /// (<see cref="ApplicationEntitlements"/>)
        /// OAuth Scope if is included in this builder.
        /// </summary>
        /// <param name="value">
        /// Whether or not the scope should be
        /// included/excluded in this builder.
        /// </param>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder SetApplicationEntitlementsValue(bool value)
        {
            if (value)
            {
                if ((this.rawValue & OAuthKnownScopes.ApplicationEntitlements) == OAuthKnownScopes.ApplicationEntitlements)
                    return this;

                this.rawValue |= OAuthKnownScopes.ApplicationEntitlements;
                this.definedCount++;
                return this;
            }

            if ((this.rawValue & OAuthKnownScopes.ApplicationEntitlements) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.ApplicationEntitlements;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Adds the
        /// <c>applications.store.update</c>
        /// (<see cref="UpdateApplicationStore"/>)
        /// OAuth Scope if it wasn't already added.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithUpdateApplicationStore()
        {
            if ((this.rawValue & OAuthKnownScopes.UpdateApplicationStore) == OAuthKnownScopes.UpdateApplicationStore)
                return this;

            this.rawValue |= OAuthKnownScopes.UpdateApplicationStore;
            this.definedCount++;
            return this;
        }

        /// <summary>
        /// Removes the
        /// <c>applications.store.update</c>
        /// (<see cref="UpdateApplicationStore"/>)
        /// OAuth Scope if is contained in this builder.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithoutUpdateApplicationStore()
        {
            if ((this.rawValue & OAuthKnownScopes.UpdateApplicationStore) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.UpdateApplicationStore;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Sets whether or not the
        /// <c>applications.store.update</c>
        /// (<see cref="UpdateApplicationStore"/>)
        /// OAuth Scope if is included in this builder.
        /// </summary>
        /// <param name="value">
        /// Whether or not the scope should be
        /// included/excluded in this builder.
        /// </param>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder SetUpdateApplicationStoreValue(bool value)
        {
            if (value)
            {
                if ((this.rawValue & OAuthKnownScopes.UpdateApplicationStore) == OAuthKnownScopes.UpdateApplicationStore)
                    return this;

                this.rawValue |= OAuthKnownScopes.UpdateApplicationStore;
                this.definedCount++;
                return this;
            }

            if ((this.rawValue & OAuthKnownScopes.UpdateApplicationStore) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.UpdateApplicationStore;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Adds the
        /// <c>bot</c>
        /// (<see cref="Bot"/>)
        /// OAuth Scope if it wasn't already added.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithBot()
        {
            if ((this.rawValue & OAuthKnownScopes.Bot) == OAuthKnownScopes.Bot)
                return this;

            this.rawValue |= OAuthKnownScopes.Bot;
            this.definedCount++;
            return this;
        }

        /// <summary>
        /// Removes the
        /// <c>bot</c>
        /// (<see cref="Bot"/>)
        /// OAuth Scope if is contained in this builder.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithoutBot()
        {
            if ((this.rawValue & OAuthKnownScopes.Bot) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.Bot;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Sets whether or not the
        /// <c>bot</c>
        /// (<see cref="Bot"/>)
        /// OAuth Scope if is included in this builder.
        /// </summary>
        /// <param name="value">
        /// Whether or not the scope should be
        /// included/excluded in this builder.
        /// </param>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder SetBotValue(bool value)
        {
            if (value)
            {
                if ((this.rawValue & OAuthKnownScopes.Bot) == OAuthKnownScopes.Bot)
                    return this;

                this.rawValue |= OAuthKnownScopes.Bot;
                this.definedCount++;
                return this;
            }

            if ((this.rawValue & OAuthKnownScopes.Bot) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.Bot;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Adds the
        /// <c>connections</c>
        /// (<see cref="Connections"/>)
        /// OAuth Scope if it wasn't already added.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithConnections()
        {
            if ((this.rawValue & OAuthKnownScopes.Connections) == OAuthKnownScopes.Connections)
                return this;

            this.rawValue |= OAuthKnownScopes.Connections;
            this.definedCount++;
            return this;
        }

        /// <summary>
        /// Removes the
        /// <c>connections</c>
        /// (<see cref="Connections"/>)
        /// OAuth Scope if is contained in this builder.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithoutConnections()
        {
            if ((this.rawValue & OAuthKnownScopes.Connections) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.Connections;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Sets whether or not the
        /// <c>connections</c>
        /// (<see cref="Connections"/>)
        /// OAuth Scope if is included in this builder.
        /// </summary>
        /// <param name="value">
        /// Whether or not the scope should be
        /// included/excluded in this builder.
        /// </param>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder SetConnectionsValue(bool value)
        {
            if (value)
            {
                if ((this.rawValue & OAuthKnownScopes.Connections) == OAuthKnownScopes.Connections)
                    return this;

                this.rawValue |= OAuthKnownScopes.Connections;
                this.definedCount++;
                return this;
            }

            if ((this.rawValue & OAuthKnownScopes.Connections) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.Connections;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Adds the
        /// <c>dm_channels.read</c>
        /// (<see cref="ReadDMChannels"/>)
        /// OAuth Scope if it wasn't already added.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithReadDMChannels()
        {
            if ((this.rawValue & OAuthKnownScopes.ReadDMChannels) == OAuthKnownScopes.ReadDMChannels)
                return this;

            this.rawValue |= OAuthKnownScopes.ReadDMChannels;
            this.definedCount++;
            return this;
        }

        /// <summary>
        /// Removes the
        /// <c>dm_channels.read</c>
        /// (<see cref="ReadDMChannels"/>)
        /// OAuth Scope if is contained in this builder.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithoutReadDMChannels()
        {
            if ((this.rawValue & OAuthKnownScopes.ReadDMChannels) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.ReadDMChannels;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Sets whether or not the
        /// <c>dm_channels.read</c>
        /// (<see cref="ReadDMChannels"/>)
        /// OAuth Scope if is included in this builder.
        /// </summary>
        /// <param name="value">
        /// Whether or not the scope should be
        /// included/excluded in this builder.
        /// </param>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder SetReadDMChannelsValue(bool value)
        {
            if (value)
            {
                if ((this.rawValue & OAuthKnownScopes.ReadDMChannels) == OAuthKnownScopes.ReadDMChannels)
                    return this;

                this.rawValue |= OAuthKnownScopes.ReadDMChannels;
                this.definedCount++;
                return this;
            }

            if ((this.rawValue & OAuthKnownScopes.ReadDMChannels) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.ReadDMChannels;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Adds the
        /// <c>email</c>
        /// (<see cref="Email"/>)
        /// OAuth Scope if it wasn't already added.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithEmail()
        {
            if ((this.rawValue & OAuthKnownScopes.Email) == OAuthKnownScopes.Email)
                return this;

            this.rawValue |= OAuthKnownScopes.Email;
            this.definedCount++;
            return this;
        }

        /// <summary>
        /// Removes the
        /// <c>email</c>
        /// (<see cref="Email"/>)
        /// OAuth Scope if is contained in this builder.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithoutEmail()
        {
            if ((this.rawValue & OAuthKnownScopes.Email) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.Email;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Sets whether or not the
        /// <c>email</c>
        /// (<see cref="Email"/>)
        /// OAuth Scope if is included in this builder.
        /// </summary>
        /// <param name="value">
        /// Whether or not the scope should be
        /// included/excluded in this builder.
        /// </param>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder SetEmailValue(bool value)
        {
            if (value)
            {
                if ((this.rawValue & OAuthKnownScopes.Email) == OAuthKnownScopes.Email)
                    return this;

                this.rawValue |= OAuthKnownScopes.Email;
                this.definedCount++;
                return this;
            }

            if ((this.rawValue & OAuthKnownScopes.Email) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.Email;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Adds the
        /// <c>gdm.join</c>
        /// (<see cref="JoinGroupDM"/>)
        /// OAuth Scope if it wasn't already added.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithJoinGroupDM()
        {
            if ((this.rawValue & OAuthKnownScopes.JoinGroupDM) == OAuthKnownScopes.JoinGroupDM)
                return this;

            this.rawValue |= OAuthKnownScopes.JoinGroupDM;
            this.definedCount++;
            return this;
        }

        /// <summary>
        /// Removes the
        /// <c>gdm.join</c>
        /// (<see cref="JoinGroupDM"/>)
        /// OAuth Scope if is contained in this builder.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithoutJoinGroupDM()
        {
            if ((this.rawValue & OAuthKnownScopes.JoinGroupDM) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.JoinGroupDM;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Sets whether or not the
        /// <c>gdm.join</c>
        /// (<see cref="JoinGroupDM"/>)
        /// OAuth Scope if is included in this builder.
        /// </summary>
        /// <param name="value">
        /// Whether or not the scope should be
        /// included/excluded in this builder.
        /// </param>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder SetJoinGroupDMValue(bool value)
        {
            if (value)
            {
                if ((this.rawValue & OAuthKnownScopes.JoinGroupDM) == OAuthKnownScopes.JoinGroupDM)
                    return this;

                this.rawValue |= OAuthKnownScopes.JoinGroupDM;
                this.definedCount++;
                return this;
            }

            if ((this.rawValue & OAuthKnownScopes.JoinGroupDM) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.JoinGroupDM;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Adds the
        /// <c>guilds</c>
        /// (<see cref="Guilds"/>)
        /// OAuth Scope if it wasn't already added.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithGuilds()
        {
            if ((this.rawValue & OAuthKnownScopes.Guilds) == OAuthKnownScopes.Guilds)
                return this;

            this.rawValue |= OAuthKnownScopes.Guilds;
            this.definedCount++;
            return this;
        }

        /// <summary>
        /// Removes the
        /// <c>guilds</c>
        /// (<see cref="Guilds"/>)
        /// OAuth Scope if is contained in this builder.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithoutGuilds()
        {
            if ((this.rawValue & OAuthKnownScopes.Guilds) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.Guilds;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Sets whether or not the
        /// <c>guilds</c>
        /// (<see cref="Guilds"/>)
        /// OAuth Scope if is included in this builder.
        /// </summary>
        /// <param name="value">
        /// Whether or not the scope should be
        /// included/excluded in this builder.
        /// </param>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder SetGuildsValue(bool value)
        {
            if (value)
            {
                if ((this.rawValue & OAuthKnownScopes.Guilds) == OAuthKnownScopes.Guilds)
                    return this;

                this.rawValue |= OAuthKnownScopes.Guilds;
                this.definedCount++;
                return this;
            }

            if ((this.rawValue & OAuthKnownScopes.Guilds) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.Guilds;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Adds the
        /// <c>guilds.join</c>
        /// (<see cref="JoinGuilds"/>)
        /// OAuth Scope if it wasn't already added.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithJoinGuilds()
        {
            if ((this.rawValue & OAuthKnownScopes.JoinGuilds) == OAuthKnownScopes.JoinGuilds)
                return this;

            this.rawValue |= OAuthKnownScopes.JoinGuilds;
            this.definedCount++;
            return this;
        }

        /// <summary>
        /// Removes the
        /// <c>guilds.join</c>
        /// (<see cref="JoinGuilds"/>)
        /// OAuth Scope if is contained in this builder.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithoutJoinGuilds()
        {
            if ((this.rawValue & OAuthKnownScopes.JoinGuilds) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.JoinGuilds;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Sets whether or not the
        /// <c>guilds.join</c>
        /// (<see cref="JoinGuilds"/>)
        /// OAuth Scope if is included in this builder.
        /// </summary>
        /// <param name="value">
        /// Whether or not the scope should be
        /// included/excluded in this builder.
        /// </param>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder SetJoinGuildsValue(bool value)
        {
            if (value)
            {
                if ((this.rawValue & OAuthKnownScopes.JoinGuilds) == OAuthKnownScopes.JoinGuilds)
                    return this;

                this.rawValue |= OAuthKnownScopes.JoinGuilds;
                this.definedCount++;
                return this;
            }

            if ((this.rawValue & OAuthKnownScopes.JoinGuilds) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.JoinGuilds;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Adds the
        /// <c>guilds.members.read</c>
        /// (<see cref="ReadGuildMembers"/>)
        /// OAuth Scope if it wasn't already added.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithReadGuildMembers()
        {
            if ((this.rawValue & OAuthKnownScopes.ReadGuildMembers) == OAuthKnownScopes.ReadGuildMembers)
                return this;

            this.rawValue |= OAuthKnownScopes.ReadGuildMembers;
            this.definedCount++;
            return this;
        }

        /// <summary>
        /// Removes the
        /// <c>guilds.members.read</c>
        /// (<see cref="ReadGuildMembers"/>)
        /// OAuth Scope if is contained in this builder.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithoutReadGuildMembers()
        {
            if ((this.rawValue & OAuthKnownScopes.ReadGuildMembers) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.ReadGuildMembers;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Sets whether or not the
        /// <c>guilds.members.read</c>
        /// (<see cref="ReadGuildMembers"/>)
        /// OAuth Scope if is included in this builder.
        /// </summary>
        /// <param name="value">
        /// Whether or not the scope should be
        /// included/excluded in this builder.
        /// </param>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder SetReadGuildMembersValue(bool value)
        {
            if (value)
            {
                if ((this.rawValue & OAuthKnownScopes.ReadGuildMembers) == OAuthKnownScopes.ReadGuildMembers)
                    return this;

                this.rawValue |= OAuthKnownScopes.ReadGuildMembers;
                this.definedCount++;
                return this;
            }

            if ((this.rawValue & OAuthKnownScopes.ReadGuildMembers) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.ReadGuildMembers;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Adds the
        /// <c>identify</c>
        /// (<see cref="Identify"/>)
        /// OAuth Scope if it wasn't already added.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithIdentify()
        {
            if ((this.rawValue & OAuthKnownScopes.Identify) == OAuthKnownScopes.Identify)
                return this;

            this.rawValue |= OAuthKnownScopes.Identify;
            this.definedCount++;
            return this;
        }

        /// <summary>
        /// Removes the
        /// <c>identify</c>
        /// (<see cref="Identify"/>)
        /// OAuth Scope if is contained in this builder.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithoutIdentify()
        {
            if ((this.rawValue & OAuthKnownScopes.Identify) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.Identify;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Sets whether or not the
        /// <c>identify</c>
        /// (<see cref="Identify"/>)
        /// OAuth Scope if is included in this builder.
        /// </summary>
        /// <param name="value">
        /// Whether or not the scope should be
        /// included/excluded in this builder.
        /// </param>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder SetIdentifyValue(bool value)
        {
            if (value)
            {
                if ((this.rawValue & OAuthKnownScopes.Identify) == OAuthKnownScopes.Identify)
                    return this;

                this.rawValue |= OAuthKnownScopes.Identify;
                this.definedCount++;
                return this;
            }

            if ((this.rawValue & OAuthKnownScopes.Identify) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.Identify;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Adds the
        /// <c>messages.read</c>
        /// (<see cref="ReadMessages"/>)
        /// OAuth Scope if it wasn't already added.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithReadMessages()
        {
            if ((this.rawValue & OAuthKnownScopes.ReadMessages) == OAuthKnownScopes.ReadMessages)
                return this;

            this.rawValue |= OAuthKnownScopes.ReadMessages;
            this.definedCount++;
            return this;
        }

        /// <summary>
        /// Removes the
        /// <c>messages.read</c>
        /// (<see cref="ReadMessages"/>)
        /// OAuth Scope if is contained in this builder.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithoutReadMessages()
        {
            if ((this.rawValue & OAuthKnownScopes.ReadMessages) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.ReadMessages;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Sets whether or not the
        /// <c>messages.read</c>
        /// (<see cref="ReadMessages"/>)
        /// OAuth Scope if is included in this builder.
        /// </summary>
        /// <param name="value">
        /// Whether or not the scope should be
        /// included/excluded in this builder.
        /// </param>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder SetReadMessagesValue(bool value)
        {
            if (value)
            {
                if ((this.rawValue & OAuthKnownScopes.ReadMessages) == OAuthKnownScopes.ReadMessages)
                    return this;

                this.rawValue |= OAuthKnownScopes.ReadMessages;
                this.definedCount++;
                return this;
            }

            if ((this.rawValue & OAuthKnownScopes.ReadMessages) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.ReadMessages;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Adds the
        /// <c>relationships.read</c>
        /// (<see cref="ReadRelationships"/>)
        /// OAuth Scope if it wasn't already added.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithReadRelationships()
        {
            if ((this.rawValue & OAuthKnownScopes.ReadRelationships) == OAuthKnownScopes.ReadRelationships)
                return this;

            this.rawValue |= OAuthKnownScopes.ReadRelationships;
            this.definedCount++;
            return this;
        }

        /// <summary>
        /// Removes the
        /// <c>relationships.read</c>
        /// (<see cref="ReadRelationships"/>)
        /// OAuth Scope if is contained in this builder.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithoutReadRelationships()
        {
            if ((this.rawValue & OAuthKnownScopes.ReadRelationships) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.ReadRelationships;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Sets whether or not the
        /// <c>relationships.read</c>
        /// (<see cref="ReadRelationships"/>)
        /// OAuth Scope if is included in this builder.
        /// </summary>
        /// <param name="value">
        /// Whether or not the scope should be
        /// included/excluded in this builder.
        /// </param>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder SetReadRelationshipsValue(bool value)
        {
            if (value)
            {
                if ((this.rawValue & OAuthKnownScopes.ReadRelationships) == OAuthKnownScopes.ReadRelationships)
                    return this;

                this.rawValue |= OAuthKnownScopes.ReadRelationships;
                this.definedCount++;
                return this;
            }

            if ((this.rawValue & OAuthKnownScopes.ReadRelationships) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.ReadRelationships;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Adds the
        /// <c>role_connections.write</c>
        /// (<see cref="WriteRoleConnections"/>)
        /// OAuth Scope if it wasn't already added.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithWriteRoleConnections()
        {
            if ((this.rawValue & OAuthKnownScopes.WriteRoleConnections) == OAuthKnownScopes.WriteRoleConnections)
                return this;

            this.rawValue |= OAuthKnownScopes.WriteRoleConnections;
            this.definedCount++;
            return this;
        }

        /// <summary>
        /// Removes the
        /// <c>role_connections.write</c>
        /// (<see cref="WriteRoleConnections"/>)
        /// OAuth Scope if is contained in this builder.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithoutWriteRoleConnections()
        {
            if ((this.rawValue & OAuthKnownScopes.WriteRoleConnections) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.WriteRoleConnections;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Sets whether or not the
        /// <c>role_connections.write</c>
        /// (<see cref="WriteRoleConnections"/>)
        /// OAuth Scope if is included in this builder.
        /// </summary>
        /// <param name="value">
        /// Whether or not the scope should be
        /// included/excluded in this builder.
        /// </param>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder SetWriteRoleConnectionsValue(bool value)
        {
            if (value)
            {
                if ((this.rawValue & OAuthKnownScopes.WriteRoleConnections) == OAuthKnownScopes.WriteRoleConnections)
                    return this;

                this.rawValue |= OAuthKnownScopes.WriteRoleConnections;
                this.definedCount++;
                return this;
            }

            if ((this.rawValue & OAuthKnownScopes.WriteRoleConnections) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.WriteRoleConnections;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Adds the
        /// <c>rpc</c>
        /// (<see cref="Rpc"/>)
        /// OAuth Scope if it wasn't already added.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithRpc()
        {
            if ((this.rawValue & OAuthKnownScopes.Rpc) == OAuthKnownScopes.Rpc)
                return this;

            this.rawValue |= OAuthKnownScopes.Rpc;
            this.definedCount++;
            return this;
        }

        /// <summary>
        /// Removes the
        /// <c>rpc</c>
        /// (<see cref="Rpc"/>)
        /// OAuth Scope if is contained in this builder.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithoutRpc()
        {
            if ((this.rawValue & OAuthKnownScopes.Rpc) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.Rpc;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Sets whether or not the
        /// <c>rpc</c>
        /// (<see cref="Rpc"/>)
        /// OAuth Scope if is included in this builder.
        /// </summary>
        /// <param name="value">
        /// Whether or not the scope should be
        /// included/excluded in this builder.
        /// </param>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder SetRpcValue(bool value)
        {
            if (value)
            {
                if ((this.rawValue & OAuthKnownScopes.Rpc) == OAuthKnownScopes.Rpc)
                    return this;

                this.rawValue |= OAuthKnownScopes.Rpc;
                this.definedCount++;
                return this;
            }

            if ((this.rawValue & OAuthKnownScopes.Rpc) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.Rpc;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Adds the
        /// <c>rpc.activities.write</c>
        /// (<see cref="WriteRpcActivities"/>)
        /// OAuth Scope if it wasn't already added.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithWriteRpcActivities()
        {
            if ((this.rawValue & OAuthKnownScopes.WriteRpcActivities) == OAuthKnownScopes.WriteRpcActivities)
                return this;

            this.rawValue |= OAuthKnownScopes.WriteRpcActivities;
            this.definedCount++;
            return this;
        }

        /// <summary>
        /// Removes the
        /// <c>rpc.activities.write</c>
        /// (<see cref="WriteRpcActivities"/>)
        /// OAuth Scope if is contained in this builder.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithoutWriteRpcActivities()
        {
            if ((this.rawValue & OAuthKnownScopes.WriteRpcActivities) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.WriteRpcActivities;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Sets whether or not the
        /// <c>rpc.activities.write</c>
        /// (<see cref="WriteRpcActivities"/>)
        /// OAuth Scope if is included in this builder.
        /// </summary>
        /// <param name="value">
        /// Whether or not the scope should be
        /// included/excluded in this builder.
        /// </param>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder SetWriteRpcActivitiesValue(bool value)
        {
            if (value)
            {
                if ((this.rawValue & OAuthKnownScopes.WriteRpcActivities) == OAuthKnownScopes.WriteRpcActivities)
                    return this;

                this.rawValue |= OAuthKnownScopes.WriteRpcActivities;
                this.definedCount++;
                return this;
            }

            if ((this.rawValue & OAuthKnownScopes.WriteRpcActivities) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.WriteRpcActivities;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Adds the
        /// <c>rpc.notifications.read</c>
        /// (<see cref="ReadRpcNotifications"/>)
        /// OAuth Scope if it wasn't already added.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithReadRpcNotifications()
        {
            if ((this.rawValue & OAuthKnownScopes.ReadRpcNotifications) == OAuthKnownScopes.ReadRpcNotifications)
                return this;

            this.rawValue |= OAuthKnownScopes.ReadRpcNotifications;
            this.definedCount++;
            return this;
        }

        /// <summary>
        /// Removes the
        /// <c>rpc.notifications.read</c>
        /// (<see cref="ReadRpcNotifications"/>)
        /// OAuth Scope if is contained in this builder.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithoutReadRpcNotifications()
        {
            if ((this.rawValue & OAuthKnownScopes.ReadRpcNotifications) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.ReadRpcNotifications;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Sets whether or not the
        /// <c>rpc.notifications.read</c>
        /// (<see cref="ReadRpcNotifications"/>)
        /// OAuth Scope if is included in this builder.
        /// </summary>
        /// <param name="value">
        /// Whether or not the scope should be
        /// included/excluded in this builder.
        /// </param>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder SetReadRpcNotificationsValue(bool value)
        {
            if (value)
            {
                if ((this.rawValue & OAuthKnownScopes.ReadRpcNotifications) == OAuthKnownScopes.ReadRpcNotifications)
                    return this;

                this.rawValue |= OAuthKnownScopes.ReadRpcNotifications;
                this.definedCount++;
                return this;
            }

            if ((this.rawValue & OAuthKnownScopes.ReadRpcNotifications) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.ReadRpcNotifications;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Adds the
        /// <c>rpc.voice.read</c>
        /// (<see cref="ReadRpcVoice"/>)
        /// OAuth Scope if it wasn't already added.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithReadRpcVoice()
        {
            if ((this.rawValue & OAuthKnownScopes.ReadRpcVoice) == OAuthKnownScopes.ReadRpcVoice)
                return this;

            this.rawValue |= OAuthKnownScopes.ReadRpcVoice;
            this.definedCount++;
            return this;
        }

        /// <summary>
        /// Removes the
        /// <c>rpc.voice.read</c>
        /// (<see cref="ReadRpcVoice"/>)
        /// OAuth Scope if is contained in this builder.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithoutReadRpcVoice()
        {
            if ((this.rawValue & OAuthKnownScopes.ReadRpcVoice) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.ReadRpcVoice;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Sets whether or not the
        /// <c>rpc.voice.read</c>
        /// (<see cref="ReadRpcVoice"/>)
        /// OAuth Scope if is included in this builder.
        /// </summary>
        /// <param name="value">
        /// Whether or not the scope should be
        /// included/excluded in this builder.
        /// </param>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder SetReadRpcVoiceValue(bool value)
        {
            if (value)
            {
                if ((this.rawValue & OAuthKnownScopes.ReadRpcVoice) == OAuthKnownScopes.ReadRpcVoice)
                    return this;

                this.rawValue |= OAuthKnownScopes.ReadRpcVoice;
                this.definedCount++;
                return this;
            }

            if ((this.rawValue & OAuthKnownScopes.ReadRpcVoice) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.ReadRpcVoice;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Adds the
        /// <c>rpc.voice.write</c>
        /// (<see cref="WriteRpcVoice"/>)
        /// OAuth Scope if it wasn't already added.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithWriteRpcVoice()
        {
            if ((this.rawValue & OAuthKnownScopes.WriteRpcVoice) == OAuthKnownScopes.WriteRpcVoice)
                return this;

            this.rawValue |= OAuthKnownScopes.WriteRpcVoice;
            this.definedCount++;
            return this;
        }

        /// <summary>
        /// Removes the
        /// <c>rpc.voice.write</c>
        /// (<see cref="WriteRpcVoice"/>)
        /// OAuth Scope if is contained in this builder.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithoutWriteRpcVoice()
        {
            if ((this.rawValue & OAuthKnownScopes.WriteRpcVoice) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.WriteRpcVoice;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Sets whether or not the
        /// <c>rpc.voice.write</c>
        /// (<see cref="WriteRpcVoice"/>)
        /// OAuth Scope if is included in this builder.
        /// </summary>
        /// <param name="value">
        /// Whether or not the scope should be
        /// included/excluded in this builder.
        /// </param>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder SetWriteRpcVoiceValue(bool value)
        {
            if (value)
            {
                if ((this.rawValue & OAuthKnownScopes.WriteRpcVoice) == OAuthKnownScopes.WriteRpcVoice)
                    return this;

                this.rawValue |= OAuthKnownScopes.WriteRpcVoice;
                this.definedCount++;
                return this;
            }

            if ((this.rawValue & OAuthKnownScopes.WriteRpcVoice) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.WriteRpcVoice;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Adds the
        /// <c>voice</c>
        /// (<see cref="Voice"/>)
        /// OAuth Scope if it wasn't already added.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithVoice()
        {
            if ((this.rawValue & OAuthKnownScopes.Voice) == OAuthKnownScopes.Voice)
                return this;

            this.rawValue |= OAuthKnownScopes.Voice;
            this.definedCount++;
            return this;
        }

        /// <summary>
        /// Removes the
        /// <c>voice</c>
        /// (<see cref="Voice"/>)
        /// OAuth Scope if is contained in this builder.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithoutVoice()
        {
            if ((this.rawValue & OAuthKnownScopes.Voice) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.Voice;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Sets whether or not the
        /// <c>voice</c>
        /// (<see cref="Voice"/>)
        /// OAuth Scope if is included in this builder.
        /// </summary>
        /// <param name="value">
        /// Whether or not the scope should be
        /// included/excluded in this builder.
        /// </param>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder SetVoiceValue(bool value)
        {
            if (value)
            {
                if ((this.rawValue & OAuthKnownScopes.Voice) == OAuthKnownScopes.Voice)
                    return this;

                this.rawValue |= OAuthKnownScopes.Voice;
                this.definedCount++;
                return this;
            }

            if ((this.rawValue & OAuthKnownScopes.Voice) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.Voice;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Adds the
        /// <c>webhook.incoming</c>
        /// (<see cref="IncomingWebhook"/>)
        /// OAuth Scope if it wasn't already added.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithIncomingWebhook()
        {
            if ((this.rawValue & OAuthKnownScopes.IncomingWebhook) == OAuthKnownScopes.IncomingWebhook)
                return this;

            this.rawValue |= OAuthKnownScopes.IncomingWebhook;
            this.definedCount++;
            return this;
        }

        /// <summary>
        /// Removes the
        /// <c>webhook.incoming</c>
        /// (<see cref="IncomingWebhook"/>)
        /// OAuth Scope if is contained in this builder.
        /// </summary>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithoutIncomingWebhook()
        {
            if ((this.rawValue & OAuthKnownScopes.IncomingWebhook) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.IncomingWebhook;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Sets whether or not the
        /// <c>webhook.incoming</c>
        /// (<see cref="IncomingWebhook"/>)
        /// OAuth Scope if is included in this builder.
        /// </summary>
        /// <param name="value">
        /// Whether or not the scope should be
        /// included/excluded in this builder.
        /// </param>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder SetIncomingWebhookValue(bool value)
        {
            if (value)
            {
                if ((this.rawValue & OAuthKnownScopes.IncomingWebhook) == OAuthKnownScopes.IncomingWebhook)
                    return this;

                this.rawValue |= OAuthKnownScopes.IncomingWebhook;
                this.definedCount++;
                return this;
            }

            if ((this.rawValue & OAuthKnownScopes.IncomingWebhook) == 0)
                return this;

            this.rawValue &= ~OAuthKnownScopes.IncomingWebhook;
            this.definedCount--;
            return this;
        }

        /// <summary>
        /// Adds the specified oauth scope to this builder if
        /// it wasn't already added.
        /// </summary>
        /// <param name="scope">
        /// The scope to add.
        /// </param>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithScope(string? scope)
        {
            if (string.IsNullOrEmpty(scope))
                return this;

            if (scope.Equals(OAuthScopes.ReadActivities, StringComparison.OrdinalIgnoreCase))
                return this.WithReadActivities();

            if (scope.Equals(OAuthScopes.WriteActivities, StringComparison.OrdinalIgnoreCase))
                return this.WithWriteActivities();

            if (scope.Equals(OAuthScopes.ReadApplicationBuilds, StringComparison.OrdinalIgnoreCase))
                return this.WithReadApplicationBuilds();

            if (scope.Equals(OAuthScopes.UploadApplicationBuilds, StringComparison.OrdinalIgnoreCase))
                return this.WithUploadApplicationBuilds();

            if (scope.Equals(OAuthScopes.ApplicationCommands, StringComparison.OrdinalIgnoreCase))
                return this.WithApplicationCommands();

            if (scope.Equals(OAuthScopes.UpdateApplicationCommands, StringComparison.OrdinalIgnoreCase))
                return this.WithUpdateApplicationCommands();

            if (scope.Equals(OAuthScopes.UpdateApplicationCommandPermissions, StringComparison.OrdinalIgnoreCase))
                return this.WithUpdateApplicationCommandPermissions();

            if (scope.Equals(OAuthScopes.ApplicationEntitlements, StringComparison.OrdinalIgnoreCase))
                return this.WithApplicationEntitlements();

            if (scope.Equals(OAuthScopes.UpdateApplicationStore, StringComparison.OrdinalIgnoreCase))
                return this.WithUpdateApplicationStore();

            if (scope.Equals(OAuthScopes.Bot, StringComparison.OrdinalIgnoreCase))
                return this.WithBot();

            if (scope.Equals(OAuthScopes.Connections, StringComparison.OrdinalIgnoreCase))
                return this.WithConnections();

            if (scope.Equals(OAuthScopes.ReadDMChannels, StringComparison.OrdinalIgnoreCase))
                return this.WithReadDMChannels();

            if (scope.Equals(OAuthScopes.Email, StringComparison.OrdinalIgnoreCase))
                return this.WithEmail();

            if (scope.Equals(OAuthScopes.JoinGroupDM, StringComparison.OrdinalIgnoreCase))
                return this.WithJoinGroupDM();

            if (scope.Equals(OAuthScopes.Guilds, StringComparison.OrdinalIgnoreCase))
                return this.WithGuilds();

            if (scope.Equals(OAuthScopes.JoinGuilds, StringComparison.OrdinalIgnoreCase))
                return this.WithJoinGuilds();

            if (scope.Equals(OAuthScopes.ReadGuildMembers, StringComparison.OrdinalIgnoreCase))
                return this.WithReadGuildMembers();

            if (scope.Equals(OAuthScopes.Identify, StringComparison.OrdinalIgnoreCase))
                return this.WithIdentify();

            if (scope.Equals(OAuthScopes.ReadMessages, StringComparison.OrdinalIgnoreCase))
                return this.WithReadMessages();

            if (scope.Equals(OAuthScopes.ReadRelationships, StringComparison.OrdinalIgnoreCase))
                return this.WithReadRelationships();

            if (scope.Equals(OAuthScopes.WriteRoleConnections, StringComparison.OrdinalIgnoreCase))
                return this.WithWriteRoleConnections();

            if (scope.Equals(OAuthScopes.Rpc, StringComparison.OrdinalIgnoreCase))
                return this.WithRpc();

            if (scope.Equals(OAuthScopes.WriteRpcActivities, StringComparison.OrdinalIgnoreCase))
                return this.WithWriteRpcActivities();

            if (scope.Equals(OAuthScopes.ReadRpcNotifications, StringComparison.OrdinalIgnoreCase))
                return this.WithReadRpcNotifications();

            if (scope.Equals(OAuthScopes.ReadRpcVoice, StringComparison.OrdinalIgnoreCase))
                return this.WithReadRpcVoice();

            if (scope.Equals(OAuthScopes.WriteRpcVoice, StringComparison.OrdinalIgnoreCase))
                return this.WithWriteRpcVoice();

            if (scope.Equals(OAuthScopes.Voice, StringComparison.OrdinalIgnoreCase))
                return this.WithVoice();

            if (scope.Equals(OAuthScopes.IncomingWebhook, StringComparison.OrdinalIgnoreCase))
                return this.WithIncomingWebhook();
            
            this.unknownScopes.AddScope(scope);
            return this;
        }

        /// <summary>
        /// Adds the specified oauth scope to this builder if
        /// it wasn't already added.
        /// </summary>
        /// <param name="scope">
        /// The scope to add.
        /// </param>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithScope(ReadOnlySpan<char> scope)
        {
            if (scope.IsEmpty)
                return this;

            if (scope.Equals(OAuthScopes.ReadActivities, StringComparison.OrdinalIgnoreCase))
                return this.WithReadActivities();

            if (scope.Equals(OAuthScopes.WriteActivities, StringComparison.OrdinalIgnoreCase))
                return this.WithWriteActivities();

            if (scope.Equals(OAuthScopes.ReadApplicationBuilds, StringComparison.OrdinalIgnoreCase))
                return this.WithReadApplicationBuilds();

            if (scope.Equals(OAuthScopes.UploadApplicationBuilds, StringComparison.OrdinalIgnoreCase))
                return this.WithUploadApplicationBuilds();

            if (scope.Equals(OAuthScopes.ApplicationCommands, StringComparison.OrdinalIgnoreCase))
                return this.WithApplicationCommands();

            if (scope.Equals(OAuthScopes.UpdateApplicationCommands, StringComparison.OrdinalIgnoreCase))
                return this.WithUpdateApplicationCommands();

            if (scope.Equals(OAuthScopes.UpdateApplicationCommandPermissions, StringComparison.OrdinalIgnoreCase))
                return this.WithUpdateApplicationCommandPermissions();

            if (scope.Equals(OAuthScopes.ApplicationEntitlements, StringComparison.OrdinalIgnoreCase))
                return this.WithApplicationEntitlements();

            if (scope.Equals(OAuthScopes.UpdateApplicationStore, StringComparison.OrdinalIgnoreCase))
                return this.WithUpdateApplicationStore();

            if (scope.Equals(OAuthScopes.Bot, StringComparison.OrdinalIgnoreCase))
                return this.WithBot();

            if (scope.Equals(OAuthScopes.Connections, StringComparison.OrdinalIgnoreCase))
                return this.WithConnections();

            if (scope.Equals(OAuthScopes.ReadDMChannels, StringComparison.OrdinalIgnoreCase))
                return this.WithReadDMChannels();

            if (scope.Equals(OAuthScopes.Email, StringComparison.OrdinalIgnoreCase))
                return this.WithEmail();

            if (scope.Equals(OAuthScopes.JoinGroupDM, StringComparison.OrdinalIgnoreCase))
                return this.WithJoinGroupDM();

            if (scope.Equals(OAuthScopes.Guilds, StringComparison.OrdinalIgnoreCase))
                return this.WithGuilds();

            if (scope.Equals(OAuthScopes.JoinGuilds, StringComparison.OrdinalIgnoreCase))
                return this.WithJoinGuilds();

            if (scope.Equals(OAuthScopes.ReadGuildMembers, StringComparison.OrdinalIgnoreCase))
                return this.WithReadGuildMembers();

            if (scope.Equals(OAuthScopes.Identify, StringComparison.OrdinalIgnoreCase))
                return this.WithIdentify();

            if (scope.Equals(OAuthScopes.ReadMessages, StringComparison.OrdinalIgnoreCase))
                return this.WithReadMessages();

            if (scope.Equals(OAuthScopes.ReadRelationships, StringComparison.OrdinalIgnoreCase))
                return this.WithReadRelationships();

            if (scope.Equals(OAuthScopes.WriteRoleConnections, StringComparison.OrdinalIgnoreCase))
                return this.WithWriteRoleConnections();

            if (scope.Equals(OAuthScopes.Rpc, StringComparison.OrdinalIgnoreCase))
                return this.WithRpc();

            if (scope.Equals(OAuthScopes.WriteRpcActivities, StringComparison.OrdinalIgnoreCase))
                return this.WithWriteRpcActivities();

            if (scope.Equals(OAuthScopes.ReadRpcNotifications, StringComparison.OrdinalIgnoreCase))
                return this.WithReadRpcNotifications();

            if (scope.Equals(OAuthScopes.ReadRpcVoice, StringComparison.OrdinalIgnoreCase))
                return this.WithReadRpcVoice();

            if (scope.Equals(OAuthScopes.WriteRpcVoice, StringComparison.OrdinalIgnoreCase))
                return this.WithWriteRpcVoice();

            if (scope.Equals(OAuthScopes.Voice, StringComparison.OrdinalIgnoreCase))
                return this.WithVoice();

            if (scope.Equals(OAuthScopes.IncomingWebhook, StringComparison.OrdinalIgnoreCase))
                return this.WithIncomingWebhook();
            
            this.unknownScopes.AddScope(scope);
            return this;
        }

        /// <summary>
        /// Removes the specified oauth scope to this builder
        /// if this builder has it.
        /// </summary>
        /// <param name="scope">
        /// The scope to remove.
        /// </param>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithoutScope(string? scope)
        {
            if (string.IsNullOrEmpty(scope))
                return this;

            if (scope.Equals(OAuthScopes.ReadActivities, StringComparison.OrdinalIgnoreCase))
                return this.WithoutReadActivities();

            if (scope.Equals(OAuthScopes.WriteActivities, StringComparison.OrdinalIgnoreCase))
                return this.WithoutWriteActivities();

            if (scope.Equals(OAuthScopes.ReadApplicationBuilds, StringComparison.OrdinalIgnoreCase))
                return this.WithoutReadApplicationBuilds();

            if (scope.Equals(OAuthScopes.UploadApplicationBuilds, StringComparison.OrdinalIgnoreCase))
                return this.WithoutUploadApplicationBuilds();

            if (scope.Equals(OAuthScopes.ApplicationCommands, StringComparison.OrdinalIgnoreCase))
                return this.WithoutApplicationCommands();

            if (scope.Equals(OAuthScopes.UpdateApplicationCommands, StringComparison.OrdinalIgnoreCase))
                return this.WithoutUpdateApplicationCommands();

            if (scope.Equals(OAuthScopes.UpdateApplicationCommandPermissions, StringComparison.OrdinalIgnoreCase))
                return this.WithoutUpdateApplicationCommandPermissions();

            if (scope.Equals(OAuthScopes.ApplicationEntitlements, StringComparison.OrdinalIgnoreCase))
                return this.WithoutApplicationEntitlements();

            if (scope.Equals(OAuthScopes.UpdateApplicationStore, StringComparison.OrdinalIgnoreCase))
                return this.WithoutUpdateApplicationStore();

            if (scope.Equals(OAuthScopes.Bot, StringComparison.OrdinalIgnoreCase))
                return this.WithoutBot();

            if (scope.Equals(OAuthScopes.Connections, StringComparison.OrdinalIgnoreCase))
                return this.WithoutConnections();

            if (scope.Equals(OAuthScopes.ReadDMChannels, StringComparison.OrdinalIgnoreCase))
                return this.WithoutReadDMChannels();

            if (scope.Equals(OAuthScopes.Email, StringComparison.OrdinalIgnoreCase))
                return this.WithoutEmail();

            if (scope.Equals(OAuthScopes.JoinGroupDM, StringComparison.OrdinalIgnoreCase))
                return this.WithoutJoinGroupDM();

            if (scope.Equals(OAuthScopes.Guilds, StringComparison.OrdinalIgnoreCase))
                return this.WithoutGuilds();

            if (scope.Equals(OAuthScopes.JoinGuilds, StringComparison.OrdinalIgnoreCase))
                return this.WithoutJoinGuilds();

            if (scope.Equals(OAuthScopes.ReadGuildMembers, StringComparison.OrdinalIgnoreCase))
                return this.WithoutReadGuildMembers();

            if (scope.Equals(OAuthScopes.Identify, StringComparison.OrdinalIgnoreCase))
                return this.WithoutIdentify();

            if (scope.Equals(OAuthScopes.ReadMessages, StringComparison.OrdinalIgnoreCase))
                return this.WithoutReadMessages();

            if (scope.Equals(OAuthScopes.ReadRelationships, StringComparison.OrdinalIgnoreCase))
                return this.WithoutReadRelationships();

            if (scope.Equals(OAuthScopes.WriteRoleConnections, StringComparison.OrdinalIgnoreCase))
                return this.WithoutWriteRoleConnections();

            if (scope.Equals(OAuthScopes.Rpc, StringComparison.OrdinalIgnoreCase))
                return this.WithoutRpc();

            if (scope.Equals(OAuthScopes.WriteRpcActivities, StringComparison.OrdinalIgnoreCase))
                return this.WithoutWriteRpcActivities();

            if (scope.Equals(OAuthScopes.ReadRpcNotifications, StringComparison.OrdinalIgnoreCase))
                return this.WithoutReadRpcNotifications();

            if (scope.Equals(OAuthScopes.ReadRpcVoice, StringComparison.OrdinalIgnoreCase))
                return this.WithoutReadRpcVoice();

            if (scope.Equals(OAuthScopes.WriteRpcVoice, StringComparison.OrdinalIgnoreCase))
                return this.WithoutWriteRpcVoice();

            if (scope.Equals(OAuthScopes.Voice, StringComparison.OrdinalIgnoreCase))
                return this.WithoutVoice();

            if (scope.Equals(OAuthScopes.IncomingWebhook, StringComparison.OrdinalIgnoreCase))
                return this.WithoutIncomingWebhook();
            
            this.unknownScopes.Remove(scope);
            return this;
        }

        /// <summary>
        /// Removes the specified oauth scope to this builder
        /// if this builder has it.
        /// </summary>
        /// <param name="scope">
        /// The scope to remove.
        /// </param>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder WithoutScope(ReadOnlySpan<char> scope)
        {
            if (scope.IsEmpty)
                return this;

            if (scope.Equals(OAuthScopes.ReadActivities, StringComparison.OrdinalIgnoreCase))
                return this.WithoutReadActivities();

            if (scope.Equals(OAuthScopes.WriteActivities, StringComparison.OrdinalIgnoreCase))
                return this.WithoutWriteActivities();

            if (scope.Equals(OAuthScopes.ReadApplicationBuilds, StringComparison.OrdinalIgnoreCase))
                return this.WithoutReadApplicationBuilds();

            if (scope.Equals(OAuthScopes.UploadApplicationBuilds, StringComparison.OrdinalIgnoreCase))
                return this.WithoutUploadApplicationBuilds();

            if (scope.Equals(OAuthScopes.ApplicationCommands, StringComparison.OrdinalIgnoreCase))
                return this.WithoutApplicationCommands();

            if (scope.Equals(OAuthScopes.UpdateApplicationCommands, StringComparison.OrdinalIgnoreCase))
                return this.WithoutUpdateApplicationCommands();

            if (scope.Equals(OAuthScopes.UpdateApplicationCommandPermissions, StringComparison.OrdinalIgnoreCase))
                return this.WithoutUpdateApplicationCommandPermissions();

            if (scope.Equals(OAuthScopes.ApplicationEntitlements, StringComparison.OrdinalIgnoreCase))
                return this.WithoutApplicationEntitlements();

            if (scope.Equals(OAuthScopes.UpdateApplicationStore, StringComparison.OrdinalIgnoreCase))
                return this.WithoutUpdateApplicationStore();

            if (scope.Equals(OAuthScopes.Bot, StringComparison.OrdinalIgnoreCase))
                return this.WithoutBot();

            if (scope.Equals(OAuthScopes.Connections, StringComparison.OrdinalIgnoreCase))
                return this.WithoutConnections();

            if (scope.Equals(OAuthScopes.ReadDMChannels, StringComparison.OrdinalIgnoreCase))
                return this.WithoutReadDMChannels();

            if (scope.Equals(OAuthScopes.Email, StringComparison.OrdinalIgnoreCase))
                return this.WithoutEmail();

            if (scope.Equals(OAuthScopes.JoinGroupDM, StringComparison.OrdinalIgnoreCase))
                return this.WithoutJoinGroupDM();

            if (scope.Equals(OAuthScopes.Guilds, StringComparison.OrdinalIgnoreCase))
                return this.WithoutGuilds();

            if (scope.Equals(OAuthScopes.JoinGuilds, StringComparison.OrdinalIgnoreCase))
                return this.WithoutJoinGuilds();

            if (scope.Equals(OAuthScopes.ReadGuildMembers, StringComparison.OrdinalIgnoreCase))
                return this.WithoutReadGuildMembers();

            if (scope.Equals(OAuthScopes.Identify, StringComparison.OrdinalIgnoreCase))
                return this.WithoutIdentify();

            if (scope.Equals(OAuthScopes.ReadMessages, StringComparison.OrdinalIgnoreCase))
                return this.WithoutReadMessages();

            if (scope.Equals(OAuthScopes.ReadRelationships, StringComparison.OrdinalIgnoreCase))
                return this.WithoutReadRelationships();

            if (scope.Equals(OAuthScopes.WriteRoleConnections, StringComparison.OrdinalIgnoreCase))
                return this.WithoutWriteRoleConnections();

            if (scope.Equals(OAuthScopes.Rpc, StringComparison.OrdinalIgnoreCase))
                return this.WithoutRpc();

            if (scope.Equals(OAuthScopes.WriteRpcActivities, StringComparison.OrdinalIgnoreCase))
                return this.WithoutWriteRpcActivities();

            if (scope.Equals(OAuthScopes.ReadRpcNotifications, StringComparison.OrdinalIgnoreCase))
                return this.WithoutReadRpcNotifications();

            if (scope.Equals(OAuthScopes.ReadRpcVoice, StringComparison.OrdinalIgnoreCase))
                return this.WithoutReadRpcVoice();

            if (scope.Equals(OAuthScopes.WriteRpcVoice, StringComparison.OrdinalIgnoreCase))
                return this.WithoutWriteRpcVoice();

            if (scope.Equals(OAuthScopes.Voice, StringComparison.OrdinalIgnoreCase))
                return this.WithoutVoice();

            if (scope.Equals(OAuthScopes.IncomingWebhook, StringComparison.OrdinalIgnoreCase))
                return this.WithoutIncomingWebhook();
            
            this.unknownScopes.Remove(scope);
            return this;
        }

        /// <summary>
        /// Sets whether or not the specified oauth scope
        /// should be included in this builder.
        /// </summary>
        /// <param name="scope">
        /// The scope to set.
        /// </param>
        /// <param name="value">
        /// Whether or not the scope should be
        /// included/excluded in this builder.
        /// </param>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder SetScopeValue(string? scope, bool value)
        {
            if (string.IsNullOrEmpty(scope))
                return this;

            if (scope.Equals(OAuthScopes.ReadActivities, StringComparison.OrdinalIgnoreCase))
                return this.SetReadActivitiesValue(value);

            if (scope.Equals(OAuthScopes.WriteActivities, StringComparison.OrdinalIgnoreCase))
                return this.SetWriteActivitiesValue(value);

            if (scope.Equals(OAuthScopes.ReadApplicationBuilds, StringComparison.OrdinalIgnoreCase))
                return this.SetReadApplicationBuildsValue(value);

            if (scope.Equals(OAuthScopes.UploadApplicationBuilds, StringComparison.OrdinalIgnoreCase))
                return this.SetUploadApplicationBuildsValue(value);

            if (scope.Equals(OAuthScopes.ApplicationCommands, StringComparison.OrdinalIgnoreCase))
                return this.SetApplicationCommandsValue(value);

            if (scope.Equals(OAuthScopes.UpdateApplicationCommands, StringComparison.OrdinalIgnoreCase))
                return this.SetUpdateApplicationCommandsValue(value);

            if (scope.Equals(OAuthScopes.UpdateApplicationCommandPermissions, StringComparison.OrdinalIgnoreCase))
                return this.SetUpdateApplicationCommandPermissionsValue(value);

            if (scope.Equals(OAuthScopes.ApplicationEntitlements, StringComparison.OrdinalIgnoreCase))
                return this.SetApplicationEntitlementsValue(value);

            if (scope.Equals(OAuthScopes.UpdateApplicationStore, StringComparison.OrdinalIgnoreCase))
                return this.SetUpdateApplicationStoreValue(value);

            if (scope.Equals(OAuthScopes.Bot, StringComparison.OrdinalIgnoreCase))
                return this.SetBotValue(value);

            if (scope.Equals(OAuthScopes.Connections, StringComparison.OrdinalIgnoreCase))
                return this.SetConnectionsValue(value);

            if (scope.Equals(OAuthScopes.ReadDMChannels, StringComparison.OrdinalIgnoreCase))
                return this.SetReadDMChannelsValue(value);

            if (scope.Equals(OAuthScopes.Email, StringComparison.OrdinalIgnoreCase))
                return this.SetEmailValue(value);

            if (scope.Equals(OAuthScopes.JoinGroupDM, StringComparison.OrdinalIgnoreCase))
                return this.SetJoinGroupDMValue(value);

            if (scope.Equals(OAuthScopes.Guilds, StringComparison.OrdinalIgnoreCase))
                return this.SetGuildsValue(value);

            if (scope.Equals(OAuthScopes.JoinGuilds, StringComparison.OrdinalIgnoreCase))
                return this.SetJoinGuildsValue(value);

            if (scope.Equals(OAuthScopes.ReadGuildMembers, StringComparison.OrdinalIgnoreCase))
                return this.SetReadGuildMembersValue(value);

            if (scope.Equals(OAuthScopes.Identify, StringComparison.OrdinalIgnoreCase))
                return this.SetIdentifyValue(value);

            if (scope.Equals(OAuthScopes.ReadMessages, StringComparison.OrdinalIgnoreCase))
                return this.SetReadMessagesValue(value);

            if (scope.Equals(OAuthScopes.ReadRelationships, StringComparison.OrdinalIgnoreCase))
                return this.SetReadRelationshipsValue(value);

            if (scope.Equals(OAuthScopes.WriteRoleConnections, StringComparison.OrdinalIgnoreCase))
                return this.SetWriteRoleConnectionsValue(value);

            if (scope.Equals(OAuthScopes.Rpc, StringComparison.OrdinalIgnoreCase))
                return this.SetRpcValue(value);

            if (scope.Equals(OAuthScopes.WriteRpcActivities, StringComparison.OrdinalIgnoreCase))
                return this.SetWriteRpcActivitiesValue(value);

            if (scope.Equals(OAuthScopes.ReadRpcNotifications, StringComparison.OrdinalIgnoreCase))
                return this.SetReadRpcNotificationsValue(value);

            if (scope.Equals(OAuthScopes.ReadRpcVoice, StringComparison.OrdinalIgnoreCase))
                return this.SetReadRpcVoiceValue(value);

            if (scope.Equals(OAuthScopes.WriteRpcVoice, StringComparison.OrdinalIgnoreCase))
                return this.SetWriteRpcVoiceValue(value);

            if (scope.Equals(OAuthScopes.Voice, StringComparison.OrdinalIgnoreCase))
                return this.SetVoiceValue(value);

            if (scope.Equals(OAuthScopes.IncomingWebhook, StringComparison.OrdinalIgnoreCase))
                return this.SetIncomingWebhookValue(value);
            
            if (value)
                this.unknownScopes.AddScope(scope);
            else
                this.unknownScopes.Remove(scope);
            return this;
        }

        /// <summary>
        /// Sets whether or not the specified oauth scope
        /// should be included in this builder.
        /// </summary>
        /// <param name="scope">
        /// The scope to set.
        /// </param>
        /// <param name="value">
        /// Whether or not the scope should be
        /// included/excluded in this builder.
        /// </param>
        /// <returns>
        /// <see langword="this"/>
        /// </returns>
        public Builder SetScopeValue(ReadOnlySpan<char> scope, bool value)
        {
            if (scope.IsEmpty)
                return this;

            if (scope.Equals(OAuthScopes.ReadActivities, StringComparison.OrdinalIgnoreCase))
                return this.SetReadActivitiesValue(value);

            if (scope.Equals(OAuthScopes.WriteActivities, StringComparison.OrdinalIgnoreCase))
                return this.SetWriteActivitiesValue(value);

            if (scope.Equals(OAuthScopes.ReadApplicationBuilds, StringComparison.OrdinalIgnoreCase))
                return this.SetReadApplicationBuildsValue(value);

            if (scope.Equals(OAuthScopes.UploadApplicationBuilds, StringComparison.OrdinalIgnoreCase))
                return this.SetUploadApplicationBuildsValue(value);

            if (scope.Equals(OAuthScopes.ApplicationCommands, StringComparison.OrdinalIgnoreCase))
                return this.SetApplicationCommandsValue(value);

            if (scope.Equals(OAuthScopes.UpdateApplicationCommands, StringComparison.OrdinalIgnoreCase))
                return this.SetUpdateApplicationCommandsValue(value);

            if (scope.Equals(OAuthScopes.UpdateApplicationCommandPermissions, StringComparison.OrdinalIgnoreCase))
                return this.SetUpdateApplicationCommandPermissionsValue(value);

            if (scope.Equals(OAuthScopes.ApplicationEntitlements, StringComparison.OrdinalIgnoreCase))
                return this.SetApplicationEntitlementsValue(value);

            if (scope.Equals(OAuthScopes.UpdateApplicationStore, StringComparison.OrdinalIgnoreCase))
                return this.SetUpdateApplicationStoreValue(value);

            if (scope.Equals(OAuthScopes.Bot, StringComparison.OrdinalIgnoreCase))
                return this.SetBotValue(value);

            if (scope.Equals(OAuthScopes.Connections, StringComparison.OrdinalIgnoreCase))
                return this.SetConnectionsValue(value);

            if (scope.Equals(OAuthScopes.ReadDMChannels, StringComparison.OrdinalIgnoreCase))
                return this.SetReadDMChannelsValue(value);

            if (scope.Equals(OAuthScopes.Email, StringComparison.OrdinalIgnoreCase))
                return this.SetEmailValue(value);

            if (scope.Equals(OAuthScopes.JoinGroupDM, StringComparison.OrdinalIgnoreCase))
                return this.SetJoinGroupDMValue(value);

            if (scope.Equals(OAuthScopes.Guilds, StringComparison.OrdinalIgnoreCase))
                return this.SetGuildsValue(value);

            if (scope.Equals(OAuthScopes.JoinGuilds, StringComparison.OrdinalIgnoreCase))
                return this.SetJoinGuildsValue(value);

            if (scope.Equals(OAuthScopes.ReadGuildMembers, StringComparison.OrdinalIgnoreCase))
                return this.SetReadGuildMembersValue(value);

            if (scope.Equals(OAuthScopes.Identify, StringComparison.OrdinalIgnoreCase))
                return this.SetIdentifyValue(value);

            if (scope.Equals(OAuthScopes.ReadMessages, StringComparison.OrdinalIgnoreCase))
                return this.SetReadMessagesValue(value);

            if (scope.Equals(OAuthScopes.ReadRelationships, StringComparison.OrdinalIgnoreCase))
                return this.SetReadRelationshipsValue(value);

            if (scope.Equals(OAuthScopes.WriteRoleConnections, StringComparison.OrdinalIgnoreCase))
                return this.SetWriteRoleConnectionsValue(value);

            if (scope.Equals(OAuthScopes.Rpc, StringComparison.OrdinalIgnoreCase))
                return this.SetRpcValue(value);

            if (scope.Equals(OAuthScopes.WriteRpcActivities, StringComparison.OrdinalIgnoreCase))
                return this.SetWriteRpcActivitiesValue(value);

            if (scope.Equals(OAuthScopes.ReadRpcNotifications, StringComparison.OrdinalIgnoreCase))
                return this.SetReadRpcNotificationsValue(value);

            if (scope.Equals(OAuthScopes.ReadRpcVoice, StringComparison.OrdinalIgnoreCase))
                return this.SetReadRpcVoiceValue(value);

            if (scope.Equals(OAuthScopes.WriteRpcVoice, StringComparison.OrdinalIgnoreCase))
                return this.SetWriteRpcVoiceValue(value);

            if (scope.Equals(OAuthScopes.Voice, StringComparison.OrdinalIgnoreCase))
                return this.SetVoiceValue(value);

            if (scope.Equals(OAuthScopes.IncomingWebhook, StringComparison.OrdinalIgnoreCase))
                return this.SetIncomingWebhookValue(value);
            
            if (value)
                this.unknownScopes.AddScope(scope);
            else
                this.unknownScopes.Remove(scope);
            return this;
        }

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

        /// <summary>
        /// Clears this builder.
        /// </summary>
        public void Clear()
        {
            this.rawValue = 0U;
            this.definedCount = 0;
            this.unknownScopes.Clear();
        }

        /// <summary>
        /// Gets an enumerator that returns
        /// ReadOnlySpan&lt;<see cref="char"/>&gt;.
        /// </summary>
        /// <returns>
        /// A char span enumerator.
        /// </returns>
        public SpanEnumerator GetSpanEnumerator()
        {
            return new SpanEnumerator(this);
        }
    
        /// <summary>
        /// Gets an enumerator that returns
        /// <see cref="string"/>.
        /// </summary>
        /// <returns>
        /// A enumerator.
        /// </returns>
        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <summary>
        /// Builds a new OAuthScopeCollection.
        /// </summary>
        /// <returns>
        /// A new OAuthScopeCollection based on the values in
        /// this builder.
        /// </returns>
        public OAuthScopeCollection Build()
        {
            StringBuilder builder = new();
            if ((this.rawValue & OAuthKnownScopes.ReadActivities) == OAuthKnownScopes.ReadActivities)
            {
                if (builder.Length > 0)
                    builder.Append(' ');
                builder.Append(OAuthScopes.ReadActivities);
            }
            if ((this.rawValue & OAuthKnownScopes.WriteActivities) == OAuthKnownScopes.WriteActivities)
            {
                if (builder.Length > 0)
                    builder.Append(' ');
                builder.Append(OAuthScopes.WriteActivities);
            }
            if ((this.rawValue & OAuthKnownScopes.ReadApplicationBuilds) == OAuthKnownScopes.ReadApplicationBuilds)
            {
                if (builder.Length > 0)
                    builder.Append(' ');
                builder.Append(OAuthScopes.ReadApplicationBuilds);
            }
            if ((this.rawValue & OAuthKnownScopes.UploadApplicationBuilds) == OAuthKnownScopes.UploadApplicationBuilds)
            {
                if (builder.Length > 0)
                    builder.Append(' ');
                builder.Append(OAuthScopes.UploadApplicationBuilds);
            }
            if ((this.rawValue & OAuthKnownScopes.ApplicationCommands) == OAuthKnownScopes.ApplicationCommands)
            {
                if (builder.Length > 0)
                    builder.Append(' ');
                builder.Append(OAuthScopes.ApplicationCommands);
            }
            if ((this.rawValue & OAuthKnownScopes.UpdateApplicationCommands) == OAuthKnownScopes.UpdateApplicationCommands)
            {
                if (builder.Length > 0)
                    builder.Append(' ');
                builder.Append(OAuthScopes.UpdateApplicationCommands);
            }
            if ((this.rawValue & OAuthKnownScopes.UpdateApplicationCommandPermissions) == OAuthKnownScopes.UpdateApplicationCommandPermissions)
            {
                if (builder.Length > 0)
                    builder.Append(' ');
                builder.Append(OAuthScopes.UpdateApplicationCommandPermissions);
            }
            if ((this.rawValue & OAuthKnownScopes.ApplicationEntitlements) == OAuthKnownScopes.ApplicationEntitlements)
            {
                if (builder.Length > 0)
                    builder.Append(' ');
                builder.Append(OAuthScopes.ApplicationEntitlements);
            }
            if ((this.rawValue & OAuthKnownScopes.UpdateApplicationStore) == OAuthKnownScopes.UpdateApplicationStore)
            {
                if (builder.Length > 0)
                    builder.Append(' ');
                builder.Append(OAuthScopes.UpdateApplicationStore);
            }
            if ((this.rawValue & OAuthKnownScopes.Bot) == OAuthKnownScopes.Bot)
            {
                if (builder.Length > 0)
                    builder.Append(' ');
                builder.Append(OAuthScopes.Bot);
            }
            if ((this.rawValue & OAuthKnownScopes.Connections) == OAuthKnownScopes.Connections)
            {
                if (builder.Length > 0)
                    builder.Append(' ');
                builder.Append(OAuthScopes.Connections);
            }
            if ((this.rawValue & OAuthKnownScopes.ReadDMChannels) == OAuthKnownScopes.ReadDMChannels)
            {
                if (builder.Length > 0)
                    builder.Append(' ');
                builder.Append(OAuthScopes.ReadDMChannels);
            }
            if ((this.rawValue & OAuthKnownScopes.Email) == OAuthKnownScopes.Email)
            {
                if (builder.Length > 0)
                    builder.Append(' ');
                builder.Append(OAuthScopes.Email);
            }
            if ((this.rawValue & OAuthKnownScopes.JoinGroupDM) == OAuthKnownScopes.JoinGroupDM)
            {
                if (builder.Length > 0)
                    builder.Append(' ');
                builder.Append(OAuthScopes.JoinGroupDM);
            }
            if ((this.rawValue & OAuthKnownScopes.Guilds) == OAuthKnownScopes.Guilds)
            {
                if (builder.Length > 0)
                    builder.Append(' ');
                builder.Append(OAuthScopes.Guilds);
            }
            if ((this.rawValue & OAuthKnownScopes.JoinGuilds) == OAuthKnownScopes.JoinGuilds)
            {
                if (builder.Length > 0)
                    builder.Append(' ');
                builder.Append(OAuthScopes.JoinGuilds);
            }
            if ((this.rawValue & OAuthKnownScopes.ReadGuildMembers) == OAuthKnownScopes.ReadGuildMembers)
            {
                if (builder.Length > 0)
                    builder.Append(' ');
                builder.Append(OAuthScopes.ReadGuildMembers);
            }
            if ((this.rawValue & OAuthKnownScopes.Identify) == OAuthKnownScopes.Identify)
            {
                if (builder.Length > 0)
                    builder.Append(' ');
                builder.Append(OAuthScopes.Identify);
            }
            if ((this.rawValue & OAuthKnownScopes.ReadMessages) == OAuthKnownScopes.ReadMessages)
            {
                if (builder.Length > 0)
                    builder.Append(' ');
                builder.Append(OAuthScopes.ReadMessages);
            }
            if ((this.rawValue & OAuthKnownScopes.ReadRelationships) == OAuthKnownScopes.ReadRelationships)
            {
                if (builder.Length > 0)
                    builder.Append(' ');
                builder.Append(OAuthScopes.ReadRelationships);
            }
            if ((this.rawValue & OAuthKnownScopes.WriteRoleConnections) == OAuthKnownScopes.WriteRoleConnections)
            {
                if (builder.Length > 0)
                    builder.Append(' ');
                builder.Append(OAuthScopes.WriteRoleConnections);
            }
            if ((this.rawValue & OAuthKnownScopes.Rpc) == OAuthKnownScopes.Rpc)
            {
                if (builder.Length > 0)
                    builder.Append(' ');
                builder.Append(OAuthScopes.Rpc);
            }
            if ((this.rawValue & OAuthKnownScopes.WriteRpcActivities) == OAuthKnownScopes.WriteRpcActivities)
            {
                if (builder.Length > 0)
                    builder.Append(' ');
                builder.Append(OAuthScopes.WriteRpcActivities);
            }
            if ((this.rawValue & OAuthKnownScopes.ReadRpcNotifications) == OAuthKnownScopes.ReadRpcNotifications)
            {
                if (builder.Length > 0)
                    builder.Append(' ');
                builder.Append(OAuthScopes.ReadRpcNotifications);
            }
            if ((this.rawValue & OAuthKnownScopes.ReadRpcVoice) == OAuthKnownScopes.ReadRpcVoice)
            {
                if (builder.Length > 0)
                    builder.Append(' ');
                builder.Append(OAuthScopes.ReadRpcVoice);
            }
            if ((this.rawValue & OAuthKnownScopes.WriteRpcVoice) == OAuthKnownScopes.WriteRpcVoice)
            {
                if (builder.Length > 0)
                    builder.Append(' ');
                builder.Append(OAuthScopes.WriteRpcVoice);
            }
            if ((this.rawValue & OAuthKnownScopes.Voice) == OAuthKnownScopes.Voice)
            {
                if (builder.Length > 0)
                    builder.Append(' ');
                builder.Append(OAuthScopes.Voice);
            }
            if ((this.rawValue & OAuthKnownScopes.IncomingWebhook) == OAuthKnownScopes.IncomingWebhook)
            {
                if (builder.Length > 0)
                    builder.Append(' ');
                builder.Append(OAuthScopes.IncomingWebhook);
            }

            OAuthScopeCollection.UnknownScopeCollection unknownScopesCollection;
            string stringValue;
            if (this.unknownScopes.Count == 0)
            {
                unknownScopesCollection = OAuthScopeCollection.UnknownScopeCollection.Empty;
                stringValue = builder.ToString();
            }
            else
            {
                ScopeStringSegment[] unknownScopes = new ScopeStringSegment[this.unknownScopes.Count];

                int index = 0;
                foreach (string unknownScope in this.unknownScopes)
                {
                    if (builder.Length > 0)
                        builder.Append(' ');

                    unknownScopes[index++] = new ScopeStringSegment(builder.Length, unknownScope.Length, unknownScope);
                    builder.Append(unknownScope);
                }
                stringValue = builder.ToString();

                unknownScopesCollection = new(
                    stringValue,
                    unknownScopes);
            }

            return new OAuthScopeCollection(
                underlyingString: stringValue,
                definedCount: this.definedCount,
                rawValue: this.rawValue,
                unknownScopes: unknownScopesCollection);
        }

        bool ICollection<string>.IsReadOnly => false;
        bool ICollection.IsSynchronized => false;
        object ICollection.SyncRoot => this;

        void ICollection<string>.Add(string item)
            => this.WithScope(item);
        
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
        
        bool ICollection<string>.Remove(string item)
        {
            int oldCount = this.Count;
            this.WithoutScope(item);
            return this.Count < oldCount;
        }

        void ICollection<string>.Clear()
            => this.Clear();

        IEnumerator<string> IEnumerable<string>.GetEnumerator()
        {
            return new SlowEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new SlowEnumerator(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string? GetNameFromIndex(int index, Builder builder)
        {
            if (index < 0 || index >= OAuthKnownScopes.Count)
                return null;
    
            uint mask = 1U << index;
            if ((builder.rawValue & mask) != mask)
                return null;
            
            return OAuthScopes.GetNameFromIndex(index);
        }

        private sealed class UnknownScopeCollection : IOAuthUnknownScopeCollection
        {
            private readonly Builder builder;
            private string[] array;
            private int count;
            private int version;

            public UnknownScopeCollection(Builder builder)
            {
                this.array = new string[4];
                this.count = 0;
                this.version = 0;
                this.builder = builder;
            }


            public int Count => this.count;
            public bool IsReadOnly => false;
            public bool IsSynchronized => false;
            public object SyncRoot => this;

            public void Add(string item)
                => this.builder.WithScope(item);

            public bool AddScope(string scope)
            {
                foreach (string unknownScope in this.array.AsSpan())
                {
                    if (scope.Equals(unknownScope, StringComparison.OrdinalIgnoreCase))
                        return false;
                }

                if (this.count + 1 >= this.array.Length)
                {
                    string[] newArray = new string[this.array.Length * 2];
                    Array.Copy(this.array, newArray, this.array.Length);
                    this.array = newArray;
                }

                this.array[this.count++] = scope;
                this.version++;
                return true;
            }

            public bool AddScope(ReadOnlySpan<char> scope)
            {
                foreach (string unknownScope in this.array.AsSpan())
                {
                    if (scope.Equals(unknownScope, StringComparison.OrdinalIgnoreCase))
                        return false;
                }

                if (this.count + 1 >= this.array.Length)
                {
                    string[] newArray = new string[this.array.Length * 2];
                    Array.Copy(this.array, newArray, this.array.Length);
                    this.array = newArray;
                }

                this.array[this.count++] = scope.ToString();
                this.version++;
                return true;
            }

            public void Clear()
            {
#if NETSTANDARD2_1_OR_GREATER
                Array.Fill(this.array, null!, 0, this.count);
#else
                for (int index = 0; index < this.count; index++)
                {
                    this.array[index] = null!;
                }
#endif
                this.count = 0;
                this.version++;
            }

            public bool Contains([NotNullWhen(true)] string? scope)
            {
                if (string.IsNullOrEmpty(scope))
                    return false;

                foreach (string unknownScope in this.array.AsSpan())
                {
                    if (scope.Equals(unknownScope, StringComparison.OrdinalIgnoreCase))
                        return true;
                }

                return false;
            }

            public bool Contains(ReadOnlySpan<char> scope)
            {
                if (scope.IsEmpty)
                    return false;

                foreach (string unknownScope in this.array.AsSpan())
                {
                    if (scope.Equals(unknownScope, StringComparison.OrdinalIgnoreCase))
                        return true;
                }

                return false;
            }

            public void CopyTo(string[] array, int arrayIndex)
            {
                Array.Copy(this.array, 0, array, arrayIndex, this.count);
            }

            public void CopyTo(Array array, int index)
            {
                Array.Copy(this.array, 0, array, index, this.count);
            }

            public bool Remove(string item)
            {
                if (string.IsNullOrEmpty(item))
                    return false;

                for (int index = 0; index < this.count; index++)
                {
                    if (this.array[index].Equals(item, StringComparison.OrdinalIgnoreCase))
                    {
                        this.count--;
                        if (index < this.count)
                        {
                            Array.Copy(this.array, index + 1, this.array, index, this.count - index);
                        }
                        this.version++;
                        return true;
                    }
                }

                return false;
            }

            public bool Remove(ReadOnlySpan<char> item)
            {
                if (item.IsEmpty)
                    return false;

                for (int index = 0; index < this.count; index++)
                {
                    if (item.Equals(this.array[index], StringComparison.OrdinalIgnoreCase))
                    {
                        this.count--;
                        if (index < this.count)
                        {
                            Array.Copy(this.array, index + 1, this.array, index, this.count - index);
                        }
                        this.version++;
                        return true;
                    }
                }

                return false;
            }

            public Enumerator GetEnumerator()
            {
                return new Enumerator(this);
            }

            IEnumerator<string> IEnumerable<string>.GetEnumerator()
            {
                return new SlowEnumerator(this);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return new SlowEnumerator(this);
            }

            public struct Enumerator
            {
                private readonly UnknownScopeCollection collection;
                private readonly int version;
                private string current;
                private int index;

                public Enumerator(UnknownScopeCollection collection)
                {
                    this.index = -1;
                    this.collection = collection;
                    this.current = null!;
                    this.version = collection.version;
                }

                public readonly string Current => this.current;

                public bool MoveNext()
                {
                    if (this.version != this.collection.version)
                        throw new InvalidOperationException();

                    if (this.index >= this.collection.count)
                        return false;

                    if (this.index + 1 >= this.collection.count)
                    {
                        this.index = this.collection.count;
                        this.current = null!;
                        return false;
                    }

                    this.current = this.collection.array[++this.index];
                    return true;
                }

                public void Reset()
                {
                    this.index = -1;
                    this.current = null!;
                }
            }

            private sealed class SlowEnumerator : IEnumerator<string>
            {
                private readonly UnknownScopeCollection collection;
                private readonly int version;
                private string current;
                private int index;

                public SlowEnumerator(UnknownScopeCollection collection)
                {
                    this.index = -1;
                    this.collection = collection;
                    this.current = null!;
                    this.version = collection.version;
                }

                public string Current => this.current;
                object IEnumerator.Current => this.current;

                public bool MoveNext()
                {
                    if (this.version != this.collection.version)
                        throw new InvalidOperationException();

                    if (this.index >= this.collection.count)
                        return false;

                    if (this.index + 1 >= this.collection.count)
                    {
                        this.index = this.collection.count;
                        this.current = null!;
                        return false;
                    }

                    this.current = this.collection.array[++this.index];
                    return true;
                }

                public void Dispose()
                { }

                public void Reset()
                {
                    this.index = -1;
                    this.current = null!;
                }
            }
        }

        /// <summary>
        /// An enumerator that enumerates through an oauth
        /// scope builder.
        /// </summary>
        public struct Enumerator
        {
            private readonly Builder builder;
            private readonly UnknownScopeCollection.Enumerator unknownScopeEnumerator;
            private int index;
            private string current;

            internal Enumerator(Builder builder)
            {
                this.builder = builder;
                this.unknownScopeEnumerator = builder.unknownScopes.GetEnumerator();
                this.index = -1;
                this.current = null!;
            }

            /// <summary>
            /// The current scope in the enumerator.
            /// </summary>
            public readonly string Current => this.current;

            /// <summary>
            /// Advances to the next oauth scope in the oauth
            /// scope builder.
            /// </summary>
            /// <returns>
            /// <see langword="true"/> if another oauth scope
            /// exists in the oauth scope builder; otherwise,
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
                    this.current = Builder.GetNameFromIndex(++this.index, this.builder)!;
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
            private readonly Builder builder;
            private readonly UnknownScopeCollection.Enumerator unknownScopeEnumerator;
            private int index;
            private ReadOnlySpan<char> current;

            internal SpanEnumerator(Builder builder)
            {
                this.builder = builder;
                this.unknownScopeEnumerator = builder.unknownScopes.GetEnumerator();
                this.index = -1;
            }

            /// <summary>
            /// The current scope in the enumerator.
            /// </summary>
            public readonly ReadOnlySpan<char> Current => this.current;

            /// <summary>
            /// Advances to the next oauth scope in the oauth
            /// scope builder.
            /// </summary>
            /// <returns>
            /// <see langword="true"/> if another oauth scope
            /// exists in the oauth scope builder; otherwise,
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
                        this.current = this.unknownScopeEnumerator.Current.AsSpan();
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
                    name = Builder.GetNameFromIndex(++this.index, this.builder)!;
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

                this.current = this.unknownScopeEnumerator.Current.AsSpan();
                return true;
            }
        }

        internal sealed class SlowEnumerator : IEnumerator<string>
        {
            private readonly Builder builder;
            private readonly UnknownScopeCollection.Enumerator unknownScopeEnumerator;
            private int index;
            private string current;

            internal SlowEnumerator(Builder builder)
            {
                this.builder = builder;
                this.unknownScopeEnumerator = builder.unknownScopes.GetEnumerator();
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
                    this.current = Builder.GetNameFromIndex(++this.index, this.builder)!;
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
            { }

            public void Reset()
            {
                this.index = -1;
                this.current = null!;
                this.unknownScopeEnumerator.Reset();
            }
        }
    }
}
