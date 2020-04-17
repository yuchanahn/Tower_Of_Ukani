using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dongjun.LevelEditor
{
    public class LevelEditorComponent : MonoBehaviour
    {
        protected LevelEditorData data { get; private set; }

        protected virtual void Awake()
        {
            data = GetComponent<LevelEditorData>();
        }
    }
}
