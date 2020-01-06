using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_Screen_ItemContextMenu : UI_Screen_Destroy
{
    [Header("Cursor Offset")]
    [SerializeField] protected Vector2 cursorOffset;

    [Header("Title")]
    [SerializeField] protected Text titleText;

    [Header("Buttons")]
    [SerializeField] protected RectTransform buttonGroup;
    [SerializeField] protected Button buttonPrefab;

    protected virtual void Start()
    {
        // Move To Mouse Pos
        UI_Utility.MoveTo(thisCanvas, buttonGroup, (Vector2)Input.mousePosition + cursorOffset);
    }

    public void SetTitle(string title)
    {
        titleText.text = title;
    }
    public void AddButton<T>(string buttonName, T item, UI_Screen_ItemContextMenu contextMenu, Action<T, UI_Screen_ItemContextMenu> action)
        where T : Item
    {
        Button button = Instantiate(buttonPrefab.gameObject, buttonGroup, false).GetComponent<Button>();
        button.name = buttonName;
        button.GetComponentInChildren<Text>().text = buttonName;
        button.onClick.AddListener(() => { action?.Invoke(item, contextMenu); });
    }
}
