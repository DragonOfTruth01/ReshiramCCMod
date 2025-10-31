using Nickel;
using FSPRO;
using System.Collections.Generic;

namespace DragonOfTruth01.ReshiramCCMod;

public sealed class ANonAttackStun : CardAction
{
    public bool targetPlayer;

    public override void Begin(G g, State s, Combat c)
    {
        Ship ship = (targetPlayer ? s.ship : c.otherShip);

        if(ship != null)
        {
            bool playSfx = false;

            foreach (Part part in ship.parts)
            {
                // If the intent is not an attack, isn't unstunnable, and isn't already null, cancel it
                if (part.intent != null && part.intent.GetType() != typeof(IntentAttack) && part.stunModifier != PStunMod.unstunnable)
                {
                    playSfx = true;
                    part.intent = null;

                    // Special handling for the rogue starnacle
                    if (part.skin == "octoMouthCharging")
                    {
                        part.skin = "octoMouthChargingOff";
                    }
                }
            }

            // If we canceled at least one intent, play a stun sound effect
            if (playSfx)
            {
                Audio.Play(Event.Status_Stun);
            }
        }        
    }
    
    public override Icon? GetIcon(State s)
    {
        return new Icon(ModEntry.Instance.ReshiramCCMod_Icon_NonAttackStun.Sprite, number: null, color: Colors.textMain, flipY: false);
    }

    public override List<Tooltip> GetTooltips(State s)
        => [
            new GlossaryTooltip($"action.{ModEntry.Instance.Package.Manifest.UniqueName}::NonAttackStun")
            {
                Icon = ModEntry.Instance.ReshiramCCMod_Icon_NonAttackStun.Sprite,
                TitleColor = Colors.action,
                Title = ModEntry.Instance.Localizations.Localize(["action", "Non-Attack Stun", "name"]),
                Description = ModEntry.Instance.Localizations.Localize(["action", "Non-Attack Stun", "description"])
            }
        ];
}