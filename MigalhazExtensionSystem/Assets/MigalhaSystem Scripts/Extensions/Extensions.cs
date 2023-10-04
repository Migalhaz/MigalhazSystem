using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
        public FloatRange(float _minValue, float _maxValue)
        {
            m_minValue = _minValue;
            m_maxValue = _maxValue;
        }
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

        public IntRange(int _minValue, int _maxValue)
        {
            m_minValue = _minValue;
            m_maxValue = _maxValue;
        }

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

    public static class MigalhazHelper
    {
        #region Camera
        static Camera m_mainCamera;
        public static Camera m_MainCamera
        {
            get
            {
                if (m_mainCamera is null)
                {
                    m_mainCamera = Camera.main;
                }
                return m_mainCamera;
            }
        }

        static Camera m_currentCamera;
        public static Camera m_CurrentCamera
        {
            get 
            {
                if (m_currentCamera is null)
                {
                    m_currentCamera = Camera.current;
                } 
                return m_currentCamera;
            }
        }
        #endregion

        #region WaitForSeconds
        static Dictionary<float, WaitForSeconds> m_waitForSecondsDictionary = new Dictionary<float, WaitForSeconds>();
        public static WaitForSeconds GetWaitForSeconds(float seconds)
        {
            if (m_waitForSecondsDictionary.TryGetValue(seconds, out WaitForSeconds wait)) return wait;
            m_waitForSecondsDictionary[seconds] = new WaitForSeconds(seconds);
            return m_waitForSecondsDictionary[seconds];
        }
        #endregion

        #region MouseOverUI
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
        public static bool IsOverUI()
        {
            m_eventDataCurrentPos = new PointerEventData(m_CurrentEventSystem)
            {
                position = Input.mousePosition
            };
            m_results = new List<RaycastResult>();
            m_CurrentEventSystem.RaycastAll(m_eventDataCurrentPos, m_results);
            return m_results.Count > 0;
        }
        #endregion

        #region RectTransformPosition
        public static Vector2 GetWorldPositionOfCanvasElement(RectTransform element)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(element, element.position, m_MainCamera, out Vector3 worldPoint);
            return worldPoint;
        }
        public static Vector2 GetCanvasPositionOfWorldElement(GameObject element)
        {
            return RectTransformUtility.WorldToScreenPoint(m_MainCamera, element.transform.position);
        }

        #endregion

        #region RangeExtend
        public static float RangeBy0(float _maxInclusive)
        {
            return Random.Range(0f, _maxInclusive);
        }
        public static int RangeBy0(int _maxExclusive)
        {
            return Random.Range(0, _maxExclusive);
        }
        #endregion

        #region TransformExtend
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

        #endregion

        #region CacheGetComponent
        public static T CacheGetComponent<T>(this GameObject obj, ref T component)
        {
            if (component is not null) return component;
            bool get = obj.TryGetComponent(out T getComponent);
            if (get)
            {
                component = getComponent;
            }
            return component;
        }

        public static T CacheGetComponent<T>(this Component obj, ref T component)
        {
            if (component is not null) return component;
            bool get = obj.TryGetComponent(out T getComponent);
            if (get)
            {
                component = getComponent;
            }
            return component;
        }
        #endregion
    }
}