using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassMover : MonoBehaviour
{
    public int moveX, moveY;
    private int moveUntil;
    private bool allowUpdate;
    private Vector3 saved;

    void Start()
    {
        saved = GetComponent<Transform>().position;
    }

    void Update()
    {
        allowUpdate = !(GetComponent<Transform>().position - saved == moveUntil * new Vector3(moveX, moveY));
        if (!allowUpdate)
            return;
        GetComponent<Transform>().position += new Vector3(0.125f * moveX, 0.125f * moveY);
    }

    public void MoveUntil(int distance)
    {
        allowUpdate = true;
        saved = GetComponent<Transform>().position;
        moveUntil = distance;
    }
}
