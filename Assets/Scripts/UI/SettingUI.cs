using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : Singleton<SettingUI>
{
    [SerializeField] private InputField inputField;
    [SerializeField] private Slider slider;
    void Start()
    {
        inputField.onValueChanged.AddListener(OnFieldChanged);
        slider.onValueChanged.AddListener(OnSliderChanged);
    }
    private void OnFieldChanged(string value)
    {
        if (int.TryParse(value, out int value2) && value2 != slider.value)
        {
            slider.value = value2;
        }
    }
    private void OnSliderChanged(float value)
    {
        value = (int)value;
        if (inputField.text != value.ToString())
        {
            inputField.text = value.ToString();
        }
    }
}