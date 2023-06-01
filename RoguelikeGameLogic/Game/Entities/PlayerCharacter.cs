using RoguelikeGameEngine.Game.DungeonStructure;
using RoguelikeGameEngine.Utils.Enums;
using RoguelikeGameEngine.Utils.Helpers;
using RoguelikeGameEngine.Utils.Representation;
using System.Drawing;
using System.Text;

namespace RoguelikeGameEngine.Game.Entities
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
                var levelUpMessage = new StringBuilder($"{Name} has reached Level {Level}!");
                levelUpMessage.AppendLine();
                levelUpMessage.AppendLine();
                if (MaxHP != oldMaxHP)
                    levelUpMessage.Append("HP increased by ").Append(MaxHP - oldMaxHP).AppendLine(".");
                if (Attack != oldAttack)
                    levelUpMessage.Append("Attack increased by ").Append(Attack - oldAttack).AppendLine(".");
                if (Defense != oldDefense)
                    levelUpMessage.Append("Defense increased by ").Append(Defense - oldDefense).AppendLine(".");
                if (Movement != oldMovement)
                    levelUpMessage.Append("Movement increased by ").Append(Movement - oldMovement).AppendLine(".");
                if (HPRegeneration != oldHPRegeneration)
                    levelUpMessage.Append("HP Regeneration increased by ").Append(HPRegeneration - oldHPRegeneration).Append('.');
                Map.AddMessageBox("CONGRATULATIONS!", levelUpMessage.ToString(), "OK", new GameColor(Color.Green));
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

        public new void RefreshCooldowns()
        {
            MaxHPModifications?.Where(a => a.RemainingTurns == 0 && a.RemainingTurns > 1).ForEach(a => Map.AppendMessage($"The effect from {a.Id} has vanished..."));
            AttackModifications?.Where(a => a.RemainingTurns == 0 && a.RemainingTurns > 1).ForEach(a => Map.AppendMessage($"The effect from {a.Id} has vanished..."));
            DefenseModifications?.Where(a => a.RemainingTurns == 0 && a.RemainingTurns > 1).ForEach(a => Map.AppendMessage($"The effect from {a.Id} has vanished..."));
            MovementModifications?.Where(a => a.RemainingTurns == 0 && a.RemainingTurns > 1).ForEach(a => Map.AppendMessage($"The effect from {a.Id} has vanished..."));
            base.RefreshCooldownsAndUpdateTurnLength();
        }

        public override void Die(Entity? attacker = null)
        {
            ExistenceStatus = EntityExistenceStatus.Dead;
            Map.DungeonStatus = DungeonStatus.GameOver;
            Passable = true;
            if (attacker != null)
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
            Map.AppendMessage($"{Name} put {item.Name} on the floor.");
        }

        public override void PickItem(Item item)
        {
            Inventory.Add(item);
            item.Owner = this;
            item.Position = null;
            item.ExistenceStatus = EntityExistenceStatus.Gone;
            Map.AppendMessage($"{Name} put up {item.Name} on the bag.");
        }
    }
}
