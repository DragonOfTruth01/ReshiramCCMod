using DragonOfTruth01.ReshiramCCMod.Cards;
using Nickel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using System.Runtime.CompilerServices;

namespace DragonOfTruth01.ReshiramCCMod.Artifacts;

[HarmonyPatch]
internal sealed class ArtifactRawstBerry : Artifact, IReshiramCCModArtifact
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("Rawst Berry", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.ReshiramCCMod_Deck.Deck,
                pools = [ArtifactPool.Common]
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/common/rawstBerry.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "common", "Rawst Berry", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "common", "Rawst Berry", "description"]).Localize
        });
    }

    bool hasTriggeredThisTurn = false;

    private class HarmonyRef
    {
        public bool triggered;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(AStatus), "Begin")]
    private static void AStatus_Begin_Prefix(AStatus __instance, State s, out HarmonyRef __state)
    {
        var artifact = s.EnumerateAllArtifacts().OfType<ArtifactRawstBerry>().FirstOrDefault();

        // If we have the artifact, populate our __state with meaningful values
        if (artifact != null)
        {
            __state = new HarmonyRef();
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
        var artifact = s.EnumerateAllArtifacts().OfType<ArtifactRawstBerry>().FirstOrDefault();

        if (artifact != null)
        {
            // If the player ship has more heat, and this is the first time heat is gained this turn...
            if (s.ship.Get(Status.heat) > s.ship.heatTrigger && !__state.triggered)
            {
                // First set that we triggered the relic for this turn, to prevent recursion
                artifact.hasTriggeredThisTurn = true;

                //Then, apply this relic's effects
                int heatMod = s.ship.heatTrigger - s.ship.Get(Status.heat) - 1;

                c.Queue([
                    new AStatus
                    {
                        status = Status.heat,
                        statusAmount = heatMod,
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
        if (hasTriggeredThisTurn)
        {
            return ModEntry.Instance.ReshiramCCMod_Character_ArtifactRawstBerry.Sprite;
        }
        else
        {
            return ModEntry.Instance.ReshiramCCMod_Character_ArtifactRawstBerry.Sprite;
        }
    }
}
