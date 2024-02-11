using System;

public class Enums
{
    public enum CameraType
    {
        FreeLook,
        Locked
    };

    public enum EnemyAttackState
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
        Attacking
    };

    public enum Abilities
    {
        Square,
        Triangle,
        L2Square,
        L2Triangle
    };

}
