using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace DragonOfTruth01.ReshiramCCMod.Cards;

internal sealed class CardBlueFlare : Card, ReshiramCCModCard
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("Blue Flare", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.ReshiramCCMod_Deck.Deck,
                rarity = Rarity.rare,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Blue Flare", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        CardData data = new CardData()
        {
            cost = upgrade == Upgrade.A ? 3 : 4,
            exhaust = upgrade != Upgrade.B
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
                    new AAttack()
                    {
                        damage = GetDmg(s, 3)
                    },
                    new AStatus()
                    {
                        status = Status.heat,
                        statusAmount = 3
                    },
                    new AStatus()
                    {
                        status = ModEntry.Instance.Flammable.Status,
                        statusAmount = 2
                    }
                };
                break;

            case Upgrade.A:
                actions = new()
                {
                    new AAttack()
                    {
                        damage = GetDmg(s, 3)
                    },
                    new AStatus()
                    {
                        status = Status.heat,
                        statusAmount = 3
                    },
                    new AStatus()
                    {
                        status = ModEntry.Instance.Flammable.Status,
                        statusAmount = 2
                    }
                };
                break;

            case Upgrade.B:
                actions = new()
                {
                    new AAttack()
                    {
                        damage = GetDmg(s, 3)
                    },
                    new AStatus()
                    {
                        status = Status.heat,
                        statusAmount = 3
                    },
                    new AStatus()
                    {
                        status = ModEntry.Instance.Flammable.Status,
                        statusAmount = 1
                    }
                };
                break;
        }
        return actions;
    }
}
