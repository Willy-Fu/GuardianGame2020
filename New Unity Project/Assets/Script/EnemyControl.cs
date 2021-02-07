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
    public float hitF;
    public float DistanceBetweenPlayer;
    public float DistanceBetweenHome;
    public float PrepareScale;
    public int AttackSpeedScale;

    private float timer;
    private bool attacked;
    private bool attack;
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
    }
    
    // Update is called once per frame
    void Update()
    {
        if(GameObject.FindWithTag("Player")==true)
        {
            GameObject Player = GameObject.FindWithTag("Player");
            Distance = this.transform.position.x - Player.transform.position.x;
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
            if (weapon.transform.eulerAngles.z < 90 || weapon.transform.eulerAngles.z > 300)
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
        bullet.transform.position += new Vector3(-0.1f, 0f, 0f);
        
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
            s += Time.deltaTime;
            if (s >= 0.2 && i < 150)
            {
                 i = i * AttackSpeedScale;
                 s = 0;
            }

        }

       // Debug.Log(weapon.transform.eulerAngles.z);
    }
}
