using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace DragonOfTruth01.ReshiramCCMod.Cards;

internal sealed class CardOutrage : Card, IReshiramCCModCard
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("Outrage", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.ReshiramCCMod_Deck.Deck,
                rarity = Rarity.rare,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Outrage", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        CardData data = new CardData()
        {
            art = ModEntry.Instance.ReshiramCCMod_Character_CardOutrageBG.Sprite,
            cost = 1,
            infinite = true
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
                        piercing = true
                    },
                    new ADrawCard()
                    {
                        count = 1
                    },
                    new ADiscard()
                    {
                        count = 1
                    }
                };
                break;

            case Upgrade.A:
                actions = new()
                {
                    new AAttack()
                    {
                        damage = GetDmg(s, 2),
                        piercing = true,
                        status = Status.heat,
                        statusAmount = 2
                    },
                    new ADrawCard()
                    {
                        count = 1
                    },
                    new ADiscard()
                    {
                        count = 1
                    }
                };
                break;

            case Upgrade.B:
                actions = new()
                {
                    new AAttack()
                    {
                        damage = GetDmg(s, 3),
                        piercing = true,
                        status = Status.heat,
                        statusAmount = 2
                    },
                    new AStatus()
                    {
                        status = Status.heat,
                        statusAmount = 1,
                        targetPlayer = true
                    },
                    new ADrawCard()
                    {
                        count = 1
                    },
                    new ADiscard()
                    {
                        count = 1
                    }
                };
                break;
        }
        return actions;
    }
}
