using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DisableAttribute : PropertyAttribute
{

}

[CustomPropertyDrawer(typeof(DisableAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property,
                                            GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position,
                               SerializedProperty property,
                               GUIContent label)
    {
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
    }
}

[System.Serializable]
public class Test
{
    [Disable] public string a;
    [Disable] public int b;
    [Disable] public Material c;
    [Disable] public List<int> d = new List<int>();
}