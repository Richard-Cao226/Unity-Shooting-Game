
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextRevealer : MonoBehaviour
{
    public TextMeshProUGUI text;
    IEnumerator en, en2;
    string[] texts;
    int index;
    bool coRunning, coRunning2;
    int run;

    void Start()
    {
        texts = new string[] {
            "The cats have acquired brainwashing taser equipment and have turned your friends into foes. It is up to you to save them. But be careful, the cats are out and prowling... [PRESS SPACE]",
            "GOAL: shoot the evil mice to convert them into allied mice. Bank the allied mice to save them, or have them FOLLOW you, SHIELD you, or FREEZE in place. Cats take 3 shots to kill and evil mice take 1 shot to convert. Bank 50 mice to win! [PRESS SPACE]",
        };
        en = RevealText();
        run = 0;
        en2 = RevealText2();
        StartCoroutine(en);
    }


    IEnumerator RevealText()
    {
        
        coRunning = true;
        run++;
        var originalString = text.text;

        var numCharsRevealed = 0;
        while (numCharsRevealed < originalString.Length)
        {
            while (originalString[numCharsRevealed] == ' ')
                ++numCharsRevealed;

            ++numCharsRevealed;

            text.text = originalString.Substring(0, numCharsRevealed);

            yield return new WaitForSeconds(0.07f);
        }
        coRunning = false;
    }

    IEnumerator RevealText2()
    {
        coRunning2 = true;
        run++;
        var originalString = text.text;

        var numCharsRevealed = 0;
        while (numCharsRevealed < originalString.Length)
        {
            while (originalString[numCharsRevealed] == ' ')
                ++numCharsRevealed;

            ++numCharsRevealed;

            text.text = originalString.Substring(0, numCharsRevealed);

            yield return new WaitForSeconds(0.07f);
        }
        coRunning = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            if (coRunning&&run<=2)
            {
                StopCoroutine(en);
                text.text = texts[0];
                coRunning = false;
            }
            else if (coRunning2&&run<=2)
            {
                StopCoroutine(en2);
                text.text = texts[1];
                coRunning2 = false;
            }
            else if (!coRunning && index == 0)
            {
                text.text = texts[1];
                StartCoroutine(en2);
                index = 1;
            }
            else 
            {
                text.text = texts[(index + 1)% 2];

                index++;

                StopCoroutine(en);
                StopCoroutine(en2);
            }
        }
    }
}
