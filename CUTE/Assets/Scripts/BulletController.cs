using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private float startX, startY, toX, toY;
    private Vector2 add;
    private Rigidbody2D body;
    public GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        Transform pBody = GameObject.FindWithTag("Player").GetComponent<Transform>();
        startX = pBody.position.x;
        startY = pBody.position.y;

        Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        toX = target.x;
        toY = target.y;

        add = new Vector2(toX - startX, toY - startY).normalized * 0.4f;
        gameObject.transform.position = new Vector3(startX, startY) + (Vector3)(add * 3.2f);
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameObject.FindWithTag("Player"))
            Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        body.position += add;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Enemy" && collision.gameObject.tag != "Boundary")
            return;
        Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
