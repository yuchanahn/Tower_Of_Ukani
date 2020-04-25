using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObj : MonoBehaviour
{
    public bool isInteractive = true;


    /// <summary>
    /// InteractiveManager에서 이 메소드의 반환값을 상호작용중인 오브젝트로 받는다.
    /// </summary>
    /// <returns></returns>
    public virtual InteractiveObj Interact()
    {
        return null;
    }
}
