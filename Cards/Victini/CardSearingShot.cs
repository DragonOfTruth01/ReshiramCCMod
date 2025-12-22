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
                    // Spoof actions for better visibility

                    ModEntry.Instance.KokoroApi.SpoofedActions.MakeAction(
                        new AExhaustRight(),
                        new ADamageHeatRightmostExhaust()
                        {
                            damageMod = GetDmg(s, 1 + 10), // +10/-10 to workaround conflicts w/ damage calculation
                            heatMod = 1
                        }
                    ).AsCardAction,
                    ModEntry.Instance.KokoroApi.SpoofedActions.MakeAction(
                        new AVariableHint()
                        {
                            status = ModEntry.Instance.ExhaustEnergy.Status,
                            secondStatus = ModEntry.Instance.One.Status
                        },
                        new ADummyAction()
                    ).AsCardAction,
                    ModEntry.Instance.KokoroApi.SpoofedActions.MakeAction(
                        new AAttack()
                        {
                            damage = 0,
                            xHint = 1
                        },
                        new ADummyAction()
                    ).AsCardAction,
                    ModEntry.Instance.KokoroApi.SpoofedActions.MakeAction(
                        new AStatus()
                        {
                            status = Status.heat,
                            xHint = 1
                        },
                        new ADummyAction()
                    ).AsCardAction
                };
                break;

            case Upgrade.A:
                actions = new()
                {
                    // Spoof actions for better visibility

                    ModEntry.Instance.KokoroApi.SpoofedActions.MakeAction(
                        new AExhaustRight(),
                        new ADamageHeatRightmostExhaust()
                        {
                            damageMod = GetDmg(s, 2 + 10), // +10/-10 to workaround conflicts w/ damage calculation
                            heatMod = 2
                        }
                    ).AsCardAction,
                    ModEntry.Instance.KokoroApi.SpoofedActions.MakeAction(
                        new AVariableHint()
                        {
                            status = ModEntry.Instance.ExhaustEnergy.Status,
                            secondStatus = ModEntry.Instance.Two.Status
                        },
                        new ADummyAction()
                    ).AsCardAction,
                    ModEntry.Instance.KokoroApi.SpoofedActions.MakeAction(
                        new AAttack()
                        {
                            damage = 0,
                            xHint = 1
                        },
                        new ADummyAction()
                    ).AsCardAction,
                    ModEntry.Instance.KokoroApi.SpoofedActions.MakeAction(
                        new AStatus()
                        {
                            status = Status.heat,
                            xHint = 1
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
                        new AChooseExhaust(),
                        new ACardSelect()
                    {
                        browseAction = new ChooseCardInYourHandToSearingShot()
                        {
                            damageMod = GetDmg(s, 1 + 10), // +10/-10 to workaround conflicts w/ damage calculation
                            heatMod = 1
                        },
                        browseSource = CardBrowse.Source.Hand
                    }
                    ).AsCardAction,
                    ModEntry.Instance.KokoroApi.SpoofedActions.MakeAction(
                        new AVariableHint()
                        {
                            status = ModEntry.Instance.ExhaustEnergy.Status,
                            secondStatus = ModEntry.Instance.One.Status
                        },
                        new ADummyAction()
                    ).AsCardAction,
                    ModEntry.Instance.KokoroApi.SpoofedActions.MakeAction(
                        new AAttack()
                        {
                            damage = 0,
                            xHint = 1
                        },
                        new ADummyAction()
                    ).AsCardAction,
                    ModEntry.Instance.KokoroApi.SpoofedActions.MakeAction(
                        new AStatus()
                        {
                            status = Status.heat,
                            xHint = 1
                        },
                        new ADummyAction()
                    ).AsCardAction
                };
                break;
        }
        return actions;
    }
}
