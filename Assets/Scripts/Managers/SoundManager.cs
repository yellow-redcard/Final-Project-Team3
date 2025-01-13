using UnityEngine;
using UnityEngine.Audio;

public enum SoundType
{
    Master,  // 마스터 볼륨
    Effects, // 효과음 볼륨
    BGM      // 배경음악 볼륨
}

public class SoundManager : MonoBehaviour
{
    // Singleton Instance
    public static SoundManager Instance;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer; // 오디오 믹서 연결

    private AudioSource audioSource;
    private SkillDatabase skillDatabase;

    private void Awake()
    {
        // Singleton Pattern 설정
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 객체 유지
        }
        else
        {
            Destroy(gameObject); // 기존 인스턴스가 있다면 파괴
        }

        // 오디오 소스 및 데이터베이스 초기화
        audioSource = gameObject.AddComponent<AudioSource>();
        skillDatabase = FindObjectOfType<SkillDatabase>();
    }

    /// <summary>
    /// 스킬 사운드 재생
    /// </summary>
    /// <param name="skillData">스킬 데이터</param>
    public void PlaySkillSound(SkillData skillData)
    {
        if (skillData.skillSound != null)
        {
            audioSource.PlayOneShot(skillData.skillSound);
        }
    }

    /// <summary>
    /// 슬라임 사운드 재생
    /// </summary>
    /// <param name="element">슬라임 속성</param>
    public void PlaySlimeSound(ElementType element)
    {
        // 슬라임 사운드 재생 로직 추가
    }

    /// <summary>
    /// 사운드 볼륨 설정
    /// </summary>
    /// <param name="type">사운드 타입</param>
    /// <param name="volume">볼륨 값 (-80~0 dB)</param>
    public void SetVolume(SoundType type, float volume)
    {
        string parameter = GetMixerParameter(type);
        if (!string.IsNullOrEmpty(parameter))
        {
            audioMixer.SetFloat(parameter, Mathf.Clamp(volume, -80f, 0f));
        }
    }

    /// <summary>
    /// SoundType에 따른 AudioMixer 파라미터 이름 반환
    /// </summary>
    /// <param name="type">사운드 타입</param>
    /// <returns>AudioMixer 파라미터 이름</returns>
    private string GetMixerParameter(SoundType type)
    {
        return type switch
        {
            SoundType.Master => "MasterVolume",
            SoundType.Effects => "EffectsVolume",
            SoundType.BGM => "BGMVolume",
            _ => null,
        };
    }
}
