using Nickel;
using System;
using System.Collections.Generic;

namespace DragonOfTruth01.ReshiramCCMod;

public sealed class ADamageHeatRightmostExhaust : CardAction
{
    public int damageMod;
    public int heatMod;

    public override void Begin(G g, State s, Combat c)
    {
        // Exhaust the rightmost card in the hand
        if(c.hand.Count > 0 && c.hand[c.hand.Count - 1] != null)
        {
            Card card = c.hand[c.hand.Count - 1];

            int damageDealt = card.GetCurrentCost(s) + damageMod;
            int heatDealt = card.GetCurrentCost(s) + heatMod;

            timer = 0.0;

            // Queue actions in inverse order because we're using QueueImmediate
            c.QueueImmediate(new AStatus
                {
                    status = Status.heat,
                    statusAmount = heatDealt
                });
            c.QueueImmediate(new AAttack
                {
                    damage = Math.Max(0, damageDealt - 10)
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