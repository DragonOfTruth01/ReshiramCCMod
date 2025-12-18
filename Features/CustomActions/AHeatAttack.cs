using Nickel;
using System;
using System.Collections.Generic;

namespace DragonOfTruth01.ReshiramCCMod;

public sealed class AHeatAttack : AAttack
{
    public override void Begin(G g, State s, Combat c)
    {
        int damageCalc = damage + s.ship.Get(Status.heat) + c.otherShip.Get(Status.heat);
        damage = Math.Max(0, damageCalc - 10);
        base.Begin(g, s, c);
    }
}