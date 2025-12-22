using Nickel;
using System;
using System.Collections.Generic;

namespace DragonOfTruth01.ReshiramCCMod;

public sealed class ADamageHeatRightmostExhaust : CardAction
{
    public int damageHeatMod;

    public override void Begin(G g, State s, Combat c)
    {
        // Exhaust the rightmost card in the hand
        if(c.hand.Count > 0 && c.hand[c.hand.Count - 1] != null)
        {
            Card card = c.hand[c.hand.Count - 1];

            int damageHeatDealt = card.GetCurrentCost(s) + damageHeatMod;

            timer = 0.0;

            // Queue actions in inverse order because we're using QueueImmediate
            c.QueueImmediate(new AAttack
                {
                    damage = Math.Max(0, damageHeatDealt - 10),
                    status = Status.heat,
                    statusAmount = Math.Max(0, damageHeatDealt - 10)
                });
            c.QueueImmediate(new AExhaustOtherCard
                {
                    uuid = card.uuid
                });
            c.QueueImmediate(new ADelay
                {
                    timer = 0.5
                });
        }
    }
}