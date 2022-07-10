using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChoiceUI : MonoBehaviour
{
    private RawImage rawImage;
    private TextMeshProUGUI choiceName;
    public void SetUp(Texture img, string choice)
    {
        rawImage.texture = img;
        choiceName.text = choice;
    }

    public void EnterLane()
    {
        rawImage.color = new Color(255, 0, 0);
    }

    public void LeaveLane()
    {
        rawImage.color = new Color(255, 255, 255);
    }
}
