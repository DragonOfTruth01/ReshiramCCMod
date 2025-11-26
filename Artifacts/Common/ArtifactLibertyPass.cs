using DragonOfTruth01.ReshiramCCMod.Cards;
using Nickel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonOfTruth01.ReshiramCCMod.Artifacts;

internal sealed class ArtifactLibertyPass : Artifact, IReshiramCCModArtifact
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("Liberty Pass", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.ReshiramCCMod_Deck.Deck,
                pools = [ArtifactPool.Common]
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/common/libertyPass.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "common", "Liberty Pass", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "common", "Liberty Pass", "description"]).Localize
        });
    }

    public override void OnReceiveArtifact(State state)
    {
        state.GetCurrentQueue().Add(new AAddCard
        {
            amount = 1,
            card = new CardSearingShot()
        });
        state.GetCurrentQueue().Add(new AAddCard
        {
            amount = 1,
            card = new CardVCreate()
        });
    }

    public override List<Tooltip>? GetExtraTooltips()
    {
        return new List<Tooltip>
        {
            new TTCard
            {
                card = new CardSearingShot()
            },
            new TTCard
            {
                card = new CardVCreate()
            }
        };
    }

    public override Spr GetSprite()
    {
        return ModEntry.Instance.ReshiramCCMod_Character_ArtifactLibertyPass.Sprite;
    }
}
