using UnityEngine;

public class PopupManager : Singleton<PopupManager>
{
    [SerializeField] private Popup popupPrefab;
    private Color defaultColor = new Color32(0xFF, 0xA6, 0x00, 0xFF);
    public void Pop(string text)
    {
        Popup popup = Instantiate(popupPrefab);
        popup.SetText(text, defaultColor);
    }
    public void Pop(string text, Color color)
    {
        Popup popup = Instantiate(popupPrefab);
        popup.SetText(text, color);
    }
}