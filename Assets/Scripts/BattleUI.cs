using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    public static BattleUI singleton;
    [SerializeField] Text stepsText;
    public Button goButton;

    void Update()
    {
        
    }

    public void SetSteps(float range)
    {
        stepsText.text = "Range: " + (int)range;
    }
}
