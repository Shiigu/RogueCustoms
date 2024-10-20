﻿using RogueCustomsGameEngine.Utils.Representation;
using System;
using System.Collections.Generic;

namespace RogueCustomsGameEngine.Utils.JsonImports
{
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    [Serializable]
    public abstract class CharacterInfo : ClassInfo
    {
        public string Faction { get; set; }
        public bool StartsVisible { get; set; }
        public List<CharacterStatInfo> Stats { get; set; }
        public string BaseSightRange { get; set; }
        public int InventorySize { get; set; }
        public string StartingWeapon { get; set; }
        public string StartingArmor { get; set; }
        public List<string> StartingInventory { get; set; }
        public int MaxLevel { get; set; }
        public bool CanGainExperience { get; set; }
        public string ExperiencePayoutFormula { get; set; }
        public string ExperienceToLevelUpFormula { get; set; }
        public ActionWithEffectsInfo OnTurnStart { get; set; } = new ActionWithEffectsInfo();
        public List<ActionWithEffectsInfo> OnAttack { get; set; } = new List<ActionWithEffectsInfo>();
        public ActionWithEffectsInfo OnAttacked { get; set; } = new ActionWithEffectsInfo();
        public ActionWithEffectsInfo OnDeath { get; set; } = new ActionWithEffectsInfo();
    }

    [Serializable]
    public class CharacterStatInfo
    {
        public string StatId { get; set; }
        public decimal Base { get; set; }
        public decimal IncreasePerLevel { get; set; }
    }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}