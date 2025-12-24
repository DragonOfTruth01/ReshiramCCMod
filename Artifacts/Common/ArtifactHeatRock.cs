using DragonOfTruth01.ReshiramCCMod.Cards;
using Nickel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonOfTruth01.ReshiramCCMod.Artifacts;

internal sealed class ArtifactHeatRock : Artifact, IReshiramCCModArtifact
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("Heat Rock", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.ReshiramCCMod_Deck.Deck,
                pools = [ArtifactPool.Common]
            },
            Sprite = ModEntry.Instance.ReshiramCCMod_Character_ArtifactHeatRock.Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "common", "Heat Rock", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "common", "Heat Rock", "description"]).Localize
        });
    }

    public override void OnCombatStart(State s, Combat c)
    {
        c.Queue([
            new AStatus
            {
                status = ModEntry.Instance.Flammable.Status,
                statusAmount = 1
            }
        ]);

        Pulse();
    }

    public override Spr GetSprite()
    {
        return ModEntry.Instance.ReshiramCCMod_Character_ArtifactHeatRock.Sprite;
    }
}
