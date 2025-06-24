using TMPro;
using UnityEngine;

public class Popup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    void Start()
    {
        Destroy(gameObject, 3f);
    }
    public void SetText(string text, Color color)
    {
        textMeshPro.SetText(text);
        textMeshPro.color = color;
    }
}