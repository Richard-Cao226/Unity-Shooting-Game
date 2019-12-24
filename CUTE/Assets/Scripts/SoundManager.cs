using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static AudioClip catSound, coinSound, bankSound, deathSound;
    static AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        catSound = Resources.Load<AudioClip>("catSound");
        coinSound = Resources.Load<AudioClip>("coinSound");
        bankSound = Resources.Load<AudioClip>("bankSound");
        deathSound = Resources.Load<AudioClip>("deathSound");
        audioSrc = GetComponent<AudioSource>();
    }

    public static void PlaySound (string clip)
    {
        switch (clip)
        {
            case "catSound":
                audioSrc.PlayOneShot(catSound);
                break;
            case "coinSound":
                audioSrc.PlayOneShot(coinSound);
                break;
            case "bankSound":
                audioSrc.PlayOneShot(bankSound);
                break;
            case "deathSound":
                audioSrc.PlayOneShot(deathSound);
                break;
        }
    }
}
