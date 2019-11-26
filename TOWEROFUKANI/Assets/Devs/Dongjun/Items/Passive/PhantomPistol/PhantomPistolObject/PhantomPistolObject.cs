using UnityEngine;

public class PhantomPistolObject : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform shootPos;
    [SerializeField] private Transform spriteRoot;

    Vector2 targetPos;

    private void FixedUpdate()
    {
        targetPos = GM.PlayerPos;
        targetPos.x += GM.Player.Dir * -0.5f;
        targetPos.y += 0.8f;

        transform.position = Vector2.MoveTowards(
            transform.position, targetPos, Vector2.Distance(targetPos, transform.position) * 5f * Time.fixedDeltaTime);
    }
    private void LateUpdate()
    {
        LookAtMouse_Logic.AimedWeapon(Global.Inst.MainCam, spriteRoot, transform);
    }

    public Bullet SpawnBullet()
    {
        return bulletPrefab.Spawn(shootPos.position, shootPos.rotation);
    }
}
