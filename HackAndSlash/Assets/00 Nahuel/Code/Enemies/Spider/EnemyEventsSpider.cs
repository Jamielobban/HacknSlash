using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEventsSpider : EnemyEvents
{
    public event Action OnFlee;

    public void Flee() => OnFlee?.Invoke();
}
