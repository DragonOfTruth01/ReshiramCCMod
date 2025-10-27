using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace DragonOfTruth01.ReshiramCCMod.Cards;

internal sealed class CardRoost : Card, IReshiramCCModCard
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("Roost", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.ReshiramCCMod_Deck.Deck,
                rarity = Rarity.rare,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Roost", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        CardData data = new CardData()
        {
            art = ModEntry.Instance.ReshiramCCMod_Character_CardRoostBG.Sprite,
            cost = upgrade == Upgrade.A ? 1 : 2,
            exhaust = true
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
                    new AHeal()
                    {
                        healAmount = 2,
                        targetPlayer = true
                    },
                    new AStatus()
                    {
                        status = Status.heat,
                        statusAmount = -3,
                        targetPlayer = true
                    },
                    new AStatus()
                    {
                        status = ModEntry.Instance.Flammable.Status,
                        statusAmount = 2,
                        targetPlayer = true
                    }
                };
                break;

            case Upgrade.A:
                actions = new()
                {
                    new AHeal()
                    {
                        healAmount = 2,
                        targetPlayer = true
                    },
                    new AStatus()
                    {
                        status = Status.heat,
                        statusAmount = -3,
                        targetPlayer = true
                    },
                    new AStatus()
                    {
                        status = ModEntry.Instance.Flammable.Status,
                        statusAmount = 2,
                        targetPlayer = true
                    }
                };
                break;

            case Upgrade.B:
                actions = new()
                {
                    new AHeal()
                    {
                        healAmount = 2,
                        targetPlayer = true
                    },
                    new AStatus()
                    {
                        status = Status.shield,
                        statusAmount = 2,
                        targetPlayer = true
                    },
                    new AStatus()
                    {
                        status = Status.heat,
                        statusAmount = -3,
                        targetPlayer = true
                    },
                    new AStatus()
                    {
                        status = ModEntry.Instance.Flammable.Status,
                        statusAmount = 2,
                        targetPlayer = true
                    }
                };
                break;
        }
        return actions;
    }
}
