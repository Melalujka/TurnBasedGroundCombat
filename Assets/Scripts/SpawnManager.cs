using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    GameObject[] characters = new GameObject[8];
    [SerializeField] GameObject characterPrefab;
    [SerializeField] GameObject[] topSpawns;
    [SerializeField] GameObject[] bottomSpawns;
    [SerializeField] BattleUI battleUI;

    private void Start()
    {
        SpawnCharacters();
    }

    public void SpawnCharacters()
    {
        for (int i = 0; i < 4; i++) {
            var prefabTop = Instantiate(characterPrefab,
                                               topSpawns[i].transform.position,
                                               topSpawns[i].transform.rotation);
            prefabTop.GetComponent<PlayerController>().battleUI = battleUI;
            characters[i] = prefabTop;
            var prefabBottom = Instantiate(characterPrefab,
                                                  bottomSpawns[i].transform.position,
                                                  bottomSpawns[i].transform.rotation);
            prefabBottom.GetComponent<PlayerController>().battleUI = battleUI;
            characters[i + 4] = prefabBottom;
        }
    }
}
