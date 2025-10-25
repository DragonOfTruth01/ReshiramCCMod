using DragonOfTruth01.ReshiramCCMod.Cards;
using Nickel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using System.Runtime.CompilerServices;

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
                pools = [ArtifactPool.Common]
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/common/flameOrb.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "common", "Flame Orb", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "common", "Flame Orb", "description"]).Localize
        });
    }

    static bool hasTriggeredThisTurn = false;

    private class HarmonyRef
    {
        public int oldHeatAmt;
        public bool triggered;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(AStatus), "Begin")]
    private static void AStatus_Begin_Prefix(AStatus __instance, State s, out HarmonyRef __state)
    {
        __state = new HarmonyRef();
        __state.oldHeatAmt = s.ship.Get(Status.heat);
        __state.triggered = hasTriggeredThisTurn;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(AStatus), "Begin")]
    private static void AStatus_Begin_Postfix(AStatus __instance, State s, Combat c, HarmonyRef __state)
    {
        var heatDiff = s.ship.Get(Status.heat) - __state.oldHeatAmt;

        // If the player ship has more heat, and this is the first time heat is gained this turn...
        if (heatDiff > 0 && !__state.triggered)
        {
            // First set that we triggered the relic for this turn, to prevent recursion
            hasTriggeredThisTurn = true;

            //Then, apply this relic's effects
            c.Queue([
                new AStatus
                {
                    status = Status.overdrive,
                    statusAmount = 1,
                    targetPlayer = true
                },
                new AStatus
                {
                    status = Status.heat,
                    statusAmount = 1,
                    targetPlayer = true
                }
            ]);
        }
    }

    public override void OnTurnStart(State s, Combat c)
    {
        hasTriggeredThisTurn = false;
    }
}
