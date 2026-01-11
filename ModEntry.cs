using Nickel;
using Nickel.Common;
using HarmonyLib;
using DragonOfTruth01.ReshiramCCMod.Cards;
using DragonOfTruth01.ReshiramCCMod.Artifacts;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonOfTruth01.ReshiramCCMod;

public sealed class ModEntry : SimpleMod
{
    internal static ModEntry Instance { get; private set; } = null!;
    internal IKokoroApi.IV2 KokoroApi { get; }
    internal readonly Harmony Harmony;
    internal ILocalizationProvider<IReadOnlyList<string>> AnyLocalizations { get; }
    internal ILocaleBoundNonNullLocalizationProvider<IReadOnlyList<string>> Localizations { get; }

    internal enum CharacterVariant
    {
        Reshiram,
        ReshiVictini,
        WKyurem,
        WKyuremVictini
    }

    internal CharacterVariant currCharVariant;

    internal ISpriteEntry ReshiramCCMod_Character_CardBackground { get; }
    internal ISpriteEntry ReshiramCCMod_Character_CardFrame { get; }

    internal ISpriteEntry ReshiramCCMod_Character_Victini_CardFrame { get; }
    internal ISpriteEntry ReshiramCCMod_Character_WKyurem_CardFrame { get; }

    // Custom Card Arts
    internal ISpriteEntry ReshiramCCMod_Character_CardIncinerateBG { get; }
    internal ISpriteEntry ReshiramCCMod_Character_CardDragonClawBG { get; }
    internal ISpriteEntry ReshiramCCMod_Character_CardFlameChargeBG { get; }
    internal ISpriteEntry ReshiramCCMod_Character_CardDragonBreathBG { get; }
    internal ISpriteEntry ReshiramCCMod_Character_CardPsychicBG { get; }
    internal ISpriteEntry ReshiramCCMod_Character_CardWillOWispBG { get; }
    internal ISpriteEntry ReshiramCCMod_Character_CardHeatWaveBG { get; }
    internal ISpriteEntry ReshiramCCMod_Character_CardFacadeBG { get; }
    internal ISpriteEntry ReshiramCCMod_Character_CardLightScreenBG { get; }
    internal ISpriteEntry ReshiramCCMod_Character_CardFlamethrowerBG { get; }

    internal ISpriteEntry ReshiramCCMod_Character_CardDragonPulseBG { get; }
    internal ISpriteEntry ReshiramCCMod_Character_CardExtrasensoryBG { get; }
    internal ISpriteEntry ReshiramCCMod_Character_CardSunnyDayBG { get; }
    internal ISpriteEntry ReshiramCCMod_Character_CardHoneClawsBG { get; }
    internal ISpriteEntry ReshiramCCMod_Character_CardSolarBeamBG { get; }
    internal ISpriteEntry ReshiramCCMod_Character_CardFireFangBG { get; }
    internal ISpriteEntry ReshiramCCMod_Character_CardImprisonBG { get; }
    internal ISpriteEntry ReshiramCCMod_Character_CardOverheatBG { get; }

    internal ISpriteEntry ReshiramCCMod_Character_CardFusionFlareBG { get; }
    internal ISpriteEntry ReshiramCCMod_Character_CardBlueFlareBG { get; }
    internal ISpriteEntry ReshiramCCMod_Character_CardDracoMeteorBG { get; }
    internal ISpriteEntry ReshiramCCMod_Character_CardRoostBG { get; }
    internal ISpriteEntry ReshiramCCMod_Character_CardOutrageBG { get; }
    internal ISpriteEntry ReshiramCCMod_Character_CardSafeguardBG { get; }

    internal ISpriteEntry ReshiramCCMod_Character_Victini_CardVCreateBG { get; }
    internal ISpriteEntry ReshiramCCMod_Character_Victini_CardSearingShotBG { get; }

    internal ISpriteEntry ReshiramCCMod_Character_WKyurem_CardGlaciateBG { get; }
    internal ISpriteEntry ReshiramCCMod_Character_WKyurem_CardIceNeedleBG { get; }
    internal ISpriteEntry ReshiramCCMod_Character_WKyurem_CardIceBurnBG { get; }

    // Artifact Arts
    internal ISpriteEntry ReshiramCCMod_Character_ArtifactHeatRock { get; }
    internal ISpriteEntry ReshiramCCMod_Character_ArtifactFireGem { get; }
    internal ISpriteEntry ReshiramCCMod_Character_ArtifactFireGem_Disabled { get; }
    internal ISpriteEntry ReshiramCCMod_Character_ArtifactCharcoal { get; }
    internal ISpriteEntry ReshiramCCMod_Character_ArtifactRawstBerry { get; }
    internal ISpriteEntry ReshiramCCMod_Character_ArtifactRawstBerry_Disabled { get; }
    internal ISpriteEntry ReshiramCCMod_Character_ArtifactLibertyPass { get; }

    internal ISpriteEntry ReshiramCCMod_Character_ArtifactFlameOrb { get; }
    internal ISpriteEntry ReshiramCCMod_Character_ArtifactDnaSplicers { get; }

    internal ISpriteEntry ReshiramCCMod_Character_Panel { get; }

    internal ISpriteEntry ReshiramCCMod_Character_Neutral_0 { get; }
    internal ISpriteEntry ReshiramCCMod_Character_Neutral_1 { get; }
    internal ISpriteEntry ReshiramCCMod_Character_Neutral_2 { get; }

