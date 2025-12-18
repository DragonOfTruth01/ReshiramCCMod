using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace DragonOfTruth01.ReshiramCCMod.Midrow;

internal sealed class MidrowFlamethrower : ShieldDrone
{
    public override Spr? GetIcon()
    {
        return ModEntry.Instance.ReshiramCCMod_Midrow_Flamethrower.Sprite;
    }

    public override double GetWiggleAmount() => 0.0;
    public override double GetWiggleRate() => 1.0;
    public override bool IsHostile() => targetPlayer;

    public override List<Tooltip> GetTooltips()
    {
        List<Tooltip> tooltips = [
            new GlossaryTooltip($"{ModEntry.Instance.Package.Manifest.UniqueName}::{GetType()}")
            {
                Icon = GetIcon()!,
                flipIconY = targetPlayer,
                Title = ModEntry.Instance.Localizations.Localize(["midrow", "Flamethrower", "name"]),
                TitleColor = Colors.midrow,
                Description = ModEntry.Instance.Localizations.Localize(["midrow", "Flamethrower", "description"])
            }
        ];
        if (bubbleShield)
            tooltips.Add(new TTGlossary("midrow.bubbleShield"));
        return tooltips;
    }

    public override bool IsFriendly()
    {
        return targetPlayer;
    }

    public override List<CardAction>? GetActions(State s, Combat c)
    {
        return new List<CardAction>
        {
            new AAttack
            {
                isBeam = true,
                fromDroneX = x,
                targetPlayer = targetPlayer,
                damage = 0,
                status = Status.heat,
                statusAmount = 1
            }
        };
    }

    public override void Render(G g, Vec v)
    {
        Spr Sprite;
        Sprite = ModEntry.Instance.ReshiramCCMod_Midrow_Flamethrower.Sprite;
        DrawWithHilight(g, Sprite, v + GetOffset(g), flipX: false, targetPlayer);
    }
    
}