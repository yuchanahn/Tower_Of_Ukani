using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VSCreate : MonoBehaviour
{
    [SerializeField] Vector2 pos = Vector2.zero;

    public void SetPosX(InputField text)
    {
        pos.x = float.Parse(text.text);
    }
    public void SetPosY(InputField text)
    {
        pos.y = float.Parse(text.text);
    }

    public void Create(GameObject prefab)
    {
        Instantiate(prefab, pos, Quaternion.identity);
    }
}
