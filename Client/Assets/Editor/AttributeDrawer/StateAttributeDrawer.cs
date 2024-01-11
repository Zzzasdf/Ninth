using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(StateAttribute))]
public class StateAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // base.OnGUI(position, property, label);
        StateAttribute State = attribute as StateAttribute;
        property.intValue = EditorGUI.MaskField(position, State.propertyName, property.intValue, property.enumDisplayNames);
    }
}