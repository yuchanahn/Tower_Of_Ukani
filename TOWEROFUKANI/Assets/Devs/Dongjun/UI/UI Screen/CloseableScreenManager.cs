using UnityEngine;

public class CloseableScreenManager : MonoBehaviour
{
    #region Var: Singleton
    public static CloseableScreenManager Inst
    { get; private set; }
    #endregion

    #region Method: Unity
    protected virtual void Awake()
    {
        Inst = this;
    }
    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            CloseRecentMenu();
    }
    #endregion

    #region Method: Helper
    public virtual UI_Screen GetFirstActiveScreen()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf)
                return transform.GetChild(i).GetComponent<UI_Screen>();
        }

        return null;
    }
    #endregion

    #region Method: Closeable Screen
    public void SendScreenToTop(UI_Screen screen)
    {
        screen.transform.SetAsFirstSibling();

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf)
                transform.GetChild(i).GetComponent<UI_Screen>().thisCanvas.sortingOrder = -i;
        }
    }
    public virtual void CloseRecentMenu()
    {
        GetFirstActiveScreen()?.Close();
    }
    #endregion
}
