﻿using System;
using AAEmu.Game.Core.Managers;
using AAEmu.Game.Core.Managers.World;
using AAEmu.Game.Models.Game.Char;
using AAEmu.Game.Models.Game.Faction;
using AAEmu.Game.Models.Game.Skills;
using AAEmu.Game.Models.Game.Skills.Static;
using AAEmu.Game.Models.Game.World;

namespace AAEmu.Game.Models.Game.Units
{
    public enum BaseUnitType : byte
    {
        Character = 0,
        Npc = 1,
        Slave = 2,
        Housing = 3,
        Transfer = 4,
        Mate = 5,
        Shipyard = 6
    }

    public enum ModelPostureType : byte
    {
        None = 0,
        HouseState = 1,
        ActorModelState = 4,
        FarmfieldState = 7,
        TurretState = 8
    }

    public class BaseUnit : GameObject
    {
        public string Name { get; set; } = string.Empty;
        public SystemFaction Faction { get; set; }

        public virtual float Scale => 1f;
        
        public Effects Effects { get; set; }
        public SkillModifiers Modifiers { get; set; }
        public CombatBuffs CombatBuffs { get; set; }

        public BaseUnit()
        {
            Effects = new Effects(this);
            Modifiers = new SkillModifiers();
            CombatBuffs = new CombatBuffs(this);
        }

        public bool CanAttack(BaseUnit target)
        {
            if (this.Faction == null || target.Faction == null)
                return false;
            if (this.ObjId == target.ObjId)
                return false;
            var relation = GetRelationStateTo(target);
            var zone = ZoneManager.Instance.GetZoneByKey(target.Position.ZoneId);
            if (this is Character me && target is Character other)
            {
                var trgIsFlagged = other.Effects.CheckBuff((uint)BuffConstants.RETRIBUTION_BUFF);

                if (other.Faction.MotherId != 0 && other.Faction.MotherId == zone.FactionId 
                    && !me.IsActivelyHostile(other) && !trgIsFlagged)
                {
                    return false;
                }

                bool isTeam = TeamManager.Instance.AreTeamMembers(me.Id, other.Id);
                if (trgIsFlagged && !isTeam && relation == RelationState.Friendly)
                {
                    return true;
                }
                else if (me.ForceAttack && relation == RelationState.Friendly && !isTeam)
                {
                    return true;
                }
            }

            //This check is for npcs
            if (target.Faction.MotherId == zone.FactionId)
                return false;

            return relation == RelationState.Hostile;
        }

        public RelationState GetRelationStateTo(BaseUnit unit) => this.Faction.GetRelationState(unit.Faction);

        public virtual void AddBonus(uint bonusIndex, Bonus bonus)
        {
        }

        public virtual void RemoveBonus(uint bonusIndex, UnitAttribute attribute)
        {
        }
        
        public virtual double ApplySkillModifiers(Skill skill, SkillAttribute attribute, double baseValue) {
            return Modifiers.ApplyModifiers(skill, attribute, baseValue);
        }
    }
}
