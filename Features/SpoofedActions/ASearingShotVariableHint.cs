using System.Collections.Generic;

namespace DragonOfTruth01.ReshiramCCMod;

internal sealed class ASearingShotVariableHint : AVariableHint
{
    public override List<Tooltip> GetTooltips(State s)
    {
        if (status.HasValue)
        {
            List<Tooltip> list = new List<Tooltip>();
            list.Add(new TTGlossary("action.xHint.desc", "<c=status>" + status.Value.GetLocName() + "</c>", "", secondStatus.HasValue ? (" </c>+ <c=status>" + secondStatus.Value.GetLocName() + "</c>") : "", ""));
            return list;
        }
        return new List<Tooltip>();
    }
}