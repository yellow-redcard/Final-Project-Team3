using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    // 사운드 클립을 속성별로 저장
    private Dictionary<ElementType, AudioClip[]> skillSounds;
    private Dictionary<ElementType, AudioClip> slimeSounds;
    private Dictionary<string, AudioClip> monsterSounds;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        // 사운드 데이터 초기화
        InitializeSoundData();
    }

    private void InitializeSoundData()
    {
        // 스킬 사운드 초기화
        skillSounds = new Dictionary<ElementType, AudioClip[]>
        {
            { ElementType.Flame, new[] {
                Resources.Load<AudioClip>("Sounds/FlameSingle"),
                Resources.Load<AudioClip>("Sounds/FlameArea"),
                Resources.Load<AudioClip>("Sounds/FlameCone"),
                Resources.Load<AudioClip>("Sounds/FlameLine")
            }},
            { ElementType.Water, new[] {
                Resources.Load<AudioClip>("Sounds/WaterSingle"),
                Resources.Load<AudioClip>("Sounds/WaterArea"),
                Resources.Load<AudioClip>("Sounds/WaterCone"),
                Resources.Load<AudioClip>("Sounds/WaterLine")
            }},
            { ElementType.Electricity, new[] {
                Resources.Load<AudioClip>("Sounds/ElectricitySingle"),
                Resources.Load<AudioClip>("Sounds/ElectricityArea"),
                Resources.Load<AudioClip>("Sounds/ElectricityCone"),
                Resources.Load<AudioClip>("Sounds/ElectricityLine")
            }},
            { ElementType.Dark, new[] {
                Resources.Load<AudioClip>("Sounds/DarkSingle"),
                Resources.Load<AudioClip>("Sounds/DarkArea"),
                Resources.Load<AudioClip>("Sounds/DarkCone"),
                Resources.Load<AudioClip>("Sounds/DarkLine")
            }}
        };

        // 슬라임 사운드 초기화
        slimeSounds = new Dictionary<ElementType, AudioClip>
        {
            { ElementType.Flame, Resources.Load<AudioClip>("Sounds/FlameSlime") },
            { ElementType.Water, Resources.Load<AudioClip>("Sounds/WaterSlime") },
            { ElementType.Electricity, Resources.Load<AudioClip>("Sounds/ElectricitySlime") },
            { ElementType.Dark, Resources.Load<AudioClip>("Sounds/DarkSlime") }
        };

        // 몬스터 사운드 초기화
        monsterSounds = new Dictionary<string, AudioClip>
        {
            { "Bat", Resources.Load<AudioClip>("Sounds/Monsters/BatSound") },
            { "Bird", Resources.Load<AudioClip>("Sounds/Monsters/BirdSound") },
            { "Bunny", Resources.Load<AudioClip>("Sounds/Monsters/BunnySound") },
            { "Cattle", Resources.Load<AudioClip>("Sounds/Monsters/CattleSound") },
            { "Dog", Resources.Load<AudioClip>("Sounds/Monsters/DogSound") },
            { "Feline", Resources.Load<AudioClip>("Sounds/Monsters/FelineSound") },
            { "Giant", Resources.Load<AudioClip>("Sounds/Monsters/GiantSound") },
            { "Horse", Resources.Load<AudioClip>("Sounds/Monsters/HorseSound") },
            { "Insect", Resources.Load<AudioClip>("Sounds/Monsters/InsectSound") },
            { "Mimic", Resources.Load<AudioClip>("Sounds/Monsters/MimicSound") },
            { "Rat", Resources.Load<AudioClip>("Sounds/Monsters/RatSound") },
            { "Scorpion", Resources.Load<AudioClip>("Sounds/Monsters/ScorpionSound") }
        };
    }

    public void PlaySkillSound(ElementType element, int skillIndex)
    {
        if (skillSounds.ContainsKey(element) && skillIndex >= 0 && skillIndex < skillSounds[element].Length && skillSounds[element][skillIndex] != null)
        {
            audioSource.PlayOneShot(skillSounds[element][skillIndex]);
        }
        else
        {
            Debug.LogWarning($"Skill sound not found for {element} at index {skillIndex}. Please ensure the sound is set.");
        }
    }

    public void PlaySlimeSound(ElementType element)
    {
        if (slimeSounds.ContainsKey(element) && slimeSounds[element] != null)
        {
            audioSource.PlayOneShot(slimeSounds[element]);
        }
        else
        {
            Debug.LogWarning($"Slime sound not found for {element}. Please ensure the sound is set.");
        }
    }

    public void PlayMonsterSound(string monsterType)
    {
        if (monsterSounds.ContainsKey(monsterType) && monsterSounds[monsterType] != null)
        {
            audioSource.PlayOneShot(monsterSounds[monsterType]);
        }
        else
        {
            Debug.LogWarning($"Monster sound not found for {monsterType}. Please ensure the sound is set.");
        }
    }

    public void PlayTitleSceneMusic()
    {
        // 타이틀 씬 음악 재생 로직
    }

    public void PlayGameSceneMusic()
    {
        // 게임 씬 음악 재생 로직
    }

    public void StopMusic()
    {
        // 음악 정지 로직
    }
}
