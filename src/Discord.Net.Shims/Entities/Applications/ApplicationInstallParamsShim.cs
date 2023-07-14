using System;
using System.Collections.Generic;
using System.Linq;

namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="ApplicationInstallParams"/>
    /// </summary>
    public class ApplicationInstallParamsShim : IConvertibleShim<ApplicationInstallParams>
    {
        private readonly List<string> m_scopes;
        private GuildPermission? m_permission;

        public ApplicationInstallParamsShim()
        {
            this.m_scopes = new();
            this.m_permission = null;
        }

        public ApplicationInstallParamsShim(ApplicationInstallParams applicationInstallParams) : this()
        {
            Preconditions.NotNull(applicationInstallParams, nameof(applicationInstallParams));

            this.Apply(applicationInstallParams);
        }

        public ApplicationInstallParams UnShim()
        {
            return new ApplicationInstallParams(this.Scopes.ToArray(), this.Permission);
        }

        public void Apply(ApplicationInstallParams value)
        {
            if (value is null)
            {
                return;
            }

            this.m_permission = value.Permission;
            this.m_scopes.Clear();
            this.m_scopes.AddRange(value.Scopes);
        }

        /// <inheritdoc cref="ApplicationInstallParams.Scopes"/>
        public virtual ICollection<string> Scopes => this.m_scopes;

        /// <inheritdoc cref="ApplicationInstallParams.Permission"/>
        public virtual GuildPermission? Permission
        {
            get => this.m_permission;
            set => this.m_permission = value;
        }

        public static implicit operator ApplicationInstallParams(ApplicationInstallParamsShim v)
        {
            return v.UnShim();
        }
    }
}
