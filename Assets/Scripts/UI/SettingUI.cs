using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : Singleton<SettingUI>
{
    [SerializeField] private TMP_InputField effectInputField;
    [SerializeField] private Slider effectSlider;
    [SerializeField] private TMP_InputField musicInputField;
    [SerializeField] private Slider musicSlider;
    private GameObject prevGO;
    void Start()
    {
        effectInputField.onValueChanged.AddListener(OnEffectFieldChanged);
        effectSlider.onValueChanged.AddListener(OnEffectSliderChanged);
        musicInputField.onValueChanged.AddListener(OnMusicFieldChanged);
        musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
    }
    private void OnEffectFieldChanged(string value)
    {
        if (int.TryParse(value, out int value2) && value2 != effectSlider.value)
        {
            effectSlider.value = value2;
            GameManager.Instance.SetVolume(value2);
        }
    }
    private void OnEffectSliderChanged(float value)
    {
        value = (int)value;
        if (effectInputField.text != value.ToString())
        {
            effectInputField.text = value.ToString();
            GameManager.Instance.SetVolume((int)value);
        }
    }
    private void OnMusicFieldChanged(string value)
    {
        if (int.TryParse(value, out int value2) && value2 != musicSlider.value)
        {
            musicSlider.value = value2;
            BGM.Instance.SetVolume(value2 / 500f);
        }
    }
    private void OnMusicSliderChanged(float value)
    {
        value = (int)value;
        if (musicInputField.text != value.ToString())
        {
            musicInputField.text = value.ToString();
            BGM.Instance.SetVolume(value / 500f);
        }
    }
    public void SetPrevGO(GameObject go)
    {
        prevGO = go;
    }
    public void ReturnToPrev()
    {
        prevGO.SetActive(true);
    }
}