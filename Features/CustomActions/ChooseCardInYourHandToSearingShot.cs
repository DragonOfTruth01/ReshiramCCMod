using Nickel;
using System;
using System.Collections.Generic;

namespace DragonOfTruth01.ReshiramCCMod;

public sealed class ChooseCardInYourHandToSearingShot : CardAction
{
    public int damageHeatMod;

    public override void Begin(G g, State s, Combat c)
    {
        // Choose a card in hand
        if(selectedCard != null)
        {
            Card card = selectedCard;

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
                    damage = Math.Max(0, damageHeatDealt - 10),
                    status = Status.heat,
                    statusAmount = Math.Max(0, damageHeatDealt - 10)
                });
            }
    }

    public override string? GetCardSelectText(State s)
	{
		return ModEntry.Instance.Localizations.Localize(["action", "SearingShotChooseExhaust", "SelectText"]);
	}
}