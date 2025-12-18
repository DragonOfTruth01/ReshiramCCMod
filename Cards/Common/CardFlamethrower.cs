using DragonOfTruth01.ReshiramCCMod.Midrow;
using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace DragonOfTruth01.ReshiramCCMod.Cards;

internal sealed class CardFlamethrower : Card, IReshiramCCModCard
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("Flamethrower", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.ReshiramCCMod_Deck.Deck,
                rarity = Rarity.common,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Flamethrower", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        CardData data = new CardData()
        {
            art = ModEntry.Instance.ReshiramCCMod_Character_CardFlamethrowerBG.Sprite,
            cost = upgrade == Upgrade.B ? 2 : 1
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
                    new ASpawn()
                    {
                        thing = new MidrowFlamethrower()
                        {
                            yAnimation = 0.0
                        }
                    }
                };
                break;

            case Upgrade.A:
                actions = new()
                {
                    new AStatus()
                    {
                        status = Status.droneShift,
                        statusAmount = 1,
                        targetPlayer = true
                    },
                    new ASpawn()
                    {
                        thing = new MidrowFlamethrower()
                        {
                            yAnimation = 0.0
                        }
                    }
                };
                break;

            case Upgrade.B:
                actions = new()
                {
                    new ASpawn()
                    {
                        thing = new MidrowFlamethrower()
                        {
                            yAnimation = 0.0
                        },
                        offset = -1
                    },
                    new ASpawn()
                    {
                        thing = new MidrowFlamethrower()
                        {
                            yAnimation = 0.0,
                        }
                    }
                };
                break;
        }
        return actions;
    }
}
