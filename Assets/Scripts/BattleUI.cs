using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    public static BattleUI singleton;
    [SerializeField] Text stepsText;
    public Button goButton;

    // Start is called before the first frame update
    void Start()
    {
        if (singleton == null)
            singleton = this;
        else
            Destroy(gameObject);


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSteps(float range)
    {
        stepsText.text = "Range: " + (int)range;
    }
}
