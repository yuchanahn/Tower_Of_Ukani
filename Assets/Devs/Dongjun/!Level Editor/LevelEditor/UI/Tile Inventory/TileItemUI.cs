using System;
using UnityEngine;
using UnityEngine.UI;

public class TileItemUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Button button;

    public Tile TilePrefab
    { get; private set; }

    public void InitData(Sprite sprite, Tile prefab, Action onClick)
    {
        icon.sprite = sprite;
        TilePrefab = prefab;
        button.onClick.AddListener(() => onClick.Invoke());
    }
    public void OnClick(Action onClick)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClick.Invoke());
    }
}
