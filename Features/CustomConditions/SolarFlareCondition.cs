using Nickel;
using System.Collections.Generic;

namespace DragonOfTruth01.ReshiramCCMod;

public sealed class SolarFlareCondition : IKokoroApi.IV2.IConditionalApi.IBoolExpression
{
    public SolarFlareCondition()
    {
    }

    public bool GetValue(State state, Combat combat)
    {
        return combat.modifier != null && combat.modifier.GetType() == typeof(MSolarFlare);
    }

    public string GetTooltipDescription(State state, Combat? combat)
    {
        return ModEntry.Instance.Localizations.Localize(["condition", "isSolarFlareActive", "description"]);
    }

    public void Render(G g, ref Vec position, bool isDisabled, bool dontRender)
    {
        if (!dontRender)
            Draw.Sprite(
                ModEntry.Instance.ReshiramCCMod_Icon_SolarFlareActive.Sprite,
                position.x,
                position.y,
                color: isDisabled ? Colors.disabledIconTint : Colors.white
            );
        position.x += 8;
    }

    public IEnumerable<Tooltip> OverrideConditionalTooltip(State state, Combat? combat, Tooltip defaultTooltip, string defaultTooltipDescription)
        => [
            new GlossaryTooltip($"AConditional::{ModEntry.Instance.Package.Manifest.UniqueName}::SolarFlare")
                {
                    Icon = ModEntry.Instance.ReshiramCCMod_Icon_SolarFlareActive.Sprite,
                    TitleColor = Colors.action,
                    Title = ModEntry.Instance.Localizations.Localize(["condition", "isSolarFlareActive", "title"]),
                    Description = defaultTooltipDescription,
                }
        ];
}