using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendChaser : MonoBehaviour
{
    public GameObject explosion;
    public float speed = 10, minDist = 3;
    private Transform playerLoc;
    private GameObject player;
    public int health = 1;
    public bool hitBoundaryHor, hitBoundaryVert;
    public Vector3 Dest { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerLoc = player.GetComponent<Transform>();
        Dest = playerLoc.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameObject.FindWithTag("Player"))
            Destroy(gameObject);
        if (player == null || (GetMode() == ArmyMode.FOLLOWING && Vector2.Distance(transform.position, playerLoc.position) < minDist))
            return;
        SetDest();
        Vector3 vec = playerLoc.position;
        // Vect
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(vec.y - transform.position.y, vec.x - transform.position.x) * Mathf.Rad2Deg - 90);
        transform.position = Vector2.MoveTowards(transform.position, Dest, speed * Time.deltaTime);
    }

    void SetDest()
    {
        Vector3 temp = Dest;
        ArmyMode mode = GetMode();
        if (mode == ArmyMode.FOLLOWING)
            Dest = playerLoc.position;
        if (mode == ArmyMode.FROZEN)
        {
            Dest = transform.position;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        }
        else
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
    }

    private ArmyMode GetMode()
    {
        return player.GetComponent<PlayerController>().mode;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    { 
        if (GetMode() == ArmyMode.SHEILD && collision.gameObject.tag == "Boundary")
        {
            Transform boundary = collision.gameObject.GetComponent<Transform>();
            if (boundary.localScale.x > boundary.localScale.y)
                hitBoundaryVert = true;
            else
                hitBoundaryHor = true;
        }
        if (collision.gameObject.tag != "Enemy")
            return;
        health--;
        if (health == 0)
        {
            Destroy(gameObject); 
            Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (hitBoundaryHor)
            hitBoundaryHor = false;
        if (hitBoundaryVert)
            hitBoundaryVert = false;
    }
}