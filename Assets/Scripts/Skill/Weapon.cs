using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id; // 무기 고유 ID
    public int prefabId; // 프리펩 ID
    public float damage; // 데미지
    public int count; //발사체 개수
    public float speed; // 발사 속도

    float timer;
    PlayerPrefs player;
    private void Awake()
    {
        player = GetComponentInParent<PlayerPrefs>();
    }
    private void Start()
    {
        Init();
    }
    private void Update()
    {
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            default:
                timer = Time.deltaTime;

                if(timer > speed)
                {
                    timer = 0f;
                    //Fire();
                }
                break;
        }
        //Test code..
        if (Input.GetButtonDown("Jump"))
        {
            LevelUp(10, 1);
        }
    }

    public void LevelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;

        if (id == 0)
            Batch();
    }
    public void Init()
    {
        switch (id)
        {
            case 0:
                speed = -150;
                break;
            default:
                speed = 0.3f;
                break;
        }
    }
    void Batch()
    {
        for( int index = 0; index < count; index++ )
        {
            Transform skill;
            if(index < transform.childCount)
            {
            skill = transform.GetChild(index);
            }
            else
            {
                skill = GameManager.Instance.poolManager.Get(prefabId).transform;
                skill.parent = transform;
            }   
            
            skill.localPosition = Vector3.zero;
            skill.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * index / count;
            skill.Rotate(rotVec);
            skill.Translate(skill.up * 1.5f, Space.World);
            skill.GetComponent<Skill>().Init(damage, -1,Vector3.zero); // -1 무한
        }
    }

//    private void Fire()
//    {
//        if (!player.scanner.nearestTarget)
//            return;

//        Vector3 targetPos = player.scanner.nearestTarget.position;
//        Vector3 dir = targetPos - transform.position;
//        dir = dir.normalized;
//        Transform skill = GameManager.Instance.poolManager.Get(prefabId).transform;
//        skill.position = transform.position;
//        skill.rotation = Quaternion.FromToRotation(Vector3.up, dir);
//        skill.GetComponent<Skill>().Init(damage, count, dir);
//    }
}
