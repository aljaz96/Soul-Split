﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caster : MonoBehaviour {

    GameObject player;
    public GameObject spell;
    MonsterStats stats;
    public float movementTimer;
    float spellTimer;
    float speed;
    int side = -1;
    float velocity;
    public bool casting = false;
    public int type = 0;
    public AudioSource audio;

    void Start()
    {
        stats = gameObject.GetComponent<MonsterStats>();
        speed = stats.speed;
        player = GameObject.FindWithTag("Player");
        spellTimer = Random.Range(1.0f, 4.0f);
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        movementTimer -= Time.deltaTime;
        spellTimer -= Time.deltaTime;
        velocity = gameObject.GetComponent<Rigidbody2D>().velocity.magnitude;
        if (stats.active)
        {
            //do stuff
            if (movementTimer < 0)
            {
                changeDirection();
            }
          
            if (spellTimer < 0 && !casting)
            {
                if (type == 1)
                {
                    casting = true;
                    GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
                    Vector3 v3 = player.transform.position;
                    v3.y = v3.y + 8.1f;
                    v3.x = v3.x + 2.2f;
                    StartCoroutine(CastLightning(0.2f, v3)); 
                }
            }
            if (velocity == 0 && !casting)
            {
                changeDirection();
            }
        }
    }

    IEnumerator CastLightning(float timer, Vector3 player_pos)
    {
        yield return new WaitForSeconds(timer);
        GameObject b1 = Instantiate(spell, player_pos, Quaternion.identity);
        b1.GetComponent<EnemyProjectile>().damage = stats.p_damage;
        audio.Play();
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        yield return new WaitForSeconds(0.25f);
        side = -1;
        spellTimer = Random.Range(2.0f, 6.0f);
        casting = false;
        changeDirection();
    }


    void OnCollisionEnter2D(Collision2D col)
    {
        changeDirection();
    }

    void changeDirection()
    {
        int decision = UnityEngine.Random.Range(1, 5);
        while (decision == side)
        {
            decision = UnityEngine.Random.Range(1, 5);
        }
        side = decision;

        switch (decision)
        {
            case 1:
                GetComponent<Rigidbody2D>().velocity = new Vector3(speed, 0, 0);
                break;
            case 2:
                GetComponent<Rigidbody2D>().velocity = new Vector3(-speed, 0, 0);
                break;
            case 3:
                GetComponent<Rigidbody2D>().velocity = new Vector3(0, -speed, 0);
                break;
            case 4:
                GetComponent<Rigidbody2D>().velocity = new Vector3(0, speed, 0);
                break;
        }
        movementTimer = UnityEngine.Random.Range(1.0f, 6.0f);
    }
}
