using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace DragonOfTruth01.ReshiramCCMod.Cards;

internal sealed class CardDragonBreath : Card, IReshiramCCModCard
{
    private static IKokoroApi.IV2.IConditionalApi Conditional => ModEntry.Instance.KokoroApi.Conditional;

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
            art = ModEntry.Instance.ReshiramCCMod_Character_CardDragonBreathBG.Sprite,
            cost = 1
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
                    new AAttack()
                    {
                        damage = GetDmg(s, 1)
                    }
                };

                // Check if other ship is at or above the overheat threshold
                // If the other ship is null, we will crash, so do a check first
                if (c.otherShip != null)
                {
                    IKokoroApi.IV2.IConditionalApi.IConditionalAction act = Conditional.MakeAction(
                        new EnemyOverheatCondition(false),
                        new AStatus()
                        {
                            status = ModEntry.Instance.Flammable.Status,
                            statusAmount = 2
                        }
                    );

                    act.SetFadeUnsatisfied(false);

                    cardActionList1.Add(act.AsCardAction);
                }

                actions = cardActionList1;
                break;

            case Upgrade.A:
                List<CardAction> cardActionList2 = new List<CardAction>()
                {
                    new AAttack()
                    {
                        damage = GetDmg(s, 1),
                        status = Status.heat,
                        statusAmount = 1,
                    }
                };

                // Check if other ship is at or above the overheat threshold
                // If the other ship is null, we will crash, so do a check first
                if (c.otherShip != null)
                {
                    IKokoroApi.IV2.IConditionalApi.IConditionalAction act = Conditional.MakeAction(
                        new EnemyOverheatCondition(false),
                        new AStatus()
                        {
                            status = ModEntry.Instance.Flammable.Status,
                            statusAmount = 2
                        }
                    );

                    act.SetFadeUnsatisfied(false);

                    cardActionList2.Add(act.AsCardAction);
                }

                actions = cardActionList2;
                break;

            case Upgrade.B:
                List<CardAction> cardActionList3 = new List<CardAction>()
                {
                    new AAttack()
                    {
                        damage = GetDmg(s, 1),
                        status = Status.heat,
                        statusAmount = 2,
                    }
                };

                // Check if other ship is at or above the overheat threshold
                // If the other ship is null, we will crash, so do a check first
                if (c.otherShip != null)
                {
                    IKokoroApi.IV2.IConditionalApi.IConditionalAction act = Conditional.MakeAction(
                        new EnemyOverheatCondition(false),
                        new AStatus()
                        {
                            status = ModEntry.Instance.Flammable.Status,
                            statusAmount = 2
                        }
                    );

                    act.SetFadeUnsatisfied(false);

                    cardActionList3.Add(act.AsCardAction);
                }

                cardActionList3.Add(
                    new AStatus()
                    {
                        status = Status.heat,
                        statusAmount = 1,
                        targetPlayer = true
                    }
                );

                actions = cardActionList3;
                break;
        }
        return actions;
    }
    
    
}
