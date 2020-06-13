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

    public CharacterController[] InactiveCharacters => GetCharactersRange(!isTopTurn);

    public bool isTopTurn = true;

    //private bool shotState = false;

    [SerializeField] BattleUI battleUI;

    void Start()
    {
        //battleUI.shotButton.onClick.AddListener(ChangeShotState);
    }

        //void ChangeShotState()
        //{
        //    shotState = !shotState;
        //    battleUI.ShotOrMove(shotState);
        //}

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

    bool CheckClose(Vector3 character, Vector3 against, float range)
    {
        return (Vector3.Distance(character, against) <= range);
    }

    bool CheckCastClose(GameObject character, GameObject against)
    {
        return CheckClose(character.transform.position, against.transform.position, character.GetComponent<CharacterController>().CastRange);
    }

    bool CheckAttackClose(GameObject character, GameObject against)
    {
        return CheckClose(character.transform.position, against.transform.position, character.GetComponent<CharacterController>().AttackRange);
    }

    bool CheckSightClose(GameObject character, GameObject against)
    {
        return CheckClose(character.transform.position, against.transform.position, character.GetComponent<CharacterController>().SightRange);
    }

    List<GameObject> GetAvailableToCast(GameObject character, GameObject[] against)
    {
        List<GameObject> available = new List<GameObject>();
        for (int i = 0; i < against.Length; ++i)
            if (CheckCastClose(character, against[i]))
                available.Add(against[i]);

        return available;
    }

    void ShowAvailableToCast(GameObject character, GameObject[] against)
    {
        List<int> available = new List<int>();
        for (int i = 0; i < against.Length; ++i)
        {
            if (CheckCastClose(character, against[i]))
                available.Add(i);
        }

        battleUI.ShowEnemyButtons(available.ToArray());
    }

    void Update()
    {
        SetCastAvailable();

    }

    void SetCastAvailable()
    {
        var activeChars = ActiveCharacters.Where(c => c.isChoosenOne).ToArray();
        if (activeChars.Length > 0) //.Aggregate((_, x) => x.isChoosenOne ? x : _).gameObject;
        {
            var activeChar = activeChars.First().gameObject;
            var arr = InactiveCharacters.Select(person => person.gameObject).ToArray();
            //List<GameObject> availableToCast = GetAvailableToCast(activeChar, arr);
            ShowAvailableToCast(activeChar, arr);
            //if (availableToCast.Count > 0)
            //{
            //    var colors = battleUI.shotButton.colors;
            //    colors.normalColor = Color.red;
            //    battleUI.shotButton.colors = colors;
            //}
            //else
            //{
            //    var colors = battleUI.shotButton.colors;
            //    colors.normalColor = Color.white;
            //    battleUI.shotButton.colors = colors;
            //}
        }
    }
}
