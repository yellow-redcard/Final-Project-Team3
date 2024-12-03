using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{    
    public float damage;
    public int projectileCount; // �߻�ü ��

    Rigidbody2D rigid;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    public void Init(SkillType skillType, Attribute attribute, float damage, int projectileCount, Vector3 direction)
    {
        this.damage = damage;
        this.projectileCount = projectileCount;

        if (projectileCount > 0)
        {
            rigid.velocity = direction * 15f;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
            return;
        projectileCount--;
        if (projectileCount < 0)
        {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }
    private void DisableSkill() // ��Ȱ��ȭ ��ų 
    {
        rigid.velocity = Vector2.zero;
        gameObject.SetActive(false);
    }
}
