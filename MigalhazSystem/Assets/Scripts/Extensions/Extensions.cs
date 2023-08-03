using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MigalhaSystem.Extensions
{
    public static class VectorExtension
    {
        public static Vector3 ToVector3(this Vector2 _vector2)
        {
            return _vector2;
        }

        public static void ClearVector(this ref Vector2 _vector2)
        {
            _vector2 = Vector2.zero;
        }

        public static void ClearVector(this ref Vector3 _vector3)
        {
            _vector3 = Vector3.zero;
        }

        public static Vector2 ToVector2(this Vector3 _vector3)
        {
            return _vector3;
        }
    }

    public static class EnumExtension
    {
        public static int GetEnumCount<TEnum>(this TEnum _enum) where TEnum : struct, System.Enum
        {
            return System.Enum.GetNames(typeof(TEnum)).Length;
        }

        public static int GetEnumCount<TEnum>() where TEnum : struct, System.Enum
        {
            return System.Enum.GetNames(typeof(TEnum)).Length;
        }

        public static int GetEnumCount(this System.Type _type)
        {
            return System.Enum.GetNames(_type.GetType()).Length;
        }
    }

    public enum UpdateMethod
    {
        N, Update, FixedUpdate, LateUpdate
    }

    public enum DrawMethod
    {
        N, OnDrawGizmos, OnDrawGizmosSelected
    }

    public static class RangeExtend
    {
        public static float RangeBy0(float _maxInclusive)
        {
            return Random.Range(0f, _maxInclusive);
        }
        public static int RangeBy0(int _maxExclusive)
        {
            return Random.Range(0, _maxExclusive);
        }
    }

    public static class StringExtend
    {
        public static string Color(this string _string, Color _textColor)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGBA(_textColor)}>{_string}</color>";
        }

        public static string Color(this string _string, string _textColorHex)
        {

            string _colorString = _textColorHex;
            if (_textColorHex.StartsWith('#'))
            {
                _colorString = _textColorHex.Split('#')[1];

            }
            return $"<color=#{_colorString}>{_string}</color>";
        }

        public static string Bold(this string _string)
        {
            return $"<b>{_string}</b>";
        }

        public static string Italic(this string _string)
        {
            return $"<i>{_string}</i>";
        }

        public static string Warning(this string _string)
        {
            return $"{"WARNING:".Bold().Color(UnityEngine.Color.yellow)} {_string.Italic()}";
        }

        public static string Error(this string _string)
        {
            return $"{"ERROR:".Bold().Color(UnityEngine.Color.red)} {_string.Italic()}";
        }
    }

    [System.Serializable]
    public struct FloatRange
    {
        [SerializeField] float m_minValue;
        [SerializeField] float m_maxValue;
        public float m_MinValue => m_minValue;
        public float m_MaxValue => m_maxValue;
        public float GetRandomValue()
        {
            return Random.Range(m_minValue, m_maxValue);
        }
        public bool InRange(float value)
        {
            return value >= m_minValue && value <= m_maxValue;
        }
        public void ChangeMinValue(float newValue)
        {
            m_minValue = newValue;
        }
        public void ChangeMaxValue(float newValue)
        {
            m_maxValue = newValue;
        }
    }

    [System.Serializable]
    public struct IntRange
    {
        [SerializeField] int m_minValue;
        [SerializeField] int m_maxValue;
        public int m_MinValue => m_minValue;
        public int m_MaxValue => m_maxValue;
        public int GetRandomValue(bool _maxInclusive = false)
        {
            return Random.Range(m_minValue, m_maxValue + (_maxInclusive ? 1 : 0));
        }
        public bool InRange(float value)
        {
            return value >= m_minValue && value <= m_maxValue;
        }
        public void ChangeMinValue(int newValue)
        {
            m_minValue = newValue;
        }
        public void ChangeMaxValue(int newValue)
        {
            m_maxValue = newValue;
        }
    }
}