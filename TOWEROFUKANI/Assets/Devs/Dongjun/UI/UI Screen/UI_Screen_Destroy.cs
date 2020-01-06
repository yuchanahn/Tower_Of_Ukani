using UnityEngine;

public class UI_Screen_Destroy : UI_Screen
{
    public override void Close()
    {
        Destroy(gameObject);
    }
}
