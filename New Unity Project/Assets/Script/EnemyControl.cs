﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    public GameObject weapon;
    public GameObject Home;
    public float MoveSpeed;
    public float CounteredTime;
    public float HitHomeCoolDown;
    //public float HitForce;
    public float DistanceBetweenPlayer;
    public float DistanceBetweenHome;
    public float PrepareScale;
    public float BulletMaxDistance;
    public float BulletMinDistance;
    public int AttackSpeedScale;
    public float BulletAttackCoolDown;

    private float timer;
    private bool attacked;
    public bool attack;
    public  bool HitHome;
    public bool counter;
    private float CounteredTimeP;
    private float HitTimer;
    private bool PlayerNearBy;
    private float Distance;
    private float s;
    private int i;
    public bool DoAttack;
    private bool HomeNearBy;
    public float stop_t;
    private bool BulletAttack;
    private float BullentTimer;

    public GameObject bullet;
    // Start is called before the first frame update
    void Start()
    {
        Home = GameObject.FindWithTag("Home");
        counter = false;
        attack = true;
        attacked = false;
        CounteredTimeP = CounteredTime;
        HitHome = false;
        HitTimer = HitHomeCoolDown;
        s = 0;
        i = 1;
        DoAttack = false;
        BullentTimer = BulletAttackCoolDown;
    }
    
    // Update is called once per frame
    void Update()
    {
        GameObject Player;
        if (GameObject.FindWithTag("Player")==true)
        {
            Player = GameObject.FindWithTag("Player");
            Distance = this.transform.position.x - Player.transform.position.x;
            if(Distance > BulletMaxDistance)
            {
                BulletAttack = false;
            }
            else if (BulletMinDistance<Distance && Distance<BulletMaxDistance)
            {
                BulletAttack = true;
            }
            else if (Distance < BulletMinDistance)
            {
                BulletAttack = false;
            }
        }
        else if (GameObject.FindWithTag("Player") == false)
        {
            PlayerNearBy = false;
            Distance = this.transform.position.x - Home.transform.position.x;
            if (Distance < DistanceBetweenHome)
            {
                HomeNearBy = true;
                DoAttack = true;
            }
            else
            {
                HomeNearBy = false;
            }
        }

        if (Distance <= DistanceBetweenPlayer) 
        {
            PlayerNearBy = true;
            DoAttack = true;
        }
        else if (Distance > DistanceBetweenPlayer)
        {
            PlayerNearBy = false;
        }

        if (attacked == false && counter == false && HitHome == false && PlayerNearBy == false && HomeNearBy == false) // Boss 移動控制
        {
            transform.Translate(new Vector3(-MoveSpeed, 0, 0) * Time.deltaTime, Space.World);
        }
        else if (counter == true)//Boss 被反擊時
        {
            if (weapon.transform.eulerAngles.z < 150 || weapon.transform.eulerAngles.z > 300)
            {
                s += Time.deltaTime;
                float x = 500;
                if (s >= 0.2 && x > 0)
                {
                    x = x/2;
                    s = 0;
                }
                weapon.transform.Rotate(Vector3.back * Time.deltaTime * x);
                Debug.Log("Reset");
            } 
            CounteredTime -= Time.deltaTime;
            if (CounteredTime<=0)
            {
                counter = false;
                CounteredTime = CounteredTimeP;
            }
        }

        if(HitHome == true) // 打到基地被彈飛後的計時器
        {
            HitTimer -= Time.deltaTime;
            if(HitTimer <= 0)
            {
                HitHome = false;
                HitTimer = HitHomeCoolDown;
            }
        }

        if (attacked == true) // 被攻擊計時器
        {
            timer += Time.deltaTime;
            if (timer >= stop_t )
            {
                attacked = false;
            }
        }
        if(counter == false)
        {
            Attack();
        }
        //簡易嘴砲
        if (BulletAttack == true)
        {
            if(GameObject.FindWithTag("Player"))
            {
                if (BullentTimer >= BulletAttackCoolDown)
                {
                    Instantiate(bullet, this.transform.position, new Quaternion(0, 0, 0, 0));
                    BullentTimer = 0;
                }
                BullentTimer += Time.deltaTime;
            }
        }

        
    }

    private void OnTriggerEnter(Collider PW)
    {
        if (PW.gameObject.tag == "Weapon" && attacked == false)
        {
            GameObject Player = GameObject.Find("Player");
            attacked = true;
            timer = 0;
            GameManeger.EnemyHP = GameManeger.EnemyHP - GameManeger.Damage_P;
            Debug.Log("Enemy"+GameManeger.EnemyHP);
            if(this.transform.position.x - Player.transform.position.x >=0)
            {
                PlayerControl.AttackEnemy = true;
                Player.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, 0), ForceMode.Impulse);
            }
        }

    }

    void Attack()
    {
        if(DoAttack == true)
        {
            if ((weapon.transform.eulerAngles.z < 90 || weapon.transform.eulerAngles.z > 200)&& attack == true)
            {
                weapon.transform.Rotate(Vector3.forward * Time.deltaTime * i);
                if (weapon.transform.eulerAngles.z > 90 && weapon.transform.eulerAngles.z <95)
                {
                    attack = false;
                }
            }
            else if (attack == false)
            {
                weapon.transform.Rotate(Vector3.back * Time.deltaTime * PrepareScale);
                if (weapon.transform.eulerAngles.z > 300 && weapon.transform.eulerAngles.z < 305)
                {
                    DoAttack = false;
                    attack = true;
                }
            }
            /*else if (weapon.transform.eulerAngles.z > 90 )
            {
                attack = false;
            }*/
                s += Time.deltaTime;
            if (s >= 0.2 && i < 150)
            {
                 i = i * AttackSpeedScale;
                 s = 0;
            }

        }
        else if (DoAttack == false && (weapon.transform.eulerAngles.z <300|| weapon.transform.eulerAngles.z > 305))
        {
            weapon.transform.Rotate(Vector3.back * Time.deltaTime * PrepareScale);
        }
        // Debug.Log(weapon.transform.eulerAngles.z);
    }
}
