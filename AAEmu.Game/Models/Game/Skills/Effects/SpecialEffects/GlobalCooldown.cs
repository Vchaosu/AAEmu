﻿using System;

using AAEmu.Game.Models.Game.Units;

using NLog;

namespace AAEmu.Game.Models.Game.Skills.Effects.SpecialEffects
{
    public class GlobalCooldown : SpecialEffectAction
    {
        protected override SpecialType SpecialEffectActionType => SpecialType.GlobalCooldown;
        
        public override void Execute(Unit caster,
            SkillCaster casterObj,
            BaseUnit target,
            SkillCastTarget targetObj,
            CastAction castObj,
            Skill skill,
            SkillObject skillObject,
            DateTime time,
            int value1,
            int value2,
            int value3,
            int value4)
        {
            // TODO only for server
            caster.GlobalCooldown = DateTime.UtcNow.AddMilliseconds(value1 * (caster.GlobalCooldownMul/100));
            _log.Warn("value1 {0}, value2 {1}, value3 {2}, value4 {3}", value1, value2, value3, value4);
        }
    }
}
