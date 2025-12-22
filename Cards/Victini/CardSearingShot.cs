using Nickel;
using System.Collections.Generic;
using System.Linq;
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

        int thisHandPos = -1;
        int rightmostEnergyVal = 0;

        switch (upgrade)
        {
            case Upgrade.None:

                // Get the energy value of the other rightmost card
                
                for(int i = 0; i < c.hand.Count(); ++i)
                {
                    if(c.hand[i].uuid == uuid)
                    {
                        thisHandPos = i;
                    }
                }
                
                // If this card is in our hand, do this logic
                if(thisHandPos != -1)
                {
                    // Check if there is a card to the right of this one
                    if(thisHandPos != c.hand.Count() - 1)
                    {
                        rightmostEnergyVal = c.hand[c.hand.Count() - 1].GetCurrentCost(s);
                    }
                    // Otherwise, check if there is a card to the left of this one (e.g. our position is not 0)
                    else if(thisHandPos > 0)
                    {
                        rightmostEnergyVal = c.hand[thisHandPos - 1].GetCurrentCost(s);
                    }
                }
                // This is possible if we play this card while it's not in our hand
                else
                {
                    if(c.hand.Count() != 0)
                    {
                        rightmostEnergyVal = c.hand[c.hand.Count() - 1].GetCurrentCost(s);
                    }
                }

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
                        new ASearingShotVariableHint()
                        {
                            status = ModEntry.Instance.ExhaustEnergy.Status,
                            secondStatus = ModEntry.Instance.One.Status
                        },
                        new ADummyAction()
                    ).AsCardAction,
                    ModEntry.Instance.KokoroApi.SpoofedActions.MakeAction(
                        new AAttack()
                        {
                            damage = GetDmg(s, 1) + rightmostEnergyVal,
                            xHint = 1
                        },
                        new ADummyAction()
                    ).AsCardAction,
                    ModEntry.Instance.KokoroApi.SpoofedActions.MakeAction(
                        new AStatus()
                        {
                            status = Status.heat,
                            statusAmount = 1 + rightmostEnergyVal,
                            xHint = 1
                        },
                        new ADummyAction()
                    ).AsCardAction
                };
                break;

            case Upgrade.A:
                // Get the energy value of the other rightmost card
                
                for(int i = 0; i < c.hand.Count(); ++i)
                {
                    if(c.hand[i].uuid == uuid)
                    {
                        thisHandPos = i;
                    }
                }
                
                // If this card is in our hand, do this logic
                if(thisHandPos != -1)
                {
                    // Check if there is a card to the right of this one
                    if(thisHandPos != c.hand.Count() - 1)
                    {
                        rightmostEnergyVal = c.hand[c.hand.Count() - 1].GetCurrentCost(s);
                    }
                    // Otherwise, check if there is a card to the left of this one (e.g. our position is not 0)
                    else if(thisHandPos > 0)
                    {
                        rightmostEnergyVal = c.hand[thisHandPos - 1].GetCurrentCost(s);
                    }
                }
                // This is possible if we play this card while it's not in our hand
                else
                {
                    if(c.hand.Count() != 0)
                    {
                        rightmostEnergyVal = c.hand[c.hand.Count() - 1].GetCurrentCost(s);
                    }
                }

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
                        new ASearingShotVariableHint()
                        {
                            status = ModEntry.Instance.ExhaustEnergy.Status,
                            secondStatus = ModEntry.Instance.Two.Status
                        },
                        new ADummyAction()
                    ).AsCardAction,
                    ModEntry.Instance.KokoroApi.SpoofedActions.MakeAction(
                        new AAttack()
                        {
                            damage = GetDmg(s, 2) + rightmostEnergyVal,
                            xHint = 1
                        },
                        new ADummyAction()
                    ).AsCardAction,
                    ModEntry.Instance.KokoroApi.SpoofedActions.MakeAction(
                        new AStatus()
                        {
                            status = Status.heat,
                            statusAmount = 2 + rightmostEnergyVal,
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
                        new ASearingShotVariableHint()
                        {
                            status = ModEntry.Instance.ExhaustEnergy.Status,
                            secondStatus = ModEntry.Instance.One.Status
                        },
                        new ADummyAction()
                    ).AsCardAction,
                    ModEntry.Instance.KokoroApi.SpoofedActions.MakeAction(
                        new AAttack()
                        {
                            damage = GetDmg(s, 1),
                            xHint = 1
                        },
                        new ADummyAction()
                    ).AsCardAction,
                    ModEntry.Instance.KokoroApi.SpoofedActions.MakeAction(
                        new AStatus()
                        {
                            status = Status.heat,
                            statusAmount = 1,
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
