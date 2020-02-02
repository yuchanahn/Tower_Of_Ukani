using UnityEngine;

public class TempMapPortal : MonoBehaviour
{
    [SerializeField] private GameObject goToMap;
    [SerializeField] private Transform goToPos;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            goToMap.SetActive(true);
            collision.transform.position = goToPos.position;
        }
    }
}
