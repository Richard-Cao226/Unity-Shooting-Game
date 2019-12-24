using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBackgroundMover : MonoBehaviour
{
    private float scrollSpeed = 0.1f;
    private MeshRenderer render;

    private void Awake()
    {
        render = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        render.sharedMaterial.SetTextureOffset("_MainTex", new Vector2(Time.time * scrollSpeed, 0));
    }
}
