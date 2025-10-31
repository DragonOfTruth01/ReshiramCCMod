using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace DragonOfTruth01.ReshiramCCMod.Cards;

internal sealed class CardFacade : Card, IReshiramCCModCard
{
    private static IKokoroApi.IV2.IConditionalApi Conditional => ModEntry.Instance.KokoroApi.Conditional;

    public static void Register(IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("Facade", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.ReshiramCCMod_Deck.Deck,
                rarity = Rarity.common,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Facade", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        CardData data = new CardData()
        {
            art = ModEntry.Instance.ReshiramCCMod_Character_CardBackground.Sprite,
            cost = upgrade == Upgrade.A ? 0 : 1,
            retain = upgrade == Upgrade.B
        };
        return data;
    }

    private int getHeatAmt(State s)
    {
        int heatAmt = 0;

        if (s.route is Combat)
        {
            heatAmt = s.ship.Get(Status.heat);
        }

        return heatAmt;
        
    }

    public override List<CardAction> GetActions(State s, Combat c)
    {
        List<CardAction> actions = new();

        switch (upgrade)
        {
            case Upgrade.None:
                List<CardAction> cardActionList1 = new List<CardAction>()
                {
                    new AVariableHint
                    {
                        status = Status.heat
                    },
                    new AAttack()
                    {
                        damage = GetDmg(s, getHeatAmt(s)),
                        piercing = true,
                        xHint = 1
                    }
                };

                actions = cardActionList1;
                break;

            case Upgrade.A:
                List<CardAction> cardActionList2 = new List<CardAction>()
                {
                    new AVariableHint
                    {
                        status = Status.heat
                    },
                    new AAttack()
                    {
                        damage = GetDmg(s, getHeatAmt(s)),
                        piercing = true,
                        xHint = 1
                    }
                };

                actions = cardActionList2;
                break;

            case Upgrade.B:
                List<CardAction> cardActionList3 = new List<CardAction>()
                {
                    new AVariableHint
                    {
                        status = Status.heat
                    },
                    new AAttack()
                    {
                        damage = GetDmg(s, getHeatAmt(s)),
                        piercing = true,
                        xHint = 1
                    }
                };

                actions = cardActionList3;
                break;
        }
        return actions;
    }
    
    
}