    internal ISpriteEntry ReshiramCCMod_Character_Victini_Neutral_0 { get; }
    internal ISpriteEntry ReshiramCCMod_Character_Victini_Neutral_1 { get; }
    internal ISpriteEntry ReshiramCCMod_Character_Victini_Neutral_2 { get; }

    internal ISpriteEntry ReshiramCCMod_Character_WKyurem_Neutral_0 { get; }
    internal ISpriteEntry ReshiramCCMod_Character_WKyurem_Neutral_1 { get; }
    internal ISpriteEntry ReshiramCCMod_Character_WKyurem_Neutral_2 { get; }

    internal ISpriteEntry ReshiramCCMod_Character_WKyurem_Victini_Neutral_0 { get; }
    internal ISpriteEntry ReshiramCCMod_Character_WKyurem_Victini_Neutral_1 { get; }
    internal ISpriteEntry ReshiramCCMod_Character_WKyurem_Victini_Neutral_2 { get; }

    internal ISpriteEntry ReshiramCCMod_Character_Mini_0 { get; }

    internal ISpriteEntry ReshiramCCMod_Character_Squint_0 { get; }
    internal ISpriteEntry ReshiramCCMod_Character_Squint_1 { get; }
    internal ISpriteEntry ReshiramCCMod_Character_Squint_2 { get; }

    internal ISpriteEntry ReshiramCCMod_Character_Victini_Squint_0 { get; }
    internal ISpriteEntry ReshiramCCMod_Character_Victini_Squint_1 { get; }
    internal ISpriteEntry ReshiramCCMod_Character_Victini_Squint_2 { get; }

    internal ISpriteEntry ReshiramCCMod_Character_WKyurem_Squint_0 { get; }
    internal ISpriteEntry ReshiramCCMod_Character_WKyurem_Squint_1 { get; }
    internal ISpriteEntry ReshiramCCMod_Character_WKyurem_Squint_2 { get; }

    internal ISpriteEntry ReshiramCCMod_Character_WKyurem_Victini_Squint_0 { get; }
    internal ISpriteEntry ReshiramCCMod_Character_WKyurem_Victini_Squint_1 { get; }
    internal ISpriteEntry ReshiramCCMod_Character_WKyurem_Victini_Squint_2 { get; }

    internal ISpriteEntry ReshiramCCMod_Icon_EnemyOverheat { get; }
    internal ISpriteEntry ReshiramCCMod_Icon_EnemyNotOverheat { get; }
    internal ISpriteEntry ReshiramCCMod_Icon_SolarFlareActive { get; }
    internal ISpriteEntry ReshiramCCMod_Icon_NonAttackStun { get; }
    internal ISpriteEntry ReshiramCCMod_Icon_ExhaustRight { get; }
    internal ISpriteEntry ReshiramCCMod_Icon_ExhaustLeftRight { get; }
    internal ISpriteEntry ReshiramCCMod_Icon_ChooseExhaust { get; }

    internal ISpriteEntry ReshiramCCMod_Icon_Smoldering { get; }
    internal ISpriteEntry ReshiramCCMod_Icon_Flammable { get; }
    internal ISpriteEntry ReshiramCCMod_Icon_Safeguard { get; }
    internal ISpriteEntry ReshiramCCMod_Icon_HeatResist { get; }
    internal ISpriteEntry ReshiramCCMod_Icon_Frozen { get; }
    internal ISpriteEntry ReshiramCCMod_Icon_Thermosensitive { get; }
    internal ISpriteEntry ReshiramCCMod_Icon_ExhaustedEnergy { get; }
    internal ISpriteEntry ReshiramCCMod_Icon_One { get; }
    internal ISpriteEntry ReshiramCCMod_Icon_Two { get; }

    // Midrow Objects

    internal ISpriteEntry ReshiramCCMod_Midrow_Flamethrower { get; }
    internal ISpriteEntry ReshiramCCMod_Icon_FlamethrowerSmall { get; }

    internal IDeckEntry ReshiramCCMod_Deck { get; }

    internal IDeckEntry ReshiramCCMod_Victini_Deck { get; }
    internal IDeckEntry ReshiramCCMod_WKyurem_Deck { get; }

    internal IStatusEntry Smoldering { get; }
    internal IStatusEntry Flammable { get; }
    internal IStatusEntry Safeguard { get; }
    internal IStatusEntry HeatResist { get; }
    internal IStatusEntry Frozen { get; }
    internal IStatusEntry Thermosensitive { get; }
    internal IStatusEntry ExhaustEnergy { get; }
    internal IStatusEntry One { get; }
    internal IStatusEntry Two { get; }

    // Define our lists of cards
    internal static IReadOnlyList<Type> ReshiramCCModCharacter_CommonCard_Types { get; } = [
        typeof(CardIncinerate),
        typeof(CardDragonClaw),
        typeof(CardFlameCharge),
        typeof(CardDragonBreath),
        typeof(CardPsychic),
        typeof(CardWillOWisp),
        typeof(CardHeatWave),
        typeof(CardFacade),
        typeof(CardLightScreen),
        typeof(CardFlamethrower)
    ];

