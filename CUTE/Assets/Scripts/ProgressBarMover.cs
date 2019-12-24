using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBarMover : MonoBehaviour
{
    private float moveUntil;
    private bool allowUpdate;
    private float saved;
    private TimelineUpdater updater;

    void Start()
    {
        saved = GetComponent<RectTransform>().rect.width;
        updater = GameObject.Find("Player").GetComponent<TimelineUpdater>();
    }

    void Update()
    {
        allowUpdate = !(GetComponent<RectTransform>().rect.width - saved >= moveUntil);
        if (!allowUpdate)
            return;
        Vector2 moved = GetComponent<RectTransform>().anchoredPosition + new Vector2(56f / 80, 0);
        GetComponent<RectTransform>().anchoredPosition = moved;
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, GetComponent<RectTransform>().rect.width + 1 / 200f);
    }

    public void MoveUntil(float distance)
    {
        allowUpdate = true;
        saved = GetComponent<RectTransform>().rect.width;
        moveUntil = distance;
    }
}
