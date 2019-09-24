using UnityEngine;
using UnityEditor;
using System.Linq;

// Mesh Renderer의 Sorting Layer 옵션을 보여주는 에디터 스크립트
// 출처: https://qiita.com/miikun109/items/dada342d23bf58cc879f

[CustomEditor(typeof(MeshRenderer))]
public class MeshRendererExposeSortingLayer : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.BeginHorizontal();

        // sorting order
        SerializedProperty sortOrderProperty = serializedObject.FindProperty("m_SortingOrder");
        sortOrderProperty.intValue = EditorGUILayout.IntField("Sort Order", sortOrderProperty.intValue);

        // sorting layer
        SerializedProperty layerIDProperty = serializedObject.FindProperty("m_SortingLayerID");
        var index = System.Array.FindIndex(SortingLayer.layers, layer => layer.id == layerIDProperty.intValue);
        index = EditorGUILayout.Popup(index, (from layer in SortingLayer.layers select layer.name).ToArray());
        layerIDProperty.intValue = SortingLayer.layers[index].id;

        EditorGUILayout.EndHorizontal();
        serializedObject.ApplyModifiedProperties();

        base.OnInspectorGUI();
    }
}
