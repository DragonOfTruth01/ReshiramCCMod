using System.Collections.Generic;

namespace DragonOfTruth01.ReshiramCCMod;

internal sealed class WrappedActionManager : IKokoroApi.IV2.IWrappedActionsApi.IHook
{
    public static ModEntry Instance => ModEntry.Instance;

    public WrappedActionManager()
    {
        /* We task Kokoro with the job to register our status into the game */
        Instance.KokoroApi.WrappedActions.RegisterHook(this, 0);
    }

    public IEnumerable<CardAction>? GetWrappedCardActions(IKokoroApi.IV2.IWrappedActionsApi.IHook.IGetWrappedCardActionsArgs args)
    {
        return null;
    }
}