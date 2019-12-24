using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fadeIn : MonoBehaviour
{
    public Image splashImage;

    IEnumerator Start()
    {
        splashImage.canvasRenderer.SetAlpha(1.0f);

        FadeIn();
        yield return new WaitForSeconds(3.0f);
        splashImage.gameObject.SetActive(false);
    }
    void FadeIn()
    {
        splashImage.CrossFadeAlpha(0.0f, 3.0f, false);
    }
}
