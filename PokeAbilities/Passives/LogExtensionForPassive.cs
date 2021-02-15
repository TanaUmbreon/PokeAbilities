using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeAbilities.Passives
{
    internal static class LogExtensionForPassive
    {
        public static void AppendLine(this Log log, PassiveAbilityBase passive, string methodName, string mesage)
        {
            log.AppendLine($"[{passive.GetType().Name}.{methodName}] {mesage}");
        }

        public static void AppendDefaultInfo(this Log log, PassiveAbilityBase passive)
        {
            log.AppendLine($"- Passive name: '{passive.name}' (Owner name: '{passive.Owner.UnitData.unitData.name}')");
        }
    }
}
