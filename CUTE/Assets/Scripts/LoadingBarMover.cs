using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingBarMover : MonoBehaviour
{
    public Sprite[] sprites;
    public float animationSpeed;
    public Image img;


    public void Update()
    {
        StartCoroutine(NukeMethod());
    }

    public IEnumerator NukeMethod()
    {
        //destroy all game objects
        for (int i = 0; i < sprites.Length; i = (i + 1) % sprites.Length)
        {
            img.sprite = sprites[i];
            yield return new WaitForSeconds(animationSpeed);
        }
    }
}
