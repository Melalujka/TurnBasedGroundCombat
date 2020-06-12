using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleManager : MonoBehaviour
{
    public GameObject[] characters = new GameObject[8];

    public CharacterController[] TopCharacters => GetCharactersRange(true);

    public CharacterController[] BottomCharacters => GetCharactersRange(false);

    public CharacterController[] ActiveCharacters => GetCharactersRange(isTopTurn);

    public bool isTopTurn = true;

    private bool shotState = false;

    [SerializeField] BattleUI battleUI;

    void Start()
    {
        battleUI.shotButton.onClick.AddListener(ChangeShotState);
    }

    void ChangeShotState()
    {
        shotState = !shotState;
        battleUI.ShotOrMove(shotState);
    }

    CharacterController[] GetCharacters()
    {
        return characters.Select(person => person.GetComponent<CharacterController>()).ToArray();
    }

    CharacterController[] GetCharactersRange(bool isFirst)
    {
        return GetCharacters().Skip(isFirst ? 0 : 4).Take(4).ToArray();
    }

    public void DeselectAllCharacters()
    {
        foreach (CharacterController character in ActiveCharacters)
        {
            character.SetChoosenOne(false);
        }

    }
}
