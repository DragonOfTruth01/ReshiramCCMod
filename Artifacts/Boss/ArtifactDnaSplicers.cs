using DragonOfTruth01.ReshiramCCMod.Cards;
using Nickel;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using System.Reflection;

namespace DragonOfTruth01.ReshiramCCMod.Artifacts;

[HarmonyPatch]
internal sealed class ArtifactDnaSplicers : Artifact, IReshiramCCModArtifact
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("DNA Splicers", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.ReshiramCCMod_Deck.Deck,
                pools = [ArtifactPool.Boss]
            },
            Sprite = ModEntry.Instance.ReshiramCCMod_Character_ArtifactFlameOrb.Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "boss", "DNA Splicers", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "boss", "DNA Splicers", "description"]).Localize
        });
    }

    public override void OnReceiveArtifact(State state)
    {
        
    }

    public override Spr GetSprite()
    {
        return ModEntry.Instance.ReshiramCCMod_Character_ArtifactDnaSplicers.Sprite;
    }
}
