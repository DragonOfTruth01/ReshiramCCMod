namespace DragonOfTruth01.ReshiramCCMod;

using HarmonyLib;
using FSPRO;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Immutable;
using OneOf.Types;

[HarmonyPatch]
internal sealed class StatusManager : IKokoroApi.IV2.IStatusLogicApi.IHook, IKokoroApi.IV2.IStatusRenderingApi.IHook
{
    public static ModEntry Instance => ModEntry.Instance;

    public StatusManager()
    {
        /* We task Kokoro with the job to register our status into the game */
        Instance.KokoroApi.StatusLogic.RegisterHook(this, 0);
        Instance.KokoroApi.StatusRendering.RegisterHook(this, 0);
    }

    private class HarmonyRef
    {
        public int oldHeatAmt;
        public int oldFlammableAmt;
        public int oldHeatResistAmt;
        public int oldFrozenAmt;
    }

    public IKokoroApi.IV2.IStatusRenderingApi.IStatusInfoRenderer? OverrideStatusInfoRenderer(IKokoroApi.IV2.IStatusRenderingApi.IHook.IOverrideStatusInfoRendererArgs args)
	{
        if (args.Status != ModEntry.Instance.Thermosensitive.Status)
            return null;

        return ModEntry.Instance.KokoroApi.StatusRendering.MakeBarStatusInfoRenderer().SetSegments(Array.Empty<Color>()).SetRows(1);
	}

    // Prefix for Frozen Status
    [HarmonyPrefix]
    [HarmonyPatch(typeof(AIHelpers), nameof(AIHelpers.MoveToAimAt))]
    [HarmonyPatch(new Type[] {typeof(State), typeof(Ship), typeof(Ship), typeof(int), typeof(int), typeof(bool), typeof(bool?), typeof(bool), typeof(bool), typeof(bool)})]
    public static bool MoveToAimAt_Prefix(State s, Ship movingShip, ref List<CardAction> __result)
    {
        if ((Combat)s.route == null)
        {
            __result = new List<CardAction>();
            return false;
        }
        
        if (movingShip.Get(ModEntry.Instance.Frozen.Status) > 0)
        {
            Audio.Play(Event.Status_PowerDown);
            movingShip.shake += 1.0;
            __result = new List<CardAction>();
            return false;
        }

        return true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(AStatus), "Begin")]
    private static void AStatus_Begin_Prefix(AStatus __instance, State s, out HarmonyRef __state)
    {
        var ship = GetShip(__instance, s);
        __state = new HarmonyRef();
        __state.oldHeatAmt = ship.Get(Status.heat);
        __state.oldFlammableAmt = ship.Get(ModEntry.Instance.Flammable.Status);
        __state.oldHeatResistAmt = ship.Get(ModEntry.Instance.HeatResist.Status);
        __state.oldFrozenAmt = ship.Get(ModEntry.Instance.Frozen.Status);

        // Set heat gain to 0 if the ship has safeguard
        if( __instance.status == Status.heat && __instance.statusAmount > 0
            && ship.Get(ModEntry.Instance.Safeguard.Status) > 0)
        {
            __instance.statusAmount = 0;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(AStatus), "Begin")]
    private static void AStatus_Begin_Postfix(AStatus __instance, State s, Combat c, HarmonyRef __state)
    {
        var ship = GetShip(__instance, s);

        // Handle Smoldering (if ship gained heat, check for smoldering damage)
        var heatDiff = ship.Get(Status.heat) - __state.oldHeatAmt;

        // Check if the amount of frozen increased
        var frozenDiff = ship.Get(ModEntry.Instance.Frozen.Status) - __state.oldFrozenAmt;

        if (ship.Get(ModEntry.Instance.Smoldering.Status) > 0 && ship.Get(Status.heat) >= ship.heatTrigger && heatDiff > 0)
        {
            ship.DirectHullDamage(s, c, ship.Get(ModEntry.Instance.Smoldering.Status));
            Audio.Play(Event.Hits_HitHurt);
        }

        // Handle Flammable (if amount changed, temporarily modify our overheat damage)
        var flammableDiff = ship.Get(ModEntry.Instance.Flammable.Status) - __state.oldFlammableAmt;
        ship.overheatDamage += flammableDiff;

        // Handle Heat Resist (if amount changed, temporarily modify our overheat threshold)
        var heatResistDiff = ship.Get(ModEntry.Instance.HeatResist.Status) - __state.oldHeatResistAmt;
        ship.heatTrigger += heatResistDiff;

        // Stun random ship part if heat or frozen was gained, while thermosensitive is active
        if( ship.Get(ModEntry.Instance.Thermosensitive.Status) > 0
            && ( heatDiff > 0 || frozenDiff > 0))
        {
            // Record each ship part
            List<int> list = new List<int>();
            int num = 0;

            foreach (Part part in c.otherShip.parts)
            {
                if (part != null)
                {
                    list.Add(num);
                }
                ++num;
            }

            // Stun a ship part at random
            int stunIndex = 0;
            if (list.Count > 0)
            {
                if(list.Count == 1)
                {
                    // If there is only one ship part, stun it
                    stunIndex = list[0];
                }
                else
                {
                    // Otherwise, roll RNG to determine the part to stun
                    int randIndex = s.rngActions.NextInt() % list.Count;
                    stunIndex = list[randIndex];
                }

                c.QueueImmediate(new AStunPart
                {
                    worldX = c.otherShip.x + stunIndex
                });
            }
        }
    }

    // Prevent player ship from moving if frozen
    [HarmonyPrefix]
    [HarmonyPatch(typeof(AMove), "Begin")]
    public static bool AMove_Begin_Prefix(AMove __instance, G g, State s, Combat c)
    {
        bool prefixFlag = FeatureFlags.Debug && Input.shift;

        if(__instance.targetPlayer && !prefixFlag && s.ship.Get(ModEntry.Instance.Frozen.Status) > 0 && __instance.fromEvade)
        {
            Audio.Play(Event.Status_PowerDown);
            s.ship.shake += 1.0;
            return false;
        }

        return true;
    }

    public bool HandleStatusTurnAutoStep(IKokoroApi.IV2.IStatusLogicApi.IHook.IHandleStatusTurnAutoStepArgs args)
    {
        // Handle start-of-turn decrements for safeguard and frozen
        if ( args.Timing != IKokoroApi.IV2.IStatusLogicApi.StatusTurnTriggerTiming.TurnStart
             && (args.Status == Instance.Safeguard.Status || args.Status == Instance.Frozen.Status)
             && args.Amount > 0)
        {
            args.Amount -= 1;
            return false;
        }

        // Handle end-of-turn decrements for smoldering
        if ( args.Timing != IKokoroApi.IV2.IStatusLogicApi.StatusTurnTriggerTiming.TurnEnd
             && args.Status == Instance.Smoldering.Status
             && args.Amount > 0)
        {
            args.Amount -= 1;
            return false;
        }

        return false;
    }

    private static Ship GetShip(AStatus instance, State s)
    {
        return instance.targetPlayer ? s.ship : ((Combat)s.route).otherShip;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Ship), "ResetAfterCombat")]
    private static void Ship_ResetAfterCombat_Prefix(Ship __instance)
    {
        __instance.overheatDamage -= __instance.Get(ModEntry.Instance.Flammable.Status);
        __instance.heatTrigger -= __instance.Get(ModEntry.Instance.HeatResist.Status);
    }
}