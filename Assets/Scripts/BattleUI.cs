using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    [SerializeField] Text stepsText;
    [SerializeField] Text rangeText;
    [SerializeField] Text healthText;
    public Button goButton;
    public Button stopButton;
    public Button shotButton;
    public Button turnButton;
    public Button nextCharButton;
    public Button endTurnButton;
    public Button[] enemyButtons;

    public void SetSteps(float steps)
    {
        stepsText.text = "Steps: " + (int)Math.Round(steps);
    }

    public void SetRange(float range)
    {
        rangeText.text = "Range: " + (int)Math.Round(range);
    }

    public void SetHealth(float health)
    {
        healthText.text = "Health: " + (int)Math.Round(health);
    }

    public void ShowEnemyButtons(int[] enemies)
    {
        HideEnemyButtons();
        for (int i = 0; i < enemies.Length; i++)
        {
            enemyButtons[enemies[i]].gameObject.SetActive(true);
        }
    }

    public void HideEnemyButtons()
    {
        foreach (Button button in enemyButtons)
            button.gameObject.SetActive(false);
    }

    private void Start()
    {
        for (int i = 0; i < enemyButtons.Length; i++)
        {
            int j = i;
            enemyButtons[j].onClick.AddListener(delegate { ButtonSet(j); });
        }
    }

    private void ButtonSet(int index)
    {
        CleanEnemyButtons();

        var colors = enemyButtons[index].colors;
        colors.normalColor = Color.red;
        enemyButtons[index].colors = colors;
    }

    private void CleanEnemyButtons()
    {
        foreach (Button button in enemyButtons)
        {
            var colors1 = button.colors;
            colors1.normalColor = Color.white;
            button.colors = colors1;
        }
    }

    public void SetDefault()
    {
        CleanEnemyButtons();
    }
    //public void ShotOrMove(bool shot)
    //{
    //    shotButton.GetComponentInChildren<Text>().text = shot ? "Move" : "Atack";
    //    goButton.GetComponentInChildren<Text>().text = shot ? "Shot" : "Go";
    //}
}
