using Nickel;
using System.Collections.Generic;
using System.Reflection;

/* Like other namespaces, this can be named whatever
 * However it's recommended that you follow the structure defined by ModEntry of <AuthorName>.<ModName> or <AuthorName>.<ModName>.Cards*/
namespace DragonOfTruth01.ReshiramCCMod.Cards;

internal sealed class CardDragonFang : Card, IDemoCard
{
    /* For a bit more info on the Register Method, look at InternalInterfaces.cs and 1. CARDS section in ModEntry */
    public static void Register(IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("Dragon Fang", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.ReshiramCCMod_Deck.Deck,
                rarity = Rarity.common,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Dragon Fang", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        CardData data = new CardData()
        {
            cost = 2,
        };
        return data;
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        /* The meat of the card, this is where we define what a card does, and some would say the most fun part of modding Cobalt Core happens here! */
        List<CardAction> actions = new();

        /* Since we want to have different actions for each Upgrade, we use a switch that covers the Upgrade paths we've defined */
        switch (upgrade)
        {
            case Upgrade.None:
                actions = new()
                {
                    new AAttack()
                    {
                        damage = GetDmg(s, 1),
                    },
                    new AAttack()
                    {
                        damage = GetDmg(s, 1),
                        status = ModEntry.Instance.Flammable.Status,
                        statusAmount = 1
                    }
                };
                break;

            case Upgrade.A:
                actions = new()
                {
                    new AAttack()
                    {
                        damage = GetDmg(s, 2),
                    },
                    new AAttack()
                    {
                        damage = GetDmg(s, 2),
                        status = ModEntry.Instance.Flammable.Status,
                        statusAmount = 1
                    }
                };
                break;

            case Upgrade.B:
                actions = new()
                {
                    new AAttack()
                    {
                        damage = GetDmg(s, 2),
                        status = Status.heat,
                        statusAmount = 2
                    },
                    new AAttack()
                    {
                        damage = GetDmg(s, 2),
                        status = ModEntry.Instance.Flammable.Status,
                        statusAmount = 1
                    }
                };
                break;
        }
        return actions;
    }
}
