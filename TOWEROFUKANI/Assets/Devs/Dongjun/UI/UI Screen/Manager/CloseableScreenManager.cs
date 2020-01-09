using UnityEngine;

public class CloseableScreenManager : SingletonBase<CloseableScreenManager>
{
    #region Method: Unity
    protected virtual void Update()
    {
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
        if (Input.GetKeyDown(KeyCode.Escape))
            GetFirstActiveScreen()?.Close();
    }
    #endregion
}
