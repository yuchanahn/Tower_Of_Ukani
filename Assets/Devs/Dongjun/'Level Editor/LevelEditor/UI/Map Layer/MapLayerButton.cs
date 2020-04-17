using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dongjun.LevelEditor
{
    public class MapLayerButton : MonoBehaviour
    {
        [SerializeField] private Sprite showIcon;
        [SerializeField] private Sprite hideIcon;
        [SerializeField] private GameObject selectedIndicator;

        [SerializeField] private Button selectButton;
        [SerializeField] private Button showHideButton;

        public void Init(Action onSelectButton, Action onShowHideButton)
        {
            selectButton.onClick.AddListener(() => onSelectButton.Invoke());
            showHideButton.onClick.AddListener(() => onShowHideButton.Invoke());
        }
        public void OnSelected(bool value)
        {
            selectedIndicator.SetActive(value);
        }
        public void ShowLayer(bool value)
        {
            showHideButton.GetComponent<Image>().sprite = value ? showIcon : hideIcon;
        }
    }
}

