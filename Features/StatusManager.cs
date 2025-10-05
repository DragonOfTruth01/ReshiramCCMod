namespace DragonOfTruth01.ReshiramCCMod;

using HarmonyLib;
using FSPRO;

[HarmonyPatch]
internal sealed class StatusManager : IStatusLogicHook
{
    public static ModEntry Instance => ModEntry.Instance;

    public StatusManager()
    {
        /* We task Kokoro with the job to register our status into the game */
        Instance.KokoroApi.RegisterStatusLogicHook(this, 0);

        Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(AStatus), nameof(AStatus.Begin)),
            prefix: new HarmonyMethod(GetType(), nameof(AStatus_Begin_Prefix)),
            postfix: new HarmonyMethod(GetType(), nameof(AStatus_Begin_Postfix))
        );
    }

    private class HarmonyRef
    {
        public int oldHeatAmt;
        public int oldFlammableAmt;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(AStatus), "Begin")]
    private static void AStatus_Begin_Prefix(AStatus __instance, State s, out HarmonyRef __state)
    {
        var ship = GetShip(__instance, s);
        __state = new HarmonyRef();
        __state.oldHeatAmt = ship.Get(Status.heat);
        __state.oldFlammableAmt = ship.Get(ModEntry.Instance.Flammable.Status);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(AStatus), "Begin")]
    private static void AStatus_Begin_Postfix(AStatus __instance, State s, Combat c, HarmonyRef __state)
    {
        var ship = GetShip(__instance, s);

        // Handle Smoldering (if ship gained heat, check for smoldering damage)
        var heatDiff = ship.Get(Status.heat) - __state.oldHeatAmt;

        if (ship.Get(ModEntry.Instance.Smoldering.Status) > 0 && ship.Get(Status.heat) >= ship.heatTrigger && heatDiff > 0)
        {
            ship.DirectHullDamage(s, c, ship.Get(ModEntry.Instance.Smoldering.Status));
            Audio.Play(Event.Hits_HitHurt);
        }

        // Handle Flammable (if amount changed, temporarily modify our overheat damage)
        var flammableDiff = ship.Get(ModEntry.Instance.Flammable.Status) - __state.oldFlammableAmt;
        ship.overheatDamage += flammableDiff;
    }

    public bool HandleStatusTurnAutoStep(State state, Combat combat, StatusTurnTriggerTiming timing, Ship ship, Status status, ref int amount, ref StatusTurnAutoStepSetStrategy setStrategy)
    {
        if (timing != StatusTurnTriggerTiming.TurnStart)
            return false;

        // Handle smoldering end-of-turn decrement
        if (status == Instance.Smoldering.Status && amount > 0)
        {
            amount -= 1;
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
    }
}