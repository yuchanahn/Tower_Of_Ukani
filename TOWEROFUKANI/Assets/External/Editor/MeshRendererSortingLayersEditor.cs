using System;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;
using UnityEngine.Rendering;

[CanEditMultipleObjects()]
[CustomEditor(typeof(MeshRenderer))]
public class MeshRendererSortingLayersEditor : Editor
{

    public override void OnInspectorGUI()
    {

        #region Get Serialized Property
        SerializedProperty sortingLayerID = serializedObject.FindProperty(propertyPath: "m_SortingLayerID");
        SerializedProperty sortingOrder = serializedObject.FindProperty("m_SortingOrder");

        SerializedProperty castShadows = serializedObject.FindProperty("m_CastShadows");
        SerializedProperty receiveShadows = serializedObject.FindProperty("m_ReceiveShadows");
        SerializedProperty motionVectors = serializedObject.FindProperty("m_MotionVectors");
        SerializedProperty materials = serializedObject.FindProperty("m_Materials");
        SerializedProperty lightProbes = serializedObject.FindProperty("m_LightProbeUsage");
        SerializedProperty reflectionProbes = serializedObject.FindProperty("m_ReflectionProbeUsage");
        SerializedProperty anchorProbes = serializedObject.FindProperty("m_ProbeAnchor");
        #endregion

        #region Draw Properties
        AddPropertyField(castShadows);
        AddPropertyField(receiveShadows);
        AddPropertyField(motionVectors);
        AddPropertyField(materials);
        AddPopup(ref lightProbes, "Light Probes", typeof(LightProbeUsage));
        AddPopup(ref reflectionProbes, "Reflection Probes", typeof(ReflectionProbeUsage));
        AddPropertyField(anchorProbes, "Anchor Override");
        #endregion


        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.richText = true;
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("<b><color=#EE4035FF>SortingLayers Options:</color></b>", style);
        #region SortingLayer
        Rect firstHoriz = EditorGUILayout.BeginHorizontal();
        EditorGUI.BeginChangeCheck();
        //    EditorGUI.PropertyField (mat, new GUIContent ("Materials"));
        EditorGUI.BeginProperty(firstHoriz, GUIContent.none, sortingLayerID);
        string[] layerNames = GetSortingLayerNames();
        int[] layerID = GetSortingLayerUniqueIDs();
        int selected = -1;
        int sID = sortingLayerID.intValue;
        for (int i = 0; i < layerID.Length; i++)
            if (sID == layerID[i])
                selected = i;
        if (selected == -1)
            for (int i = 0; i < layerID.Length; i++)
                if (layerID[i] == 0)
                    selected = i;
        selected = EditorGUILayout.Popup("Sorting Layer", selected, layerNames);

        sortingLayerID.intValue = layerID[selected];
        EditorGUI.EndProperty();
        EditorGUILayout.EndHorizontal();
        #endregion

        #region OrderInLayer
        EditorGUILayout.BeginHorizontal();
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(sortingOrder, new GUIContent("Order in Layer"));
        EditorGUILayout.EndHorizontal();
        serializedObject.ApplyModifiedProperties();
        #endregion


    }

    void AddPropertyField(SerializedProperty ourSerializedProperty)
    {
        Rect ourRect = EditorGUILayout.BeginHorizontal();
        EditorGUI.BeginProperty(ourRect, GUIContent.none, ourSerializedProperty);
        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(property: ourSerializedProperty, includeChildren: true); //I set includeChildren:true to display material children

        EditorGUI.EndProperty();
        EditorGUILayout.EndHorizontal();
    }

    void AddPropertyField(SerializedProperty ourSerializedProperty, string name)
    {
        Rect ourRect = EditorGUILayout.BeginHorizontal();
        EditorGUI.BeginProperty(ourRect, GUIContent.none, ourSerializedProperty);
        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(ourSerializedProperty, new GUIContent(name), true);

        EditorGUI.EndProperty();
        EditorGUILayout.EndHorizontal();
    }

    void AddPopup(ref SerializedProperty ourSerializedProperty, string nameOfLabel, Type typeOfEnum)
    {
        Rect ourRect = EditorGUILayout.BeginHorizontal();
        EditorGUI.BeginProperty(ourRect, GUIContent.none, ourSerializedProperty);
        EditorGUI.BeginChangeCheck();

        int actualSelected = 1;
        int selectionFromInspector = ourSerializedProperty.intValue;
        string[] enumNamesList = System.Enum.GetNames(typeOfEnum);
        actualSelected = EditorGUILayout.Popup(nameOfLabel, selectionFromInspector, enumNamesList);
        ourSerializedProperty.intValue = actualSelected;

        EditorGUI.EndProperty();
        EditorGUILayout.EndHorizontal();
    }


    public string[] GetSortingLayerNames()
    {
        Type internalEditorUtilityType = typeof(InternalEditorUtility);
        PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
        return (string[])sortingLayersProperty.GetValue(null, new object[0]);
    }

    public int[] GetSortingLayerUniqueIDs()
    {
        Type internalEditorUtilityType = typeof(InternalEditorUtility);
        PropertyInfo sortingLayerUniqueIDsProperty = internalEditorUtilityType.GetProperty("sortingLayerUniqueIDs", BindingFlags.Static | BindingFlags.NonPublic);
        return (int[])sortingLayerUniqueIDsProperty.GetValue(null, new object[0]);
    }
}
