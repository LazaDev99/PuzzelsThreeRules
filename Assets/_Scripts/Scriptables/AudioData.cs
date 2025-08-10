using System.Diagnostics.Contracts;
using UnityEngine;


namespace Game.Scriptables
{
    [CreateAssetMenu(fileName = "NewAudioData", menuName = "Audio/Audio Data", order = 1)]
    public class AudioData : ScriptableObject
    {
        public AudioClip audioClip;
        public float volume = 1.0f;
        public float pitch = 1.0f;
        public bool loop = false;

        public AudioCategory audioCategory = AudioCategory.SFX;

        private void OnValidate()
        {
            Contract.Requires(audioClip != null, "AudioClip cannot be null");
            Contract.Requires(volume >= 0.0f && volume <= 1.0f, "Volume must be between 0.0 and 1.0");
            Contract.Requires(pitch >= 0.1f && pitch <= 3.0f, "Pitch must be between 0.1 and 3.0");
        }
    }
}

public enum AudioCategory
{
    SFX,
    Music,
    Ambient,
    UI
}
