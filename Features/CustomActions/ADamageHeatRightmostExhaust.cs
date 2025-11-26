using Nickel;
using System.Collections.Generic;

namespace DragonOfTruth01.ReshiramCCMod;

public sealed class ADamageHeatRightmostExhaust : CardAction
{
    public int damageHeatMod;

    public override void Begin(G g, State s, Combat c)
    {
        // Get the rightmost card in the hand
        if(c.hand[c.hand.Count - 1] != null)
        {
            Card card = c.hand[c.hand.Count - 1];

            int damageHeatDealt = card.GetCurrentCost(s) + damageHeatMod;

            timer = 0.0;
            c.Queue(new ADelay
                {
                    timer = 0.5
                });
            c.Queue(new AExhaustOtherCard
                {
                    uuid = card.uuid
                });
            c.Queue(new AAttack
                {
                    damage = Card.GetActualDamage(s, damageHeatDealt, card: null),
                    status = Status.heat,
                    statusAmount = damageHeatDealt
                });
        }
    }
}