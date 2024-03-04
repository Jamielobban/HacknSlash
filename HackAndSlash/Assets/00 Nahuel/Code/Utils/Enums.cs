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

    public enum Music
    {
    };
    public enum Effects
    {
    };
    public enum MusicEffects
    {
    };
}

