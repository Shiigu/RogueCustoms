using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.Helpers;
using RogueCustomsGameEngine.Utils.Representation;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;

namespace RogueCustomsGameEngine.Game.Entities
{
    public class PlayerCharacter : Character
    {
        public PlayerCharacter(EntityClass entityClass, int level, Map map) : base(entityClass, level, map)
        {
        }

        public new void GainExperience(int pointsToAdd)
        {
            var oldLevel = Level;
            var oldMaxHP = MaxHP;
            var oldAttack = Attack;
            var oldDefense = Defense;
            var oldMovement = Movement;
            var oldHPRegeneration = HPRegeneration;
            base.GainExperience(pointsToAdd);
            if (Level > oldLevel)
            {
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
                    levelUpMessage.AppendLine(Map.Locale["CharacterStatGotBuffed"].Format(new { CharacterName = Name, StatName = Map.Locale["CharacterHPRegenerationStat"], Amount = (HPRegeneration - oldHPRegeneration).ToString() }));
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
            ExistenceStatus = EntityExistenceStatus.Dead;
            Map.DungeonStatus = DungeonStatus.GameOver;
            Passable = true;
            if (attacker == null || attacker is Character)
                OnDeathActions?.ForEach(oda => oda.Do(this, attacker));
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
            Map.AppendMessage(Map.Locale["PlayerPutItemOnFloor"].Format(new { CharacterName = Name, ItemName = item.Name }));
        }

        public override void PickItem(Item item)
        {
            Inventory.Add(item);
            item.Owner = this;
            item.Position = null;
            item.ExistenceStatus = EntityExistenceStatus.Gone;
            Map.AppendMessage(Map.Locale["PlayerPutItemOnBag"].Format(new { CharacterName = Name, ItemName = item.Name }));
        }
    }
}
