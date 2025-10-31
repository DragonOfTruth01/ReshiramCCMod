using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace DragonOfTruth01.ReshiramCCMod.Cards;

internal sealed class CardSolarBeam : Card, IReshiramCCModCard
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("Solar Beam", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.ReshiramCCMod_Deck.Deck,
                rarity = Rarity.uncommon,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Solar Beam", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        object damageString = "";

        // Only populate our damage number if we are in combat.
        if (state.route is Combat combat && combat.otherShip != null)
        {
            int damageCalc = state.ship.Get(Status.heat) + combat.otherShip.Get(Status.heat);
            damageString = ": <c=redd>" + GetDmg(state, damageCalc) + "</c>";
            if(upgrade == Upgrade.B)
            {
                damageString += " <c=redd>(+2)</c>";
            }
        }

        CardData data = new CardData()
        {
            art = ModEntry.Instance.ReshiramCCMod_Character_CardSolarBeamBG.Sprite,
            description = ModEntry.Instance.Localizations.Localize(["card", "Solar Beam", "description", upgrade.ToString()], new { damageString }),
            cost = 2,
            exhaust = true,
            retain = upgrade == Upgrade.A
        };
        return data;
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        List<CardAction> actions = new();

        switch (upgrade)
        {
            case Upgrade.None:
                actions = new()
                {
                    new AHeatAttack()
                    {
                        piercing = true
                    }
                };
                break;

            case Upgrade.A:
                actions = new()
                {
                    new AHeatAttack()
                    {
                        piercing = true
                    }
                };
                break;

            case Upgrade.B:
                actions = new()
                {
                    new AStatus()
                    {
                        status = Status.heat,
                        statusAmount = 1
                    },
                    new AStatus()
                    {
                        status = Status.heat,
                        statusAmount = 1,
                        targetPlayer = true
                    },
                    new AHeatAttack()
                    {
                        piercing = true
                    }
                };
                break;
        }
        return actions;
    }
}
