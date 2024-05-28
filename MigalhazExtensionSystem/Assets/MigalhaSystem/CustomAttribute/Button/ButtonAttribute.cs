using System;
using UnityEngine;

namespace MigalhaSystem
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ButtonAttribute : Attribute
    {
		public string m_MethodName;
		public ButtonAttribute(string methodName)
		{
            m_MethodName = methodName;
        }
	}
}