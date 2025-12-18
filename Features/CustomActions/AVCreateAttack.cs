using Nickel;
using System.Collections.Generic;

namespace DragonOfTruth01.ReshiramCCMod;

public sealed class AVCreateAttack : CardAction
{
    public int damageAmount;
    public int smolderingAmount;
    public int heatAmount;

    public override void Begin(G g, State s, Combat c)
    {
        // Get the leftmost and rightmost cards to exhaust

        // If we have no cards, don't exhaust anything

        // If we have one card, only exhaust that one
        if(c.hand.Count == 1 && c.hand[0] != null)
        {
            Card card = c.hand[0];

            c.Queue(new ADelay
                {
                    timer = 0.5
                });
            c.Queue(new AExhaustOtherCard
                {
                    uuid = card.uuid
                });
        }

        // Otherwise, exhaust the leftmost and rightmost cards
        if(c.hand.Count > 1 && c.hand[0] != null && c.hand[c.hand.Count - 1] != null)
        {
            Card leftmost = c.hand[0];
            Card rightmost = c.hand[c.hand.Count - 1];

            c.Queue(new ADelay
                {
                    timer = 0.5
                });
            c.Queue(new AExhaustOtherCard
                {
                    uuid = leftmost.uuid
                });
            c.Queue(new AExhaustOtherCard
                {
                    uuid = rightmost.uuid
                });
        }

        c.Queue(new AAttack
            {
                damage = damageAmount,
                status = ModEntry.Instance.Smoldering.Status,
                statusAmount = smolderingAmount
            });

        if(heatAmount != 0)
        {
            c.Queue(new AStatus
            {
                status = Status.heat,
                statusAmount = heatAmount
            });
        }

        base.Begin(g, s, c);
    }
}