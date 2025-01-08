using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource audioSource;
    private SkillDatabase skillDatabase;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        skillDatabase = FindObjectOfType<SkillDatabase>();
    }

    public void PlaySkillSound(SkillData skillData)
    {
        if (skillData.skillSound != null)
        {
            audioSource.PlayOneShot(skillData.skillSound);
        }
    }

    public void PlaySlimeSound(ElementType element)
    {
        // 슬라임 사운드 재생 로직 추가
    }
}