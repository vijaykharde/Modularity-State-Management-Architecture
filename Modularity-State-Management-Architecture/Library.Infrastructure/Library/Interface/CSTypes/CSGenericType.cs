using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Infrastructure.Library.Interface
{
    public enum ValueStatus
    {
        VALID,
        UNSPECIFIED,
        BLOCKED,
        UNAVAILABLE,
        ENUM_PARSE_ERROR
    }
    public class CSGenericType<T> : IConvertible, IComparable<T>, IComparable<CSGenericType<T>>, IEquatable<CSGenericType<T>>
    {

        #region Constructors
        public CSGenericType()
        {
        }

        public CSGenericType(T value, ValueStatus status)
        {
            this._value = value;
            this._status = status;
        }

        public CSGenericType(T value)
            : this(value, ValueStatus.VALID)
        {
        }
        #endregion

        #region Fields


        protected internal T _value;
        protected internal ValueStatus _status;

        #endregion

        #region Properties


        public T Value
        {
            get { return _value; }
            set { _value = value; }
        }
        public ValueStatus Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public bool IsValid
        {
            get { return this._status == ValueStatus.VALID; }
        }
        #endregion

        #region Operator Overloads

        public static implicit operator T(CSGenericType<T> x)
        {
            if ((object)default(T) != null) // T is a value type
            {
                return ((x as object) != null) ? x.Value : default(T);
            }
            else // T is a reference type
            {
                return object.Equals(x, null) ? default(T) : x.Value;
            };
        }

        public static implicit operator CSGenericType<T>(T x)
        {
            return new CSGenericType<T>(x);
        }

        public bool Equals(CSGenericType<T> other)
        {
            if (other == null) return false;
            if (Value == null && other.Value == null) return true;
            if (Value == null && other.Value != null) return false;
            if (Value != null && other.Value == null) return false;

            return Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as CSGenericType<T>);
        }

        public override int GetHashCode()
        {
            int hash = 37;
            hash = hash * 23 + typeof(T).Name.GetHashCode();
            if (Value != null)
            {
                hash = hash * 23 + Value.GetHashCode();
            }
            return hash;
        }



        public override string ToString()
        {

            if (this.IsValid)
            {
                if (this._value == null)
                    return string.Empty;
                return this._value.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion

        #region IConvertible Members

        TypeCode IConvertible.GetTypeCode()
        {
            return TypeCode.Object;
        }

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            return Convert.ToBoolean(this._value, provider);
        }

        byte IConvertible.ToByte(IFormatProvider provider)
        {
            return Convert.ToByte(this._value, provider);
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            return Convert.ToChar(this._value, provider);
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            return Convert.ToDateTime(this._value, provider);
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            return Convert.ToDecimal(this._value, provider);
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            return Convert.ToDouble(this._value, provider);
        }

        short IConvertible.ToInt16(IFormatProvider provider)
        {
            return Convert.ToInt16(this._value, provider);
        }

        int IConvertible.ToInt32(IFormatProvider provider)
        {
            return Convert.ToInt32(this._value, provider);
        }

        long IConvertible.ToInt64(IFormatProvider provider)
        {
            return Convert.ToInt64(this._value, provider);
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            return Convert.ToSByte(this._value, provider);
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            return Convert.ToSingle(this._value, provider);
        }

        string IConvertible.ToString(IFormatProvider provider)
        {
            return this.ToString();
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            return Convert.ChangeType(this._value, conversionType, provider);
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            return Convert.ToUInt16(this._value, provider);
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            return Convert.ToUInt32(this._value, provider);
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            return Convert.ToUInt64(this._value, provider);
        }

        #endregion

        #region IComparable<T> Members

        public virtual int CompareTo(T other)
        {
            if (this._value != null)
            {
                return string.Compare(this._value.ToString(), other.ToString());
            }
            return string.Compare(this.ToString(), other.ToString());
        }

        #endregion

        #region IComparable<CSGenericType<T>> Members

        public int CompareTo(CSGenericType<T> other)
        {
            if (other == null)
            {
                return 1;
            }
            return string.Compare(this.ToString(), other.ToString());
        }

        #endregion
    }
}