    internal static IReadOnlyList<Type> ReshiramCCModCharacter_UncommonCard_Types { get; } = [
        typeof(CardDragonPulse),
        typeof(CardExtrasensory),
        typeof(CardSunnyDay),
        typeof(CardHoneClaws),
        typeof(CardSolarBeam),
        typeof(CardFireFang),
        typeof(CardImprison),
        typeof(CardOverheat)
    ];

    internal static IReadOnlyList<Type> ReshiramCCModCharacter_RareCard_Types { get; } = [
        typeof(CardFusionFlare),
        typeof(CardBlueFlare),
        typeof(CardDracoMeteor),
        typeof(CardRoost),
        typeof(CardOutrage),
        typeof(CardSafeguard)
    ];

    internal static IReadOnlyList<Type> ReshiramCCModCharacter_VictiniCard_Types { get; } = [
        typeof(CardSearingShot),
        typeof(CardVCreate)
    ];

    internal static IReadOnlyList<Type> ReshiramCCModCharacter_WKyuremCard_Types { get; } = [
        typeof(CardGlaciate),
        typeof(CardIceBurn),
        typeof(CardIceNeedle)
    ];

    internal static IReadOnlyList<Type> ReshiramCCModCharacter_ExeCard_Types { get; } = [
        typeof(CardReshiramEXE)
    ];

    // Combine all the lists into a single object for reference
    internal static IEnumerable<Type> ReshiramCCMod_AllCard_Types = [
        .. ReshiramCCModCharacter_CommonCard_Types,
        .. ReshiramCCModCharacter_UncommonCard_Types,
        .. ReshiramCCModCharacter_RareCard_Types,
        .. ReshiramCCModCharacter_VictiniCard_Types,
        .. ReshiramCCModCharacter_WKyuremCard_Types,
        .. ReshiramCCModCharacter_ExeCard_Types
    ];

    // Define our artifact lists
    internal static IReadOnlyList<Type> ReshiramCCMod_CommonArtifact_Types { get; } = [
        typeof(ArtifactHeatRock),
        typeof(ArtifactFlameOrb),
        typeof(ArtifactCharcoal),
        typeof(ArtifactRawstBerry),
        typeof(ArtifactLibertyPass)
    ];

    internal static IReadOnlyList<Type> ReshiramCCMod_BossArtifact_Types { get; } = [
        typeof(ArtifactFireGem),
        typeof(ArtifactDnaSplicers)
    ];

    // Combine all the artifacts into a single list
    internal static IEnumerable<Type> ReshiramCCMod_AllArtifact_Types = [
        .. ReshiramCCMod_CommonArtifact_Types,
        .. ReshiramCCMod_BossArtifact_Types
    ];


