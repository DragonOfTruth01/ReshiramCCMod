using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace DragonOfTruth01.ReshiramCCMod.Cards;

internal sealed class CardGlaciate : Card, IReshiramCCModCard
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("Glaciate", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.ReshiramCCMod_WKyurem_Deck.Deck,
                rarity = Rarity.uncommon,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Glaciate", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        CardData data = new CardData()
        {
            art = ModEntry.Instance.ReshiramCCMod_Character_WKyurem_CardGlaciateBG.Sprite,
            description = ModEntry.Instance.Localizations.Localize(["card", "Glaciate", "description", upgrade.ToString()]),
            cost = 1,
            exhaust = upgrade == Upgrade.B
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
                    new AAddCard()
                    {
                        card = new CardIceNeedle(),
                        destination = CardDestination.Hand,
                        amount = 1
                    }
                };
                break;

            case Upgrade.A:
                actions = new()
                {
                    new AAddCard()
                    {
                        card = new CardIceNeedle(){ upgrade = Upgrade.A },
                        destination = CardDestination.Deck,
                        amount = 1
                    }
                };
                break;

            case Upgrade.B:
                actions = new()
                {
                    new AAddCard()
                    {
                        card = new CardIceNeedle(),
                        destination = CardDestination.Hand,
                        amount = 1
                    },
                    new AAddCard()
                    {
                        card = new CardIceNeedle(),
                        destination = CardDestination.Discard,
                        amount = 2
                    }
                };
                break;
        }
        return actions;
    }
}
