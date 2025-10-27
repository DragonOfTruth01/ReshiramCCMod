using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace DragonOfTruth01.ReshiramCCMod.Cards;

internal sealed class CardHeatWave : Card, IReshiramCCModCard
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("Heat Wave", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.ReshiramCCMod_Deck.Deck,
                rarity = Rarity.common,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Heat Wave", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        CardData data = new CardData()
        {
            art = ModEntry.Instance.ReshiramCCMod_Character_CardHeatWaveBG.Sprite,
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
                actions = new()
                {
                    new AStatus(){
                        status = ModEntry.Instance.Smoldering.Status,
                        statusAmount = 1
                    },
                    new AStatus(){
                        status = Status.heat,
                        statusAmount = 1
                    },
                    new AStatus(){
                        status = Status.heat,
                        statusAmount = 1,
                        targetPlayer = true
                    }
                };
                break;

            case Upgrade.A:
                actions = new()
                {
                    new AStatus(){
                        status = ModEntry.Instance.Smoldering.Status,
                        statusAmount = 1
                    },
                    new AStatus(){
                        status = Status.heat,
                        statusAmount = 1
                    }
                };
                break;

            case Upgrade.B:
                actions = new()
                {
                    new AStatus(){
                        status = ModEntry.Instance.Smoldering.Status,
                        statusAmount = 2
                    },
                    new AStatus(){
                        status = Status.heat,
                        statusAmount = 2
                    },
                    new AStatus(){
                        status = Status.heat,
                        statusAmount = 3,
                        targetPlayer = true
                    }
                };
                break;
        }
        return actions;
    }
}