    public ModEntry(IPluginPackage<IModManifest> package, IModHelper helper, ILogger logger) : base(package, helper, logger)
    {
        Instance = this;

        KokoroApi = helper.ModRegistry.GetApi<IKokoroApi>("Shockah.Kokoro")!.V2;

        Harmony = new Harmony("DragonOfTruth01.ReshiramCCMod");

        // This can be done in place of all Instance.Harmony.Patch() calls in class constructors.
        // However, this will case your IDE/text editor to think the function is unused (since 
        // the patch hasn't been given visibility via constructor). This is expected behavior.
        Harmony.PatchAll();

        // Setup localization support
        AnyLocalizations = new JsonLocalizationProvider(
            tokenExtractor: new SimpleLocalizationTokenExtractor(),
            localeStreamFunction: locale => package.PackageRoot.GetRelativeFile($"i18n/{locale}.json").OpenRead()
        );
        Localizations = new MissingPlaceholderLocalizationProvider<IReadOnlyList<string>>(
            new CurrentLocaleOrEnglishLocalizationProvider<IReadOnlyList<string>>(AnyLocalizations)
        );

        // When loading the mod, default to the regular Reshiram variant
        // This gets updated each frame depending on which relics you have
        currCharVariant = CharacterVariant.Reshiram;

        ReshiramCCMod_Character_CardBackground = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/CardBGs/ReshiramCCMod_character_cardbackground.png"));
        ReshiramCCMod_Character_CardFrame = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/ReshiramCCMod_character_cardframe.png"));

        ReshiramCCMod_Character_Victini_CardFrame = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/ReshiramCCMod_character_victini_cardframe.png"));
        ReshiramCCMod_Character_WKyurem_CardFrame = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/ReshiramCCMod_character_wkyurem_cardframe.png"));

        // Custom Card Arts
        ReshiramCCMod_Character_CardIncinerateBG = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/CardBGs/common/ReshiramCCMod_CardIncinerateBG.png"));
        ReshiramCCMod_Character_CardDragonClawBG = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/CardBGs/common/ReshiramCCMod_CardDragonClawBG.png"));
        ReshiramCCMod_Character_CardFlameChargeBG = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/CardBGs/common/ReshiramCCMod_CardFlameChargeBG.png"));
        ReshiramCCMod_Character_CardDragonBreathBG = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/CardBGs/common/ReshiramCCMod_CardDragonBreathBG.png"));
        ReshiramCCMod_Character_CardPsychicBG = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/CardBGs/common/ReshiramCCMod_CardPsychicBG.png"));
        ReshiramCCMod_Character_CardWillOWispBG = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/CardBGs/common/ReshiramCCMod_CardWillOWispBG.png"));
        ReshiramCCMod_Character_CardHeatWaveBG = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/CardBGs/common/ReshiramCCMod_CardHeatWaveBG.png"));
        ReshiramCCMod_Character_CardFacadeBG = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/CardBGs/common/ReshiramCCMod_CardFacadeBG.png"));
        ReshiramCCMod_Character_CardLightScreenBG = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/CardBGs/common/ReshiramCCMod_CardLightScreenBG.png"));
        ReshiramCCMod_Character_CardFlamethrowerBG = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/CardBGs/common/ReshiramCCMod_CardFlamethrowerBG.png"));

        ReshiramCCMod_Character_CardDragonPulseBG = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/CardBGs/uncommon/ReshiramCCMod_CardDragonPulseBG.png"));
        ReshiramCCMod_Character_CardExtrasensoryBG = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/CardBGs/uncommon/ReshiramCCMod_CardExtrasensoryBG.png"));
        ReshiramCCMod_Character_CardSunnyDayBG = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/CardBGs/uncommon/ReshiramCCMod_CardSunnyDayBG.png"));
        ReshiramCCMod_Character_CardHoneClawsBG = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/CardBGs/uncommon/ReshiramCCMod_CardHoneClawsBG.png"));
        ReshiramCCMod_Character_CardSolarBeamBG = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/CardBGs/uncommon/ReshiramCCMod_CardSolarBeamBG.png"));
        ReshiramCCMod_Character_CardFireFangBG = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/CardBGs/uncommon/ReshiramCCMod_CardFireFangBG.png"));
        ReshiramCCMod_Character_CardImprisonBG = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/CardBGs/uncommon/ReshiramCCMod_CardImprisonBG.png"));
        ReshiramCCMod_Character_CardOverheatBG = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/CardBGs/uncommon/ReshiramCCMod_CardOverheatBG.png"));

        ReshiramCCMod_Character_CardFusionFlareBG = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/CardBGs/rare/ReshiramCCMod_CardFusionFlareBG.png"));
        ReshiramCCMod_Character_CardBlueFlareBG = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/CardBGs/rare/ReshiramCCMod_CardBlueFlareBG.png"));
        ReshiramCCMod_Character_CardDracoMeteorBG = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/CardBGs/rare/ReshiramCCMod_CardDracoMeteorBG.png"));
        ReshiramCCMod_Character_CardRoostBG = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/CardBGs/rare/ReshiramCCMod_CardRoostBG.png"));
        ReshiramCCMod_Character_CardOutrageBG = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/CardBGs/rare/ReshiramCCMod_CardOutrageBG.png"));
        ReshiramCCMod_Character_CardSafeguardBG = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/CardBGs/rare/ReshiramCCMod_CardSafeguardBG.png"));

        ReshiramCCMod_Character_Victini_CardVCreateBG = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/CardBGs/victini/ReshiramCCMod_CardVCreateBG.png"));
        ReshiramCCMod_Character_Victini_CardSearingShotBG = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/CardBGs/victini/ReshiramCCMod_CardSearingShotBG.png"));

        ReshiramCCMod_Character_WKyurem_CardGlaciateBG = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/CardBGs/wkyurem/ReshiramCCMod_CardGlaciateBG.png"));
        ReshiramCCMod_Character_WKyurem_CardIceNeedleBG = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/CardBGs/wkyurem/ReshiramCCMod_CardIceNeedleBG.png"));
        ReshiramCCMod_Character_WKyurem_CardIceBurnBG = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/CardBGs/wkyurem/ReshiramCCMod_CardIceBurnBG.png"));

        // Artifact Arts
        ReshiramCCMod_Character_ArtifactHeatRock = helper.Content.Sprites.RegisterSprite(Package.PackageRoot.GetRelativeFile("assets/artifacts/common/heatRock.png"));
        ReshiramCCMod_Character_ArtifactFireGem = helper.Content.Sprites.RegisterSprite(Package.PackageRoot.GetRelativeFile("assets/artifacts/common/fireGem.png"));
        ReshiramCCMod_Character_ArtifactFireGem_Disabled = helper.Content.Sprites.RegisterSprite(Package.PackageRoot.GetRelativeFile("assets/artifacts/common/fireGem_disabled.png"));
        ReshiramCCMod_Character_ArtifactCharcoal = helper.Content.Sprites.RegisterSprite(Package.PackageRoot.GetRelativeFile("assets/artifacts/common/charcoal.png"));
        ReshiramCCMod_Character_ArtifactRawstBerry = helper.Content.Sprites.RegisterSprite(Package.PackageRoot.GetRelativeFile("assets/artifacts/common/rawstBerry.png"));
        ReshiramCCMod_Character_ArtifactRawstBerry_Disabled = helper.Content.Sprites.RegisterSprite(Package.PackageRoot.GetRelativeFile("assets/artifacts/common/rawstBerry_disabled.png"));
        ReshiramCCMod_Character_ArtifactLibertyPass = helper.Content.Sprites.RegisterSprite(Package.PackageRoot.GetRelativeFile("assets/artifacts/common/libertyPass.png"));

        ReshiramCCMod_Character_ArtifactFlameOrb = helper.Content.Sprites.RegisterSprite(Package.PackageRoot.GetRelativeFile("assets/artifacts/boss/flameOrb.png"));
        ReshiramCCMod_Character_ArtifactDnaSplicers = helper.Content.Sprites.RegisterSprite(Package.PackageRoot.GetRelativeFile("assets/artifacts/boss/dnaSplicers.png"));

        // Midrow Objects

        ReshiramCCMod_Midrow_Flamethrower = helper.Content.Sprites.RegisterSprite(Package.PackageRoot.GetRelativeFile("assets/midrow/flamethrower.png"));
        ReshiramCCMod_Icon_FlamethrowerSmall = helper.Content.Sprites.RegisterSprite(Package.PackageRoot.GetRelativeFile("assets/icons/flamethrowerSmall.png"));

        ReshiramCCMod_Character_Panel = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/ReshiramCCMod_character_panel.png"));

        ReshiramCCMod_Character_Neutral_0 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/reshi/ReshiramCCMod_character_reshi_neutral_0.png"));
        ReshiramCCMod_Character_Neutral_1 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/reshi/ReshiramCCMod_character_reshi_neutral_1.png"));
        ReshiramCCMod_Character_Neutral_2 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/reshi/ReshiramCCMod_character_reshi_neutral_2.png"));

        ReshiramCCMod_Character_Victini_Neutral_0 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/victini/ReshiramCCMod_character_victini_neutral_0.png"));
        ReshiramCCMod_Character_Victini_Neutral_1 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/victini/ReshiramCCMod_character_victini_neutral_1.png"));
        ReshiramCCMod_Character_Victini_Neutral_2 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/victini/ReshiramCCMod_character_victini_neutral_2.png"));

        ReshiramCCMod_Character_WKyurem_Neutral_0 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/wkyurem/ReshiramCCMod_character_wkyurem_neutral_0.png"));
        ReshiramCCMod_Character_WKyurem_Neutral_1 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/wkyurem/ReshiramCCMod_character_wkyurem_neutral_1.png"));
        ReshiramCCMod_Character_WKyurem_Neutral_2 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/wkyurem/ReshiramCCMod_character_wkyurem_neutral_2.png"));
    
        ReshiramCCMod_Character_WKyurem_Victini_Neutral_0 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/wkyurem_victini/ReshiramCCMod_character_wkyurem_victini_neutral_0.png"));
        ReshiramCCMod_Character_WKyurem_Victini_Neutral_1 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/wkyurem_victini/ReshiramCCMod_character_wkyurem_victini_neutral_1.png"));
        ReshiramCCMod_Character_WKyurem_Victini_Neutral_2 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/wkyurem_victini/ReshiramCCMod_character_wkyurem_victini_neutral_2.png"));

        ReshiramCCMod_Character_Mini_0 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/reshi/ReshiramCCMod_character_reshi_mini_0.png"));

        ReshiramCCMod_Character_Squint_0 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/reshi/ReshiramCCMod_character_reshi_squint_0.png"));
        ReshiramCCMod_Character_Squint_1 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/reshi/ReshiramCCMod_character_reshi_squint_1.png"));
        ReshiramCCMod_Character_Squint_2 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/reshi/ReshiramCCMod_character_reshi_squint_2.png"));

        ReshiramCCMod_Character_Victini_Squint_0 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/victini/ReshiramCCMod_character_victini_squint_0.png"));
        ReshiramCCMod_Character_Victini_Squint_1 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/victini/ReshiramCCMod_character_victini_squint_1.png"));
        ReshiramCCMod_Character_Victini_Squint_2 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/victini/ReshiramCCMod_character_victini_squint_2.png"));

        ReshiramCCMod_Character_WKyurem_Squint_0 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/wkyurem/ReshiramCCMod_character_wkyurem_squint_0.png"));
        ReshiramCCMod_Character_WKyurem_Squint_1 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/wkyurem/ReshiramCCMod_character_wkyurem_squint_1.png"));
        ReshiramCCMod_Character_WKyurem_Squint_2 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/wkyurem/ReshiramCCMod_character_wkyurem_squint_2.png"));

        ReshiramCCMod_Character_WKyurem_Victini_Squint_0 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/wkyurem_victini/ReshiramCCMod_character_wkyurem_victini_squint_0.png"));
        ReshiramCCMod_Character_WKyurem_Victini_Squint_1 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/wkyurem_victini/ReshiramCCMod_character_wkyurem_victini_squint_1.png"));
        ReshiramCCMod_Character_WKyurem_Victini_Squint_2 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/wkyurem_victini/ReshiramCCMod_character_wkyurem_victini_squint_2.png"));

        ReshiramCCMod_Icon_EnemyOverheat = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/enemyOverheat.png"));
        ReshiramCCMod_Icon_EnemyNotOverheat = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/enemyNotOverheat.png"));
        ReshiramCCMod_Icon_SolarFlareActive = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/solarFlareActive.png"));
        ReshiramCCMod_Icon_NonAttackStun = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/nonAttackStun.png"));
        ReshiramCCMod_Icon_ExhaustRight = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/exhaustRight.png"));
        ReshiramCCMod_Icon_ExhaustLeftRight = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/exhaustLeftRight.png"));
        ReshiramCCMod_Icon_ChooseExhaust = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/chooseExhaust.png"));

        ReshiramCCMod_Icon_Smoldering = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/smoldering.png"));
        ReshiramCCMod_Icon_Flammable = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/flammable.png"));
        ReshiramCCMod_Icon_Safeguard = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/safeguard.png"));
        ReshiramCCMod_Icon_HeatResist = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/heatResist.png"));
        ReshiramCCMod_Icon_Frozen = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/frozen.png"));
        ReshiramCCMod_Icon_Thermosensitive = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/thermosensitive.png"));
        ReshiramCCMod_Icon_ExhaustedEnergy = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/exhaustedEnergy.png"));
        ReshiramCCMod_Icon_One = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/one.png"));
        ReshiramCCMod_Icon_Two = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/two.png"));

        // Register the mod character's deck
        ReshiramCCMod_Deck = helper.Content.Decks.RegisterDeck("ReshiramCCModDeck", new DeckConfiguration()
        {
            Definition = new DeckDef()
            {
                color = new Color("f4f7f0"),
                titleColor = new Color("000000")
            },

            DefaultCardArt = ReshiramCCMod_Character_CardBackground.Sprite,
            BorderSprite = ReshiramCCMod_Character_CardFrame.Sprite,

            Name = AnyLocalizations.Bind(["character", "ReshiramCCMod", "name"]).Localize,
        });

        // Register the alt starters for the MoreDifficulties mod
        helper.ModRegistry.AwaitApi<IMoreDifficultiesApi>(
            "TheJazMaster.MoreDifficulties",
            new SemanticVersion(1, 3, 0),
            api => api.RegisterAltStarters(
                deck: ReshiramCCMod_Deck.Deck,
                starterDeck: new StarterDeck
                {
                    cards = [
                        new CardFlameCharge(),
                        new CardLightScreen(),
                    ]
                }

            )
        );  

        // Register NPC Decks
        ReshiramCCMod_Victini_Deck = helper.Content.Decks.RegisterDeck("ReshiramCCModVictiniDeck", new DeckConfiguration()
        {
            Definition = new DeckDef()
            {
                color = new Color("de543d"),

                titleColor = new Color("000000")
            },

            DefaultCardArt = ReshiramCCMod_Character_CardBackground.Sprite,
            BorderSprite = ReshiramCCMod_Character_Victini_CardFrame.Sprite,

            Name = AnyLocalizations.Bind(["character", "ReshiramCCMod_Victini", "name"]).Localize,
        });

        ReshiramCCMod_WKyurem_Deck = helper.Content.Decks.RegisterDeck("ReshiramCCModWKyuremDeck", new DeckConfiguration()
        {
            Definition = new DeckDef()
            {
                color = new Color("86cece"),

                titleColor = new Color("000000")
            },

            DefaultCardArt = ReshiramCCMod_Character_CardBackground.Sprite,
            BorderSprite = ReshiramCCMod_Character_WKyurem_CardFrame.Sprite,

            Name = AnyLocalizations.Bind(["character", "ReshiramCCMod_WKyurem", "name"]).Localize,
        });

        // Register character animations
        helper.Content.Characters.V2.RegisterCharacterAnimation(new CharacterAnimationConfigurationV2()
        {
            CharacterType = ReshiramCCMod_Deck.Deck.Key(),

            LoopTag = "neutral",

            Frames = new[]
            {
                ReshiramCCMod_Character_Neutral_0.Sprite,
                ReshiramCCMod_Character_Neutral_1.Sprite,
                ReshiramCCMod_Character_Neutral_2.Sprite,
                ReshiramCCMod_Character_Neutral_0.Sprite,
                ReshiramCCMod_Character_Neutral_1.Sprite,
                ReshiramCCMod_Character_Neutral_2.Sprite
            }
        });

        helper.Content.Characters.V2.RegisterCharacterAnimation(new CharacterAnimationConfigurationV2()
        {
            CharacterType = ReshiramCCMod_Deck.Deck.Key(),

            LoopTag = "neutral_victini",

            Frames = new[]
            {
                ReshiramCCMod_Character_Victini_Neutral_0.Sprite,
                ReshiramCCMod_Character_Victini_Neutral_1.Sprite,
                ReshiramCCMod_Character_Victini_Neutral_2.Sprite,
                ReshiramCCMod_Character_Victini_Neutral_0.Sprite,
                ReshiramCCMod_Character_Victini_Neutral_1.Sprite,
                ReshiramCCMod_Character_Victini_Neutral_2.Sprite
            }
        });

        helper.Content.Characters.V2.RegisterCharacterAnimation(new CharacterAnimationConfigurationV2()
        {
            CharacterType = ReshiramCCMod_Deck.Deck.Key(),

            LoopTag = "neutral_wkyurem",

            Frames = new[]
            {
                ReshiramCCMod_Character_WKyurem_Neutral_0.Sprite,
                ReshiramCCMod_Character_WKyurem_Neutral_1.Sprite,
                ReshiramCCMod_Character_WKyurem_Neutral_2.Sprite,
                ReshiramCCMod_Character_WKyurem_Neutral_0.Sprite,
                ReshiramCCMod_Character_WKyurem_Neutral_1.Sprite,
                ReshiramCCMod_Character_WKyurem_Neutral_2.Sprite
            }
        });

        helper.Content.Characters.V2.RegisterCharacterAnimation(new CharacterAnimationConfigurationV2()
        {
            CharacterType = ReshiramCCMod_Deck.Deck.Key(),

            LoopTag = "neutral_wkyurem_victini",

            Frames = new[]
            {
                ReshiramCCMod_Character_WKyurem_Victini_Neutral_0.Sprite,
                ReshiramCCMod_Character_WKyurem_Victini_Neutral_1.Sprite,
                ReshiramCCMod_Character_WKyurem_Victini_Neutral_2.Sprite,
                ReshiramCCMod_Character_WKyurem_Victini_Neutral_0.Sprite,
                ReshiramCCMod_Character_WKyurem_Victini_Neutral_1.Sprite,
                ReshiramCCMod_Character_WKyurem_Victini_Neutral_2.Sprite
            }
        });

        helper.Content.Characters.V2.RegisterCharacterAnimation(new CharacterAnimationConfigurationV2()
        {
            CharacterType = ReshiramCCMod_Deck.Deck.Key(),
            LoopTag = "mini",
            Frames = new[]
            {
                ReshiramCCMod_Character_Mini_0.Sprite
            }
        });

        helper.Content.Characters.V2.RegisterCharacterAnimation(new CharacterAnimationConfigurationV2()
        {
            CharacterType = ReshiramCCMod_Deck.Deck.Key(),
            LoopTag = "squint",
            Frames = new[]
            {
                ReshiramCCMod_Character_Squint_0.Sprite,
                ReshiramCCMod_Character_Squint_1.Sprite,
                ReshiramCCMod_Character_Squint_2.Sprite,
                ReshiramCCMod_Character_Squint_0.Sprite,
                ReshiramCCMod_Character_Squint_1.Sprite,
                ReshiramCCMod_Character_Squint_2.Sprite
            }
        });

        helper.Content.Characters.V2.RegisterCharacterAnimation(new CharacterAnimationConfigurationV2()
        {
            CharacterType = ReshiramCCMod_Deck.Deck.Key(),
            LoopTag = "squint_victini",
            Frames = new[]
            {
                ReshiramCCMod_Character_Victini_Squint_0.Sprite,
                ReshiramCCMod_Character_Victini_Squint_1.Sprite,
                ReshiramCCMod_Character_Victini_Squint_2.Sprite,
                ReshiramCCMod_Character_Victini_Squint_0.Sprite,
                ReshiramCCMod_Character_Victini_Squint_1.Sprite,
                ReshiramCCMod_Character_Victini_Squint_2.Sprite
            }
        });

        helper.Content.Characters.V2.RegisterCharacterAnimation(new CharacterAnimationConfigurationV2()
        {
            CharacterType = ReshiramCCMod_Deck.Deck.Key(),
            LoopTag = "squint_wkyurem",
            Frames = new[]
            {
                ReshiramCCMod_Character_WKyurem_Squint_0.Sprite,
                ReshiramCCMod_Character_WKyurem_Squint_1.Sprite,
                ReshiramCCMod_Character_WKyurem_Squint_2.Sprite,
                ReshiramCCMod_Character_WKyurem_Squint_0.Sprite,
                ReshiramCCMod_Character_WKyurem_Squint_1.Sprite,
                ReshiramCCMod_Character_WKyurem_Squint_2.Sprite
            }
        });

        helper.Content.Characters.V2.RegisterCharacterAnimation(new CharacterAnimationConfigurationV2()
        {
            CharacterType = ReshiramCCMod_Deck.Deck.Key(),
            LoopTag = "squint_wkyurem_victini",
            Frames = new[]
            {
                ReshiramCCMod_Character_WKyurem_Victini_Squint_0.Sprite,
                ReshiramCCMod_Character_WKyurem_Victini_Squint_1.Sprite,
                ReshiramCCMod_Character_WKyurem_Victini_Squint_2.Sprite,
                ReshiramCCMod_Character_WKyurem_Victini_Squint_0.Sprite,
                ReshiramCCMod_Character_WKyurem_Victini_Squint_1.Sprite,
                ReshiramCCMod_Character_WKyurem_Victini_Squint_2.Sprite
            }
        });

        helper.Content.Characters.V2.RegisterCharacterAnimation(new CharacterAnimationConfigurationV2()
        {
            CharacterType = ReshiramCCMod_Deck.Deck.Key(),
            LoopTag = "gameover",
            Frames = new[]
            {
                // The squint sprite is okay to use here...
                ReshiramCCMod_Character_Squint_0.Sprite,
            }
        });

        helper.Content.Characters.V2.RegisterCharacterAnimation(new CharacterAnimationConfigurationV2()
        {
            CharacterType = ReshiramCCMod_Deck.Deck.Key(),
            LoopTag = "gameover_victini",
            Frames = new[]
            {
                // The squint sprite is okay to use here...
                ReshiramCCMod_Character_Victini_Squint_0.Sprite,
            }
        });

        helper.Content.Characters.V2.RegisterCharacterAnimation(new CharacterAnimationConfigurationV2()
        {
            CharacterType = ReshiramCCMod_Deck.Deck.Key(),
            LoopTag = "gameover_wkyurem",
            Frames = new[]
            {
                // The squint sprite is okay to use here...
                ReshiramCCMod_Character_WKyurem_Squint_0.Sprite,
            }
        });

        helper.Content.Characters.V2.RegisterCharacterAnimation(new CharacterAnimationConfigurationV2()
        {
            CharacterType = ReshiramCCMod_Deck.Deck.Key(),
            LoopTag = "gameover_wkyurem_victini",
            Frames = new[]
            {
                // The squint sprite is okay to use here...
                ReshiramCCMod_Character_WKyurem_Victini_Squint_0.Sprite,
            }
        });
        
        // Register the mod character as a playable character
        helper.Content.Characters.V2.RegisterPlayableCharacter("ReshiramCCMod", new PlayableCharacterConfigurationV2()
        {
            Deck = ReshiramCCMod_Deck.Deck,

            Starters = new()
            {
                cards = [
                    new CardIncinerate(),
                    new CardDragonClaw()
                ]
            },

            Description = AnyLocalizations.Bind(["character", "ReshiramCCMod", "description"]).Localize,

            BorderSprite = ReshiramCCMod_Character_Panel.Sprite
        });

        // Register our cards
        foreach (var cardType in ReshiramCCMod_AllCard_Types)
            AccessTools.DeclaredMethod(cardType, nameof(IReshiramCCModCard.Register))?.Invoke(null, [helper]);

        // Register our artifacts
        foreach (var artifactType in ReshiramCCMod_AllArtifact_Types)
            AccessTools.DeclaredMethod(artifactType, nameof(IReshiramCCModArtifact.Register))?.Invoke(null, [helper]);

        // Register our statuses
        Smoldering = helper.Content.Statuses.RegisterStatus("Smoldering", new()
        {
            Definition = new()
            {
                icon = ReshiramCCMod_Icon_Smoldering.Sprite,
                color = new("8b93af"),
                isGood = false
            },
            Name = AnyLocalizations.Bind(["status", "Smoldering", "name"]).Localize,
            Description = AnyLocalizations.Bind(["status", "Smoldering", "description"]).Localize
        });

        Flammable = helper.Content.Statuses.RegisterStatus("Flammable", new()
        {
            Definition = new()
            {
                icon = ReshiramCCMod_Icon_Flammable.Sprite,
                color = new("285cc4"),
                isGood = false
            },
            Name = AnyLocalizations.Bind(["status", "Flammable", "name"]).Localize,
            Description = AnyLocalizations.Bind(["status", "Flammable", "description"]).Localize
        });

        Safeguard = helper.Content.Statuses.RegisterStatus("Safeguard", new()
        {
            Definition = new()
            {
                icon = ReshiramCCMod_Icon_Safeguard.Sprite,
                color = new("249fde"),
                isGood = true
            },
            Name = AnyLocalizations.Bind(["status", "Safeguard", "name"]).Localize,
            Description = AnyLocalizations.Bind(["status", "Safeguard", "description"]).Localize
        });

        HeatResist = helper.Content.Statuses.RegisterStatus("Heat Resist", new()
        {
            Definition = new()
            {
                icon = ReshiramCCMod_Icon_HeatResist.Sprite,
                color = new("ff687d"),
                isGood = true
            },
            Name = AnyLocalizations.Bind(["status", "Heat Resist", "name"]).Localize,
            Description = AnyLocalizations.Bind(["status", "Heat Resist", "description"]).Localize
        });

        Frozen = helper.Content.Statuses.RegisterStatus("Frozen", new()
        {
            Definition = new()
            {
                icon = ReshiramCCMod_Icon_Frozen.Sprite,
                color = new("4485ab"),
                isGood = false
            },
            Name = AnyLocalizations.Bind(["status", "Frozen", "name"]).Localize,
            Description = AnyLocalizations.Bind(["status", "Frozen", "description"]).Localize
        });

        Thermosensitive = helper.Content.Statuses.RegisterStatus("Thermosensitive", new()
        {
            Definition = new()
            {
                icon = ReshiramCCMod_Icon_Thermosensitive.Sprite,
                color = new("ff687d"),
                isGood = false
            },
            Name = AnyLocalizations.Bind(["status", "Thermosensitive", "name"]).Localize,
            Description = AnyLocalizations.Bind(["status", "Thermosensitive", "description"]).Localize
        });

        // Below are fake statuses only used to abuse equation formatting on cards
        // They don't do anything, but apparently some modded characters apply random statuses
        // So maybe it would be funny if these did something completely unhinged at some point

        ExhaustEnergy = helper.Content.Statuses.RegisterStatus("Exhaust Energy", new()
        {
            Definition = new()
            {
                icon = ReshiramCCMod_Icon_ExhaustedEnergy.Sprite,
                color = new("285cc4"),
                isGood = true
            },
            Name = AnyLocalizations.Bind(["status", "Exhaust Energy", "name"]).Localize,
            Description = AnyLocalizations.Bind(["status", "Exhaust Energy", "description"]).Localize
        });

        One = helper.Content.Statuses.RegisterStatus("One", new()
        {
            Definition = new()
            {
                icon = ReshiramCCMod_Icon_One.Sprite,
                color = new("ff687d"),
                isGood = true
            },
            Name = AnyLocalizations.Bind(["status", "One", "name"]).Localize,
            Description = AnyLocalizations.Bind(["status", "One", "description"]).Localize
        });

        Two = helper.Content.Statuses.RegisterStatus("Two", new()
        {
            Definition = new()
            {
                icon = ReshiramCCMod_Icon_Two.Sprite,
                color = new("ff687d"),
                isGood = true
            },
            Name = AnyLocalizations.Bind(["status", "Two", "name"]).Localize,
            Description = AnyLocalizations.Bind(["status", "Two", "description"]).Localize
        });
        
        _ = new StatusManager();
    }
}
