using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemDB))]
public class ItemDBInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ItemDB itemDB = target as ItemDB;

        if (GUILayout.Button("Load All Item Prefabs"))
        {
            itemDB.LoadAllItemPrefabs();
        }
    }
}

