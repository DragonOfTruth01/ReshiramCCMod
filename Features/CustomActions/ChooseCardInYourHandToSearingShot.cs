using Nickel;
using System;
using System.Collections.Generic;

namespace DragonOfTruth01.ReshiramCCMod;

public sealed class ChooseCardInYourHandToSearingShot : CardAction
{
    public int damageMod;
    public int heatMod;

    public override void Begin(G g, State s, Combat c)
    {
        // Choose a card in hand
        if(selectedCard != null)
        {
            Card card = selectedCard;

            int damageDealt = card.GetCurrentCost(s) + damageMod;
            int heatDealt = card.GetCurrentCost(s) + heatMod;

            timer = 0.0;

            // Queue actions in inverse order because we're using QueueImmediate
            c.QueueImmediate(new AAttack
                {
                    damage = Math.Max(0, damageDealt - 10),
                    status = Status.heat,
                    statusAmount = heatDealt
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

    public override string? GetCardSelectText(State s)
	{
		return ModEntry.Instance.Localizations.Localize(["action", "SearingShotChooseExhaust", "SelectText"]);
	}
}