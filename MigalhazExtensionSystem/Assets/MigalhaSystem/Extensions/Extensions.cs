using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace MigalhaSystem.Extensions
{
    public static class VectorExtension
    {
        public static Vector2 Where(this Vector2 _vector2, float? x = null, float? y = null)
        {
            float newX = x is null ? _vector2.x : (float) x;
            float newY = y is null ? _vector2.y : (float) y;
            _vector2.Set(newX, newY);
            return _vector2;
        }
        public static Vector3 Where(this Vector3 _vector3, float? x = null, float? y = null, float? z = null)
        {
            float newX = x is null ? _vector3.x : (float)x;
            float newY = y is null ? _vector3.y : (float)y;
            float newZ = z is null ? _vector3.z : (float)z;
            _vector3.Set(newX, newY, newZ);
            return _vector3;
        }

        public static Vector2 Increase(this Vector2 _vector3, float? x = null, float? y = null)
        {
            float newX = x is null ? _vector3.x : _vector3.x + (float)x;
            float newY = y is null ? _vector3.y : _vector3.y + (float)y;
            _vector3.Set(newX, newY);
            return _vector3;
        }
        public static Vector3 Increase(this Vector3 _vector3, float? x = null, float? y = null, float? z = null)
        {
            float newX = x is null ? _vector3.x : _vector3.x + (float)x;
            float newY = y is null ? _vector3.y : _vector3.y + (float)y;
            float newZ = z is null ? _vector3.z : _vector3.z + (float)z;
            _vector3.Set(newX, newY, newZ);
            return _vector3;
        }

        public static Vector2 Decrease(this Vector2 _vector3, float? x = null, float? y = null)
        {
            float newX = x is null ? _vector3.x : _vector3.x - (float)x;
            float newY = y is null ? _vector3.y : _vector3.y - (float)y;
            _vector3.Set(newX, newY);
            return _vector3;
        }
        public static Vector3 Decrease(this Vector3 _vector3, float? x = null, float? y = null, float? z = null)
        {
            float newX = x is null ? _vector3.x : _vector3.x - (float)x;
            float newY = y is null ? _vector3.y : _vector3.y - (float)y;
            float newZ = z is null ? _vector3.z : _vector3.z - (float)z;
            _vector3.Set(newX, newY, newZ);
            return _vector3;
        }

        public static Vector3 DirectionFromDeg(this float angleInDeg, Vector3? rotationAxis = null, Vector3? startDirection = null)
        {
            rotationAxis ??= Vector3.forward;
            Quaternion myRotation = Quaternion.AngleAxis(angleInDeg, (Vector3)rotationAxis);

            startDirection ??= Vector3.right;
            Vector3 result = myRotation * (Vector3) startDirection;

            return result;
        }
        public static Vector3 DirectionFromRad(this float angleInRad, Vector3? rotationAxis = null, Vector3? startDirection = null) => DirectionFromDeg(angleInRad * Mathf.Rad2Deg, rotationAxis, startDirection);

        public static Vector2 GetRandomPosition(Vector2 minValue, Vector2 maxValue)
        {
            float x = Random.Range(minValue.x, maxValue.x);
            float y = Random.Range(minValue.y, maxValue.y);
            return new(x, y);
        }
        public static Vector3 GetRandomPosition(Vector3 minValue, Vector3 maxValue)
        {
            float x = Random.Range(minValue.x, maxValue.x);
            float y = Random.Range(minValue.y, maxValue.y);
            float z = Random.Range(minValue.z, maxValue.z);
            return new(x, y, z);
        }
    }

    public static class EnumExtension
    {
        public static int GetEnumCount<TEnum>(this TEnum _enum) where TEnum : struct, System.Enum => System.Enum.GetNames(typeof(TEnum)).Length;
        public static int GetEnumCount<TEnum>() where TEnum : struct, System.Enum => System.Enum.GetNames(typeof(TEnum)).Length;
        public static int GetEnumCount(this System.Type _type) => System.Enum.GetNames(_type.GetType()).Length;
    }

    public enum UpdateMethod { N, Update, FixedUpdate, LateUpdate }

    public enum DrawMethod { N, OnDrawGizmos, OnDrawGizmosSelected }

    [System.Serializable]
    public class DrawSettings
    {
        [SerializeField] bool m_draw = true;
        [SerializeField] Color m_color = Color.white;
        [SerializeField] DrawMethod m_drawMethod = DrawMethod.OnDrawGizmos;
        public bool CanDraw(DrawMethod drawMethod) => m_draw && drawMethod == m_drawMethod;
        public Color GetColor() => m_color;
    }

    public static class FloatExtend
    {
        public static bool IsInteger(this float value) => value % 1 <= float.Epsilon * 1000;
        public static bool IsInteger(this double value) => value % 1 <= double.Epsilon * 1000;
    }

    public static class RandomExtend
    {
        public static float RangeBy0(this float value) => Random.Range(0f, value);
        public static int RangeBy0(this int value, bool maxInclusive = false) => Random.Range(0, value + (maxInclusive ? 1 : 0));

        public static float VariateWithinRange(this float value) => Random.Range(-value, value);
        public static int VariateWithinRange(this int value, bool minValueInclusive = true, bool maxValueInclusive = false) => Random.Range(-value + (minValueInclusive ? 0 : 1), value + (maxValueInclusive ? 1 : 0));

        public static float ChooseAltNum(this float value) => Choose(-value, value);
        public static int ChooseAltNum(this int value) => Choose(-value, value);

        public static T Choose<T>(params T[] elements) => elements.GetRandom();
    }

    public static class StringExtend
    {
        public static string Color(this string _string, Color _textColor) => $"<color=#{ColorUtility.ToHtmlStringRGBA(_textColor)}>{_string}</color>";
        public static string Color(this string _string, string _textColorHex)
        {
            string _colorString = _textColorHex;
            if (_textColorHex.StartsWith('#'))
            {
                _colorString = _textColorHex.Split('#')[1];
            }
            return $"<color=#{_colorString}>{_string}</color>";
        }
        public static string Bold(this string _string) => $"<b>{_string}</b>";
        public static string Italic(this string _string) => $"<i>{_string}</i>";
        public static string Size(this string _string, float size = 15) => $"<size={size}>{_string}</size>";
        public static string Warning(this string _string) => $"{"WARNING:".Bold().Color(UnityEngine.Color.yellow)} {_string.Italic()}";
        public static string Error(this string _string) => $"{"ERROR:".Bold().Color(UnityEngine.Color.red)} {_string.Italic()}";
        public static string Capitalize(this string _string)
        {
            if (string.IsNullOrEmpty(_string)) return _string;
            string normalizedString = "";
            char lastChar = ' ';
            foreach (char c in _string)
            {
                char newChar = c;
                if (lastChar == ' ')
                {
                    newChar = char.ToUpper(c);
                }
                normalizedString += newChar;
                lastChar = newChar;
            }
            return normalizedString;
        }
    }

    public static class ListExtend
    {
        public static T GetRandom<T>(this List<T> list) => list[list.Count.RangeBy0()];
        public static T GetRandom<T>(this T[] array) => array[array.Length.RangeBy0()];

        public static List<T> Shuffle<T>(this List<T> list)
        {
            if (list.IsNullOrEmpty()) return list;
            for (int i = 0; i < list.Count; i++)
            {
                int newItemIndex = list.Count.RangeBy0();
                T newItem = list[newItemIndex];
                list[newItemIndex] = list[i];
                list[i] = newItem;
            }
            return list;
        }
        public static List<T> Reverse<T>(this List<T> list)
        {
            if (list.IsNullOrEmpty()) return list;
            List<T> newList = new(list);
            for (int i = 0; i < list.Count; i++)
            {
                list[i] = newList[list.Count - i - 1];
            }
            return list;
        }

        public static TType FindType<TList, TType>(this List<TList> list) where TType : class, TList where TList : class => list?.Find(x => x is TType) as TType;
        public static List<TType> FindAllType<TList, TType>(this List<TList> list) where TType : class, TList => list.OfType<TType>().ToList();
        
        public static List<T> Without<T>(this List<T> list, params T[] item)
        {
            List<T> newList = new List<T>(list);
            newList.RemoveAll(item);
            return newList;
        }
        public static List<T> Without<T>(this List<T> list, IEnumerable<T> itens)
        {
            List<T> newList = new List<T>(list);
            newList.RemoveAll(itens);
            return newList;
        }
        public static List<T> RemoveAll<T>(this List<T> list, params T[] item)
        {
            if (item is null) return list;
            if (item.Length <= 0) return list;
            list.RemoveAll(x => item.Contains(x));
            return list;
        }
        public static List<T> RemoveAll<T>(this List<T> list, IEnumerable<T> itens)
        {
            if (itens is null) return list;
            list.RemoveAll(x => itens.Contains(x));
            return list;
        }
        public static List<T> RemoveDoubleItens<T>(this List<T> list)
        {
            List<T> newList = new();
            for (int i = 0; i < list.Count; i++)
            {
                T item = list[i];
                if (newList.Contains(item)) continue;
                newList.Add(item);
            }
            list.Clear();
            list.AddRange(newList);
            return list;
        }

        public static List<T> Override<T>(this List<T> list, T itemToOverride, T newItem)
        {
            if (list.IsNullOrEmpty()) return list;
            if (list.Contains(itemToOverride))
            {
                int index = list.IndexOf(itemToOverride);
                list[index] = newItem;
            }
            return list;
        }

        public static List<T> AddOnce<T>(this List<T> list, T item)
        {
            if (!list.Contains(item)) list.Add(item);
            return list;
        }
        public static List<T> AddRangeOnce<T>(this List<T> list, IEnumerable<T> item)
        {
            foreach (T i in item)
            {
                list.AddOnce(i);
            }
            return list;
        }

        public static int CountItens<T>(this List<T> list, System.Predicate<T> match) => list.FindAll(match).Count;
        public static bool IsNullOrEmpty<T>(this List<T> list) => list.IsNull() || list.IsEmpty();
        public static bool IsEmpty<T>(this List<T> list) => list.Count <= 0;
        public static bool IsNull<T>(this List<T> list) => list == null;
        public static bool IsNullOrEmpty<T>(this T[] array)
        {
            if (array == null) return true;
            if (array.Length <= 0) return true;
            return false;
        }
    }

    public static class ColorExtend
    {
        public static Color GetRandomColor() => Random.ColorHSV();
        public static Color SetAlpha(this Color color, float alpha)
        {
            Color newColor = color;
            alpha = Mathf.Clamp01(alpha);
            newColor.a = alpha;
            return newColor;
        }
    }

    public static class GameObjectExtensions
    {
        private static bool Requires(System.Type obj, System.Type requirement)
        {
            return System.Attribute.IsDefined(obj, typeof(RequireComponent)) &&
                   System.Attribute.GetCustomAttributes(obj, typeof(RequireComponent)).OfType<RequireComponent>()
                   .Any(rc => rc.m_Type0.IsAssignableFrom(requirement));
        }
        public static bool CanDestroy(this GameObject go, System.Type t)
        {
            return !go.GetComponents<Component>().Any(c => Requires(c.GetType(), t));
        }
        public static T CreateIfItsNull<T>(this T obj) where T : class, new()
        {
            if (obj is null)
            {
                obj = new T();
            }
            return obj;
        }
    }

    [System.Serializable]
    public struct FloatRange
    {
        [SerializeField] float m_minValue;
        [SerializeField] float m_maxValue;
        public float m_MinValue => m_minValue;
        public float m_MaxValue => m_maxValue;

        public FloatRange(Vector2 value)
        {
            m_minValue = value.x;
            m_maxValue = value.y;
        }
        public FloatRange(Vector3 value)
        {
            m_minValue = value.x;
            m_maxValue = value.y;
        }
        public FloatRange(float value)
        {
            m_minValue = value;
            m_maxValue = value;
        }
        public FloatRange(float _minValue, float _maxValue)
        {
            m_minValue = _minValue;
            m_maxValue = _maxValue;
        }

        public void SetMinValue(float newValue) => m_minValue = newValue;
        public void SetMaxValue(float newValue) => m_maxValue = newValue;
        public void SetRangeValue(float newMinValue, float newMaxValue)
        {
            m_minValue = newMinValue;
            m_maxValue = newMaxValue;
        }

        public bool InRange(float value) => value >= m_minValue && value <= m_maxValue;
        public bool InRange(int value) => value >= m_minValue && value <= m_maxValue;

        public float GetRandomValue() => Random.Range(m_minValue, m_maxValue);
        public float Clamp(float value) => Mathf.Clamp(value, m_minValue, m_maxValue);
        public float Lerp(float t) => Mathf.Lerp(m_minValue, m_maxValue, t);
        public float InverseLerp(float value) => Mathf.InverseLerp(m_minValue, m_maxValue, value);
        public float LerpUnclamped(float t) => Mathf.LerpUnclamped(m_minValue, m_maxValue, t);
        public float Deepness() => m_MaxValue - m_MinValue;

        public static FloatRange operator +(FloatRange a, FloatRange b) => new(a.m_minValue + b.m_minValue, a.m_maxValue + b.m_maxValue);
        public static FloatRange operator +(FloatRange a, float value) => new(a.m_minValue + value, a.m_maxValue + value);
        public static FloatRange operator -(FloatRange a, FloatRange b) => new(a.m_minValue - b.m_minValue, a.m_maxValue - b.m_maxValue);
        public static FloatRange operator -(FloatRange a, float value) => new(a.m_minValue - value, a.m_maxValue - value);
        public static FloatRange operator *(FloatRange a, float value) => new(a.m_minValue * value, a.m_maxValue * value);
        public static FloatRange operator /(FloatRange a, float value) => new(a.m_minValue / value, a.m_maxValue / value);

        public static bool operator <(FloatRange a, FloatRange b) => a.Deepness() < b.Deepness();
        public static bool operator >(FloatRange a, FloatRange b) => a.Deepness() > b.Deepness();
        public static bool operator <=(FloatRange a, FloatRange b) => a.Deepness() <= b.Deepness();
        public static bool operator >=(FloatRange a, FloatRange b) => a.Deepness() >= b.Deepness();

        public static implicit operator Vector2(FloatRange a) => new Vector2(a.m_minValue, a.m_maxValue);
        public static implicit operator Vector3(FloatRange a) => new Vector3(a.m_minValue, a.m_maxValue);

        public static implicit operator FloatRange(Vector2 vector) => new FloatRange(vector.x, vector.y);
        public static implicit operator FloatRange(Vector3 vector) => new FloatRange(vector.x, vector.y);
    }

    [System.Serializable]
    public struct IntRange
    {
        [SerializeField] int m_minValue;
        [SerializeField] int m_maxValue;
        public int m_MinValue => m_minValue;
        public int m_MaxValue => m_maxValue;

        public IntRange(Vector2 value)
        {
            m_minValue = (int)value.x;
            m_maxValue = (int)value.y;
        }
        public IntRange(Vector3 value)
        {
            m_minValue = (int)value.x;
            m_maxValue = (int)value.y;
        }
        public IntRange(int value)
        {
            m_minValue = value;
            m_maxValue = value;
        }
        public IntRange(int _minValue, int _maxValue)
        {
            m_minValue = _minValue;
            m_maxValue = _maxValue;
        }

        public void SetMinValue(int newValue) => m_minValue = newValue;
        public void SetMaxValue(int newValue) => m_maxValue = newValue;
        public void SetRangeValue(int newMinValue, int newMaxValue)
        {
            m_minValue = newMinValue;
            m_maxValue = newMaxValue;
        }

        public bool InRange(float value) => value >= m_minValue && value <= m_maxValue;
        public bool InRange(int value) => value >= m_minValue && value <= m_maxValue;

        public int GetRandomValue(bool _maxInclusive = false) => Random.Range(m_minValue, m_maxValue + (_maxInclusive ? 1 : 0));
        public int Clamp(int value) => Mathf.Clamp(value, m_minValue, m_maxValue);
        public float Lerp(float t) => Mathf.Lerp(m_minValue, m_maxValue, t);
        public float InverseLerp(float value) => Mathf.InverseLerp(m_minValue, m_maxValue, value);
        public float LerpUnclamped(float t) => Mathf.LerpUnclamped(m_minValue, m_maxValue, t);
        public int Deepness() => m_maxValue - m_minValue;

        public static IntRange operator +(IntRange a, IntRange b) => new(a.m_minValue + b.m_minValue, a.m_maxValue + b.m_maxValue);
        public static IntRange operator +(IntRange a, int value) => new(a.m_minValue + value, a.m_maxValue + value);
        public static IntRange operator -(IntRange a, IntRange b) => new(a.m_minValue - b.m_minValue, a.m_maxValue - b.m_maxValue);
        public static IntRange operator -(IntRange a, int value) => new(a.m_minValue - value, a.m_maxValue - value);
        public static IntRange operator *(IntRange a, int value) => new(a.m_minValue * value, a.m_maxValue * value);
        public static IntRange operator /(IntRange a, int value) => new(a.m_minValue / value, a.m_maxValue / value);
        public static bool operator <(IntRange a, IntRange b) => a.Deepness() < b.Deepness();
        public static bool operator >(IntRange a, IntRange b) => a.Deepness() > b.Deepness();
        public static bool operator <=(IntRange a, IntRange b) => a.Deepness() <= b.Deepness();
        public static bool operator >=(IntRange a, IntRange b) => a.Deepness() >= b.Deepness();

        public static implicit operator Vector2(IntRange a) => new Vector2(a.m_minValue, a.m_maxValue);
        public static implicit operator Vector3(IntRange a) => new Vector3(a.m_minValue, a.m_maxValue);

        public static implicit operator IntRange(Vector2 vector) => new IntRange((int)vector.x, (int)vector.y);
        public static implicit operator IntRange(Vector3 vector) => new IntRange((int)vector.x, (int)vector.y);
    }

    [System.Serializable]
    public class Timer
    {
        [Header("Timer Settings")]
        [SerializeField] bool m_countdown = true;
        [SerializeField] bool m_repeater = true;
        [SerializeField] FloatRange m_startTimer = new(1, 1);
        float m_pickedTimerValue;
        float m_currentTimerValue;

        [Header("Event Settings")]
        [SerializeField] protected List<TimerEvent> m_timerEvents;
        [SerializeField] protected UnityEvent m_onTimerElapsed;
        [SerializeField] protected UnityEvent m_onTimerStart;

        public bool m_Active => m_countdown;
        public List<TimerEvent> m_TimerEvents => m_timerEvents;
        public UnityEvent m_OnTimerElapsed => m_onTimerElapsed;
        public float m_PickedTimerValue => m_pickedTimerValue;
        public float m_CurrentTimerValue => m_currentTimerValue;
        public FloatRange m_StartTimer => m_startTimer;
        public void SetStartTimer(FloatRange newStartTimer)
        {
            m_startTimer = newStartTimer;
        }
        public void SetStartTimer(float newMinValue, float newMaxValue)
        {
            m_startTimer.SetRangeValue(newMinValue, newMaxValue);
        }
        public void ChangeMinStartTimerValue(float newMinValue)
        {
            m_startTimer.SetMinValue(newMinValue);
        }
        public void ChangeMaxStartTimerValue(float newMaxValue)
        {
            m_startTimer.SetMaxValue(newMaxValue);
        }
        public void DecreaseStartTimerValue(float decreaseValue)
        {
            m_startTimer.SetMinValue(m_startTimer.m_MinValue - decreaseValue);
            m_startTimer.SetMaxValue(m_startTimer.m_MaxValue - decreaseValue);
        }
        public void DecreaseStartTimerValue(float decreaseMinValue, float decreaseMaxValue)
        {
            m_startTimer.SetMinValue(m_startTimer.m_MinValue - decreaseMinValue);
            m_startTimer.SetMaxValue(m_startTimer.m_MaxValue - decreaseMaxValue);
        }
        public void IncreaseStartTimerValue(float decreaseValue)
        {
            m_startTimer.SetMinValue(m_startTimer.m_MinValue + decreaseValue);
            m_startTimer.SetMaxValue(m_startTimer.m_MaxValue + decreaseValue);
        }
        public void IncreaseStartTimerValue(float decreaseMinValue, float decreaseMaxValue)
        {
            m_startTimer.SetMinValue(m_startTimer.m_MinValue + decreaseMinValue);
            m_startTimer.SetMaxValue(m_startTimer.m_MaxValue + decreaseMaxValue);
        }

        public float GetTimeElapsedPercentage() => Mathf.Lerp(m_pickedTimerValue, 0, m_currentTimerValue);
        public void ActiveTimer(bool active)
        {
            m_countdown = active;
            if (active)
            {
                m_onTimerStart?.Invoke();
                SetupTimer();
            }
        }
        public virtual void SetupTimer()
        {
            m_pickedTimerValue = m_startTimer.GetRandomValue();
            m_currentTimerValue = m_pickedTimerValue;
            //m_currentTimerValue = m_startTimer.GetRandomValue();
            if (m_timerEvents == null) m_timerEvents = new();
            foreach (TimerEvent timerEvent in m_timerEvents)
            {
                timerEvent.Setup();
            }
        }
        public virtual bool TimerElapse(float deltaTime, bool persistentCheck = false)
        {
            if (persistentCheck)
            {
                if (!m_countdown) return false;
            }
            else
            {
                if (!m_countdown && m_currentTimerValue <= 0) return true;
            }
            
            m_currentTimerValue -= deltaTime;
            if (!m_timerEvents.IsNullOrEmpty())
            {
                foreach (TimerEvent timerEvent in m_timerEvents)
                {
                    timerEvent.CallEvent(m_currentTimerValue);
                }
            }
            if (m_currentTimerValue <= 0)
            {
                TimerElapsedAction();
                return true;
            }
            return false;
        }
        protected virtual void TimerElapsedAction()
        {
            ActiveTimer(m_repeater);
            m_onTimerElapsed?.Invoke();
        }

        public void AddTimeElapsedEvent(UnityAction action)
        {
            m_onTimerElapsed.AddListener(action);
        }
        public void RemoveTimeElapsedEvent(UnityAction action)
        {
            m_onTimerElapsed.RemoveListener(action);
        }
        public void ClearTimeElapsedEvent()
        {
            m_onTimerElapsed.RemoveAllListeners();
        }

        public void AddTimeStartEvent(UnityAction action)
        {
            m_onTimerStart.AddListener(action);
        }
        public void RemoveTimeStartEvent(UnityAction action)
        {
            m_onTimerStart.RemoveListener(action);
        }
        public void ClearTimeStartEvent()
        {
            m_onTimerStart.RemoveAllListeners();
        }

        public void AddTimerEvent(TimerEvent newTimerEvent)
        {
            if (m_timerEvents == null) return;
            m_timerEvents.Add(newTimerEvent);
        }
        public void RemoveTimerEvent(TimerEvent newTimerEvent)
        {
            if (m_timerEvents.IsNullOrEmpty()) return;
            if (!m_timerEvents.Contains(newTimerEvent)) return;
            m_timerEvents.Remove(newTimerEvent);
        }
        public void ClearTimerEvents()
        {
            if (m_timerEvents.IsNullOrEmpty()) return;
            m_timerEvents.Clear();
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            float minValue = m_StartTimer.m_MinValue;
            float maxValue = m_StartTimer.m_MaxValue;

            if (minValue <= 0)
            {
                minValue = 0;
            }

            if (maxValue < minValue)
            {
                maxValue = minValue;
            }

            if (minValue > maxValue)
            {
                minValue = maxValue;
            }

            m_startTimer.SetRangeValue(minValue, maxValue);
        }
#endif
    }

    [System.Serializable]
    public class TimerEvent
    {
        [SerializeField] bool m_persistentCall;
        bool m_called;
        [SerializeField] float m_timerToTrigger;
        [SerializeField] UnityEvent m_onTimeElapsed;
        public UnityEvent m_OnTimeElapsed => m_onTimeElapsed;

        public TimerEvent(bool persistentCall, float timerToTrigger, UnityEvent unityEvent)
        {
            m_called = false;
            m_persistentCall = persistentCall;
            m_timerToTrigger = timerToTrigger;
            m_onTimeElapsed = unityEvent;
        }
        public void Setup()
        {
            m_called = false;
        }
        public bool CallEvent(float currentTime)
        {
            if (!m_persistentCall)
            {
                if (m_called)
                {
                    return false;
                }
            }
            if (currentTime <= m_timerToTrigger)
            {
                m_onTimeElapsed?.Invoke();
                m_called = true;
                return true;
            }
            return m_called;
        }
    }

    [System.Serializable]
    public class ChancePicker
    {
        [SerializeField, Range(0, 100)] float m_chance;
        public ChancePicker() => SetChance(50);
        public ChancePicker(float value) => SetChance(value);
        public void SetChance(float value) => m_chance = Mathf.Clamp(value, 0, 100);
        public bool PickChance() => Random.value < (m_chance * 0.01f);
        public static bool PickChance(float chance) => Random.value < (chance * 0.01f);
    }

    [System.Serializable]
    public class DamageSystem
    {
        [SerializeField] FloatRange m_damageValue;
        [SerializeField, Min(0)] float m_criticalHitMultiplyer = 2f;
        [SerializeField] ChancePicker m_criticalHitChance;
        public virtual float GetDamageValue()
        {
            float damage = m_damageValue.GetRandomValue();
            if (IsCritical())
            {
                damage *= m_criticalHitMultiplyer;
            }
            return damage;
        }
        public virtual bool IsCritical() => m_criticalHitChance.PickChance();
    }

    [System.Serializable]
    public class DamageSystemInt
    {
        [SerializeField] IntRange m_damageValue;
        [SerializeField] bool m_maxInclusive = true;
        [SerializeField, Min(0)] int m_criticalHitMultiplyer = 2;
        [SerializeField] ChancePicker m_criticalHitChance;
        public virtual int GetDamageValue()
        {
            int damage = m_damageValue.GetRandomValue(m_maxInclusive);
            if (IsCritical())
            {
                damage *= m_criticalHitMultiplyer;
            }
            return damage;
        }
        public virtual bool IsCritical() => m_criticalHitChance.PickChance();
    }

    [System.Serializable]
    public class ComponentContainer
    {
        [SerializeField] List<Component> m_components;
        public List<Component> m_Components => m_components;

        public void AddComponentInContainer(params Component[] component)
        {
            m_components.RemoveAll(x => x == null);
            if (component == null) return;
            if (component.Length <= 0) return;
            m_components.AddRangeOnce(component);
        }


        public T GetComponentFromContainer<T>() where T : Component
        {
            m_components ??= new();
            m_components.RemoveAll(x => x == null);
            return m_components.FindType<Component, T>();
        }
        public T GetComponentFromContainer<T>(System.Action notFoundComponentAction, bool TryGetComponentAgain = false) where T : Component
        {
            T component = GetComponentFromContainer<T>();
            if (component == null) notFoundComponentAction?.Invoke();
            if (TryGetComponentAgain) component = GetComponentFromContainer<T>();
            return component;
        }
        public bool TryGetComponentFromContainer<T>(out T component) where T : Component
        {
            component = GetComponentFromContainer<T>();
            return component != null;
        }

        public List<T> GetComponentsFromContainer<T>() where T : Component
        {
            m_components ??= new();
            m_components.RemoveAll(x => x == null);
            return m_components.FindAllType<Component, T>();
        }
    }

    [System.Serializable]
    public sealed class SerializableDictionary<TKey, TValue>
    {
        [SerializeField] List<KeyValue<TKey, TValue>> m_dictionary;
        public int Count => m_dictionary.Count;
        public List<KeyValue<TKey, TValue>> GetList() => m_dictionary;
        public Dictionary<TKey, TValue> ToDictionary()
        {
            return m_dictionary.ToDictionary(itemInList => itemInList.GetKey(), itemInList => itemInList.GetValue());
        }
        public List<TKey> GetAllKeys()
        {
            List<TKey> keys = new List<TKey>();
            foreach (var item in m_dictionary)
            {
                keys.Add(item.GetKey());
            }
            return keys;
        }
        public List<TValue> GetAllValues()
        {
            List<TValue> value = new List<TValue>();
            foreach (var item in m_dictionary)
            {
                value.Add(item.GetValue());
            }
            return value;
        }
        public void RemoveNullItems()
        {
            m_dictionary.RemoveAll(x => x == null);
            m_dictionary.RemoveAll(x => x.GetValue() == null && x.GetKey() == null);
        }
        public void RemoveNullKeys()
        {
            List<TKey> nullKeys = GetAllKeys().FindAll(x => x is null);
            foreach (TKey key in nullKeys)
            {
                Remove(key);
            }
        }
        public void RemoveNullValues()
        {
            List<TValue> nullValues = GetAllValues().FindAll(x => x is null);
            foreach (TValue value in nullValues)
            {
                Remove(value);
            }
        }
        public void Add(TKey key, TValue value)
        {
            m_dictionary.Add(new KeyValue<TKey, TValue>(key, value));
        }
        public void Clear()
        {
            m_dictionary.Clear();
        }
        public KeyValue<TKey, TValue> GetItem(TKey key)
        {
            EqualityComparer<TKey> comparer = EqualityComparer<TKey>.Default;
            KeyValue<TKey, TValue> item = m_dictionary.Find(x => comparer.Equals(key, x.GetKey()));
            return item;
        }
        public KeyValue<TKey, TValue> GetItem(TValue value)
        {
            EqualityComparer<TValue> comparer = EqualityComparer<TValue>.Default;
            KeyValue<TKey, TValue> item = m_dictionary.Find(x => comparer.Equals(value, x.GetValue()));
            return item;
        }
        public bool GetItem(TKey key, out KeyValue<TKey, TValue> item)
        {
            item = null;
            if (!ContainsKey(key)) return false;
            item = GetItem(key);
            return true;
        }
        public bool GetItem(TValue value, out KeyValue<TKey, TValue> item)
        {
            item = null;
            if (!ContainsValue(value))
            {
                return false;
            }
            item = GetItem(value);
            return true;
        }
        public bool ContainsKey(TKey key) => GetItem(key) != null;
        public bool ContainsValue(TValue value) => GetItem(value) != null;
        public bool ContainsItem(KeyValue<TKey, TValue> item) => m_dictionary.Contains(item);
        public bool Remove(TKey key)
        {
            if (!GetItem(key, out KeyValue<TKey, TValue> item)) return false;
            m_dictionary.Remove(item);
            return true;
        }
        public bool Remove(TKey key, out TValue value)
        {
            value = default;
            if (!GetItem(key, out KeyValue<TKey, TValue> item)) return false;
            value = item.GetValue();
            m_dictionary.Remove(item);
            return true;
        }
        public bool Remove(TValue value)
        {
            if (!GetItem(value, out KeyValue<TKey, TValue> item)) return false;
            m_dictionary.Remove(item);
            return true;
        }
        public bool Remove(TValue value, out TKey key)
        {
            key = default;
            if (!GetItem(value, out KeyValue<TKey, TValue> item)) return false;
            key = item.GetKey();
            m_dictionary.Remove(item);
            return true;
        }
        public bool TryAdd(TKey key, TValue value)
        {
            if (ContainsKey(key)) return false;
            Add(key, value);
            return true;
        }
        public TValue GetValue(TKey key)
        {
            TValue value = default;
            if (GetItem(key, out KeyValue<TKey, TValue> item))
            {
                value = item.GetValue();
            }
            return value;
        }
        public bool TryGetValue(TKey key, out TValue value)
        {
            value = default;
            if (!ContainsKey(key)) return false;
            value = GetValue(key);
            return true;
        }
        public TKey GetKey(TValue value)
        {
            TKey key = default;
            if (GetItem(value, out KeyValue<TKey, TValue> item))
            {
                key = item.GetKey();
            }
            return key;
        }
        public bool TryGetKey(TValue value, out TKey key)
        {
            key = default;
            if (!ContainsValue(value)) return false;
            key = GetKey(value);
            return true;
        }
    }

    [System.Serializable]
    public sealed class KeyValue<TKey, TValue>
    {
        [SerializeField] TKey m_key;
        [SerializeField] TValue m_value;

        public KeyValue(TKey key, TValue value)
        {
            m_key = key;
            m_value = value;
        }

        public TKey GetKey() => m_key;
        public TValue GetValue() => m_value;
    }

    public static class Benchmark
    {
        public static void TimeExecution(System.Action function, string label = null, bool ms = true)
        {
            System.Diagnostics.Stopwatch timer = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                function?.Invoke();
            }
            finally
            {
                timer.Stop();
                string timeMessage = ms ? $"{timer.ElapsedMilliseconds}ms" : $"{timer.Elapsed}";
                timeMessage = timeMessage.Color(Color.green);

                string result = $"{"Took:".Color(Color.white)} {timeMessage}".Bold();
                if (label != null) result = $"{$"({label})".Bold().Color(Color.white)} {result}";
                Debug.Log(result);
            }
        }
    }

    public static class MigalhazHelper
    {
        static Camera m_mainCamera;
        public static Camera m_MainCamera
        {
            get
            {
                if (m_mainCamera is null) m_mainCamera = Camera.main;
                return m_mainCamera;
            }
        }
        static Camera m_currentCamera;
        public static Camera m_CurrentCamera
        {
            get
            {
                if (m_currentCamera is null) m_currentCamera = Camera.current;
                return m_currentCamera;
            }
        }

        static Dictionary<float, WaitForSeconds> m_waitForSecondsDictionary = new Dictionary<float, WaitForSeconds>();

        static PointerEventData m_eventDataCurrentPos;
        static List<RaycastResult> m_results;
        static EventSystem m_currentEventSystem;
        public static EventSystem m_CurrentEventSystem
        {
            get
            {
                if (m_currentEventSystem is null) m_currentEventSystem = EventSystem.current;
                return m_currentEventSystem;
            }
        }
        
        public static Camera GetMainCamera() => m_MainCamera;
        public static Camera GetCurrentCamera() => m_CurrentCamera;

        public static WaitForSeconds GetWaitForSeconds(float seconds)
        {
            if (m_waitForSecondsDictionary.TryGetValue(seconds, out WaitForSeconds wait)) return wait;
            m_waitForSecondsDictionary[seconds] = new WaitForSeconds(seconds);
            return m_waitForSecondsDictionary[seconds];
        }
        
        public static bool IsOverUI(Vector3 _position)
        {
            m_eventDataCurrentPos = new PointerEventData(m_CurrentEventSystem)
            {
                position = _position
            };
            m_results = new List<RaycastResult>();
            m_CurrentEventSystem.RaycastAll(m_eventDataCurrentPos, m_results);
            return m_results.Count > 0;
        }

        public static Vector2 GetWorldPositionOfCanvasElement(RectTransform element)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(element, element.position, m_MainCamera, out Vector3 worldPoint);
            return worldPoint;
        }
        public static Vector2 GetCanvasPositionOfWorldElement(GameObject element) => RectTransformUtility.WorldToScreenPoint(m_MainCamera, element.transform.position);
        public static Vector2 GetCanvasPositionOfWorldPosition(Vector3 element) => RectTransformUtility.WorldToScreenPoint(m_MainCamera, element);

        public static void DeleteChildren(this Transform transform)
        {
            foreach (Transform child in transform)
            {
                Object.Destroy(child.gameObject);
            }
        }
        public static Transform GetMainParent(this Transform transform)
        {
            Transform mainParent = transform;
            while (mainParent.parent is not null)
            {
                mainParent = mainParent.parent;
            }
            return mainParent;
        }
        public static void ResetLocalTransformation(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }
        public static void ResetTransformation(this Transform transform)
        {
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }
        public static void BooleanMessage(bool check, string trueCheckMessage, string falseCheckMessage, Object context = null)
        {
            trueCheckMessage ??= "";
            falseCheckMessage ??= "";
            string msg = check  ? trueCheckMessage : falseCheckMessage;
            if (string.IsNullOrEmpty(msg)) return;
            if (context == null) Debug.Log(msg);
            else Debug.Log(msg, context);
        }
    }
}