using Nickel;
using System.Collections.Generic;

namespace DragonOfTruth01.ReshiramCCMod;

public sealed class EnemyNotOverheatCondition : IKokoroApi.IV2.IConditionalApi.IBoolExpression
{
    private bool conditionalFade;

    public EnemyNotOverheatCondition(bool cf)
    {
        conditionalFade = cf;
    }

    public bool GetValue(State state, Combat combat)
    {
        if (combat.otherShip != null)
        {
            return combat.otherShip.Get(Status.heat) < combat.otherShip.heatTrigger;
        }
        else
        {
            return false;
        }
    }

    public string GetTooltipDescription(State state, Combat? combat)
    {
        return ModEntry.Instance.Localizations.Localize(["condition", "isEnemyNotOverheating", "description"]);
    }

    public void Render(G g, ref Vec position, bool isDisabled, bool dontRender)
    {
        if (!dontRender)
            Draw.Sprite(
                ModEntry.Instance.ReshiramCCMod_Icon_EnemyNotOverheat.Sprite,
                position.x,
                position.y,
                color: conditionalFade && isDisabled ? Colors.disabledIconTint : Colors.white
            );
        position.x += 8;
    }

    public IEnumerable<Tooltip> OverrideConditionalTooltip(State state, Combat? combat, Tooltip defaultTooltip, string defaultTooltipDescription)
        => [
            new GlossaryTooltip($"AConditional::{ModEntry.Instance.Package.Manifest.UniqueName}::EnemyNotOverheat")
                {
                    Icon = ModEntry.Instance.ReshiramCCMod_Icon_EnemyNotOverheat.Sprite,
                    TitleColor = Colors.action,
                    Title = ModEntry.Instance.Localizations.Localize(["condition", "isEnemyNotOverheating", "title"]),
                    Description = defaultTooltipDescription,
                }
        ];
}