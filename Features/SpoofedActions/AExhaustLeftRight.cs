using Nickel;
using FSPRO;
using System.Collections.Generic;
using HarmonyLib;

namespace DragonOfTruth01.ReshiramCCMod;

[HarmonyPatch]
public sealed class AExhaustLeftRight : DynamicWidthCardAction
{
    public override void Begin(G g, State s, Combat c)
    {
        // Do nothing - to be spoofed only
    }

    public override Icon? GetIcon(State s)
    {
        return new Icon(ModEntry.Instance.ReshiramCCMod_Icon_ExhaustLeftRight.Sprite, number: null, color: Colors.textMain, flipY: false);
    }

    public override List<Tooltip> GetTooltips(State s)
        => [
            new GlossaryTooltip($"action.{ModEntry.Instance.Package.Manifest.UniqueName}::ExhaustLeftRight")
            {
                Icon = ModEntry.Instance.ReshiramCCMod_Icon_ExhaustRight.Sprite,
                TitleColor = Colors.action,
                Title = ModEntry.Instance.Localizations.Localize(["action", "Exhaust Left Right", "name"]),
                Description = ModEntry.Instance.Localizations.Localize(["action", "Exhaust Left Right", "description"])
            }
        ];
}