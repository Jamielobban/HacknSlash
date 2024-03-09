using System;

public class Enums
{

    public enum CameraType
    {
        FreeLook,
        Locked
    };

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

    public enum CharacterState
    {
        Idle,
        Moving,
        Dashing,
        Air,
        Attacking
    };
    public enum PlayerMovementState
    {
        Stop,
        Walking,
        Sprinting,
        Dashing,
        Attacking,
        Air
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
    public enum TutorialState
    {
        JUMPS,
        SPRINT,
        ROLLS,
        COMBOS,
        FINISHED,
        INACTIVE
    }
    public enum GameState
    {
        Menu,
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
        TutorialMusic
    };

    public enum Effects
    {
        ShockWave
    };

    public enum MusicEffects
    {
        Atmosfera,
        Wind,
        Click
    };

    public enum RarityType
    {
        Common,
        Uncommon,
        Rare,
        Legendary
    }
}

