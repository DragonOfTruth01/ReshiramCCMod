using Nickel;
using System.Collections.Generic;

namespace DragonOfTruth01.ReshiramCCMod;

public sealed class ASolarFlare : CardAction
{
    public override void Begin(G g, State s, Combat c)
    {
        // It's okay for the FightModifier to be null, if so we summon a solar flare
        if (c.modifier == null)
        {
            c.SetFightModifier(s, new MSolarFlare());
        }
        // If it's not null, we need to clear it first,
        // then we can implement the effect and summon its background
        else if (c.modifier.GetType() != typeof(MSolarFlare))
        {
            c.modifier = null;
            c.SetFightModifier(s, new MSolarFlare());
        }
    }

    public override Icon? GetIcon(State s)
    {
        return new Icon(ModEntry.Instance.ReshiramCCMod_Icon_SolarFlareActive.Sprite, number: null, color: Colors.redd, flipY: false);
    }

    public override List<Tooltip> GetTooltips(State s)
        => [
            new GlossaryTooltip($"action.{ModEntry.Instance.Package.Manifest.UniqueName}::Solar Flare")
            {
                Icon = ModEntry.Instance.ReshiramCCMod_Icon_SolarFlareActive.Sprite,
                TitleColor = Colors.action,
                Title = ModEntry.Instance.Localizations.Localize(["action", "Solar Flare", "name"]),
                Description = ModEntry.Instance.Localizations.Localize(["action", "Solar Flare", "description"])
            }
        ];
}