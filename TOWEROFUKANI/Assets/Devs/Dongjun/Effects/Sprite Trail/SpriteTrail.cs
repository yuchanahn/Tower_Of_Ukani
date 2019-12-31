using UnityEngine;

public static class SpriteTrail
{ 
    public static void SpawnTrail(this SpriteRenderer sr, SpriteTrailObject sto, float duration, Transform tf)
    {
        SpriteTrailObject thisSTO = sto.Spawn(tf.position, tf.rotation);
        thisSTO.sleepTimer.EndTime = duration;
        thisSTO.SetSprite(sr);
    }
}
