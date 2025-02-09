using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace MigalhaSystem.Extensions
{
    public static class VectorExtension
    {
        static List<Vector2> m_vector2List;
        static List<Vector3> m_vector3List;

        public static Vector2 Provide(float x, float y)
        {
            m_vector2List ??= new();
            if (m_vector2List.CheckAndGet(vector => vector.x == x && vector.y == y, out Vector2 result))
            {
                return result;
            }
            Vector2 expected = new Vector2(x, y);
            m_vector2List.Add(expected);
            return expected;
        }

        public static Vector3 Provide(float x, float y, float z)
        {
            m_vector3List ??= new();
            if (m_vector3List.CheckAndGet(vector => vector.x == x && vector.y == y && vector.z == z, out Vector3 result))
            {
                return result;
            }
            Vector3 expected = new Vector3(x, y);
            m_vector3List.Add(expected);
            return expected;
        }

        public static Vector2 With(this Vector2 _vector2, float? x = null, float? y = null)
        {
            float newX = x is null ? _vector2.x : (float) x;
            float newY = y is null ? _vector2.y : (float) y;
            _vector2.Set(newX, newY);
            return _vector2;
        }
        public static Vector3 With(this Vector3 _vector3, float? x = null, float? y = null, float? z = null)
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
        public static T GetRandom<T>(this IEnumerable<T> enumerable) => enumerable.ElementAt(enumerable.Count().RangeBy0());

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

        public static bool CheckAndGet<T>(this List<T> list, System.Predicate<T> match, out T item)
        {
            if (!list.Exists(match))
            {
                item = default;
                return false;
            }

            item = list.Find(match);
            return true;
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
            color.a = Mathf.Clamp01(alpha);
            return color;
        }
        public static Color Set(this Color color, float? r = null, float? g = null, float? b = null, float? a = null)
        {
            color.r = Mathf.Clamp01(r ?? color.r);
            color.g = Mathf.Clamp01(g ?? color.g);
            color.b = Mathf.Clamp01(b ?? color.b);
            color.a = Mathf.Clamp01(a ?? color.a);
            return color;
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

        public static T GetOrAdd<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            if (!component) component = gameObject.AddComponent<T>();
            return component;
        }

        public static T OrNull<T>(this T obj) where T : Object => obj ? obj : null;
        public static List<GameObject> SortByDistance(this List<GameObject> components, Vector3 position) => components.OrderBy(x => Vector3.Distance(x.transform.position, position)).ToList();
    }

    public static class TransformExtensions
    {
        public static IEnumerable<Transform> Children(this Transform parent)
        {
            yield return parent;
            //foreach (Transform child in parent)
            //{
            //    yield return child;
            //}
        }

        public static void DestroyChildren(this Transform parent) => parent.PerfomActionOnChildren(child => Object.Destroy(child.gameObject));
        public static void SetChildrenActive(this Transform parent, bool active) => parent.PerfomActionOnChildren(child => child.gameObject.SetActive(active));
        public static void PerfomActionOnChildren(this Transform parent, System.Action<Transform> action)
        {
            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                action?.Invoke(parent.GetChild(i));
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
    }

    public static class ComponentExtension
    {
        public static List<T> SortByDistance<T>(this List<T> components, Vector3 position) where T : Component => components.OrderBy(x => Vector3.Distance(x.transform.position, position)).ToList();
    }

    [System.Serializable]
    public struct FloatRange
    {
        public float minValue;
        public float maxValue;
        public float this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0: return minValue;
                    case 1: return maxValue;
                    default: throw new System.IndexOutOfRangeException();
                }
            }
        }

        public float deepness => maxValue - minValue;
        public float value => Random.Range(minValue, maxValue);

        public FloatRange(Vector2 value)
        {
            minValue = value.x;
            maxValue = value.y;
        }
        public FloatRange(Vector3 value)
        {
            minValue = value.x;
            maxValue = value.y;
        }
        public FloatRange(IntRange range)
        {
            minValue = range.minValue;
            maxValue = range.maxValue;
        }
        public FloatRange(float value)
        {
            minValue = value;
            maxValue = value;
        }
        public FloatRange(float _minValue, float _maxValue)
        {
            minValue = _minValue;
            maxValue = _maxValue;
        }

        public void Set(float? minValue = null, float? maxValue = null)
        {
            this.minValue = minValue ?? this.minValue;
            this.maxValue = maxValue ?? this.maxValue;
        }

        public bool InRange(float value) => value >= minValue && value <= maxValue;
        public bool InRange(int value) => value >= minValue && value <= maxValue;

        public float GetRandomValue() => value;
        public float Clamp(float value) => Mathf.Clamp(value, minValue, maxValue);
        public float Lerp(float t) => Mathf.Lerp(minValue, maxValue, t);
        public float InverseLerp(float value) => Mathf.InverseLerp(minValue, maxValue, value);
        public float LerpUnclamped(float t) => Mathf.LerpUnclamped(minValue, maxValue, t);
        public float Deepness() => deepness;

        public static FloatRange operator +(FloatRange a, FloatRange b) => new(a.minValue + b.minValue, a.maxValue + b.maxValue);
        public static FloatRange operator +(FloatRange a, float value) => new(a.minValue + value, a.maxValue + value);
        public static FloatRange operator -(FloatRange a, FloatRange b) => new(a.minValue - b.minValue, a.maxValue - b.maxValue);
        public static FloatRange operator -(FloatRange a, float value) => new(a.minValue - value, a.maxValue - value);
        public static FloatRange operator *(FloatRange a, float value) => new(a.minValue * value, a.maxValue * value);
        public static FloatRange operator /(FloatRange a, float value) => new(a.minValue / value, a.maxValue / value);

        public static bool operator <(FloatRange a, FloatRange b) => a.Deepness() < b.Deepness();
        public static bool operator >(FloatRange a, FloatRange b) => a.Deepness() > b.Deepness();
        public static bool operator <=(FloatRange a, FloatRange b) => a.Deepness() <= b.Deepness();
        public static bool operator >=(FloatRange a, FloatRange b) => a.Deepness() >= b.Deepness();

        public static implicit operator Vector2(FloatRange a) => new Vector2(a.minValue, a.maxValue);
        public static implicit operator Vector3(FloatRange a) => new Vector3(a.minValue, a.maxValue);

        public static implicit operator FloatRange(Vector2 vector) => new FloatRange(vector);
        public static implicit operator FloatRange(Vector3 vector) => new FloatRange(vector);
        public static implicit operator FloatRange(IntRange range) => new FloatRange(range);
    }

    [System.Serializable]
    public struct IntRange
    {
        public int minValue;
        public int maxValue;

        public int this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0: return minValue;
                    case 1: return maxValue;
                    default: throw new System.IndexOutOfRangeException();
                }
            }
        }

        public int deepness => maxValue - minValue;
        public IntRange(Vector2 value)
        {
            minValue = (int)value.x;
            maxValue = (int)value.y;
        }
        public IntRange(Vector3 value)
        {
            minValue = (int)value.x;
            maxValue = (int)value.y;
        }
        public IntRange(FloatRange value)
        {
            minValue = (int)value.minValue;
            maxValue = (int)value.maxValue;
        }
        public IntRange(int value)
        {
            minValue = value;
            maxValue = value;
        }
        public IntRange(int _minValue, int _maxValue)
        {
            minValue = _minValue;
            maxValue = _maxValue;
        }

        public void Set(int? newMinValue = null, int? newMaxValue = null)
        {
            minValue = newMinValue ?? minValue;
            maxValue = newMaxValue ?? maxValue;
        }

        public bool InRange(float value) => value >= minValue && value <= maxValue;
        public bool InRange(int value) => value >= minValue && value <= maxValue;

        public int GetRandomValue(bool _maxInclusive = false) => Random.Range(minValue, maxValue + (_maxInclusive ? 1 : 0));
        public int Clamp(int value) => Mathf.Clamp(value, minValue, maxValue);
        public float Lerp(float t) => Mathf.Lerp(minValue, maxValue, t);
        public float InverseLerp(float value) => Mathf.InverseLerp(minValue, maxValue, value);
        public float LerpUnclamped(float t) => Mathf.LerpUnclamped(minValue, maxValue, t);
        public int Deepness() => maxValue - minValue;

        public static IntRange operator +(IntRange a, IntRange b) => new(a.minValue + b.minValue, a.maxValue + b.maxValue);
        public static IntRange operator +(IntRange a, int value) => new(a.minValue + value, a.maxValue + value);
        public static IntRange operator -(IntRange a, IntRange b) => new(a.minValue - b.minValue, a.maxValue - b.maxValue);
        public static IntRange operator -(IntRange a, int value) => new(a.minValue - value, a.maxValue - value);
        public static IntRange operator *(IntRange a, int value) => new(a.minValue * value, a.maxValue * value);
        public static IntRange operator /(IntRange a, int value) => new(a.minValue / value, a.maxValue / value);
        public static bool operator <(IntRange a, IntRange b) => a.Deepness() < b.Deepness();
        public static bool operator >(IntRange a, IntRange b) => a.Deepness() > b.Deepness();
        public static bool operator <=(IntRange a, IntRange b) => a.Deepness() <= b.Deepness();
        public static bool operator >=(IntRange a, IntRange b) => a.Deepness() >= b.Deepness();

        public static implicit operator Vector2(IntRange a) => new Vector2(a.minValue, a.maxValue);
        public static implicit operator Vector3(IntRange a) => new Vector3(a.minValue, a.maxValue);

        public static implicit operator IntRange(Vector2 vector) => new IntRange(vector);
        public static implicit operator IntRange(Vector3 vector) => new IntRange(vector);
        public static implicit operator IntRange(FloatRange floatRange) => new IntRange(floatRange);
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
        [SerializeField] protected UnityEvent m_onTimerStart;
        [SerializeField] protected UnityEvent m_onTimerElapsed;

        public bool m_Active => m_countdown;
        public List<TimerEvent> m_TimerEvents => m_timerEvents;
        public UnityEvent m_OnTimerElapsed => m_onTimerElapsed;
        public float m_PickedTimerValue => m_pickedTimerValue;
        public float m_CurrentTimerValue => m_currentTimerValue;
        public FloatRange m_StartTimer => m_startTimer;

        public Timer(FloatRange startTimer)
        {
            m_startTimer = startTimer;
            m_timerEvents = new();
            m_onTimerStart = new();
            m_onTimerElapsed = new();
        }
        public Timer(float startTimer)
        {
            m_startTimer = new(startTimer);
            m_timerEvents = new();
            m_onTimerStart = new();
            m_onTimerElapsed = new();
        }
        public Timer(float startTimerMinValue, float startTimerMaxValue)
        {
            m_startTimer = new(startTimerMinValue, startTimerMaxValue);
            m_timerEvents = new();
            m_onTimerStart = new();
            m_onTimerElapsed = new();
        }

        public void SetStartTimer(FloatRange newStartTimer)
        {
            m_startTimer = newStartTimer;
        }
        public void SetStartTimer(float? newMinValue = null, float? newMaxValue = null)
        {
            m_startTimer.Set(newMinValue, newMaxValue);
        }

        public float GetTimeElapsedPercentage() => Mathf.Abs(1 - (m_currentTimerValue/m_pickedTimerValue));

        public void StartTimer()
        {
            SetupTimer();
            m_countdown = true;
            m_onTimerStart?.Invoke();
        }
        public void PauseTimer()
        {
            m_countdown = false;
        }
        public void ReleaseTimer()
        {
            m_countdown = true;
        }
        public void BreakTimer()
        {
            m_currentTimerValue = 0;
        }

        public virtual void SetupTimer()
        {
            m_pickedTimerValue = m_startTimer.GetRandomValue();
            m_currentTimerValue = m_pickedTimerValue;
            m_timerEvents ??= new();
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
            m_onTimerElapsed?.Invoke();
            if (m_repeater)
            {
                StartTimer();
            }
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

    public interface IBuilder<T>
    {
        T Build();
    }

    public static class MigalhazHelper
    {
        static Camera m_mainCamera;
        public static Camera m_MainCamera => m_mainCamera ??= Camera.main;

        static Camera m_currentCamera;
        public static Camera m_CurrentCamera => m_currentCamera ??= Camera.current;

        static Dictionary<float, WaitForSeconds> m_waitForSecondsDictionary = new Dictionary<float, WaitForSeconds>();

        static PointerEventData m_eventDataCurrentPos;
        static List<RaycastResult> m_results;
        
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
            m_eventDataCurrentPos = new PointerEventData(EventSystem.current)
            {
                position = _position
            };
            m_results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(m_eventDataCurrentPos, m_results);
            return m_results.Count > 0;
        }

        public static Vector2 GetWorldPositionOfCanvasElement(RectTransform element)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(element, element.position, m_MainCamera, out Vector3 worldPoint);
            return worldPoint;
        }
        public static Vector2 GetCanvasPositionOfWorldElement(GameObject element) => RectTransformUtility.WorldToScreenPoint(m_MainCamera, element.transform.position);
        public static Vector2 GetCanvasPositionOfWorldPosition(Vector3 element) => RectTransformUtility.WorldToScreenPoint(m_MainCamera, element);

        public static void BooleanLog(bool check, string trueCheckMessage, string falseCheckMessage, Object context = null)
        {
#if DEBUG
            trueCheckMessage ??= "";
            falseCheckMessage ??= "";
            string msg = check ? trueCheckMessage : falseCheckMessage;
            if (string.IsNullOrEmpty(msg)) return;
            if (context == null) Debug.Log(msg);
            else Debug.Log(msg, context);
#endif
        }
    }
}