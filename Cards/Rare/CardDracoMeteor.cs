using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace DragonOfTruth01.ReshiramCCMod.Cards;

internal sealed class CardDracoMeteor : Card, ReshiramCCModCard
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("Draco Meteor", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.ReshiramCCMod_Deck.Deck,
                rarity = Rarity.rare,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Draco Meteor", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        CardData data = new CardData()
        {
            cost = upgrade == Upgrade.B ? 2 : 3,
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
                    new AAttack()
                    {
                        damage = GetDmg(s, 2),
                        status = Status.heat,
                        statusAmount = 1
                    },
                    new AAttack()
                    {
                        damage = GetDmg(s, 2),
                        status = ModEntry.Instance.Flammable.Status,
                        statusAmount = 1
                    },
                    new AAttack()
                    {
                        damage = GetDmg(s, 3)
                    },
                    new AStatus()
                    {
                        status = Status.energyLessNextTurn,
                        statusAmount = 1,
                        targetPlayer = true
                    }
                };
                break;

            case Upgrade.A:
                actions = new()
                {
                    new AAttack()
                    {
                        damage = GetDmg(s, 2),
                        status = Status.heat,
                        statusAmount = 1
                    },
                    new AAttack()
                    {
                        damage = GetDmg(s, 2),
                        status = ModEntry.Instance.Flammable.Status,
                        statusAmount = 2
                    },
                    new AAttack()
                    {
                        damage = GetDmg(s, 3)
                    },
                    new AStatus()
                    {
                        status = Status.energyLessNextTurn,
                        statusAmount = 1,
                        targetPlayer = true
                    }
                };
                break;

            case Upgrade.B:
                actions = new()
                {
                    new AAttack()
                    {
                        damage = GetDmg(s, 2),
                        status = Status.heat,
                        statusAmount = 1
                    },
                    new AAttack()
                    {
                        damage = GetDmg(s, 2),
                        status = ModEntry.Instance.Flammable.Status,
                        statusAmount = 1
                    },
                    new AAttack()
                    {
                        damage = GetDmg(s, 3)
                    },
                    new AStatus()
                    {
                        status = Status.energyLessNextTurn,
                        statusAmount = 1,
                        targetPlayer = true
                    }
                };
                break;
        }
        return actions;
    }
}
