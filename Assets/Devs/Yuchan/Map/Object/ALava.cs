using System.Collections.Generic;
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
    [SerializeField]
    private AnimationCurve curve;
    [SerializeField]
    private float Damage;
    float dt = 0;

    StatusID SE_ID = new StatusID();



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
    List<Corpse> corpses = new List<Corpse>();
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
                //Destroy(mob.gameObject);
                mob.GetComponent<IDamage>().Hit(new AttackData(Damage));
                StatusEffect_Knokback.Create(mob.gameObject, Vector2.up, 10, 0.1f);
            }

            var corpse = overlaps[i].GetComponent<Corpse>();

            if (corpse != null && rb2D != null && !corpses.Contains(corpse))
            {
                rb2D.velocity = new Vector2(0, speed - corpseFallSpeed);
                rb2D.isKinematic = true;
                corpses.Add(corpse);
                corpse.DestroyOfTime( 3f, ()=> { corpses.Remove(corpse.GetComponent<Corpse>()); } );
            }

            if (overlaps[i].CompareTag("Player"))
            {
                PlayerStatus.AddEffect(new PlayerStatus_Knockback(SE_ID, gameObject, KnockbackMode.Strong, true, Vector2.up, curve));
                dt += Time.fixedDeltaTime;
                if (dt > 0.1f)
                {
                    dt = 0;
                    PlayerStats.Inst.Damage(Damage);
                }
                return;
            }
        }
    }
}