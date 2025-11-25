using Nickel;
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

    // Artifact Arts
    internal ISpriteEntry ReshiramCCMod_Character_ArtifactHeatRock { get; }
    internal ISpriteEntry ReshiramCCMod_Character_ArtifactFlameOrb { get; }
    internal ISpriteEntry ReshiramCCMod_Character_ArtifactFlameOrb_Disabled { get; }
    internal ISpriteEntry ReshiramCCMod_Character_ArtifactCharcoal { get; }
    internal ISpriteEntry ReshiramCCMod_Character_ArtifactRawstBerry { get; }
    internal ISpriteEntry ReshiramCCMod_Character_ArtifactRawstBerry_Disabled { get; }

    internal ISpriteEntry ReshiramCCMod_Character_ArtifactFireGem { get; }

    internal ISpriteEntry ReshiramCCMod_Character_Panel { get; }

    internal ISpriteEntry ReshiramCCMod_Character_Neutral_0 { get; }
    internal ISpriteEntry ReshiramCCMod_Character_Neutral_1 { get; }
    internal ISpriteEntry ReshiramCCMod_Character_Neutral_2 { get; }

    internal ISpriteEntry ReshiramCCMod_Character_Victini_Neutral_0 { get; }
    internal ISpriteEntry ReshiramCCMod_Character_Victini_Neutral_1 { get; }
    internal ISpriteEntry ReshiramCCMod_Character_Victini_Neutral_2 { get; }

    internal ISpriteEntry ReshiramCCMod_Character_Mini_0 { get; }

    internal ISpriteEntry ReshiramCCMod_Character_Squint_0 { get; }
    internal ISpriteEntry ReshiramCCMod_Character_Squint_1 { get; }
    internal ISpriteEntry ReshiramCCMod_Character_Squint_2 { get; }

    internal ISpriteEntry ReshiramCCMod_Character_Victini_Squint_0 { get; }
    internal ISpriteEntry ReshiramCCMod_Character_Victini_Squint_1 { get; }
    internal ISpriteEntry ReshiramCCMod_Character_Victini_Squint_2 { get; }

    internal ISpriteEntry ReshiramCCMod_Icon_EnemyOverheat { get; }
    internal ISpriteEntry ReshiramCCMod_Icon_EnemyNotOverheat { get; }
    internal ISpriteEntry ReshiramCCMod_Icon_SolarFlareActive { get; }
    internal ISpriteEntry ReshiramCCMod_Icon_NonAttackStun { get; }
    internal ISpriteEntry ReshiramCCMod_Icon_HeatResist { get; }

    internal IDeckEntry ReshiramCCMod_Deck { get; }

    internal IStatusEntry Smoldering { get; }
    internal IStatusEntry Flammable { get; }
    internal IStatusEntry Safeguard { get; }
    internal IStatusEntry HeatResist { get; }

    // NOTE TO SELF ABOUT CARDS:
    // It would be cool to have them follow these rules:
    // Fire cards (cards reated to heat) should have red backgrounds (and are usually associated with the smoldering status)
    // Dragon cards should have purple backgrounds (and are usually associated with the flammable status)
    // Ice cards (kyurem cards) can have light blue backgrounds (and are associated with engine lock)
    // Other cards could be associated with colors related to the type of the move from pokemon

    /* You can create many IReadOnlyList<Type> as a way to organize your content.
     * We recommend having a Starter Cards list, a Common Cards list, an Uncommon Cards list, and a Rare Cards list
     * However you can be more detailed, or you can be more loose, if that's your style */
    internal static IReadOnlyList<Type> ReshiramCCModCharacter_CommonCard_Types { get; } = [
        typeof(CardIncinerate),
        typeof(CardDragonClaw),
        typeof(CardFlameCharge),
        typeof(CardDragonBreath),
        typeof(CardPsychic),
        typeof(CardWillOWisp),
        typeof(CardHeatWave),
        typeof(CardFacade),
        typeof(CardLightScreen)
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

    /* We can use an IEnumerable to combine the lists we made above, and modify it if needed
     * Maybe you created a new list for Uncommon cards, and want to add it.
     * If so, you can .Concat(TheUncommonListYouMade) */
    internal static IEnumerable<Type> ReshiramCCMod_AllCard_Types
        = [
            .. ReshiramCCModCharacter_CommonCard_Types,
            .. ReshiramCCModCharacter_UncommonCard_Types,
            .. ReshiramCCModCharacter_RareCard_Types
        //     typeof(Whatever_Other_Cards_To_Add)
        ];

    /* We'll organize our artifacts the same way: making lists and then feed those to an IEnumerable */
    internal static IReadOnlyList<Type> ReshiramCCMod_CommonArtifact_Types { get; } = [
        typeof(ArtifactHeatRock),
        typeof(ArtifactFlameOrb),
        typeof(ArtifactCharcoal),
        typeof(ArtifactRawstBerry)
    ];
    internal static IReadOnlyList<Type> ReshiramCCMod_BossArtifact_Types { get; } = [
        typeof(ArtifactFireGem)
    ];
    internal static IEnumerable<Type> ReshiramCCMod_AllArtifact_Types
        => ReshiramCCMod_CommonArtifact_Types
        .Concat(ReshiramCCMod_BossArtifact_Types);


    public ModEntry(IPluginPackage<IModManifest> package, IModHelper helper, ILogger logger) : base(package, helper, logger)
    {
        Instance = this;

        // Kokoro is needed to handle statuses
        KokoroApi = helper.ModRegistry.GetApi<IKokoroApi>("Shockah.Kokoro")!.V2;

        Harmony = new Harmony("DragonOfTruth01.ReshiramCCMod");

        // This can be done in place of all Instance.Harmony.Patch() calls in class constructors.
        // However, this will case your IDE/text editor to think the function is unused (since 
        // the patch hasn't been given visibility via constructor). This is expected behavior.
        Harmony.PatchAll();

        /* These localizations lists help us organize our mod's text and messages by language.
         * For general use, prefer AnyLocalizations, as that will provide an easier time to potential localization submods that are made for your mod 
         * IMPORTANT: These localizations are found in the i18n folder (short for internationalization). The Demo Mod comes with a barebones en.json localization file that you might want to check out before continuing 
         * Whenever you add a card, artifact, character, ship, pretty much whatever, you will want to update your locale file in i18n with the necessary information
         * Example: You added your own character, you will want to create an appropiate entry in the i18n file. 
         * If you would rather use simple strings whenever possible, that's also an option -you do you. */
        AnyLocalizations = new JsonLocalizationProvider(
            tokenExtractor: new SimpleLocalizationTokenExtractor(),
            localeStreamFunction: locale => package.PackageRoot.GetRelativeFile($"i18n/{locale}.json").OpenRead()
        );
        Localizations = new MissingPlaceholderLocalizationProvider<IReadOnlyList<string>>(
            new CurrentLocaleOrEnglishLocalizationProvider<IReadOnlyList<string>>(AnyLocalizations)
        );

        // When loading the mod, always default to the regular Reshiram variant
        // This may have to change when loading a run - maybe by doing a relic check
        currCharVariant = CharacterVariant.Reshiram;

        /* Assigning our ISpriteEntry objects manually. This is the easiest way to do it when starting out!
         * Of note: GetRelativeFile is case sensitive. Double check you've written the file names correctly */
        ReshiramCCMod_Character_CardBackground = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/CardBGs/ReshiramCCMod_character_cardbackground.png"));
        ReshiramCCMod_Character_CardFrame = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/ReshiramCCMod_character_cardframe.png"));

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

        // Artifact Arts
        ReshiramCCMod_Character_ArtifactHeatRock = helper.Content.Sprites.RegisterSprite(Package.PackageRoot.GetRelativeFile("assets/artifacts/common/heatRock.png"));
        ReshiramCCMod_Character_ArtifactFlameOrb = helper.Content.Sprites.RegisterSprite(Package.PackageRoot.GetRelativeFile("assets/artifacts/common/flameOrb.png"));
        ReshiramCCMod_Character_ArtifactFlameOrb_Disabled = helper.Content.Sprites.RegisterSprite(Package.PackageRoot.GetRelativeFile("assets/artifacts/common/flameOrb_disabled.png"));
        ReshiramCCMod_Character_ArtifactCharcoal = helper.Content.Sprites.RegisterSprite(Package.PackageRoot.GetRelativeFile("assets/artifacts/common/charcoal.png"));
        ReshiramCCMod_Character_ArtifactRawstBerry = helper.Content.Sprites.RegisterSprite(Package.PackageRoot.GetRelativeFile("assets/artifacts/common/rawstBerry.png"));
        ReshiramCCMod_Character_ArtifactRawstBerry_Disabled = helper.Content.Sprites.RegisterSprite(Package.PackageRoot.GetRelativeFile("assets/artifacts/common/rawstBerry_disabled.png"));
        ReshiramCCMod_Character_ArtifactFireGem = helper.Content.Sprites.RegisterSprite(Package.PackageRoot.GetRelativeFile("assets/artifacts/boss/fireGem.png"));

        ReshiramCCMod_Character_Panel = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/ReshiramCCMod_character_panel.png"));

        ReshiramCCMod_Character_Neutral_0 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/reshi/ReshiramCCMod_character_reshi_neutral_0.png"));
        ReshiramCCMod_Character_Neutral_1 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/reshi/ReshiramCCMod_character_reshi_neutral_1.png"));
        ReshiramCCMod_Character_Neutral_2 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/reshi/ReshiramCCMod_character_reshi_neutral_2.png"));

        ReshiramCCMod_Character_Victini_Neutral_0 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/victini/ReshiramCCMod_character_victini_neutral_0.png"));
        ReshiramCCMod_Character_Victini_Neutral_1 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/victini/ReshiramCCMod_character_victini_neutral_1.png"));
        ReshiramCCMod_Character_Victini_Neutral_2 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/victini/ReshiramCCMod_character_victini_neutral_2.png"));

        ReshiramCCMod_Character_Mini_0 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/reshi/ReshiramCCMod_character_reshi_mini_0.png"));

        ReshiramCCMod_Character_Squint_0 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/reshi/ReshiramCCMod_character_reshi_squint_0.png"));
        ReshiramCCMod_Character_Squint_1 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/reshi/ReshiramCCMod_character_reshi_squint_1.png"));
        ReshiramCCMod_Character_Squint_2 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/reshi/ReshiramCCMod_character_reshi_squint_2.png"));

        ReshiramCCMod_Character_Victini_Squint_0 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/victini/ReshiramCCMod_character_victini_squint_0.png"));
        ReshiramCCMod_Character_Victini_Squint_1 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/victini/ReshiramCCMod_character_victini_squint_1.png"));
        ReshiramCCMod_Character_Victini_Squint_2 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/victini/ReshiramCCMod_character_victini_squint_2.png"));

        ReshiramCCMod_Icon_EnemyOverheat = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/enemyOverheat.png"));
        ReshiramCCMod_Icon_EnemyNotOverheat = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/enemyNotOverheat.png"));
        ReshiramCCMod_Icon_SolarFlareActive = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/solarFlareActive.png"));
        ReshiramCCMod_Icon_NonAttackStun = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/nonAttackStun.png"));
        ReshiramCCMod_Icon_HeatResist = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/heatResist.png"));

        /* Decks are assigned separate of the character. This is because the game has decks like Trash which is not related to a playable character
         * Do note that Color accepts a HEX string format (like Color("a1b2c3")) or a Float RGB format (like Color(0.63, 0.7, 0.76). It does NOT allow a traditional RGB format (Meaning Color(161, 178, 195) will NOT work) */
        ReshiramCCMod_Deck = helper.Content.Decks.RegisterDeck("ReshiramCCModDeck", new DeckConfiguration()
        {
            Definition = new DeckDef()
            {
                /* This color is used in various situations. 
                 * It is used as the deck's rarity 'shine'
                 * If a playable character uses this deck, the character Name will use this color
                 * If a playable character uses this deck, the character mini panel will use this color */
                color = new Color("f4f7f0"),

                /* This color is for the card name in-game
                 * Make sure it has a good contrast against the CardFrame, and take rarity 'shine' into account as well */
                titleColor = new Color("000000")
            },
            /* We give it a default art and border some Sprite types by adding '.Sprite' at the end of the ISpriteEntry definitions we made above. */
            DefaultCardArt = ReshiramCCMod_Character_CardBackground.Sprite,
            BorderSprite = ReshiramCCMod_Character_CardFrame.Sprite,

            /* Since this deck will be used by our Demo Character, we'll use their name. */
            Name = AnyLocalizations.Bind(["character", "ReshiramCCMod", "name"]).Localize,
        });

        /* Let's create some animations, because if you were to boot up this mod from what you have above,
         * DemoCharacter would be a blank void inside a box, we haven't added their sprites yet! 
         * We first begin by registering the animations. I know, weird. 'Why are we making animations when we still haven't made the character itself', stick with me, okay? 
         * Animations are actually assigned to Deck types, not Characters! */

        /*Of Note: You may notice we aren't assigning these ICharacterAnimationEntry and ICharacterEntry to any object, unlike stuff above,
        * It's totally fine to assign them if you'd like, but we don't have a reason to so in this mod */
        helper.Content.Characters.V2.RegisterCharacterAnimation(new CharacterAnimationConfigurationV2()
        {
            /* What we registered above was an IDeckEntry object, but when you register a character animation the Helper will ask for you to provide its Deck 'id'
             * This is simple enough, as you can get it from ReshiramCCMod_Deck */
            CharacterType = ReshiramCCMod_Deck.Deck.Key(),

            /* The Looptag is the 'name' of the animation. When making shouts and events, and you want your character to show emotions, the LoopTag is what you want
             * In vanilla Cobalt Core, there are 4 'animations' looptags that any character should have: "neutral", "mini", "squint" and "gameover",
             * as these are used in: Neutral is used as default, mini is used in character select and out-of-combat UI, Squink is hardcoded used in certain events, and Gameover is used when your run ends */
            LoopTag = "neutral",

            /* The game doesn't use frames properly when there are only 2 or 3 frames. If you want a proper animation, avoid only adding 2 or 3 frames to it */
            Frames = new[]
            {
                // ThE GaMe DoEsNt UsE fRaMeS pRoPeRlY wHeN tHeRe ArE oNlY 2 oR 3 fRaMeS
                // get looped idiot
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
            LoopTag = "mini",
            Frames = new[]
            {
                /* Mini only needs one sprite. We call it animation just because we add it the same way as other expressions. */
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
            LoopTag = "gameover",
            Frames = new[]
            {
                // The squint sprite is okay to use here...
                ReshiramCCMod_Character_Squint_0.Sprite,
            }
        });
        
        /* Let's continue with the character creation and finally, actually, register the character! */
        helper.Content.Characters.V2.RegisterPlayableCharacter("ReshiramCCMod", new PlayableCharacterConfigurationV2()
        {
            /* Same as animations, we want to provide the appropiate Deck type */
            Deck = ReshiramCCMod_Deck.Deck,

            /* The Starter Card Types are, as the name implies, the cards you will start a DemoCharacter run with. 
             * You could provide vanilla cards if you want, but it's way more fun to create your own cards! */
            Starters = new()
            {
                cards = [
                    new CardIncinerate(),
                    new CardDragonClaw()
                ]
            },

            /* This is the little blurb that appears when you hover over the character in-game.
             * You can make it fluff, use it as a way to tell players about the character's playstyle, or a little bit of both! */
            Description = AnyLocalizations.Bind(["character", "ReshiramCCMod", "description"]).Localize,

            /* This is the fancy panel that encapsulates your character while in active combat.
             * It's recommended that it follows the same color scheme as the character and deck, for cohesion */
            BorderSprite = ReshiramCCMod_Character_Panel.Sprite
        });

        /* The basics for a Character mod are done!
         * But you may still have mechanics you want to tackle, such as,
         * 1. How to make cards
         * 2. How to make artifacts
         * 3. How to make ships
         * 4. How to make statuses */

        /* 1. CARDS
         * DemoMod comes with a neat folder called Cards where all the .cs files for our cards are stored. Take a look.
         * You can decide to not use the folder, or to add more folders to further organize your cards. That is up to you.
         * We do recommend keeping files organized, however. It's way easier to traverse a project when the paths are clear and meaningful */

        /* Here we register our cards so we can find them in game.
         * Notice the IDemoCard interface, you can find it in InternalInterfaces.cs
         * Each card in the IEnumerable 'ReshiramCCMod_AllCard_Types' will be asked to run their 'Register' method. Open a card's .cs file, and see what it does 
         * We *can* instead register characts one by one, like what we did with the sprites. If you'd like an example of what that looks like, check out the Randall mod by Arin! */
        foreach (var cardType in ReshiramCCMod_AllCard_Types)
            AccessTools.DeclaredMethod(cardType, nameof(IReshiramCCModCard.Register))?.Invoke(null, [helper]);

        /* 2. ARTIFACTS
         * Creating artifacts is pretty similar to creating Cards
         * Take a look at the Artifacts folder for demo artifacts!
         * You may also notice we're using the other interface from InternalInterfaces.cs, IDemoArtifact, to help us out */
        foreach (var artifactType in ReshiramCCMod_AllArtifact_Types)
            AccessTools.DeclaredMethod(artifactType, nameof(IReshiramCCModArtifact.Register))?.Invoke(null, [helper]);

        /* 4. STATUSES
         * You might, now, with all this code behind our backs, start recognizing patterns in the way we can register stuff. */
        Smoldering = helper.Content.Statuses.RegisterStatus("Smoldering", new()
        {
            Definition = new()
            {
                icon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/smoldering.png")).Sprite,
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
                icon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/flammable.png")).Sprite,
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
                icon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/safeguard.png")).Sprite,
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
                icon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/heatResist.png")).Sprite,
                color = new("ff687d"),
                isGood = true
            },
            Name = AnyLocalizations.Bind(["status", "Heat Resist", "name"]).Localize,
            Description = AnyLocalizations.Bind(["status", "Heat Resist", "description"]).Localize
        });
        
        _ = new StatusManager();
    }
}
