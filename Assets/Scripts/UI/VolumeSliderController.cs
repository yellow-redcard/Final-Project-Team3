using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderController : MonoBehaviour
{
    public Slider masterSlider;
    public Slider effectsSlider;
    public Slider bgmSlider;

    private void Start()
    {
        // Load saved volume values
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0f);
        effectsSlider.value = PlayerPrefs.GetFloat("EffectsVolume", 0f);
        bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 0f);

        // Add listeners to sliders
        masterSlider.onValueChanged.AddListener(value =>
        {
            SoundManager.Instance.SetVolume(SoundType.Master, value); // Use Singleton
            PlayerPrefs.SetFloat("MasterVolume", value); // Save to PlayerPrefs
        });

        effectsSlider.onValueChanged.AddListener(value =>
        {
            SoundManager.Instance.SetVolume(SoundType.Effects, value);
            PlayerPrefs.SetFloat("EffectsVolume", value);
        });

        bgmSlider.onValueChanged.AddListener(value =>
        {
            SoundManager.Instance.SetVolume(SoundType.BGM, value);
            PlayerPrefs.SetFloat("BGMVolume", value);
        });
    }
}
