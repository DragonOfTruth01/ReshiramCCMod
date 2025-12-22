using Nickel;
using FSPRO;
using System.Collections.Generic;
using HarmonyLib;

namespace DragonOfTruth01.ReshiramCCMod;

[HarmonyPatch]
public sealed class AChooseExhaust : DynamicWidthCardAction
{
    public override void Begin(G g, State s, Combat c)
    {
        // Do nothing - to be spoofed only
    }

    public override Icon? GetIcon(State s)
    {
        return new Icon(ModEntry.Instance.ReshiramCCMod_Icon_ChooseExhaust.Sprite, number: null, color: Colors.textMain, flipY: false);
    }

    public override List<Tooltip> GetTooltips(State s)
        => [
            new GlossaryTooltip($"action.{ModEntry.Instance.Package.Manifest.UniqueName}::ChooseExhaust")
            {
                Icon = ModEntry.Instance.ReshiramCCMod_Icon_ChooseExhaust.Sprite,
                TitleColor = Colors.action,
                Title = ModEntry.Instance.Localizations.Localize(["action", "Choose Exhaust", "name"]),
                Description = ModEntry.Instance.Localizations.Localize(["action", "Chose Exhaust", "description"])
            }
        ];
}