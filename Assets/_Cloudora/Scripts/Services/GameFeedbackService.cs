using UnityEngine;

namespace Cloudora.Services
{
    public sealed class GameFeedbackService : MonoBehaviour
    {
        [SerializeField] private AudioClip selectClip;
        [SerializeField] private AudioClip moveClip;
        [SerializeField] private AudioClip invalidClip;
        [SerializeField] private AudioClip solvedClip;
        [SerializeField] private AudioClip completeClip;

        private AudioSource _source;

        public bool SoundEnabled
        {
            get => PlayerPrefs.GetInt("cloudora.sound", 1) == 1;
            set => PlayerPrefs.SetInt("cloudora.sound", value ? 1 : 0);
        }

        public bool HapticsEnabled
        {
            get => PlayerPrefs.GetInt("cloudora.haptics", 1) == 1;
            set => PlayerPrefs.SetInt("cloudora.haptics", value ? 1 : 0);
        }

        private void Awake()
        {
            _source = gameObject.AddComponent<AudioSource>();
            _source.playOnAwake = false;
        }

        public void Select() => Play(selectClip);
        public void Move() => Play(moveClip);
        public void Solved() => Play(solvedClip);
        public void Complete() => Play(completeClip);

        public void Invalid()
        {
            Play(invalidClip);
            if (HapticsEnabled && Application.isMobilePlatform)
            {
                Handheld.Vibrate();
            }
        }

        private void Play(AudioClip clip)
        {
            if (SoundEnabled && clip != null)
            {
                _source.PlayOneShot(clip);
            }
        }
    }
}
