using DragonOfTruth01.ReshiramCCMod.Cards;
using Nickel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using System.Runtime.CompilerServices;

namespace DragonOfTruth01.ReshiramCCMod.Artifacts;

[HarmonyPatch]
internal sealed class ArtifactCharcoal : Artifact, IReshiramCCModArtifact
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("Charcoal", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.ReshiramCCMod_Deck.Deck,
                pools = [ArtifactPool.Common]
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/common/charcoal.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "common", "Charcoal", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "common", "Charcoal", "description"]).Localize
        });
    }

    int counter = 0;
    readonly int artifactTriggerAmt = 3;

    public override int? GetDisplayNumber(State s)
    {
        return counter;
    }

    public override void OnTurnStart(State s, Combat c)
    {
        if (++counter >= artifactTriggerAmt)
        {
            c.Queue([
                new AStatus
                {
                    status = ModEntry.Instance.Smoldering.Status,
                    statusAmount = 1
                }
            ]);

            Pulse();

            counter = 0;
        }
    }
    
    public override Spr GetSprite()
    {
        return ModEntry.Instance.ReshiramCCMod_Character_ArtifactCharcoal.Sprite;
    }
}
