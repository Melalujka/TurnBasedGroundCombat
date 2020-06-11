using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public GameObject[] characters = new GameObject[8];

    public PlayerController[] TopCharacters => GetCharactersRange(true);

    public PlayerController[] BottomCharacters => GetCharactersRange(false);

    public PlayerController[] ActiveCharacters => GetCharactersRange(isTopTurn);

    public bool isTopTurn = true;

    PlayerController[] GetCharacters()
    {
        return characters.Select(person => person.GetComponent<PlayerController>()).ToArray();
    }

    PlayerController[] GetCharactersRange(bool isFirst)
    {
        return GetCharacters().Skip(isFirst ? 0 : 4).Take(4).ToArray();
    }

    public void DeselectAllCharacters()
    {
        foreach (PlayerController character in ActiveCharacters)
        {
            character.SetChoosenOne(false);
        }

    }
}
