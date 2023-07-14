using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="MessageRoleSubscriptionData"/>
    /// </summary>
    public class MessageRoleSubscriptionDataShim : IConvertibleShim<MessageRoleSubscriptionData>
    {
        private ulong m_id;
        private string m_tierName;
        private int m_monthsSubscribed;
        private bool m_isRenewal;

        public MessageRoleSubscriptionDataShim(string name)
        {
            this.m_tierName = string.Empty;
            this.TierName = name;
        }

        public MessageRoleSubscriptionDataShim(MessageRoleSubscriptionData value)
        {
            this.m_tierName = string.Empty;
            Preconditions.NotNull(value, nameof(value));

            this.Apply(value);
        }

        public MessageRoleSubscriptionData UnShim()
        {
            return new MessageRoleSubscriptionData(
                id: this.Id,
                tierName: this.TierName,
                monthsSubscribed: this.MonthsSubscribed,
                isRenewal: this.IsRenewal);
        }

        public void Apply(MessageRoleSubscriptionData value)
        {
            if (value is null)
            {
                return;
            }

            this.m_isRenewal = value.IsRenewal;
            this.m_id = value.Id;
            this.m_tierName = value.TierName;
            this.m_monthsSubscribed = value.MonthsSubscribed;
        }


        /// <inheritdoc cref="MessageRoleSubscriptionData.Id"/>
        public virtual ulong Id
        {
            get => this.m_id;
            set => this.m_id = value;
        }

        /// <inheritdoc cref="MessageRoleSubscriptionData.TierName"/>
        public virtual string TierName
        {
            get => this.m_tierName;
            set
            {
                Preconditions.NotNull(value, nameof(value));
                this.m_tierName = value;
            }
        }

        /// <inheritdoc cref="MessageRoleSubscriptionData.MonthsSubscribed"/>
        public virtual int MonthsSubscribed
        {
            get => this.m_monthsSubscribed;
            set => this.m_monthsSubscribed = value;
        }

        /// <inheritdoc cref="MessageRoleSubscriptionData.IsRenewal"/>
        public virtual bool IsRenewal
        {
            get => this.m_isRenewal;
            set => this.m_isRenewal = value;
        }

        public static implicit operator MessageRoleSubscriptionData(MessageRoleSubscriptionDataShim v)
        {
            return v.UnShim();
        }
    }
}
