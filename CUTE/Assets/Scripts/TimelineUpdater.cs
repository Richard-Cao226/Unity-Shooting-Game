using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class TimelineUpdater : MonoBehaviour
{
    private GameObject[] acheivements;
    private GameObject[] acheivementNums;
    private GameObject[] acheivementTypes;
    private GameObject progressBar;
    private float savedProgressLength;
    private int[] acheivementOrder;

    // Start is called before the first frame update
    void Start()
    {
        acheivements = new GameObject[6];
        for (int i = 0; i < acheivements.Length; i++)
            acheivements[i] = GameObject.Find("Acheivement" + (i + 1));
        acheivementNums = new GameObject[6];
        for (int i = 0; i < acheivementNums.Length; i++)
            acheivementNums[i] = GameObject.Find("AcheivementNumText" + (i + 1));
        acheivementTypes = new GameObject[6];
        for (int i = 0; i < acheivementTypes.Length; i++)
            acheivementTypes[i] = GameObject.Find("AcheivementTypeText" + (i + 1));
        acheivementOrder = new int[] {0, 1, 2, 3, 4, 5};
        progressBar = GameObject.Find("ProgressBar");
    }

    public void UpdateTimeline(int friendAmt)
    {
        int index = acheivementOrder[friendAmt / 10];

        Color blue = new Color(100 / 255.0f, 250 / 255.0f, 255 / 255.0f);
        for (int i = 0; i <= index; i++)
        {
            acheivements[i].GetComponent<Image>().color = blue;
            acheivementNums[i].GetComponent<Text>().color = blue;
            acheivementTypes[i].GetComponent<Text>().color = blue;
        }
        progressBar.GetComponent<ProgressBarMover>().MoveUntil(index / 5f - savedProgressLength);
        savedProgressLength = index / 5f;
    }
}
