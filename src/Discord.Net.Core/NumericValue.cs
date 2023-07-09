using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if NET7_0_OR_GREATER
using System.Numerics;
#endif

namespace Discord
{
    /// <summary>
    /// Represents a numeric value. Can be either a
    /// <see cref="long"/>, or a <see cref="double"/>.
    /// </summary>
    public readonly struct NumericValue : IEquatable<NumericValue>, IEquatable<byte>, IEquatable<sbyte>, IEquatable<short>, IEquatable<ushort>, IEquatable<int>, IEquatable<uint>, IEquatable<long>, IEquatable<ulong>, IEquatable<float>, IEquatable<double>, IComparable<NumericValue>, IComparable<byte>, IComparable<sbyte>, IComparable<short>, IComparable<ushort>, IComparable<int>, IComparable<uint>, IComparable<long>, IComparable<ulong>, IConvertible
#if NET7_0_OR_GREATER
        , INumber<NumericValue>
#endif
    {
        private readonly long m_integerValue;
        private readonly double m_doubleValue;
        private readonly bool? m_isDouble;

        public NumericValue(long value)
        {
            this.m_integerValue = value;
            this.m_doubleValue = value;
            this.m_isDouble = false;
        }
        public NumericValue(double value)
        {
            this.m_integerValue = (long)value;
            this.m_doubleValue = value;
            this.m_isDouble = true;
        }
        internal NumericValue(long integer, double fractional)
        {
            this.m_integerValue = integer;
            this.m_doubleValue = fractional;
            this.m_isDouble = null;
        }
        private NumericValue(long integer, double fractional, bool? isDouble)
        {
            this.m_integerValue = integer;
            this.m_doubleValue = fractional;
            this.m_isDouble = isDouble;
        }

        internal double UnderlyingDouble => this.m_doubleValue;
        internal long UnderlyingInteger => this.m_integerValue;

        public bool IsDouble => this.m_isDouble ?? true;
        public bool IsInteger => !(this.m_isDouble ?? false);
        public bool IsIntegerAndDouble => !this.m_isDouble.HasValue;

        private static readonly NumericValue One = new(1L);
        private static readonly NumericValue Zero = new(0L);

        private static bool? GetIsDoubleValue(NumericValue x, NumericValue y)
        {
            if (!x.m_isDouble.HasValue)
            {
                return y.m_isDouble;
            }
            else if (!y.m_isDouble.HasValue)
            {
                return x.m_isDouble;
            }
            else if (x.m_isDouble.Value != y.m_isDouble.Value)
            {
                return null;
            }
            else
            {
                return x.m_isDouble.Value;
            }
        }
        private static NumericValue PreformOperation(NumericValue value, Func<long, long> integerOperation, Func<double, double> fractionalOperation)
        {
            if (!value.m_isDouble.HasValue)
            {
                return new NumericValue(integerOperation(value.m_integerValue), fractionalOperation(value.m_doubleValue));
            }
            else if (value.m_isDouble.Value)
            {
                return new NumericValue(fractionalOperation(value.m_doubleValue));
            }
            else
            {
                return new NumericValue(integerOperation(value.m_integerValue));
            }
        }
        private static NumericValue PreformOperation(NumericValue x, NumericValue y, Func<long, long, long> integerOperation, Func<double, double, double> fractionalOperation)
        {
            bool? isDouble = NumericValue.GetIsDoubleValue(x, y);
            if (!isDouble.HasValue)
            {
                return new NumericValue(integerOperation(x.m_integerValue, y.m_integerValue), fractionalOperation(x.m_doubleValue, y.m_doubleValue));
            }
            else if (isDouble.Value)
            {
                return new NumericValue(fractionalOperation(x.m_doubleValue, y.m_doubleValue));
            }
            else
            {
                return new NumericValue(integerOperation(x.m_integerValue, y.m_integerValue));
            }
        }
        private static bool TryConvertFromCast([NotNullWhen(true)] object? value, out NumericValue result)
        {
            if (value is null)
            {
                result = default;
                return false;
            }

            Type type = value.GetType();
            if (type == typeof(double))
            {
                double actualValue = (double)(object)value;
                result = new NumericValue(actualValue);
                return true;
            }
            else if (type == typeof(float))
            {
                float actualValue = (float)(object)value;
                result = new NumericValue(
                    float.IsPositiveInfinity(actualValue) ? double.PositiveInfinity :
                    float.IsNegativeInfinity(actualValue) ? double.NegativeInfinity :
                    (double)actualValue);
                return true;
            }
            else if (type == typeof(int))
            {
                int actualValue = (int)(object)value;
                result = new NumericValue(actualValue);
                return true;
            }
            else if (type == typeof(long))
            {
                long actualValue = (long)(object)value;
                result = new NumericValue(actualValue);
                return true;
            }
            else if (type == typeof(ulong))
            {
                ulong actualValue = (ulong)(object)value;
                result = new NumericValue(
                    (actualValue >= long.MaxValue) ? long.MaxValue :
                    (actualValue <= 0UL) ? long.MinValue :
                    (long)actualValue);
                return true;
            }
            else if (type == typeof(ushort))
            {
                ushort actualValue = (ushort)(object)value;
                result = new NumericValue(actualValue);
                return true;
            }
            else if (type == typeof(short))
            {
                short actualValue = (short)(object)value;
                result = new NumericValue(actualValue);
                return true;
            }
            else if (type == typeof(uint))
            {
                uint actualValue = (uint)(object)value;
                result = new NumericValue(actualValue);
                return true;
            }
            else if (type == typeof(sbyte))
            {
                sbyte actualValue = (sbyte)(object)value;
                result = new NumericValue(actualValue);
                return true;
            }
            else if (type == typeof(byte))
            {
                byte actualValue = (byte)(object)value;
                result = new NumericValue(actualValue);
                return true;
            }
#if NET5_0_OR_GREATER
            else if (type == typeof(Half))
            {
                Half actualValue = (Half)(object)value;
                result = new NumericValue((double)actualValue);
                return true;
            }
#endif
#if NET7_0_OR_GREATER
            else if (type == typeof(Int128))
            {
                Int128 actualValue = (Int128)(object)value;
                result = new NumericValue((actualValue >= long.MaxValue) ? long.MaxValue :
                    (actualValue <= 0UL) ? long.MinValue :
                    (long)actualValue);
                return true;
            }
#endif
            else
            {
                result = default;
                return false;
            }
        }

        private static bool TryConvertToCast(NumericValue value, Type type, [NotNullWhen(true)] out object? result)
        {
            if (type == typeof(double))
            {
                result = (double)value;
                return true;
            }
            else if (type == typeof(float))
            {
                double underlyingValue = (double)value;
                float actualResult = (double.IsPositiveInfinity(underlyingValue) || underlyingValue > float.MaxValue) ? float.PositiveInfinity :
                    (double.IsNegativeInfinity(underlyingValue) || underlyingValue < -float.MaxValue) ? float.NegativeInfinity :
                    (underlyingValue > 0 && underlyingValue < float.MinValue) ? float.MinValue :
                    (underlyingValue < 0 && underlyingValue > -float.MinValue) ? float.MinValue :
                    (float)underlyingValue;
                result = actualResult;
                return true;
            }
            else if (type == typeof(int))
            {
                long underlyingValue = (long)value;
                int actualResult = (value < int.MinValue) ? int.MinValue :
                    (value > int.MaxValue) ? int.MaxValue :
                    (int)underlyingValue;
                result = actualResult;
                return true;
            }
            else if (type == typeof(long))
            {
                result = (long)value;
                return true;
            }
            else if (type == typeof(ulong))
            {
                long underlyingValue = (long)value;
                ulong actualResult = underlyingValue < 0 ? ulong.MinValue : (ulong)underlyingValue;
                result = actualResult;
                return true;
            }
            else if (type == typeof(ushort))
            {
                long underlyingValue = (long)value;
                ushort actualResult = (value < ushort.MinValue) ? ushort.MinValue :
                    (value > ushort.MaxValue) ? ushort.MaxValue :
                    (ushort)underlyingValue;
                result = actualResult;
                return true;
            }
            else if (type == typeof(short))
            {
                long underlyingValue = (long)value;
                short actualResult = (value < short.MinValue) ? short.MinValue :
                    (value > short.MaxValue) ? short.MaxValue :
                    (short)underlyingValue;
                result = actualResult;
                return true;
            }
            else if (type == typeof(uint))
            {
                long underlyingValue = (long)value;
                uint actualResult = (value < uint.MinValue) ? uint.MinValue :
                    (value > uint.MaxValue) ? uint.MaxValue :
                    (uint)underlyingValue;
                result = actualResult;
                return true;
            }
            else if (type == typeof(sbyte))
            {
                long underlyingValue = (long)value;
                sbyte actualResult = (value < sbyte.MinValue) ? sbyte.MinValue :
                    (value > sbyte.MaxValue) ? sbyte.MaxValue :
                    (sbyte)underlyingValue;
                result = actualResult;
                return true;
            }
            else if (type == typeof(byte))
            {
                long underlyingValue = (long)value;
                byte actualResult = (value < byte.MinValue) ? byte.MinValue :
                    (value > byte.MaxValue) ? byte.MaxValue :
                    (byte)underlyingValue;
                result = actualResult;
                return true;
            }
#if NET5_0_OR_GREATER
            else if (type == typeof(Half))
            {
                double underlyingValue = (double)value;
                Half actualResult = (double.IsPositiveInfinity(underlyingValue) || underlyingValue > float.MaxValue) ? Half.PositiveInfinity :
                    (double.IsNegativeInfinity(underlyingValue) || underlyingValue < -float.MaxValue) ? Half.NegativeInfinity :
                    (underlyingValue > 0 && underlyingValue < (double)Half.MinValue) ? Half.MinValue :
                    (underlyingValue < 0 && underlyingValue > -(double)Half.MinValue) ? Half.MinValue :
                    (Half)underlyingValue;
                result = actualResult;
                return true;
            }
#endif
#if NET7_0_OR_GREATER
            else if (type == typeof(Int128))
            {
                result = (Int128)(long)value;
                return true;
            }
#endif
            else
            {
                result = null;
                return false;
            }
        }

#if NET7_0_OR_GREATER

        private static bool TryConvertFromChecked<TOther>(TOther value, out NumericValue result)
            where TOther : INumberBase<TOther>
        {
            if (typeof(TOther) == typeof(double))
            {
                double actualValue = (double)(object)value;
                result = new NumericValue(actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(float))
            {
                float actualValue = (float)(object)value;
                result = new NumericValue((double)actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(int))
            {
                int actualValue = (int)(object)value;
                result = new NumericValue((long)actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(long))
            {
                long actualValue = (long)(object)value;
                result = new NumericValue(actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(ulong))
            {
                ulong actualValue = (ulong)(object)value;
                result = new NumericValue(checked((long)actualValue));
                return true;
            }
            else if (typeof(TOther) == typeof(ushort))
            {
                ushort actualValue = (ushort)(object)value;
                result = new NumericValue(actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(short))
            {
                short actualValue = (short)(object)value;
                result = new NumericValue(actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(uint))
            {
                uint actualValue = (uint)(object)value;
                result = new NumericValue(actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(sbyte))
            {
                sbyte actualValue = (sbyte)(object)value;
                result = new NumericValue(actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(byte))
            {
                byte actualValue = (byte)(object)value;
                result = new NumericValue(actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(Half))
            {
                Half actualValue = (Half)(object)value;
                result = new NumericValue((double)actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(Int128))
            {
                Int128 actualValue = (Int128)(object)value;
                result = new NumericValue(checked((long)actualValue));
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        private static bool TryConvertToChecked<TOther>(NumericValue value, out TOther result)
            where TOther : INumberBase<TOther>
        {
            if (typeof(TOther) == typeof(double))
            {
                double actualResult = checked((double)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(float))
            {
                float actualResult = checked((float)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(int))
            {
                int actualResult = checked((int)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(long))
            {
                long actualResult = checked((long)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(ulong))
            {
                ulong actualResult = checked((ulong)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(ushort))
            {
                ushort actualResult = checked((ushort)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(short))
            {
                short actualResult = checked((short)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(uint))
            {
                uint actualResult = checked((uint)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(sbyte))
            {
                sbyte actualResult = checked((sbyte)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(byte))
            {
                byte actualResult = checked((byte)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(Half))
            {
                Half actualResult = checked((Half)(double)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(Int128))
            {
                Int128 actualResult = checked((Int128)(long)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else
            {
                result = TOther.Zero;
                return false;
            }
        }

        private static bool TryConvertFrom<TOther>(TOther value, out NumericValue result)
            where TOther : INumberBase<TOther>
        {
            if (typeof(TOther) == typeof(double))
            {
                double actualValue = (double)(object)value;
                result = new NumericValue(actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(float))
            {
                float actualValue = (float)(object)value;
                result = new NumericValue(
                    float.IsPositiveInfinity(actualValue) ? double.PositiveInfinity :
                    float.IsNegativeInfinity(actualValue) ? double.NegativeInfinity :
                    (double)actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(int))
            {
                int actualValue = (int)(object)value;
                result = new NumericValue(actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(long))
            {
                long actualValue = (long)(object)value;
                result = new NumericValue(actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(ulong))
            {
                ulong actualValue = (ulong)(object)value;
                result = new NumericValue(
                    (actualValue >= long.MaxValue) ? long.MaxValue :
                    (actualValue <= 0UL) ? long.MinValue :
                    (long)actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(ushort))
            {
                ushort actualValue = (ushort)(object)value;
                result = new NumericValue(actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(short))
            {
                short actualValue = (short)(object)value;
                result = new NumericValue(actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(uint))
            {
                uint actualValue = (uint)(object)value;
                result = new NumericValue(actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(sbyte))
            {
                sbyte actualValue = (sbyte)(object)value;
                result = new NumericValue(actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(byte))
            {
                byte actualValue = (byte)(object)value;
                result = new NumericValue(actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(Half))
            {
                Half actualValue = (Half)(object)value;
                result = new NumericValue((double)actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(Int128))
            {
                Int128 actualValue = (Int128)(object)value;
                result = new NumericValue((actualValue >= long.MaxValue) ? long.MaxValue :
                    (actualValue <= 0UL) ? long.MinValue :
                    (long)actualValue);
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        private static bool TryConvertTo<TOther>(NumericValue value, out TOther result)
            where TOther : INumberBase<TOther>
        {
            if (typeof(TOther) == typeof(double))
            {
                double actualResult = (double)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(float))
            {
                double underlyingValue = (double)value;
                float actualResult = (double.IsPositiveInfinity(underlyingValue) || underlyingValue > float.MaxValue) ? float.PositiveInfinity :
                    (double.IsNegativeInfinity(underlyingValue) || underlyingValue < -float.MaxValue) ? float.NegativeInfinity :
                    (underlyingValue > 0 && underlyingValue < float.MinValue) ? float.MinValue :
                    (underlyingValue < 0 && underlyingValue > -float.MinValue) ? float.MinValue :
                    (float)underlyingValue;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(int))
            {
                long underlyingValue = (long)value;
                int actualResult = (value < int.MinValue) ? int.MinValue :
                    (value > int.MaxValue) ? int.MaxValue :
                    (int)underlyingValue;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(long))
            {
                long actualResult = (long)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(ulong))
            {
                long underlyingValue = (long)value;
                ulong actualResult = underlyingValue < 0 ? ulong.MinValue : (ulong)underlyingValue;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(ushort))
            {
                long underlyingValue = (long)value;
                ushort actualResult = (value < ushort.MinValue) ? ushort.MinValue :
                    (value > ushort.MaxValue) ? ushort.MaxValue :
                    (ushort)underlyingValue;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(short))
            {
                long underlyingValue = (long)value;
                short actualResult = (value < short.MinValue) ? short.MinValue :
                    (value > short.MaxValue) ? short.MaxValue :
                    (short)underlyingValue;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(uint))
            {
                long underlyingValue = (long)value;
                uint actualResult = (value < uint.MinValue) ? uint.MinValue :
                    (value > uint.MaxValue) ? uint.MaxValue :
                    (uint)underlyingValue;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(sbyte))
            {
                long underlyingValue = (long)value;
                sbyte actualResult = (value < sbyte.MinValue) ? sbyte.MinValue :
                    (value > sbyte.MaxValue) ? sbyte.MaxValue :
                    (sbyte)underlyingValue;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(byte))
            {
                long underlyingValue = (long)value;
                byte actualResult = (value < byte.MinValue) ? byte.MinValue :
                    (value > byte.MaxValue) ? byte.MaxValue :
                    (byte)underlyingValue;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(Half))
            {
                double underlyingValue = (double)value;
                Half actualResult = (double.IsPositiveInfinity(underlyingValue) || underlyingValue > float.MaxValue) ? Half.PositiveInfinity :
                    (double.IsNegativeInfinity(underlyingValue) || underlyingValue < -float.MaxValue) ? Half.NegativeInfinity :
                    (underlyingValue > 0 && underlyingValue < (double)Half.MinValue) ? Half.MinValue :
                    (underlyingValue < 0 && underlyingValue > -(double)Half.MinValue) ? Half.MinValue :
                    (Half)underlyingValue;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(Int128))
            {
                Int128 actualResult = (Int128)(long)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else
            {
                result = TOther.Zero;
                return false;
            }
        }

        static bool INumberBase<NumericValue>.TryConvertFromChecked<TOther>(TOther value, out NumericValue result)
        {
            return NumericValue.TryConvertFromChecked<TOther>(value, out result);
        }
        static bool INumberBase<NumericValue>.TryConvertFromSaturating<TOther>(TOther value, out NumericValue result)
        {
            return NumericValue.TryConvertFrom<TOther>(value, out result);
        }
        static bool INumberBase<NumericValue>.TryConvertFromTruncating<TOther>(TOther value, out NumericValue result)
        {
            return NumericValue.TryConvertFrom<TOther>(value, out result);
        }
        static bool INumberBase<NumericValue>.TryConvertToChecked<TOther>(NumericValue value, out TOther result)
        {
            return NumericValue.TryConvertToChecked(value, out result);
        }
        static bool INumberBase<NumericValue>.TryConvertToSaturating<TOther>(NumericValue value, out TOther result)
        {
            return NumericValue.TryConvertTo(value, out result);
        }
        static bool INumberBase<NumericValue>.TryConvertToTruncating<TOther>(NumericValue value, out TOther result)
        {
            return NumericValue.TryConvertTo(value, out result);
        }

        static int INumberBase<NumericValue>.Radix => 2;

        static NumericValue IAdditiveIdentity<NumericValue, NumericValue>.AdditiveIdentity => One;

        static NumericValue IMultiplicativeIdentity<NumericValue, NumericValue>.MultiplicativeIdentity => Zero;

        static NumericValue INumberBase<NumericValue>.One => One;
        static NumericValue INumberBase<NumericValue>.Zero => Zero;

        static bool INumberBase<NumericValue>.IsCanonical(NumericValue value) => true;
        static bool INumberBase<NumericValue>.IsComplexNumber(NumericValue value) => false;
        static bool INumberBase<NumericValue>.IsEvenInteger(NumericValue value)
        {
            if (value.IsDouble)
            {
                return double.IsEvenInteger(value.m_doubleValue);
            }
            else
            {
                return long.IsEvenInteger(value.m_integerValue);
            }
        }
        static bool INumberBase<NumericValue>.IsFinite(NumericValue value)
        {
            if (value.IsDouble)
            {
                return double.IsFinite(value.m_doubleValue);
            }
            else
            {
                return true;
            }
        }
        static bool INumberBase<NumericValue>.IsImaginaryNumber(NumericValue value) => false;
        static bool INumberBase<NumericValue>.IsInteger(NumericValue value)
        {
            if (value.IsDouble)
            {
                return double.IsInteger(value.m_doubleValue);
            }
            else
            {
                return true;
            }
        }
        static bool INumberBase<NumericValue>.IsNegative(NumericValue value)
        {
            if (value.IsDouble)
            {
                return double.IsNegative(value.m_doubleValue);
            }
            else
            {
                return long.IsNegative(value.m_integerValue);
            }
        }
        static bool INumberBase<NumericValue>.IsNormal(NumericValue value)
        {
            if (value.IsDouble && !double.IsNormal(value.m_doubleValue))
            {
                return false;
            }
            if (value.IsInteger && value.m_integerValue == 0)
            {
                return false;
            }
            return true;
        }
        static bool INumberBase<NumericValue>.IsOddInteger(NumericValue value)
        {
            if (value.IsDouble)
            {
                return double.IsOddInteger(value.m_doubleValue);
            }
            else
            {
                return long.IsOddInteger(value.m_integerValue);
            }
        }
        static bool INumberBase<NumericValue>.IsPositive(NumericValue value)
        {
            if (value.IsDouble)
            {
                return double.IsPositive(value.m_doubleValue);
            }
            else
            {
                return long.IsPositive(value.m_integerValue);
            }
        }
        static bool INumberBase<NumericValue>.IsRealNumber(NumericValue value) => true;
        static bool INumberBase<NumericValue>.IsSubnormal(NumericValue value)
        {
            return value.IsDouble && double.IsSubnormal(value.m_doubleValue);
        }
        static bool INumberBase<NumericValue>.IsZero(NumericValue value)
        {
            if (value.IsDouble && value.m_doubleValue != 0)
            {
                return false;
            }
            if (value.IsInteger && value.m_integerValue != 0)
            {
                return false;
            }
            return true;
        }
        static NumericValue INumberBase<NumericValue>.MaxMagnitude(NumericValue x, NumericValue y)
        {
            return NumericValue.PreformOperation(x, y, long.MaxMagnitude, double.MaxMagnitude);
        }
        static NumericValue INumberBase<NumericValue>.MaxMagnitudeNumber(NumericValue x, NumericValue y)
        {
            return NumericValue.PreformOperation(x, y, long.MaxMagnitude, double.MaxMagnitudeNumber);
        }
        static NumericValue INumberBase<NumericValue>.MinMagnitude(NumericValue x, NumericValue y)
        {
            return NumericValue.PreformOperation(x, y, long.MinMagnitude, double.MinMagnitude);
        }
        static NumericValue INumberBase<NumericValue>.MinMagnitudeNumber(NumericValue x, NumericValue y)
        {
            return NumericValue.PreformOperation(x, y, long.MinMagnitude, double.MinMagnitudeNumber);
        }
#endif

        public static NumericValue Abs(NumericValue value)
        {
            return new NumericValue(Math.Abs(value.m_integerValue), Math.Abs(value.m_doubleValue), value.m_isDouble);
        }
        public static bool IsInfinity(NumericValue value)
        {
            return !(value.IsInteger || !double.IsInfinity(value.m_doubleValue));
        }
        public static bool IsNaN(NumericValue value)
        {
            return !(value.IsInteger || !double.IsNaN(value.m_doubleValue));
        }
        public static bool IsNegativeInfinity(NumericValue value)
        {
            return !(value.IsInteger || !double.IsNegativeInfinity(value.m_doubleValue));
        }
        public static bool IsPositiveInfinity(NumericValue value)
        {
            return !(value.IsInteger || !double.IsPositiveInfinity(value.m_doubleValue));
        }

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        public static NumericValue Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
        {
            Exception? firstException = null;

            long longVal = 0;
            double doubleVal;

            try
            {
                longVal = long.Parse(s, style, provider);
            }
            catch (Exception ex1)
            {
                firstException = ex1;
            }

            try
            {
                doubleVal = double.Parse(s, style, provider);
            }
            catch (Exception ex2)
            {
                if (firstException == null)
                {
                    return new NumericValue(longVal);
                }
                else
                {
                    throw new AggregateException(firstException, ex2);
                }
            }

            if (firstException == null)
            {
                return new NumericValue(longVal, doubleVal);
            }
            else
            {
                return new NumericValue(doubleVal);
            }
        }
        public static NumericValue Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        {
            Exception? firstException = null;

            long longVal = 0;
            double doubleVal;

            try
            {
                longVal = long.Parse(s, provider: provider);
            }
            catch (Exception ex1)
            {
                firstException = ex1;
            }

            try
            {
                doubleVal = double.Parse(s, provider: provider);
            }
            catch (Exception ex2)
            {
                if (firstException == null)
                {
                    return new NumericValue(longVal);
                }
                else
                {
                    throw new AggregateException(firstException, ex2);
                }
            }

            if (firstException == null)
            {
                return new NumericValue(longVal, doubleVal);
            }
            else
            {
                return new NumericValue(doubleVal);
            }
        }
        public static NumericValue Parse(ReadOnlySpan<char> s)
        {
            return NumericValue.Parse(s, provider: null);
        }
        public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out NumericValue result)
        {
            bool parsedInt = long.TryParse(s, style, provider, out long intVal);
            bool parsedDouble = double.TryParse(s, style, provider, out double doubleVal);

            if (parsedInt)
            {
                if (parsedDouble)
                {
                    result = new NumericValue(intVal, doubleVal);
                }
                else
                {
                    result = new NumericValue(intVal);
                }
                return true;
            }
            else if (parsedDouble)
            {
                result = new NumericValue(doubleVal);
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }
        public static bool TryParse(ReadOnlySpan<char> s, out NumericValue result)
        {
#if NET7_0_OR_GREATER
            return NumericValue.TryParse(s, null, out result);
#else
            bool parsedInt = long.TryParse(s, out long intVal);
            bool parsedDouble = double.TryParse(s, out double doubleVal);

            if (parsedInt)
            {
                if (parsedDouble)
                {
                    result = new NumericValue(intVal, doubleVal);
                }
                else
                {
                    result = new NumericValue(intVal);
                }
                return true;
            }
            else if (parsedDouble)
            {
                result = new NumericValue(doubleVal);
                return true;
            }
            else
            {
                result = default;
                return false;
            }
#endif
        }
#endif

            public static NumericValue Parse(string s, NumberStyles style, IFormatProvider? provider)
        {
            Exception? firstException = null;

            long longVal = 0;
            double doubleVal;

            try
            {
                longVal = long.Parse(s, style, provider);
            }
            catch (Exception ex1)
            {
                firstException = ex1;
            }

            try
            {
                doubleVal = double.Parse(s, style, provider);
            }
            catch (Exception ex2)
            {
                if (firstException == null)
                {
                    return new NumericValue(longVal);
                }
                else
                {
                    throw new AggregateException(firstException, ex2);
                }
            }

            if (firstException == null)
            {
                return new NumericValue(longVal, doubleVal);
            }
            else
            {
                return new NumericValue(doubleVal);
            }
        }
        public static NumericValue Parse(string s, IFormatProvider? provider)
        {
            Exception? firstException = null;

            long longVal = 0;
            double doubleVal;

            try
            {
                longVal = long.Parse(s, provider);
            }
            catch (Exception ex1)
            {
                firstException = ex1;
            }

            try
            {
                doubleVal = double.Parse(s, provider);
            }
            catch (Exception ex2)
            {
                if (firstException == null)
                {
                    return new NumericValue(longVal);
                }
                else
                {
                    throw new AggregateException(firstException, ex2);
                }
            }

            if (firstException == null)
            {
                return new NumericValue(longVal, doubleVal);
            }
            else
            {
                return new NumericValue(doubleVal);
            }
        }
        public static NumericValue Parse(string s)
        {
            return NumericValue.Parse(s, provider: null);
        }

        public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out NumericValue result)
        {
            bool parsedInt = long.TryParse(s, style, provider, out long intVal);
            bool parsedDouble = double.TryParse(s, style, provider, out double doubleVal);

            if (parsedInt)
            {
                if (parsedDouble)
                {
                    result = new NumericValue(intVal, doubleVal);
                }
                else
                {
                    result = new NumericValue(intVal);
                }
                return true;
            }
            else if (parsedDouble)
            {
                result = new NumericValue(doubleVal);
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }
#if NET7_0_OR_GREATER
        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out NumericValue result)
        {
            bool parsedInt = long.TryParse(s, provider, out long intVal);
            bool parsedDouble = double.TryParse(s, provider, out double doubleVal);

            if (parsedInt)
            {
                if (parsedDouble)
                {
                    result = new NumericValue(intVal, doubleVal);
                }
                else
                {
                    result = new NumericValue(intVal);
                }
                return true;
            }
            else if (parsedDouble)
            {
                result = new NumericValue(doubleVal);
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }
        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out NumericValue result)
        {
            bool parsedInt = long.TryParse(s, provider, out long intVal);
            bool parsedDouble = double.TryParse(s, provider, out double doubleVal);

            if (parsedInt)
            {
                if (parsedDouble)
                {
                    result = new NumericValue(intVal, doubleVal);
                }
                else
                {
                    result = new NumericValue(intVal);
                }
                return true;
            }
            else if (parsedDouble)
            {
                result = new NumericValue(doubleVal);
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }
#endif
        public static bool TryParse([NotNullWhen(true)] string? s, out NumericValue result)
        {
#if NET7_0_OR_GREATER
            return NumericValue.TryParse(s, null, out result);
#else
            bool parsedInt = long.TryParse(s, out long intVal);
            bool parsedDouble = double.TryParse(s, out double doubleVal);

            if (parsedInt)
            {
                if (parsedDouble)
                {
                    result = new NumericValue(intVal, doubleVal);
                }
                else
                {
                    result = new NumericValue(intVal);
                }
                return true;
            }
            else if (parsedDouble)
            {
                result = new NumericValue(doubleVal);
                return true;
            }
            else
            {
                result = default;
                return false;
            }
#endif
        }

        public int CompareTo(object? obj)
        {
            if (obj is null)
            {
                return 1;
            }

            if (obj is NumericValue numericValue)
            {
                return this.CompareTo(numericValue);
            }
            if (obj is float otherFloat)
            {
                return this.CompareTo(otherFloat);
            }
            if (obj is double otherDouble)
            {
                return this.CompareTo(otherDouble);
            }
            if (obj is byte otherByte)
            {
                return this.CompareTo(otherByte);
            }
            if (obj is sbyte otherSbyte)
            {
                return this.CompareTo(otherSbyte);
            }
            if (obj is short otherShort)
            {
                return this.CompareTo(otherShort);
            }
            if (obj is ushort otherUShort)
            {
                return this.CompareTo(otherUShort);
            }
            if (obj is int otherInt)
            {
                return this.CompareTo(otherInt);
            }
            if (obj is uint otherUInt)
            {
                return this.CompareTo(otherUInt);
            }
            if (obj is long otherLong)
            {
                return this.CompareTo(otherLong);
            }
            if (obj is ulong otherULong)
            {
                return this.CompareTo(otherULong);
            }

            throw new ArgumentException("Argument must be of type NumericValue, Single, Double, Byte, SByte, Int16, Int32, Int64, UInt16, UInt32, or UInt64");
        }

        public int CompareTo(float other) => this.CompareTo((double)other);
        public int CompareTo(float other, bool allowCast) => this.CompareTo((double)other, allowCast);

        public int CompareTo(double other) => this.CompareTo(other, allowCast: true);
        public int CompareTo(double other, bool allowCast)
        {
            double value;
            if (this.IsDouble)
            {
                value = this.m_doubleValue;
            }
            else if (allowCast)
            {
                value = this.m_integerValue;
            }
            else
            {
                return -1;
            }

            return value.CompareTo(other);
        }
        public int CompareTo(byte other) => this.CompareTo((long)other);
        public int CompareTo(byte other, bool allowCast) => this.CompareTo((long)other, allowCast);

        public int CompareTo(sbyte other) => this.CompareTo((long)other);
        public int CompareTo(sbyte other, bool allowCast) => this.CompareTo((long)other, allowCast);

        public int CompareTo(short other) => this.CompareTo((long)other);
        public int CompareTo(short other, bool allowCast) => this.CompareTo((long)other, allowCast);

        public int CompareTo(ushort other) => this.CompareTo((long)other);
        public int CompareTo(ushort other, bool allowCast) => this.CompareTo((long)other, allowCast);

        public int CompareTo(int other) => this.CompareTo((long)other);
        public int CompareTo(int other, bool allowCast) => this.CompareTo((long)other, allowCast);

        public int CompareTo(uint other) => this.CompareTo((long)other);
        public int CompareTo(uint other, bool allowCast) => this.CompareTo((long)other, allowCast);

        public int CompareTo(long other) => this.CompareTo(other, allowCast: Math.Round(this.m_doubleValue) == this.m_doubleValue);
        public int CompareTo(long other, bool allowCast)
        {
            long value;
            if (this.IsInteger)
            {
                value = this.m_integerValue;
            }
            else if (allowCast)
            {
                value = (long)this.m_doubleValue;
            }
            else
            {
                return -1;
            }

            return value.CompareTo(other);
        }
        public int CompareTo(ulong other) => this.CompareTo(other, allowCast: Math.Round(this.m_doubleValue) == this.m_doubleValue);
        public int CompareTo(ulong other, bool allowCast)
        {
            long value;
            if (this.IsInteger)
            {
                value = this.m_integerValue;
            }
            else if (allowCast)
            {
                value = (long)this.m_doubleValue;
            }
            else
            {
                return -1;
            }

            if (value < 0)
            {
                return -1;
            }

            return ((ulong)value).CompareTo(other);
        }

        public int CompareTo(NumericValue other) => this.CompareTo(other, allowCast: true);
        public int CompareTo(NumericValue other, bool allowCast)
        {
            int result = 0;
            bool casted = false;
            if (this.IsInteger)
            {
                if (other.IsInteger)
                {
                    result = this.CompareTo(other.m_integerValue);
                }
                else if (allowCast)
                {
                    result = this.CompareTo((long)other.m_doubleValue);
                    casted = true;
                }
                else
                {
                    throw new ArgumentException("This value is an integer, but other isn't", nameof(other));
                }
            }
            if (this.IsDouble && (result == 0 || casted))
            {
                if (other.IsDouble)
                {
                    result = this.CompareTo(other.m_doubleValue);
                }
                else if (allowCast)
                {
                    result = this.CompareTo((double)other.m_integerValue);
                }
                else
                {
                    throw new ArgumentException("This value is a double, but other isn't", nameof(other));
                }
            }
            return result;
        }
        public bool Equals(float other) => this.Equals((double)other);
        public bool Equals(float other, bool allowCast) => this.Equals((double)other, allowCast);

        public override int GetHashCode()
        {
            if (this.IsIntegerAndDouble)
            {
                return HashCode.Combine(this.m_integerValue, this.m_doubleValue);
            }
            else if (this.IsInteger)
            {
                return this.m_integerValue.GetHashCode();
            }
            else
            {
                return this.m_doubleValue.GetHashCode();
            }
        }

        public bool Equals(double other) => this.Equals(other, allowCast: true);
        public bool Equals(double other, bool allowCast)
        {
            double value;
            if (this.IsDouble)
            {
                value = this.m_doubleValue;
            }
            else if (allowCast)
            {
                value = this.m_integerValue;
            }
            else
            {
                return false;
            }

            return value.Equals(other);
        }

        public bool Equals(byte other) => this.Equals((long)other);
        public bool Equals(byte other, bool allowCast) => this.Equals((long)other, allowCast);

        public bool Equals(sbyte other) => this.Equals((long)other);
        public bool Equals(sbyte other, bool allowCast) => this.Equals((long)other, allowCast);

        public bool Equals(short other) => this.Equals((long)other);
        public bool Equals(short other, bool allowCast) => this.Equals((long)other, allowCast);

        public bool Equals(ushort other) => this.Equals((long)other);
        public bool Equals(ushort other, bool allowCast) => this.Equals((long)other, allowCast);

        public bool Equals(int other) => this.Equals((long)other);
        public bool Equals(int other, bool allowCast) => this.Equals((long)other, allowCast);

        public bool Equals(uint other) => this.Equals((long)other);
        public bool Equals(uint other, bool allowCast) => this.Equals((long)other, allowCast);

        public bool Equals(long other) => this.Equals(other, allowCast: Math.Round(this.m_doubleValue) == this.m_doubleValue);
        public bool Equals(long other, bool allowCast)
        {
            long value;
            if (this.IsInteger)
            {
                value = this.m_integerValue;
            }
            else if (allowCast)
            {
                value = (long)this.m_doubleValue;
            }
            else
            {
                return false;
            }

            return value.Equals(other);
        }
        public bool Equals(ulong other) => this.Equals(other, allowCast: Math.Round(this.m_doubleValue) == this.m_doubleValue);
        public bool Equals(ulong other, bool allowCast)
        {
            long value;
            if (this.IsInteger)
            {
                value = this.m_integerValue;
            }
            else if (allowCast)
            {
                value = (long)this.m_doubleValue;
            }
            else
            {
                return false;
            }
            if (value < 0)
            {
                return false;
            }

            return ((ulong)value).Equals(other);
        }

        public bool Equals(NumericValue other) => this.Equals(other, allowCast: false);
        public bool Equals(NumericValue other, bool allowCast)
        {
            bool result = false;
            bool hasResult = false;
            if (this.IsInteger)
            {
                hasResult = true;
                if (other.IsInteger)
                {
                    result = this.Equals(other.m_integerValue);
                }
                else if (allowCast)
                {
                    result = this.Equals((long)other.m_doubleValue);
                }
                else
                {
                    hasResult = false;
                }
            }
            if (this.IsDouble && (!result || !hasResult))
            {
                if (other.IsDouble)
                {
                    result = this.Equals(other.m_doubleValue);
                }
                else if (allowCast)
                {
                    result = this.Equals((double)other.m_integerValue);
                }
            }
            return result;
        }
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (obj is NumericValue numericValue)
            {
                return this.Equals(numericValue);
            }
            if (obj is float otherFloat)
            {
                return this.Equals(otherFloat);
            }
            if (obj is double otherDouble)
            {
                return this.Equals(otherDouble);
            }
            if (obj is byte otherByte)
            {
                return this.Equals(otherByte);
            }
            if (obj is sbyte otherSbyte)
            {
                return this.Equals(otherSbyte);
            }
            if (obj is short otherShort)
            {
                return this.Equals(otherShort);
            }
            if (obj is ushort otherUShort)
            {
                return this.Equals(otherUShort);
            }
            if (obj is int otherInt)
            {
                return this.Equals(otherInt);
            }
            if (obj is uint otherUInt)
            {
                return this.Equals(otherUInt);
            }
            if (obj is long otherLong)
            {
                return this.Equals(otherLong);
            }
            if (obj is ulong otherULong)
            {
                return this.Equals(otherULong);
            }
            return false;
        }

        public override string ToString()
        {
            if (this.IsInteger)
            {
                return this.m_integerValue.ToString();
            }
            else
            {
                return this.m_doubleValue.ToString();
            }
        }

        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            if (this.IsInteger)
            {
                return this.m_integerValue.ToString(format, formatProvider);
            }
            else
            {
                return this.m_doubleValue.ToString(format, formatProvider);
            }
        }
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
        {
            if (this.IsDouble)
            {
                return this.m_doubleValue.TryFormat(destination, out charsWritten, format, provider);
            }
            else
            {
                return this.m_integerValue.TryFormat(destination, out charsWritten, format, provider);
            }
        }
#endif

        bool IConvertible.ToBoolean(IFormatProvider? provider)
        {
            if (this.IsInteger)
            {
                return System.Convert.ToBoolean(this.m_integerValue);
            }
            else
            {
                return System.Convert.ToBoolean(this.m_doubleValue);
            }
        }

        byte IConvertible.ToByte(IFormatProvider? provider)
        {
            return (byte)this;
        }

        sbyte IConvertible.ToSByte(IFormatProvider? provider)
        {
            return (sbyte)this;
        }

        char IConvertible.ToChar(IFormatProvider? provider)
        {
            if (this.IsInteger)
            {
                return System.Convert.ToChar(this.m_integerValue);
            }
            else if (Math.Round(this.m_doubleValue) == this.m_doubleValue)
            {
                return System.Convert.ToChar((long)this.m_doubleValue);
            }
            else
            {
                throw new InvalidCastException("Cannot convert decimal numeric value to char.");
            }
        }

        DateTime IConvertible.ToDateTime(IFormatProvider? provider)
        {
            throw new InvalidCastException("Cannot convert NumericValue to DateTime");
        }

        decimal IConvertible.ToDecimal(IFormatProvider? provider)
        {
            if (this.IsInteger)
            {
                return System.Convert.ToDecimal(this.m_integerValue);
            }
            else
            {
                return System.Convert.ToDecimal(this.m_doubleValue);
            }
        }

        double IConvertible.ToDouble(IFormatProvider? provider)
        {
            return (double)this;
        }

        float IConvertible.ToSingle(IFormatProvider? provider)
        {
            return (float)this;
        }

        short IConvertible.ToInt16(IFormatProvider? provider)
        {
            return (short)this;
        }

        ushort IConvertible.ToUInt16(IFormatProvider? provider)
        {
            return (ushort)this;
        }

        int IConvertible.ToInt32(IFormatProvider? provider)
        {
            return (int)this;
        }

        uint IConvertible.ToUInt32(IFormatProvider? provider)
        {
            return (uint)this;
        }

        long IConvertible.ToInt64(IFormatProvider? provider)
        {
            return (long)this;
        }

        ulong IConvertible.ToUInt64(IFormatProvider? provider)
        {
            return (ulong)this;
        }

        public string ToString(IFormatProvider? provider)
        {
            if (this.IsInteger)
            {
                return this.m_integerValue.ToString(provider);
            }
            else
            {
                return this.m_doubleValue.ToString(provider);
            }
        }

        TypeCode IConvertible.GetTypeCode()
        {
            return TypeCode.Object;
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider? provider)
        {
            if (!NumericValue.TryConvertToCast(this, conversionType, out object? result))
            {
                throw new InvalidCastException("Cannot convert NumericValue to " + conversionType.FullName);
            }
            return result;
        }


        public static NumericValue operator +(NumericValue value)
        {
            return NumericValue.PreformOperation(value, (v) => +v, (v) => +v);
        }
        public static NumericValue operator +(NumericValue left, NumericValue right)
        {
            return NumericValue.PreformOperation(left, right, (a, b) => a + b, (a, b) => a + b);
        }

        public static NumericValue operator -(NumericValue value)
        {
            return NumericValue.PreformOperation(value, (v) => -v, (v) => -v);
        }
        public static NumericValue operator -(NumericValue left, NumericValue right)
        {
            return NumericValue.PreformOperation(left, right, (a, b) => a - b, (a, b) => a - b);
        }
        public static NumericValue operator ++(NumericValue value)
        {
            return NumericValue.PreformOperation(value, (v) => ++v, (v) => ++v);
        }
        public static NumericValue operator --(NumericValue value)
        {
            return NumericValue.PreformOperation(value, (v) => --v, (v) => --v);
        }
        public static NumericValue operator *(NumericValue left, NumericValue right)
        {
            return NumericValue.PreformOperation(left, right, (a, b) => a * b, (a, b) => a * b);
        }
        public static NumericValue operator /(NumericValue left, NumericValue right)
        {
            return NumericValue.PreformOperation(left, right, (a, b) => a / b, (a, b) => a / b);
        }
        public static NumericValue operator %(NumericValue left, NumericValue right)
        {
            return NumericValue.PreformOperation(left, right, (a, b) => a % b, (a, b) => a % b);
        }
        public static bool operator ==(NumericValue left, NumericValue right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(NumericValue left, NumericValue right)
        {
            return !(left == right);
        }
        public static bool operator <(NumericValue left, NumericValue right)
        {
            return left.CompareTo(right) < 0;
        }
        public static bool operator >(NumericValue left, NumericValue right)
        {
            return left.CompareTo(right) > 0;
        }
        public static bool operator <=(NumericValue left, NumericValue right)
        {
            return left.CompareTo(right) <= 0;
        }
        public static bool operator >=(NumericValue left, NumericValue right)
        {
            return left.CompareTo(right) >= 0;
        }

#if NET7_0_OR_GREATER

        public static explicit operator checked NumericValue(ulong v) => new(checked((long)v));
        public static explicit operator checked ulong(NumericValue v)
        {
            return v.IsInteger ? checked((ulong)v.m_integerValue) : (ulong)v.m_doubleValue;
        }
#endif

        public static implicit operator NumericValue(byte v) => new(v);
        public static implicit operator NumericValue(sbyte v) => new(v);
        public static implicit operator NumericValue(short v) => new(v);
        public static implicit operator NumericValue(ushort v) => new(v);
        public static implicit operator NumericValue(int v) => new(v);
        public static implicit operator NumericValue(uint v) => new(v);
        public static implicit operator NumericValue(long v) => new(v);
        public static explicit operator NumericValue(ulong v) => new((long)v);
        public static implicit operator NumericValue(float v) => new(v);
        public static implicit operator NumericValue(double v) => new(v);

        /// <summary>
        /// Attempts to convert the value to a numeric value.
        /// </summary>
        /// <param name="v">The type of value.</param>
        /// <returns>The converted value.</returns>
        /// <exception cref="InvalidCastException"><paramref name="v"/> cannot be converted.</exception>
        public static NumericValue Convert(object v)
        {
            if (v is null)
            {
                throw new InvalidCastException("Cannot convert from null to NumericValue");
            }
            if (!NumericValue.TryConvertFromCast(v, out NumericValue result))
            {
                throw new InvalidCastException($"Cannot convert from {v.GetType()} to NumericValue");
            }
            return result;
        }

        public static explicit operator byte(NumericValue v)
        {
            return v.IsInteger ? (byte)v.m_integerValue : (byte)v.m_doubleValue;
        }
        public static explicit operator sbyte(NumericValue v)
        {
            return v.IsInteger ? (sbyte)v.m_integerValue : (sbyte)v.m_doubleValue;
        }
        public static explicit operator short(NumericValue v)
        {
            return v.IsInteger ? (short)v.m_integerValue : (short)v.m_doubleValue;
        }
        public static explicit operator ushort(NumericValue v)
        {
            return v.IsInteger ? (ushort)v.m_integerValue : (ushort)v.m_doubleValue;
        }
        public static explicit operator int(NumericValue v)
        {
            return v.IsInteger ? (int)v.m_integerValue : (int)v.m_doubleValue;
        }
        public static explicit operator uint(NumericValue v)
        {
            return v.IsInteger ? (uint)v.m_integerValue : (uint)v.m_doubleValue;
        }
        public static explicit operator long(NumericValue v)
        {
            return v.IsInteger ? (long)v.m_integerValue : (long)v.m_doubleValue;
        }
        public static explicit operator ulong(NumericValue v)
        {
            return v.IsInteger ? (ulong)v.m_integerValue : (ulong)v.m_doubleValue;
        }
        public static explicit operator float(NumericValue v)
        {
            return v.IsDouble ? (float)v.m_doubleValue : (float)v.m_integerValue;
        }
        public static explicit operator double(NumericValue v)
        {
            return v.IsDouble ? v.m_doubleValue : (double)v.m_integerValue;
        }
    }
}
