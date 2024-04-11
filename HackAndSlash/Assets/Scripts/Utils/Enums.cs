using System;

public class Enums
{
    public enum EnemyStates
    {
        Idle,
        Spawning,
        Wander,
        Chase,
        Attack,
        Roll,
        Stun,
        Countering,
        Hit,
        Dead
    }

    public enum StateEvent
    {
        DetectPlayer,
        LostPlayer,
        HitEnemy,
        DeadEnemy,
        SpawnEnemy,
        DespawnEnemy,
        RollImpact
    }

    public enum AttackState
    {
        ReadyToUse = 0,
        Casting,
        Cooldown
    };

    public enum SpawnMethod 
    { 
        RoundRobin, 
        Random, 
        Probability
    };
    
    public enum TutorialActions
    {
        JUMP,
        DOBLEJUMP,
        RUN,
        ROLL,
        SQUARE,
        TRIANGLE,
        HOLDSQUARE,
        HOLDTRIANGLE,
        AIRSQUARE,
        AIRTRIANGLE
    }
    public enum TutorialInputs
    {
        Cross,
        Run,
        Roll,
        Square,
        HoldSquare,
        Triangle,
        HoldTriangle,        
    }
    public enum TutorialState
    {
        MOVEMENTS,
        COMBOS,
        AIRCOMBOS,
        FINISHED,
        INACTIVE
    }
    public enum NewTutorialState
    {
        INACTIVE,
        COMBOS,
        PYRAMIDE,
        FINISHED
    }
    public enum InputsAttack
    {
        Square,
        Triangle,
        HoldSquare,
        HoldTriangle,
        L2Square,
        L2Triangle,
        AirSquare,
        AirTriangle
    };
    public enum GameState
    {
        Menu,
        Tutorial,
        StartPlaying,
        Playing,
        Pause,
        ReturningMenu,
        Exit

    };

    public enum EventState
    {
        INACTIVE, PLAYING, FINISHED
    };

    public enum EventDificulty
    {
        EASY, MID, HARD
    }

    public enum Music
    {
        MainMenuMusic,
        MainTheme,
        EpicTheme,
        UnoVSUno,
        InGameMenu,
        Helicopter
    };

    public enum Effects
    {
        Click,
        Evento,
        Negativo,
        Positivo,
        WolfAttack,
        ChestOpen,
        MoveByUI,
        MonsterSpeak1,MonsterSpeak2,
        AlienSound1,AlienSound2,AlienSound3,
        SpiderSound1, SpiderSound2, SpiderSound3, SpiderSound4, SpiderSound5, SpiderSound6, SpiderSound7, SpiderSound8, SpiderSound9, SpiderSound10, SpiderSound11, SpiderSound12,
        MonsterSpeak3,MonsterSpeak4, MonsterSpeak5, MonsterSpeak6, MonsterSpeak7, MonsterSpeak8, MonsterSpeak9,MonsterSpeak10,MonsterSpeak11,MonsterSpeak12,
        MonsterFootstep, MonsterFootstep1, MonsterFootstep2,
        DeadWolf1,DeadWolf2,
        RisaMonster,monsterSpeak,monsterSpeak2,
        OpenInventory,
        MonsterAttack1,MonsterAttack2,
        ErrorButton,
        OpenItemsToPick,
        Selected,
        SelectItem,
        OpenItemsToPickEpic,
        FailEvent,SuccessEvent,
        PositiveClickTuto,
        TutoEnded
    };

    public enum MusicEffects
    {
        Atmosfera,
        Wind,
        IceWind
       
        
    };

    public enum RarityType
    {
        Common,
        Uncommon,
        Rare,
        Legendary
    }
}

