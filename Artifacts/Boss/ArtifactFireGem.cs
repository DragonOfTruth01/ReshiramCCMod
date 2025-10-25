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
                pools = [ArtifactPool.Common]
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/boss/fireGem.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "boss", "Fire Gem", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "boss", "Fire Gem", "description"]).Localize
        });
    }

    static int counter = 0;
    static readonly int artifactTriggerAmt = 5;

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
        var ship = GetShip(__instance, s);
        __state = new HarmonyRef();
        __state.oldHeatAmt = ship.Get(Status.heat);
        __state.ct = counter;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(AStatus), "Begin")]
    private static void AStatus_Begin_Postfix(AStatus __instance, State s, Combat c, HarmonyRef __state)
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
        if (__state.ct >= artifactTriggerAmt)
        {
            c.Queue([
                new AEnergy
                {
                    changeAmount = 1,
                }
            ]);

            __state.ct = 0;
        }

        // Finally, propagate the state value to the relic's static value
        counter = __state.ct;
    }

    private static Ship GetShip(AStatus instance, State s)
    {
        return instance.targetPlayer ? s.ship : ((Combat)s.route).otherShip;
    }
}
