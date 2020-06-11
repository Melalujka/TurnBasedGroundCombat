using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] BattleManager manager;
    [SerializeField] GameObject[] characterPrefabs;
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
            var pref = characterPrefabs[i];
            var prefabTop = Instantiate(pref,
                                        topSpawns[i].transform.position,
                                        topSpawns[i].transform.rotation);
            prefabTop.GetComponent<PlayerController>().battleUI = battleUI;
            manager.characters[i] = prefabTop;
            var prefabBottom = Instantiate(pref,
                                           bottomSpawns[i].transform.position,
                                           bottomSpawns[i].transform.rotation);
            prefabBottom.GetComponent<PlayerController>().battleUI = battleUI;
            manager.characters[i + 4] = prefabBottom;
        }
    }
}
