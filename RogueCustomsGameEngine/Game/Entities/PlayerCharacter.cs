using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.Helpers;
using RogueCustomsGameEngine.Utils.Representation;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System;

namespace RogueCustomsGameEngine.Game.Entities
{
    #pragma warning disable CS8604 // Posible argumento de referencia nulo
    #pragma warning disable CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
    [Serializable]
    public class PlayerCharacter : Character
    {
        public PlayerCharacter(EntityClass entityClass, int level, Map map) : base(entityClass, level, map)
        {
        }

        public new void GainExperience(int GamePointsToAdd)
        {
            var oldLevel = Level;
            var oldMaxHP = MaxHP;
            var oldAttack = Attack;
            var oldDefense = Defense;
            var oldMovement = Movement;
            var oldHPRegeneration = HPRegeneration;
            var oldMPRegeneration = MPRegeneration;
            base.GainExperience(GamePointsToAdd);
            if (Level > oldLevel)
            {
                Map.AddSpecialEffectIfPossible(SpecialEffect.LevelUp);
                var levelUpMessage = new StringBuilder(Map.Locale["CharacterLevelsUpMessage"].Format(new { CharacterName = Name, Level = Level }));
                levelUpMessage.AppendLine();
                levelUpMessage.AppendLine();
                if (MaxHP != oldMaxHP)
                    levelUpMessage.AppendLine(Map.Locale["CharacterStatGotBuffed"].Format(new { CharacterName = Name, StatName = Map.Locale["CharacterMaxHPStat"], Amount = (MaxHP - oldMaxHP).ToString() }));
                if (Attack != oldAttack)
                    levelUpMessage.AppendLine(Map.Locale["CharacterStatGotBuffed"].Format(new { CharacterName = Name, StatName = Map.Locale["CharacterAttackStat"], Amount = (Attack - oldAttack).ToString() }));
                if (Defense != oldDefense)
                    levelUpMessage.AppendLine(Map.Locale["CharacterStatGotBuffed"].Format(new { CharacterName = Name, StatName = Map.Locale["CharacterDefenseStat"], Amount = (Defense - oldDefense).ToString() }));
                if (Movement != oldMovement)
                    levelUpMessage.AppendLine(Map.Locale["CharacterStatGotBuffed"].Format(new { CharacterName = Name, StatName = Map.Locale["CharacterMovementStat"], Amount = (Movement - oldMovement).ToString() }));
                if (HPRegeneration != oldHPRegeneration)
                    levelUpMessage.AppendLine(Map.Locale["CharacterStatGotBuffed"].Format(new { CharacterName = Name, StatName = Map.Locale["CharacterHPRegenerationStat"], Amount = (HPRegeneration - oldHPRegeneration).ToString("0.#####") }));
                if(UsesMP && MPRegeneration != oldMPRegeneration)
                    levelUpMessage.AppendLine(Map.Locale["CharacterStatGotBuffed"].Format(new { CharacterName = Name, StatName = Map.Locale["CharacterMPRegenerationStat"], Amount = (MPRegeneration - oldMPRegeneration).ToString("0.#####") }));
                Map.AddMessageBox(Map.Locale["CharacterLevelsUpHeader"], levelUpMessage.ToString(), "OK", new GameColor(Color.Lime));
            }
        }

        public void UpdateVisibility()
        {
            Map.Tiles.ForEach(t => t.Visible = false);
            var tiles = ComputeFOVTiles();
            foreach (var tile in tiles)
            {
                var mapTile = Map.GetTileFromCoordinates(tile.Position);
                mapTile.Discovered = mapTile.Visible = true;
            }
            FOVTiles = tiles;
        }

        public override void Die(Entity? attacker = null)
        {
            base.Die(attacker);
            if (ExistenceStatus == EntityExistenceStatus.Dead)
            {
                Map.DungeonStatus = DungeonStatus.GameOver;
                Map.AddSpecialEffectIfPossible(SpecialEffect.GameOver);
            }
        }

        public void DropItem(int slot)
        {
            var item = Inventory.ElementAtOrDefault(slot);
            if (item != null)
            {
                DropItem(item);
            }
        }

        public override void DropItem(Item item)
        {
            if (item == EquippedWeapon)
                EquippedWeapon = null;
            else if (item == EquippedArmor)
                EquippedArmor = null;
            else
                Inventory.Remove(item);
            item.Position = Position;
            item.Owner = null!;
            item.ExistenceStatus = EntityExistenceStatus.Alive;
            Map.AddSpecialEffectIfPossible(SpecialEffect.ItemDrop);
            Map.AppendMessage(Map.Locale["PlayerPutItemOnFloor"].Format(new { CharacterName = Name, ItemName = item.Name }));
        }

        public override void PickItem(Item item, bool informToPlayer)
        {
            Inventory.Add(item);
            item.Owner = this;
            item.Position = null;
            item.ExistenceStatus = EntityExistenceStatus.Gone;
            if (informToPlayer)
            {
                if(item.EntityType == EntityType.Key)
                    Map.AddSpecialEffectIfPossible(SpecialEffect.KeyGet);
                else
                    Map.AddSpecialEffectIfPossible(SpecialEffect.ItemGet);
                Map.AppendMessage(Map.Locale["PlayerPutItemOnBag"].Format(new { CharacterName = Name, ItemName = item.Name }));
            }
        }
    }
    #pragma warning restore CS8604 // Posible argumento de referencia nulo
    #pragma warning restore CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
}
