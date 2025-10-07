using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace DragonOfTruth01.ReshiramCCMod.Cards;

internal sealed class CardIncinerate : Card, ReshiramCCModCard
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("Incinerate", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.ReshiramCCMod_Deck.Deck,
                rarity = Rarity.common,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Incinerate", "name"]).Localize
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
                    new AAttack()
                    {
                        damage = GetDmg(s, 0),
                        status = Status.heat,
                        statusAmount = 3
                    }
                };
                break;

            case Upgrade.A:
                actions = new()
                {
                    new AStatus()
                    {
                        status = ModEntry.Instance.Smoldering.Status,
                        statusAmount = 1
                    },
                    new AAttack()
                    {
                        damage = GetDmg(s, 0),
                        status = Status.heat,
                        statusAmount = 3
                    }
                };
                break;

            case Upgrade.B:
                actions = new()
                {
                    new AAttack()
                    {
                        damage = GetDmg(s, 0),
                        status = Status.heat,
                        statusAmount = 2
                    },
                    new AAttack()
                    {
                        damage = GetDmg(s, 0),
                        status = Status.heat,
                        statusAmount = 2
                    }
                };
                break;
        }
        return actions;
    }
}
