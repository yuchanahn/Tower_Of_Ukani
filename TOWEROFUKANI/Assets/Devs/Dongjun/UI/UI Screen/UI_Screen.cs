using UnityEngine;

public class UI_Screen : MonoBehaviour
{
    public Canvas thisCanvas
    { get; private set; }

    protected virtual void Awake()
    {
        thisCanvas = GetComponent<Canvas>();
    }

    public virtual void Open()
    {
        // Send Screen To Top
        CloseableScreenManager.Inst.SendScreenToTop(this);

        gameObject.SetActive(true);
    }
    public virtual void Close()
    {
        gameObject.SetActive(false);
    }
}
