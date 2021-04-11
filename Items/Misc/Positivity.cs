using Microsoft.Xna.Framework;
using Terraria;
using Plethorum.Items.Bars;
using Terraria.ID;
using Terraria.ModLoader;

namespace Plethorum.Items.Misc
{
	// Making an item like Life Fruit (That goes above 500) involves a lot of code, as there are many things to consider. (An alternate that approaches 500 can simply follow vanilla code, however.):
	//    You can't make player.statLifeMax more than 500 (it won't save), so you'll have to maintain your extra life within your mod.
	//    Within your ModPlayer, you need to save/load a count of usages. You also need to sync the data to other players. 
	internal class ExampleLifeFruit : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("Permanently increases maximum life by 200");
		}

		public override void SetDefaults() {
			item.CloneDefaults(ItemID.LifeFruit);
			item.color = Color.Purple;
		}

		public override bool CanUseItem(Player player) {
			// Any mod that changes statLifeMax to be greater than 500 is broken and needs to fix their code.
			// This check also prevents this item from being used before vanilla health upgrades are maxed out.
			return player.statLifeMax == 500 && player.GetModPlayer<PlethorumPlayer>().Positivity < PlethorumPlayer.maxPositivity;
		}

		public override bool UseItem(Player player) {
			// Do not do this: player.statLifeMax += 2;
			player.statLifeMax2 += 200;
			player.statLife += 200;
			if (Main.myPlayer == player.whoAmI) {
				// This spawns the green numbers showing the heal value and informs other clients as well.
				player.HealEffect(200, true);
			}
			// This is very important. This is what makes it permanent.
			player.GetModPlayer<PlethorumPlayer>().Positivity += 1;
			// This handles the 2 achievements related to using any life increasing item or getting to exactly 500 hp and 200 mp.
			// Ignored since our item is only useable after this achievement is reached
			// AchievementsHelper.HandleSpecialEvent(player, 2);
			return true;
		}

		public override void AddRecipes() {
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.LunarBar, 5);
			recipe.AddIngredient(ModContent.ItemType<OxiumBar>(), 10);
            recipe.AddIngredient(ItemID.LifeCrystal, 5);
            recipe.AddIngredient(ItemID.LifeFruit, 5);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}