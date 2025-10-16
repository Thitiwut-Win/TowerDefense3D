using System.Collections.Generic;
using UnityEngine;

public class PopupManager : Singleton<PopupManager>
{
    [SerializeField] private Popup popupPrefab;
    private Color defaultColor = new Color32(0xFF, 0xA6, 0x00, 0xFF);
    private List<Popup> popups = new List<Popup>();
    public void Pop(string text)
    {
        Popup popup = Instantiate(popupPrefab);
        popup.SetText(text, defaultColor);
        popups.Add(popup);
    }
    public void Pop(string text, Color color)
    {
        Popup popup = Instantiate(popupPrefab);
        popup.SetText(text, color);
        popups.Add(popup);
    }
    public void Clear()
    {
        for (int i = popups.Count - 1; i >= 0; i--)
        {
            Destroy(popups[i]);
        }
        popups.Clear();
    }
}