using System.Reflection;
using Nikomata.Runtime;
using UnityEditor;
using UnityEngine;

namespace Nikomata.Editor
{
    [CustomPropertyDrawer(typeof(Automata<>))]
    public class AutomataPropertyDrawer: PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            EditorGUI.BeginProperty(position, label, property);
            object fieldValue = GetSerializedPropertyValue(property);
            if (fieldValue is Automata automata)
            {
                position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
                GUI.Box(position, automata.GetCurrentStateName());
            }
            
            EditorGUI.EndProperty();
        }
        
        public static object GetSerializedPropertyValue(SerializedProperty property)
        {
            object targetObject = property.serializedObject.targetObject;
            string path = property.propertyPath.Replace(".Array.data[", "[");
    
            string[] parts = path.Split('.');
            FieldInfo field = null;
    
            foreach (string part in parts)
            {
                if (part.Contains("[")) // Массив или список
                {
                    string elementName = part.Substring(0, part.IndexOf("["));
                    int index = int.Parse(part.Substring(part.IndexOf("[")).Replace("[", "").Replace("]", ""));
            
                    field = targetObject.GetType().GetField(elementName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    if (field == null) return null;
            
                    object list = field.GetValue(targetObject);
                    if (list is System.Collections.IList iList && index < iList.Count)
                    {
                        targetObject = iList[index];
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    field = targetObject.GetType().GetField(part, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    if (field == null) return null;
                    targetObject = field.GetValue(targetObject);
                }
            }
    
            return targetObject;
        }
    }
}