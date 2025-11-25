using Nickel;
using HarmonyLib;

namespace DragonOfTruth01.ReshiramCCMod;

[HarmonyPatch]
public sealed class AnimationHandler
{
    public static ModEntry Instance => ModEntry.Instance;

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Character), "DrawFace")]
    private static void DrawFace_Prefix(Character __instance, ref string animTag, bool mini = false)
    {
        // Only do the following for the Reshiram character
        if(__instance.type == ModEntry.Instance.ReshiramCCMod_Deck.Deck.Key())
        {
            // Logic goes here
            
            // if(animTag == "neutral")
            // {
            //     animTag = "neutral_victini";
            // }            
        }
    }
}