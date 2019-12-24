﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrenemyChaser : MonoBehaviour
{
    public GameObject friend, explosion;
    public float speed = 10, minDist = 0;
    private Transform dest;
    public int health = 1;
    private bool blink;
    private float savedTime;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player)
            dest = player.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (blink && (int)GetComponent<SpriteRenderer>().color.b == 1)
            GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
        else if (blink && Time.time - savedTime > 0.125)
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
            blink = false;
        }
        if (!GameObject.FindWithTag("Player"))
            Destroy(gameObject);
        if (!dest || Vector2.Distance(transform.position, dest.position) < minDist)
            return;
        Vector3 vec = dest.position;
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(vec.y - transform.position.y, vec.x - transform.position.x) * Mathf.Rad2Deg - 90);
        transform.position = Vector2.MoveTowards(transform.position, dest.position, speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
            return;
        health--;
        if (health == 0)
        {
            blink = true;
            savedTime = Time.time;
            if (collision.gameObject.name == "Bullet(Clone)" || collision.gameObject.name == "Friend(Clone)")
                SoundManager.PlaySound("coinSound");
            Vector3 pos = gameObject.transform.position;
            Destroy(gameObject);
            if (collision.gameObject.name == "Bullet(Clone)" || collision.gameObject.name == "Friend(Clone)")
                Instantiate(friend, pos, Quaternion.identity);
            Instantiate(explosion, pos, Quaternion.identity);
        }
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Boundary")
            return;
        Destroy(collision.gameObject);
    }
}
