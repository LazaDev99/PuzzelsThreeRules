using Game.Controllers;
using UnityEngine.UI;
using UnityEngine;


namespace Game.UI.Panels
{
    /// <summary>
    /// Class that handles the settings panel in the game.
    /// Inherits from BasePanel and provides functionality for toggling sound effects and adjusting music volume.
    /// </summary>
    public class SettingsPanel : BasePanel
    {
        [SerializeField] private Toggle soundEffectsToggle;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Button closeButton;
        protected override void Awake()
        {
            base.Awake();
            panelType = PanelType.Settings;

            // Button listeners
            soundEffectsToggle.onValueChanged.AddListener(OnSoundToggle);
            musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            closeButton.onClick.AddListener(() =>
            {
                PanelsController.Instance.HidePanel(panelType);
                AudioController.Instance.PlaySound(clickSFX);
            });
        }

        public override void Open()
        {
            base.Open();

            soundEffectsToggle.isOn = PlayerPrefs.GetInt("sound_enabled", 1) == 1;
            musicSlider.value = PlayerPrefs.GetFloat("music_volume", 1.0f);
        }

        private void OnSoundToggle(bool isOn)
        {
            AudioController.Instance.PlaySound(clickSFX);
            PlayerPrefs.SetInt("sound_enabled", isOn ? 1 : 0);
            PlayerPrefs.Save();
            AudioController.Instance.SFXOnOff(isOn);
        }

        private void OnMusicVolumeChanged(float volume)
        {
            PlayerPrefs.SetFloat("music_volume", volume);
            AudioController.Instance.MusicVolume(volume);
            PlayerPrefs.Save();
        }
    }
}
