using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace DragonOfTruth01.ReshiramCCMod.Cards;

internal sealed class CardIceBurn : Card, IReshiramCCModCard
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("Ice Burn", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.ReshiramCCMod_WKyurem_Deck.Deck,
                rarity = Rarity.rare,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Ice Burn", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        CardData data = new CardData()
        {
            art = ModEntry.Instance.ReshiramCCMod_Character_WKyurem_CardIceBurnBG.Sprite,
            cost = 2,
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
                        damage = GetDmg(s, 1),
                        status = ModEntry.Instance.Frozen.Status,
                        statusAmount = 1
                    },
                    new AAttack()
                    {
                        damage = GetDmg(s, 1),
                        status = Status.heat,
                        statusAmount = 3
                    }
                };
                break;

            case Upgrade.A:
                actions = new()
                {
                    new AAttack()
                    {
                        damage = GetDmg(s, 2),
                        status = ModEntry.Instance.Frozen.Status,
                        statusAmount = 1
                    },
                    new AAttack()
                    {
                        damage = GetDmg(s, 2),
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
                        damage = GetDmg(s, 1),
                        status = ModEntry.Instance.Frozen.Status,
                        statusAmount = 2
                    },
                    new AAttack()
                    {
                        damage = GetDmg(s, 1),
                        status = Status.heat,
                        statusAmount = 3
                    },
                    new AStatus()
                    {
                        status = ModEntry.Instance.Frozen.Status,
                        statusAmount = 1,
                        targetPlayer = true
                    }
                };
                break;
        }
        return actions;
    }
}
