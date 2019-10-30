using UnityEngine;
using UnityEngine.SceneManagement;

public class ALava : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private Transform maxHeight;


    [SerializeField]
    private float corpseFallSpeed;
    [SerializeField]
    private Vector2 size;
    [SerializeField]
    private LayerMask layerMask;

    float dt = 0;

    private void FixedUpdate()
    {
        MoveUp();
        CheckOverlap();
    }

    private void MoveUp()
    {
        if (maxHeight.position.y > transform.position.y + (size.y / 2))
            transform.Translate(speed * transform.up * Time.fixedDeltaTime);
    }
    private void CheckOverlap()
    {
        Collider2D[] overlaps = Physics2D.OverlapBoxAll(transform.position, size, 0f, layerMask);

        if (overlaps == null)
            return;

        for (int i = 0; i < overlaps.Length; i++)
        {
            Rigidbody2D rb2D = overlaps[i].GetComponent<Rigidbody2D>();

            var mob = overlaps[i].GetComponent<Mob_Base>();

            if (mob != null && rb2D != null)
            {
                Destroy(mob.gameObject);
            }

            var corpse = overlaps[i].GetComponent<Corpse>();

            if (corpse != null && rb2D != null)
            {
                rb2D.velocity = new Vector2(0, speed - corpseFallSpeed);
                rb2D.isKinematic = true;
                //overlaps[i].transform.Translate(Vector3.up * (speed - corpseFallSpeed) * Time.fixedDeltaTime);

                corpse.GetComponent<Corpse>().DestroyOfTime( 3f );
            }

            if (overlaps[i].CompareTag("Player"))
            {
                dt += Time.fixedDeltaTime;
                if (dt > 0.1f)
                {
                    dt = 0;
                    PlayerStats.Damage(10);
                }
                return;
            }
        }
    }
}