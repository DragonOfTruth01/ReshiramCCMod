using Nickel;
using HarmonyLib;
using System.Runtime.CompilerServices;
using System;
using System.Linq;
using System.Collections.Generic;
using DragonOfTruth01.ReshiramCCMod.Artifacts;

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
        if (__instance.type == ModEntry.Instance.ReshiramCCMod_Deck.Deck.Key())
        {
            // Change the animation to whatever we'd like to display
            animTag = getTag(animTag);
        }
    }

    private static Dictionary<string, string> defaultAnim = new Dictionary<string, string>();
    private static Dictionary<string, string> victiniAnim = new Dictionary<string, string>();
    private static Dictionary<string, string> wKyuremAnim = new Dictionary<string, string>();
    private static Dictionary<string, string> wKyuremVictiniAnim = new Dictionary<string, string>();

    static AnimationHandler()
    {
        // Populate default animations
        defaultAnim["neutral"]  = "neutral";
        defaultAnim["mini"]     = "mini";
        defaultAnim["squint"]   = "squint";
        defaultAnim["gameover"] = "gameover";

        // Populate victini animations
        victiniAnim["neutral"]  = "neutral_victini";
        victiniAnim["mini"]     = "mini";
        victiniAnim["squint"]   = "squint_victini";
        victiniAnim["gameover"] = "gameover";

        // Populate white kyurem animations
        wKyuremAnim["neutral"]  = "neutral";
        wKyuremAnim["mini"]     = "mini";
        wKyuremAnim["squint"]   = "squint";
        wKyuremAnim["gameover"] = "gameover";

        // Populate white kyurem + victini animations
        wKyuremVictiniAnim["neutral"]  = "neutral";
        wKyuremVictiniAnim["mini"]     = "mini";
        wKyuremVictiniAnim["squint"]   = "squint";
        wKyuremVictiniAnim["gameover"] = "gameover";

    }

    private static string getTag(string originalTag)
    {
        switch (ModEntry.Instance.currCharVariant){
            case ModEntry.CharacterVariant.Reshiram:
                return defaultAnim[originalTag];

            case ModEntry.CharacterVariant.ReshiVictini:
                return victiniAnim[originalTag];

            case ModEntry.CharacterVariant.WKyurem:
                return wKyuremAnim[originalTag];

            case ModEntry.CharacterVariant.WKyuremVictini:
                return wKyuremVictiniAnim[originalTag];

            default:
                return defaultAnim[originalTag];
        }
    }

    public static void UpdateCharacterVariant(State s)
    {
        // Check for artifacts
        var libertyPass = s.EnumerateAllArtifacts().OfType<ArtifactLibertyPass>().FirstOrDefault();
        bool hasLibertyPass = false;

        if (libertyPass != null)
        {
            hasLibertyPass = true;
        }

        // Apply the correct character variant
        if (hasLibertyPass)
        {
            ModEntry.Instance.currCharVariant = ModEntry.CharacterVariant.ReshiVictini;
        }
        else
        {
            ModEntry.Instance.currCharVariant = ModEntry.CharacterVariant.Reshiram;
        }
    }
}