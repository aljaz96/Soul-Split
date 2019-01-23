﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour {

    // Use this for initialization
    public float timer;
    float totalTime;
    public int type = 1;
    Animator anim;
    MonsterStats stats;
    bool hatched = false;

	void Start () {
        anim = GetComponent<Animator>();
        stats = GetComponent<MonsterStats>();
        if (type == 1)
        {
            totalTime = timer = Random.Range(1.00f, 7.00f);
        }

	}
	
	// Update is called once per frame
	void Update () {
        if (stats.active)
        {
            timer -= Time.deltaTime;
            if (timer < 0 && !hatched)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector3(0,0,0);
                anim.SetTrigger("Hatch");
                hatched = true;
                Destroy(GetComponent<CircleCollider2D>());
                GameObject spoder = Instantiate(Resources.Load("Prefabs/Hatchling", typeof(GameObject)) as GameObject, transform.position, Quaternion.identity);
                spoder.transform.SetParent(transform.parent);
                spoder.GetComponent<MonsterStats>().timer = 0;
                spoder.GetComponent<MonsterStats>().wait = 0;
                transform.SetParent(null);
                Destroy(this);
            }
        }
	}
}