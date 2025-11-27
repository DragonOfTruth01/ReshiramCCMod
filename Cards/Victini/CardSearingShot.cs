using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace DragonOfTruth01.ReshiramCCMod.Cards;

internal sealed class CardSearingShot : Card, IReshiramCCModCard
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("Searing Shot", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.ReshiramCCMod_Victini_Deck.Deck,
                rarity = Rarity.uncommon,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Searing Shot", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        CardData data = new CardData()
        {
            art = ModEntry.Instance.ReshiramCCMod_Character_Victini_CardSearingShotBG.Sprite,
            description = ModEntry.Instance.Localizations.Localize(["card", "Searing Shot", "description", upgrade.ToString()]),
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
                    new ADamageHeatRightmostExhaust()
                    {
                        damageHeatMod = 1
                    }
                };
                break;

            case Upgrade.A:
                actions = new()
                {
                    new ADamageHeatRightmostExhaust()
                    {
                        damageHeatMod = 2
                    }
                };
                break;

            case Upgrade.B:
                actions = new()
                {
                    new ACardSelect()
                    {
                        browseAction = new ChooseCardInYourHandToSearingShot(){ damageHeatMod = 1 },
                        browseSource = CardBrowse.Source.Hand
                    }
                };
                break;
        }
        return actions;
    }
}
