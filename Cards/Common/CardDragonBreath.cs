using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace DragonOfTruth01.ReshiramCCMod.Cards;

internal sealed class CardDragonBreath : Card, IReshiramCCModCard
{
    private static IKokoroApi.IV2.IConditionalApi Conditional => ModEntry.Instance.KokoroApi.Conditional;

    public static void Register(IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("Dragon Breath", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.ReshiramCCMod_Deck.Deck,
                rarity = Rarity.common,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Dragon Breath", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        CardData data = new CardData()
        {
            cost = upgrade == Upgrade.B ? 2 : 1
        };
        return data;
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        List<CardAction> actions = new();

        switch (upgrade)
        {
            case Upgrade.None:
                List<CardAction> cardActionList1 = new List<CardAction>()
                {
                    new AAttack()
                    {
                        damage = GetDmg(s, 0),
                        status = Status.heat,
                        statusAmount = 1,
                    }
                };

                // Check if other ship is at or above the overheat threshold
                // If the other ship is null, we will crash, so do a check first
                if (c.otherShip != null)
                {
                    cardActionList1.Add(
                        Conditional.MakeAction(
                            new EnemyOverheatCondition(),
                            new AStatus()
                            {
                                status = ModEntry.Instance.Flammable.Status,
                                statusAmount = 1
                            }
                        ).AsCardAction
                    );
                }

                actions = cardActionList1;
                break;

            case Upgrade.A:
                List<CardAction> cardActionList2 = new List<CardAction>()
                {
                    new AAttack()
                    {
                        damage = GetDmg(s, 0),
                        status = Status.heat,
                        statusAmount = 2,
                    }
                };

                // Check if other ship is at or above the overheat threshold
                // If the other ship is null, we will crash, so do a check first
                if (c.otherShip != null)
                {
                    cardActionList2.Add(
                        Conditional.MakeAction(
                            new EnemyOverheatCondition(),
                            new AStatus()
                            {
                                status = ModEntry.Instance.Flammable.Status,
                                statusAmount = 1
                            }
                        ).AsCardAction
                    );
                }

                actions = cardActionList2;
                break;

            case Upgrade.B:
                List<CardAction> cardActionList3 = new List<CardAction>()
                {
                    new AAttack()
                    {
                        damage = GetDmg(s, 1),
                        status = Status.heat,
                        statusAmount = 1,
                    }
                };

                // Check if other ship is at or above the overheat threshold
                // If the other ship is null, we will crash, so do a check first
                if (c.otherShip != null)
                {
                    cardActionList3.Add(
                        Conditional.MakeAction(
                            new EnemyOverheatCondition(),
                            new AStatus()
                            {
                                status = ModEntry.Instance.Flammable.Status,
                                statusAmount = 2
                            }
                        ).AsCardAction
                    );
                }

                actions = cardActionList3;
                break;
        }
        return actions;
    }
    
    private sealed class EnemyOverheatCondition : IKokoroApi.IV2.IConditionalApi.IBoolExpression
    {
        public bool GetValue(State state, Combat combat)
        {
            if (combat.otherShip != null)
            {
                return combat.otherShip.Get(Status.heat) >= combat.otherShip.heatTrigger;
            }
            else
            {
                return false;
            }
        }

        public string GetTooltipDescription(State state, Combat? combat)
		{
            return ModEntry.Instance.Localizations.Localize(["condition", "isEnemyOverheating", "description"]);
		}

		public void Render(G g, ref Vec position, bool isDisabled, bool dontRender)
		{
			if (!dontRender)
				Draw.Sprite(
					ModEntry.Instance.ReshiramCCMod_Icon_EnemyOverheat.Sprite,
					position.x,
					position.y,
					color: isDisabled ? Colors.disabledIconTint : Colors.white
				);
			position.x += 8;
		}

		public IEnumerable<Tooltip> OverrideConditionalTooltip(State state, Combat? combat, Tooltip defaultTooltip, string defaultTooltipDescription)
			=> [
				new GlossaryTooltip($"AConditional::{ModEntry.Instance.Package.Manifest.UniqueName}::EnemyOverheat")
				{
					Icon = ModEntry.Instance.ReshiramCCMod_Icon_EnemyOverheat.Sprite,
					TitleColor = Colors.action,
					Title = ModEntry.Instance.Localizations.Localize(["condition", "isEnemyOverheating", "title"]),
					Description = defaultTooltipDescription,
				}
			];
    }
}
