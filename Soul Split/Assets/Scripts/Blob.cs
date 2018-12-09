﻿using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blob : MonoBehaviour {

    public GameObject LesserBlob;
    public GameObject Bullet;
    MonsterStats stats;
    GameObject player;
    GameObject parent;
    public int hp = 999;
    public int blobSize = 1;
    public float rushTimer;
    public bool rush = false;
    AIPath AI;

    void Start()
    {
        rushTimer = Random.Range(3, 5);
        player = GameObject.FindWithTag("Player");
        stats = gameObject.GetComponent<MonsterStats>();
        AI = gameObject.GetComponent<AIPath>();
        parent = gameObject.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (stats.active)
        {
            rushTimer -= Time.deltaTime;
            if (stats.hp < hp)
            {
                if (blobSize == 2 || blobSize == 3)
                {
                    MakeBlobs(2);
                }
                else if (blobSize == 4)
                {
                    SpawnBullets();
                    MakeBlobs(2);
                }
                else if (blobSize == 5)
                {
                    BulletExplosion(20);
                    MakeBlobs(2);
                }
                Destroy(gameObject);
            }
            if(blobSize == 5 && rushTimer < 0 && !rush)
            {
                GetComponent<AIPath>().enabled = false;
                GetComponent<Seeker>().enabled = false;
                GetComponent<AIDestinationSetter>().enabled = false;
                rush = true;
                Vector3 p = player.transform.position;
                StartCoroutine(Rush(p));
            }
        }
    }

    IEnumerator Rush(Vector3 player_pos)
    {
        yield return new WaitForSeconds(1);
        Vector3 direction = player_pos - transform.position;
        direction.Normalize();
        gameObject.GetComponent<Rigidbody2D>().velocity = direction * 10;
        StartCoroutine(Stop());
    }

    IEnumerator Stop()
    {
        yield return new WaitForSeconds(8);
        GetComponent<AIPath>().enabled = true;
        GetComponent<Seeker>().enabled = true;
        GetComponent<AIDestinationSetter>().enabled = true;
        rushTimer = Random.Range(5, 10);
        Vector3 s = new Vector3(0, 0, 0);
        GetComponent<Rigidbody2D>().velocity = s;
        rush = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (blobSize == 5 && collision.gameObject.tag != "Player")
        {
            SpawnManyBullets();
        }
    }

    void SpawnManyBullets()
    {
        GameObject b1 = Instantiate(Bullet, transform.position, Quaternion.identity);
        GameObject b2 = Instantiate(Bullet, transform.position, Quaternion.identity);
        GameObject b3 = Instantiate(Bullet, transform.position, Quaternion.identity);
        GameObject b4 = Instantiate(Bullet, transform.position, Quaternion.identity);
        GameObject b5 = Instantiate(Bullet, transform.position, Quaternion.identity);
        GameObject b6 = Instantiate(Bullet, transform.position, Quaternion.identity);
        GameObject b7 = Instantiate(Bullet, transform.position, Quaternion.identity);
        GameObject b8 = Instantiate(Bullet, transform.position, Quaternion.identity);
        Vector3 v = new Vector3();
        v.x = 1;
        v.y = 1;
        b1.GetComponent<Rigidbody2D>().velocity = v * 2; //1,1
        v.y = -1;
        b2.GetComponent<Rigidbody2D>().velocity = v * 2; //1,-1
        v.x = -1;
        b3.GetComponent<Rigidbody2D>().velocity = v * 2; //-1,-1
        v.y = 1;
        b4.GetComponent<Rigidbody2D>().velocity = v * 2; //-1,1
        v.x = 0;
        b5.GetComponent<Rigidbody2D>().velocity = v * 3; //0,1
        v.y = -1;
        b6.GetComponent<Rigidbody2D>().velocity = v * 3; //0,-1
        v.y = 0;
        v.x = 1;
        b7.GetComponent<Rigidbody2D>().velocity = v * 3; //1,0
        v.x = -1;
        b8.GetComponent<Rigidbody2D>().velocity = v * 3; //-1,0
    }


    void SpawnBullets()
    {
        GameObject b1 = Instantiate(Bullet, transform.position, Quaternion.identity);
        GameObject b2 = Instantiate(Bullet, transform.position, Quaternion.identity);
        GameObject b3 = Instantiate(Bullet, transform.position, Quaternion.identity);
        GameObject b4 = Instantiate(Bullet, transform.position, Quaternion.identity);
        Vector3 v = new Vector3();
        v.x = 1;
        v.y = 1;
        b1.GetComponent<Rigidbody2D>().velocity = v * 3; //1,1
        v.y = -1;
        b2.GetComponent<Rigidbody2D>().velocity = v * 3; //1,-1
        v.x = -1;
        b3.GetComponent<Rigidbody2D>().velocity = v * 3; //-1,-1
        v.y = 1;
        b4.GetComponent<Rigidbody2D>().velocity = v * 3; //-1,1
    }

    void BulletExplosion(int numberOfBullets)
    {
        Vector3 v3 = new Vector3();
        for (int i = 0; i < numberOfBullets; i++)
        {
            GameObject b1 = Instantiate(Bullet, transform.position, Quaternion.identity);
            v3.x += (Random.Range(-10f, 11f) / 10) * 4;
            v3.y += (Random.Range(-10f, 11f) / 10) * 4;
            v3.Normalize();
            b1.GetComponent<Rigidbody2D>().velocity = v3 * 4;
        }
    }

    void MakeBlobs(int numOfBlobs)
    {
        Vector3 v = new Vector3();
        for (int i = 0; i < numOfBlobs; i++)
        {
            v.x = transform.position.x + ((Random.Range(-0.500f, 0.501f)));
            v.y = transform.position.y + ((Random.Range(-0.500f, 0.501f)));
            GameObject blob = Instantiate(LesserBlob, v, Quaternion.identity);
            blob.transform.SetParent(parent.transform);
            blob.GetComponent<AIPath>().enabled = true;
            blob.GetComponent<Seeker>().enabled = true;
            blob.GetComponent<AIDestinationSetter>().enabled = true;
            blob.GetComponent<AIPath>().maxSpeed = blob.GetComponent<MonsterStats>().speed;

        }
        Destroy(gameObject);

    }

}