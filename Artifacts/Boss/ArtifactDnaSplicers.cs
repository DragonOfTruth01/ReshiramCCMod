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

    public override List<Tooltip>? GetExtraTooltips()
    {
        return new List<Tooltip>
        {
            new TTCard
            {
                card = new CardGlaciate()
            },
            new TTCard
            {
                card = new CardIceBurn()
            }
        };
    }

    public override void OnReceiveArtifact(State state)
    {
        state.GetCurrentQueue().Add(new AAddCard
        {
            amount = 1,
            card = new CardGlaciate()
        });
        state.GetCurrentQueue().Add(new AAddCard
        {
            amount = 1,
            card = new CardIceBurn()
        });
    }

    public override void OnCombatStart(State state, Combat combat)
    {
        combat.QueueImmediate(new AStatus()
        {
            status = ModEntry.Instance.Thermosensitive.Status,
            statusAmount = 1,
            targetPlayer = false
        });
    }

    public override Spr GetSprite()
    {
        return ModEntry.Instance.ReshiramCCMod_Character_ArtifactDnaSplicers.Sprite;
    }
}
