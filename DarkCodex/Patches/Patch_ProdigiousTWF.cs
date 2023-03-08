﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;

namespace DarkCodex
{
    [HarmonyPatch]
    public class Patch_ProdigiousTWF
    {
        [HarmonyPatch(typeof(TwoWeaponFightingAttackPenalty), nameof(TwoWeaponFightingAttackPenalty.OnEventAboutToTrigger), typeof(RuleCalculateAttackBonusWithoutTarget))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler1(IEnumerable<CodeInstruction> instructions, ILGenerator generator, MethodBase original)
        {
            var data = new TranspilerTool(instructions, generator, original);

            // find last access to a local boolean variable
            data.Last().Rewind(f => f.IsStloc(7));

            // set variable to true, if weapon should be considered light
            //data.InsertAfter(PatchTWF);
			//The above throws an OpCode conversion error. It's ambiguous which "InsertAfter" it is calling so it goes for the first one and not the Delegate one. --Teraunce

            return data.Code;
        }

        public static void PatchTWF(TwoWeaponFightingAttackPenalty __instance, [LocalParameter(type: typeof(bool), index: 7)] ref bool flag)
        {
            flag = flag || __instance.Owner.HasFlag(MechanicFeature.ProdigiousTWF);
        }
    }
}
