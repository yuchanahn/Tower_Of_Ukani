using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPotal : MonoBehaviour
{
    [SerializeField] Transform Tp;
    [SerializeField] GameObject Map2;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            GM.Player.transform.position = Tp.position;
            Map2.SetActive(true);
        }
    }
}
