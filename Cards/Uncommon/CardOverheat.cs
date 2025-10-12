using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace DragonOfTruth01.ReshiramCCMod.Cards;

internal sealed class CardOverheat : Card, IReshiramCCModCard
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("Overheat", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.ReshiramCCMod_Deck.Deck,
                rarity = Rarity.uncommon,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Overheat", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        CardData data = new CardData()
        {
            cost = 1,
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
                    new AStatus(){
                        status = ModEntry.Instance.Smoldering.Status,
                        statusAmount = 1
                    },
                    new AStatus()
                    {
                        status = Status.heat,
                        statusAmount = 3
                    },
                    new AStatus
                    {
                        status = ModEntry.Instance.Flammable.Status,
                        statusAmount = 1,
                    },
                    new AStatus
                    {
                        status = Status.heat,
                        statusAmount = 3,
                        targetPlayer = true
                    }
                };
                break;

            case Upgrade.A:
                actions = new()
                {
                    new AStatus(){
                        status = ModEntry.Instance.Smoldering.Status,
                        statusAmount = 1
                    },
                    new AStatus()
                    {
                        status = Status.heat,
                        statusAmount = 3
                    },
                    new AStatus
                    {
                        status = ModEntry.Instance.Flammable.Status,
                        statusAmount = 1,
                    },
                    new AStatus
                    {
                        status = Status.heat,
                        statusAmount = 2,
                        targetPlayer = true
                    }
                };
                break;

            case Upgrade.B:
                actions = new()
                {
                    new AStatus(){
                        status = ModEntry.Instance.Smoldering.Status,
                        statusAmount = 1
                    },
                    new AStatus()
                    {
                        status = Status.heat,
                        statusAmount = 4
                    },
                    new AStatus
                    {
                        status = ModEntry.Instance.Flammable.Status,
                        statusAmount = 2,
                    },
                    new AStatus
                    {
                        status = Status.heat,
                        statusAmount = 3,
                        targetPlayer = true
                    }
                };
                break;
        }
        return actions;
    }
}
