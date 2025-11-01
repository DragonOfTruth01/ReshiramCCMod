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
            Sprite = ModEntry.Instance.ReshiramCCMod_Character_ArtifactFlameOrb.Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "common", "Flame Orb", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "common", "Flame Orb", "description"]).Localize
        });
    }

    bool hasTriggeredThisTurn = false;

    private class HarmonyRef
    {
        public int oldHeatAmt;
        public bool triggered;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(AStatus), "Begin")]
    private static void AStatus_Begin_Prefix(AStatus __instance, State s, out HarmonyRef __state)
    {
        var artifact = s.EnumerateAllArtifacts().OfType<ArtifactFlameOrb>().FirstOrDefault();

        // If we have the artifact, populate our __state with meaningful values
        if (artifact != null)
        {
            __state = new HarmonyRef();
            __state.oldHeatAmt = s.ship.Get(Status.heat);
            __state.triggered = artifact.hasTriggeredThisTurn;
        }
        // If we don't have an instance of the artifact, we dont' care what we assign it
        // (Don't worry, we're not going to use __state in the postfix anyway)
        else
        {
            __state = new HarmonyRef();
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(AStatus), "Begin")]
    private static void AStatus_Begin_Postfix(AStatus __instance, State s, Combat c, HarmonyRef __state)
    {
        // Only do this postfix if we have the artifact
        var artifact = s.EnumerateAllArtifacts().OfType<ArtifactFlameOrb>().FirstOrDefault();

        if (artifact != null)
        {
            var heatDiff = s.ship.Get(Status.heat) - __state.oldHeatAmt;

            // If the player ship has more heat, and this is the first time heat is gained this turn...
            if (heatDiff > 0 && !__state.triggered)
            {
                // First set that we triggered the relic for this turn, to prevent recursion
                artifact.hasTriggeredThisTurn = true;

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

                artifact.Pulse();
            }
        }
    }

    public override void OnTurnStart(State s, Combat c)
    {
        hasTriggeredThisTurn = false;
    }

    public override Spr GetSprite()
    {
        if (!hasTriggeredThisTurn)
        {
            return ModEntry.Instance.ReshiramCCMod_Character_ArtifactFlameOrb.Sprite;
        }
        else
        {
            return ModEntry.Instance.ReshiramCCMod_Character_ArtifactFlameOrb_Disabled.Sprite;
        }
    }
}
