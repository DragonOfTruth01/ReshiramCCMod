using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace DragonOfTruth01.ReshiramCCMod.Cards;

internal sealed class CardDragonBreath : Card, IDemoCard
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("Dragon Breath", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.ReshiramCCMod_Deck.Deck,
                rarity = Rarity.common,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Dragon Breath", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        CardData data = new CardData()
        {
            cost = upgrade == Upgrade.A ? 2 : 1,
            description = ModEntry.Instance.Localizations.Localize(["card", "Dragon Breath", "description", upgrade.ToString()])
        };
        return data;
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        List<CardAction> actions = new();
        switch (upgrade)
        {
            case Upgrade.None:
                List<CardAction> cardActionList1 = new List<CardAction>()
                {
                    new AStatus()
                    {
                        status = Status.heat,
                        statusAmount = 1,
                    }
                };

                // Add 1 flammable if this attack would overheat the enemy (current heat + 1)
                // if (((Combat)s.route).otherShip.Get(Status.heat) + 1 >= ((Combat)s.route).otherShip.heatTrigger)
                // {
                //     cardActionList1.Add(
                //         new AStatus()
                //         {
                //             status = ModEntry.Instance.Flammable.Status,
                //             statusAmount = 1,
                //         }
                //     );
                // }

                actions = cardActionList1;
                break;
            case Upgrade.A:
                List<CardAction> cardActionList2 = new List<CardAction>()
                {
                    new AStatus()
                    {
                        status = Status.heat,
                        statusAmount = 1,
                    }
                };

                // Add 2 flammable if this attack would overheat the enemy (current heat + 1)
                // if (((Combat)s.route).otherShip.Get(Status.heat) + 1 >= ((Combat)s.route).otherShip.heatTrigger)
                // {
                //     cardActionList2.Add(
                //         new AStatus()
                //         {
                //             status = ModEntry.Instance.Flammable.Status,
                //             statusAmount = 2,
                //         }
                //     );
                // }

                actions = cardActionList2;
                break;
            case Upgrade.B:
                List<CardAction> cardActionList3 = new List<CardAction>()
                {
                    new AStatus()
                    {
                        status = Status.heat,
                        statusAmount = 2,
                    }
                };

                // Add 1 flammable if this attack would overheat the enemy (current heat + 2)
                // if (((Combat)s.route).otherShip.Get(Status.heat) + 2 >= ((Combat)s.route).otherShip.heatTrigger)
                // {
                //     cardActionList3.Add(
                //         new AStatus()
                //         {
                //             status = ModEntry.Instance.Flammable.Status,
                //             statusAmount = 1,
                //         }
                //     );
                // }

                actions = cardActionList3;
                break;
        }
        return actions;
    }
}
