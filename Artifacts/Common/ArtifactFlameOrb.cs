using DragonOfTruth01.ReshiramCCMod.Cards;
using Nickel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonOfTruth01.ReshiramCCMod.Artifacts;

internal sealed class ArtifactFlameOrb : Artifact, IReshiramCCModArtifact
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("Flame Orb", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.ReshiramCCMod_Deck.Deck,
                pools = [ArtifactPool.Common]
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/flameOrb.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "common", "Flame Orb", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "common", "Flame Orb", "description"]).Localize
        });
    }

    public override List<Tooltip>? GetExtraTooltips()
        => new List<Tooltip>
        {
            new TTCard
            {
                card = new CardIncinerate
                {
                    temporaryOverride = true
                }
            }
        }
        .ToList();

    public override void OnTurnStart(State s, Combat c)
    {
        if (!c.isPlayerTurn)
            return;
        c.QueueImmediate([
            new AAddCard
            {
                card = new CardIncinerate
                {
                    temporaryOverride = true
                },
                destination = CardDestination.Hand
            }
        ]);
    }
}
