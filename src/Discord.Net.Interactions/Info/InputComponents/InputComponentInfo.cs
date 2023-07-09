using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;

namespace Discord.Interactions
{
    /// <summary>
    ///     Represents the base info class for <see cref="IModal"/> input components.
    /// </summary>
    public abstract class InputComponentInfo
    {
        private Lazy<Func<object, object>> _getter;
        internal Func<object, object> Getter => _getter.Value;


        /// <summary>
        ///     Gets the parent modal of this component.
        /// </summary>
        public ModalInfo Modal { get; }

        /// <summary>
        ///     Gets the custom id of this component.
        /// </summary>
        public string CustomId { get; }

        /// <summary>
        ///     Gets the label of this component.
        /// </summary>
        public string Label { get; }

        /// <summary>
        ///     Gets whether or not this component requires a user input.
        /// </summary>
        public bool IsRequired { get; }

        /// <summary>
        ///     Gets the type of this component.
        /// </summary>
        public ComponentType ComponentType { get; }

        /// <summary>
        ///     Gets the reference type of this component.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        ///     Gets the property linked to this component.
        /// </summary>
        public PropertyInfo PropertyInfo { get; }

        /// <summary>
        ///     Gets the <see cref="ComponentTypeConverter"/> assigned to this component.
        /// </summary>
        public ComponentTypeConverter TypeConverter { get; }

        /// <summary>
        ///     Gets the default value of this component.
        /// </summary>
        public object? DefaultValue { get; }

        /// <summary>
        ///     Gets a collection of the attributes of this command.
        /// </summary>
        public IReadOnlyCollection<Attribute> Attributes { get; }

        protected InputComponentInfo(Builders.IInputComponentBuilder builder, ModalInfo modal)
        {
            Modal = modal;
            CustomId = builder.CustomId ?? throw new ArgumentException("Builder's CustomId property was null", nameof(builder));
            Label = builder.Label ?? throw new ArgumentException("Builder's Label property was null", nameof(builder));
            IsRequired = builder.IsRequired;
            ComponentType = builder.ComponentType;
            Type = builder.Type ?? throw new ArgumentException("Builder's Type property was null", nameof(builder));
            PropertyInfo = builder.PropertyInfo ?? throw new ArgumentException("Builder's PropertyInfo property was null", nameof(builder));
            TypeConverter = builder.TypeConverter ?? throw new ArgumentException("Builder's TypeConverter property was null", nameof(builder));
            DefaultValue = builder.DefaultValue;
            Attributes = builder.Attributes.ToImmutableArray();

            _getter = new(() => ReflectionUtils<object>.CreateLambdaPropertyGetter(Modal.Type, PropertyInfo));
        }
    }
}
