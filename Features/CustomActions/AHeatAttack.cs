using Nickel;
using System.Collections.Generic;

namespace DragonOfTruth01.ReshiramCCMod;

public sealed class AHeatAttack : AAttack
{
    public override void Begin(G g, State s, Combat c)
    {
        int damageCalc = s.ship.Get(Status.heat) + c.otherShip.Get(Status.heat);
        damage = Card.GetActualDamage(s, damageCalc, card: null);
        base.Begin(g, s, c);
    }
}