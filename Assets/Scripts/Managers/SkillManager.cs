using System.Collections.Generic;
using UnityEngine;

    public class SkillManager : MonoBehaviour, IManager
    {
        public enum Element { Dark, Electricity, Flame, Water }

        private Dictionary<Element, Dictionary<Skill.SkillType, int>> skillPrefabIndices;
        public PoolManager poolManager;
        private List<Skill> activeSkills = new List<Skill>(); // 활성화된 스킬 리스트

        private Element currentElement;

        public void init()
        {
            skillPrefabIndices = new Dictionary<Element, Dictionary<Skill.SkillType, int>>();
            LoadSkillPrefabs();
        }

        private void LoadSkillPrefabs()
        {
            foreach (Element element in System.Enum.GetValues(typeof(Element)))
            {
                Dictionary<Skill.SkillType, int> elementSkills = new Dictionary<Skill.SkillType, int>();

                for (int i = 0; i < poolManager.prefabs.Length; i++)
                {
                    string prefabName = poolManager.prefabs[i].name;

                    if (prefabName.Contains($"{element}Single"))
                        elementSkills[Skill.SkillType.Single] = i;
                    else if (prefabName.Contains($"{element}Cone"))
                        elementSkills[Skill.SkillType.Cone] = i;
                    else if (prefabName.Contains($"{element}Line"))
                        elementSkills[Skill.SkillType.Line] = i;
                    else if (prefabName.Contains($"{element}Area"))
                        elementSkills[Skill.SkillType.Area] = i;
                }

                skillPrefabIndices[element] = elementSkills;
            }
        }

        public void SetCurrentElement(Element element)
        {
            currentElement = element;
        }

        public void FireSkill(Skill.SkillType skillType, Vector3 spawnPosition)
        {
            if (skillPrefabIndices.ContainsKey(currentElement) && skillPrefabIndices[currentElement].ContainsKey(skillType))
            {
                int prefabIndex = skillPrefabIndices[currentElement][skillType];
                GameObject skillInstance = poolManager.Get(prefabIndex);
                skillInstance.transform.position = spawnPosition;
                skillInstance.SetActive(true);

                Skill skill = skillInstance.GetComponent<Skill>();
                if (skill != null)
                {
                    activeSkills.Add(skill); // 활성화된 스킬 리스트에 추가
                    skill.UseSkill();
                }
            }
            else
            {
                Debug.LogError($"Skill not found for {currentElement} - {skillType}");
            }
        }

        public void AdjustFireRate(float rate)
        {
            foreach (var skill in activeSkills)
            {
                skill.cooldown *= (1f - rate); // 발사 속도 증가
            }
            Debug.Log($"[SkillManager] 스킬 발사 속도 조정! Rate: {rate}");
        }

        public void release()
        {
            // 활성화된 스킬 리스트 정리
            foreach (var skill in activeSkills)
            {
                skill.Deactivate(); // 비활성화
            }
            activeSkills.Clear();
            Debug.Log("[SkillManager] 모든 스킬 정리 완료.");
        }
    }

