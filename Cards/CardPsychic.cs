using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace DragonOfTruth01.ReshiramCCMod.Cards;

internal sealed class CardPsychic : Card, ReshiramCCModCard
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("Psychic", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.ReshiramCCMod_Deck.Deck,
                rarity = Rarity.common,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Psychic", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        CardData data = new CardData()
        {
            cost = 1,
            flippable = true
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
                    new AMove(){
                        dir = -2
                    }
                };
                break;

            case Upgrade.A:
                actions = new()
                {
                    new AMove(){
                        dir = -2
                    },
                    new AMove(){
                        dir = 1,
                        targetPlayer = true
                    }
                };
                break;

            case Upgrade.B:
                actions = new()
                {
                    new AMove(){
                        dir = -3
                    },
                    new AStatus(){
                        status = Status.evade,
                        statusAmount = 1,
                        targetPlayer = true
                    },
                    new AStatus(){
                        status = Status.heat,
                        statusAmount = 1,
                        targetPlayer = true
                    }
                };
                break;
        }
        return actions;
    }
}
