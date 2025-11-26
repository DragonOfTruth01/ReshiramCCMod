using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace DragonOfTruth01.ReshiramCCMod.Cards;

internal sealed class CardVCreate : Card, IReshiramCCModCard
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("V-Create", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.ReshiramCCMod_Victini_Deck.Deck,
                rarity = Rarity.rare,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "V-Create", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        object damageString = "";

            int damageCalc = upgrade == Upgrade.B ? 5 : 3;
            damageString = "<c=redd>" + GetDmg(state, damageCalc) + "</c>";

        CardData data = new CardData()
        {
            art = ModEntry.Instance.ReshiramCCMod_Character_CardFireFangBG.Sprite,
            description = ModEntry.Instance.Localizations.Localize(["card", "V-Create", "description", upgrade.ToString()], new { damageString }),
            cost = upgrade == Upgrade.B ? 3 : 2,
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
                    new AVCreateAttack()
                    {
                        damageAmount = 3,
                        smolderingAmount = 2,
                        heatAmount = 0
                    }
                };
                break;

            case Upgrade.A:
                actions = new()
                {
                    new AVCreateAttack()
                    {
                        damageAmount = 3,
                        smolderingAmount = 2,
                        heatAmount = 3
                    }
                };
                break;

            case Upgrade.B:
                actions = new()
                {
                    new AVCreateAttack()
                    {
                        damageAmount = 5,
                        smolderingAmount = 3,
                        heatAmount = 0
                    }
                };
                break;
        }
        return actions;
    }
}
