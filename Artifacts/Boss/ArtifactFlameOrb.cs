using DragonOfTruth01.ReshiramCCMod.Cards;
using Nickel;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using System.Reflection;

namespace DragonOfTruth01.ReshiramCCMod.Artifacts;

[HarmonyPatch]
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
                pools = [ArtifactPool.Boss]
            },
            Sprite = ModEntry.Instance.ReshiramCCMod_Character_ArtifactFlameOrb.Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "boss", "Flame Orb", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "boss", "Flame Orb", "description"]).Localize
        });
    }

    [HarmonyPostfix]
	[HarmonyPatch(typeof(Ship), "CanBeNegative")]
    private static void Ship_CanBeNegative_Postfix(Status status, ref bool __result)
    {
        if (status == ModEntry.Instance.HeatResist.Status)
        {
            __result = true;
        }
    }

    public override void OnReceiveArtifact(State state)
    {
        state.ship.baseEnergy++;
    }

    public override void OnRemoveArtifact(State state)
    {
        state.ship.baseEnergy--;
    }

    public override void OnCombatStart(State state, Combat combat)
    {
        combat.QueueImmediate(new AStatus()
        {
            status = ModEntry.Instance.HeatResist.Status,
            statusAmount = -1,
            targetPlayer = true
        });
    }

    public override Spr GetSprite()
    {
        return ModEntry.Instance.ReshiramCCMod_Character_ArtifactFlameOrb.Sprite;
    }
}
