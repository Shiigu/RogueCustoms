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
            var oldMaxHP = HP.BaseAfterLevelUp;
            var oldAttack = Attack.BaseAfterLevelUp;
            var oldDefense = Defense.BaseAfterLevelUp;
            var oldMovement = Movement.BaseAfterLevelUp;
            var oldHPRegeneration = HPRegeneration.BaseAfterLevelUp;
            var oldMPRegeneration = MPRegeneration != null ? MPRegeneration.BaseAfterLevelUp : 0;
            base.GainExperience(GamePointsToAdd);
            if (Level > oldLevel)
            {
                Map.AddSpecialEffectIfPossible(SpecialEffect.LevelUp);
                var levelUpMessage = new StringBuilder(Map.Locale["CharacterLevelsUpMessage"].Format(new { CharacterName = Name, Level = Level }));
                levelUpMessage.AppendLine();
                levelUpMessage.AppendLine();
                if (HP.BaseAfterLevelUp != oldMaxHP)
                    levelUpMessage.AppendLine(Map.Locale["CharacterStatGotBuffed"].Format(new { CharacterName = Name, StatName = Map.Locale["CharacterMaxHPStat"], Amount = (HP.BaseAfterLevelUp - oldMaxHP).ToString() }));
                if (Attack.BaseAfterLevelUp != oldAttack)
                    levelUpMessage.AppendLine(Map.Locale["CharacterStatGotBuffed"].Format(new { CharacterName = Name, StatName = Map.Locale["CharacterAttackStat"], Amount = (Attack.BaseAfterLevelUp - oldAttack).ToString() }));
                if (Defense.BaseAfterLevelUp != oldDefense)
                    levelUpMessage.AppendLine(Map.Locale["CharacterStatGotBuffed"].Format(new { CharacterName = Name, StatName = Map.Locale["CharacterDefenseStat"], Amount = (Defense.BaseAfterLevelUp - oldDefense).ToString() }));
                if (Movement.BaseAfterLevelUp != oldMovement)
                    levelUpMessage.AppendLine(Map.Locale["CharacterStatGotBuffed"].Format(new { CharacterName = Name, StatName = Map.Locale["CharacterMovementStat"], Amount = (Movement.BaseAfterLevelUp - oldMovement).ToString() }));
                if (HPRegeneration.BaseAfterLevelUp != oldHPRegeneration)
                    levelUpMessage.AppendLine(Map.Locale["CharacterStatGotBuffed"].Format(new { CharacterName = Name, StatName = Map.Locale["CharacterHPRegenerationStat"], Amount = (HPRegeneration.BaseAfterLevelUp - oldHPRegeneration).ToString("0.#####") }));
                if(UsesMP && MPRegeneration.BaseAfterLevelUp != oldMPRegeneration)
                    levelUpMessage.AppendLine(Map.Locale["CharacterStatGotBuffed"].Format(new { CharacterName = Name, StatName = Map.Locale["CharacterMPRegenerationStat"], Amount = (MPRegeneration.BaseAfterLevelUp - oldMPRegeneration).ToString("0.#####") }));
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
                Map.AddSpecialEffectIfPossible(SpecialEffect.ItemGet);
                Map.AppendMessage(Map.Locale["PlayerPutItemOnBag"].Format(new { CharacterName = Name, ItemName = item.Name }));
            }
        }

        public override void PickKey(Key key, bool informToPlayer)
        {
            KeySet.Add(key);
            key.Owner = this;
            key.Position = null;
            key.ExistenceStatus = EntityExistenceStatus.Gone;
            if (informToPlayer)
            {
                Map.AddSpecialEffectIfPossible(SpecialEffect.KeyGet);
                Map.AppendMessage(Map.Locale["PlayerPutItemOnBag"].Format(new { CharacterName = Name, ItemName = key.Name }));
            }
        }
    }
    #pragma warning restore CS8604 // Posible argumento de referencia nulo
    #pragma warning restore CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
}
