using Nickel;
using System.Collections.Generic;
using System.Reflection;
using DragonOfTruth01.ReshiramCCMod;

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
            art = ModEntry.Instance.ReshiramCCMod_Character_Victini_CardVCreateBG.Sprite,
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
                    // Spoof actions for better visibility

                    ModEntry.Instance.KokoroApi.SpoofedActions.MakeAction(
                        new AExhaustLeftRight(),
                        new ADummyAction()
                    ).AsCardAction,
                    ModEntry.Instance.KokoroApi.SpoofedActions.MakeAction(
                        new AAttack()
                        {
                            damage = GetDmg(s, 3),
                            status = ModEntry.Instance.Smoldering.Status,
                            statusAmount = 2
                        },
                        new AVCreateAttack()
                        {
                            damageAmount = GetDmg(s, 3),
                            smolderingAmount = 2,
                            heatAmount = 0
                        }
                    ).AsCardAction
                };
                break;

            case Upgrade.A:
                actions = new()
                {
                    // Spoof actions for better visibility

                    ModEntry.Instance.KokoroApi.SpoofedActions.MakeAction(
                        new AExhaustLeftRight(),
                        new ADummyAction()
                    ).AsCardAction,
                    ModEntry.Instance.KokoroApi.SpoofedActions.MakeAction(
                        new AAttack()
                        {
                            damage = GetDmg(s, 3),
                            status = ModEntry.Instance.Smoldering.Status,
                            statusAmount = 2
                        },
                        new AVCreateAttack()
                        {
                            damageAmount = GetDmg(s, 3),
                            smolderingAmount = 2,
                            heatAmount = 3
                        }
                    ).AsCardAction,
                    ModEntry.Instance.KokoroApi.SpoofedActions.MakeAction(
                        new AStatus()
                        {
                            status = Status.heat,
                            statusAmount = 3
                        },
                        new ADummyAction()
                    ).AsCardAction
                };
                break;

            case Upgrade.B:
                actions = new()
                {
                    // Spoof actions for better visibility

                    ModEntry.Instance.KokoroApi.SpoofedActions.MakeAction(
                        new AExhaustLeftRight(),
                        new ADummyAction()
                    ).AsCardAction,
                    ModEntry.Instance.KokoroApi.SpoofedActions.MakeAction(
                        new AAttack()
                        {
                            damage = GetDmg(s, 5),
                            status = ModEntry.Instance.Smoldering.Status,
                            statusAmount = 3
                        },
                        new AVCreateAttack()
                        {
                            damageAmount = GetDmg(s, 5),
                            smolderingAmount = 3,
                            heatAmount = 0
                        }
                    ).AsCardAction
                };
                break;
        }
        return actions;
    }
}
