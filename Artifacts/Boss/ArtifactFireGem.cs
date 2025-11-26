using DragonOfTruth01.ReshiramCCMod.Cards;
using Nickel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using System.Runtime.CompilerServices;

namespace DragonOfTruth01.ReshiramCCMod.Artifacts;

[HarmonyPatch]
internal sealed class ArtifactFireGem : Artifact, IReshiramCCModArtifact
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("Fire Gem", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.ReshiramCCMod_Deck.Deck,
                pools = [ArtifactPool.Boss]
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/boss/fireGem.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "boss", "Fire Gem", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "boss", "Fire Gem", "description"]).Localize
        });
    }

    public int counter = 0;
    public readonly int artifactTriggerAmt = 5;

    private class HarmonyRef
    {
        public int oldHeatAmt;
        public int ct;
    }

    public override int? GetDisplayNumber(State s)
    {
        return counter;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(AStatus), "Begin")]
    private static void AStatus_Begin_Prefix(AStatus __instance, State s, out HarmonyRef __state)
    {
        var artifact = s.EnumerateAllArtifacts().OfType<ArtifactFireGem>().FirstOrDefault();

        // If we have the artifact, populate our __state with meaningful values
        if (artifact != null)
        {
            var ship = GetShip(__instance, s);
            __state = new HarmonyRef();
            __state.oldHeatAmt = ship.Get(Status.heat);
            __state.ct = artifact.counter;
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
        var artifact = s.EnumerateAllArtifacts().OfType<ArtifactFireGem>().FirstOrDefault();

        if (artifact != null)
        {
            var ship = GetShip(__instance, s);

            // Check if either ship gained heat
            var heatDiff = ship.Get(Status.heat) - __state.oldHeatAmt;

            // If a ship did gain heat, increment the artifact counter
            if (heatDiff > 0)
            {
                ++__state.ct;
            }

            // If the artifact counter is at the trigger threshold, reset it and trigger the effect
            if (__state.ct >= artifact.artifactTriggerAmt)
            {
                c.Queue([
                    new AEnergy
                    {
                        changeAmount = 1,
                    }
                ]);

                artifact.Pulse();

                __state.ct = 0;
            }

            // Finally, propagate the state value to the relic's static value
            artifact.counter = __state.ct;
        }

        
    }

    private static Ship GetShip(AStatus instance, State s)
    {
        return instance.targetPlayer ? s.ship : ((Combat)s.route).otherShip;
    }

    public override Spr GetSprite()
    {
        return ModEntry.Instance.ReshiramCCMod_Character_ArtifactFireGem.Sprite;
    }
}
