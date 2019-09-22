using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform followTarget;

    [SerializeField] private Vector2 followOffset = new Vector2(0, 0);

    private void Update()
    {
        FollowTarget();
    }
    private void FollowTarget()
    {
        Vector2 targetPos = (Vector2)followTarget.position + followOffset;
        transform.position = new Vector3(targetPos.x, targetPos.y, transform.position.z);
    }
}