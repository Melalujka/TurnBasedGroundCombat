using UnityEngine;

enum Tags
{
    Character,
}

enum SceneTag
{
    Battle,
    Menu,
}

public interface ICharConsts
{
    int SightRange { get; set; }
    int Health { get; set; }
    int Power { get; set; }
    int MovementPoints { get; set; }
}

public enum Chars
{
    Archer,
    Warrior,
    Druid,
    Mage,
}

public class Constants
{
    public static ICharConsts GetChar(string tag)
    {
        switch (tag)
        {
            case "Archer":
                return new Archer();
            case "Warrior":
                return new Warrior();
            case "Druid":
                return new Druid();
            case "Mage":
                return new Mage();
            default:
                Debug.LogError("unknown type of character");
                return null;
        }

    }

    public static string BattleManager = "BattleManager";

    public class Archer : ICharConsts
    {
        public int SightRange  // read-write instance property
        { get; set; } = 120;
        public int Health  // read-write instance property
        { get; set; } = 60;
        public int Power  // read-write instance property
        { get; set; } = 100;
        public int MovementPoints  // read-write instance property
        { get; set; } = 55;
    }

    public class Warrior : ICharConsts
    {
        public int SightRange  // read-write instance property
        { get; set; } = 90;
        public int Health  // read-write instance property
        { get; set; } = 100;
        public int Power  // read-write instance property
        { get; set; } = 120;
        public int MovementPoints  // read-write instance property
        { get; set; } = 50;
    }

    public class Druid : ICharConsts
    {
        public int SightRange  // read-write instance property
        { get; set; } = 110;
        public int Health  // read-write instance property
        { get; set; } = 100;
        public int Power  // read-write instance property
        { get; set; } = 80;
        public int MovementPoints  // read-write instance property
        { get; set; } = 40;
    }

    public class Mage : ICharConsts
    {
        public int SightRange  // read-write instance property
        { get; set; } = 80;
        public int Health  // read-write instance property
        { get; set; } = 50;
        public int Power  // read-write instance property
        { get; set; } = 120;
        public int MovementPoints  // read-write instance property
        { get; set; } = 40;
    }
}