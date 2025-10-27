using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace DragonOfTruth01.ReshiramCCMod.Cards;

internal sealed class CardHoneClaws : Card, IReshiramCCModCard
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("Hone Claws", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.ReshiramCCMod_Deck.Deck,
                rarity = Rarity.uncommon,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Hone Claws", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        CardData data = new CardData()
        {
            art = ModEntry.Instance.ReshiramCCMod_Character_CardHoneClawsBG.Sprite,
            cost = upgrade == Upgrade.A ? 0 : 1,
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
                    new AStatus
                    {
                        status = Status.overdrive,
                        statusAmount = 2,
                        targetPlayer = true
                    },
                    new AStatus
                    {
                        status = Status.heat,
                        statusAmount = 3,
                        targetPlayer = true
                    },
                    new AStatus
                    {
                        status = ModEntry.Instance.Flammable.Status,
                        statusAmount = 1,
                        targetPlayer = true
                    }
                };
                break;

            case Upgrade.A:
                actions = new()
                {
                    new AStatus
                    {
                        status = Status.overdrive,
                        statusAmount = 2,
                        targetPlayer = true
                    },
                    new AStatus
                    {
                        status = Status.heat,
                        statusAmount = 3,
                        targetPlayer = true
                    },
                    new AStatus
                    {
                        status = ModEntry.Instance.Flammable.Status,
                        statusAmount = 1,
                        targetPlayer = true
                    }
                };
                break;

            case Upgrade.B:
                actions = new()
                {
                    new AStatus
                    {
                        status = Status.powerdrive,
                        statusAmount = 1,
                        targetPlayer = true
                    },
                    new AStatus
                    {
                        status = Status.heat,
                        statusAmount = 3,
                        targetPlayer = true
                    },
                    new AStatus
                    {
                        status = ModEntry.Instance.Flammable.Status,
                        statusAmount = 1,
                        targetPlayer = true
                    }
                };
                break;
        }
        return actions;
    }
}
