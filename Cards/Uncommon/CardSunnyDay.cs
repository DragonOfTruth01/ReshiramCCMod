using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace DragonOfTruth01.ReshiramCCMod.Cards;

internal sealed class CardSunnyDay : Card, IReshiramCCModCard
{
    private static IKokoroApi.IV2.IConditionalApi Conditional => ModEntry.Instance.KokoroApi.Conditional;

    public static void Register(IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("Sunny Day", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.ReshiramCCMod_Deck.Deck,
                rarity = Rarity.uncommon,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Sunny Day", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        CardData data = new CardData()
        {
            art = ModEntry.Instance.ReshiramCCMod_Character_CardSunnyDayBG.Sprite,
            description = ModEntry.Instance.Localizations.Localize(["card", "Sunny Day", "description", upgrade.ToString()]),
            cost = 1,
            exhaust = upgrade != Upgrade.B
        };
        return data;
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        List<CardAction> actions = new();

        switch (upgrade)
        {
            case Upgrade.None:
                actions = new List<CardAction>()
                {
                    new ASolarFlare(),
                    new AStatus()
                    {
                        status = ModEntry.Instance.HeatResist.Status,
                        statusAmount = 2,
                        targetPlayer = true
                    }
                };
                break;

            case Upgrade.A:
                actions = new List<CardAction>()
                {
                    new ASolarFlare(),
                    new AStatus()
                    {
                        status = ModEntry.Instance.HeatResist.Status,
                        statusAmount = 3,
                        targetPlayer = true
                    },
                    new AStatus()
                    {
                        status = Status.serenity,
                        statusAmount = 1,
                        targetPlayer = true
                    }
                };
                break;

            case Upgrade.B:
                actions = new List<CardAction>();

                IKokoroApi.IV2.IConditionalApi.IConditionalAction act = Conditional.MakeAction(
                        new SolarFlareCondition(),
                        new AStatus()
                        {
                            status = ModEntry.Instance.Flammable.Status,
                            statusAmount = 2
                        }
                    );

                actions.Add(act.AsCardAction);

                actions.Add(new ASolarFlare());

                actions.Add(new AStatus()
                            {
                                status = ModEntry.Instance.HeatResist.Status,
                                statusAmount = 2,
                                targetPlayer = true
                            });
                break;
        }
        return actions;
    }
}
