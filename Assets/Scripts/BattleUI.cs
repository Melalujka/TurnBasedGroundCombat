using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    [SerializeField] Text stepsText;
    [SerializeField] Text rangeText;
    public Button goButton;
    public Button stopButton;
    public Button shotButton;
    public Button turnButton;

    public void SetSteps(float steps)
    {
        stepsText.text = "Steps: " + (int)Math.Round(steps);
    }

    public void SetRange(float range)
    {
        rangeText.text = "Range: " + (int)Math.Round(range);
    }

    public void ShotOrMove(bool shot)
    {
        shotButton.GetComponentInChildren<Text>().text = shot ? "Move" : "Atack";
        goButton.GetComponentInChildren<Text>().text = shot ? "Shot" : "Go";
    }
}
