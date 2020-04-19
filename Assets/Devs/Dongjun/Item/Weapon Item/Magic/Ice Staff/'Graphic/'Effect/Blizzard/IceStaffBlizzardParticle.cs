using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceStaffBlizzardParticle : MonoBehaviour
{
    [SerializeField] private float spinDur;

    private void Start()
    {
        transform.DORotate(new Vector3(0, 0, 180f), spinDur).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
    }
}
