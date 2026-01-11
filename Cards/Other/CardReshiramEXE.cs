using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace DragonOfTruth01.ReshiramCCMod.Cards;

internal sealed class CardReshiramEXE : Card, IReshiramCCModCard
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("Reshiram.EXE", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = Deck.colorless,
                rarity = Rarity.common,
                dontOffer = false,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Reshiram.EXE", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        CardData data = new CardData()
        {
            art = ModEntry.Instance.ReshiramCCMod_Character_CardBackground.Sprite,
            description = ModEntry.Instance.Localizations.Localize(["card", "Reshiram.EXE", "description", upgrade.ToString()]),
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
                    new ACardOffering
                        {
                            amount = 2,
                            limitDeck = ModEntry.Instance.ReshiramCCMod_Deck.Deck,
                            makeAllCardsTemporary = true,
                            overrideUpgradeChances = false,
                            canSkip = false,
                            inCombat = true,
                            discount = -1
                        }
                };
                break;

            case Upgrade.A:
                actions = new()
                {
                    new ACardOffering
                        {
                            amount = 2,
                            limitDeck = ModEntry.Instance.ReshiramCCMod_Deck.Deck,
                            makeAllCardsTemporary = true,
                            overrideUpgradeChances = false,
                            canSkip = false,
                            inCombat = true,
                            discount = -1
                        }
                };
                break;

            case Upgrade.B:
                actions = new()
                {
                    new ACardOffering
                        {
                            amount = 3,
                            limitDeck = ModEntry.Instance.ReshiramCCMod_Deck.Deck,
                            makeAllCardsTemporary = true,
                            overrideUpgradeChances = false,
                            canSkip = false,
                            inCombat = true,
                            discount = -1
                        }
                };
                break;
        }
        return actions;
    }
}
