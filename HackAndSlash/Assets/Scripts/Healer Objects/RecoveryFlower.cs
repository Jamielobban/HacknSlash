using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryFlower : StructureEnemiesLife
{
    public GameObject gemsOfPower;

    protected override void Awake()
    {
        gemsOfPower.SetActive(false);
    }

    protected override void Activate()
    {
        base.Activate();
        gemsOfPower.SetActive(true);
    }

    public override void ResetStructure()
    {
        base.ResetStructure();
        gemsOfPower.SetActive(false);
    }
}
