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
        Ranged,
        Invoking,
        AreaAttack,
        Roll,
        Stun,
        Countering,
        Returning,
        Hit,
        AutoDestruction,
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
        Helicopter,
        MusicaMenuNuevo,
        MusicaCombbat, 
        Glitch,
        LoadingFX,
        MusicaTutoNou,
        Defeat,
        Victory
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
        MonsterFootstep, MonsterFootstep1, MonsterFootstep2,
        OpenInventory,
        MonsterAttack1,MonsterAttack2,
        ErrorButton,
        OpenItemsToPick,
        Selected,
        SelectItem,
        OpenItemsToPickEpic,
        FailEvent,SuccessEvent,
        PositiveClickTuto,
        TutoEnded,
        MoveSelect,
        MoveUI,
        SelectOptionMenu,
        DoorOpen,
        RotatePyramid,
        MenuSpeach,
        FootstepsRobot
    };

    public enum MusicEffects
    {
        Atmosfera,
        Wind,
        IceWind,
       Glitch
       
        
    };

    public enum RarityType
    {
        Common,
        Uncommon,
        Rare,
        Legendary,
        Ability
    }
}

