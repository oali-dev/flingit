using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance = null;

    [SerializeField]
    private AudioClip[] _audioClips = null;
    [SerializeField]
    private AudioSource _audioSourcePrefab = null;

    public enum SoundType
    {
        HIT_WALL,
        HIT_FORCE_FIELD,
        COLLECTED_POINT,
        DIED,
        BEAT_LEVEL,
        LAUNCH_BALL
    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            GameObject.DontDestroyOnLoad(this);
        }
    }

    public void PlaySoundEffect(SoundType soundType)
    {
        AudioSource audioSource = Instantiate(_audioSourcePrefab);
        audioSource.clip = _audioClips[(int)soundType];
        audioSource.Play();

        float audioClipLength = audioSource.clip.length;
        Destroy(audioSource, audioClipLength);
    }
}
