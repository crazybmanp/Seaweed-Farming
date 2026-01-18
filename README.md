# Seaweed Farming (Cultivated Seaweed)

A lightweight, performance-focused mod for **Vintage Story** that adds renewable seaweed farming mechanics.

## 🚀 Features

* **Renewable Seaweed:** Craft a "Cultivated Seaweed Root" to grow your own kelp farms at home.
* **Performance Focused:** This mod uses an optimized growth system where only your specific cultivated roots are active. The rest of the ocean remains static, ensuring maximum server performance.
* **Safe for Existing Worlds:** Does not alter terrain generation or biomes. It is strictly a gameplay addition that is safe to add to established saves.
* **Vanilla Mechanics:** Grows standard vanilla `seaweed-kelp` blocks. Harvested items are fully compatible with *Expanded Foods* (drying, roasting, etc.) and other mods that use vanilla seaweed.

## 🛠️ Mechanics

### The Cultivated Root
The core of this mod is the **Cultivated Seaweed Root** (`seaweedfarming:cultivated-seaweed`). This is a custom block that simulates growth logic.

1.  **Crafting:** Combine **1x Seaweed Section** + **1x Compost** in the crafting grid.
2.  **Planting:** Place the root underwater (must be submerged).
3.  **Growth:** The root will randomly tick (targeted **~2.25 in-game days** per block) and grow a stalk of vanilla seaweed upwards.
    * It respects the visual logic of "Seaweed Tops" vs "Seaweed Sections."
    * It caps at a height of 10 blocks.
    * It will stop if it hits the surface or an obstacle.
4.  **Harvesting:** Swim out and break the seaweed stalk *above* the root. The root stays behind and begins the growth cycle again automatically.

## ⚙️ Configuration

The mod generates a `SeaweedFarming.json` config file in your `ModConfig` folder upon first launch.

| Option | Default | Description |
| :--- | :--- | :--- |
| `MaxHeight` | `10` | Maximum height (in blocks) the cultivated seaweed can grow. |
| `BaseGrowthHours` | `36.0` | Average in-game hours between growth stages. |
| `GrowthVariance` | `0.25` | Random variance percentage for growth (0.25 = ±25%). |
| `TickIntervalMinutes` | `1.0` | How often the server checks for growth (in real-world minutes). |

## 📦 Installation

1.  Download the latest release (or build from source).
2.  Place the `SeaweedFarming.zip` file into your Vintage Story `Mods` folder:
    * **Windows:** `%appdata%/Vintagestory/Mods`
    * **Linux:** `~/.config/Vintagestory/Mods`
3.  Launch the game.

## 💻 Technical Details (For Developers)

This mod uses a **Custom Block Class** (`BlockCultivatedSeaweed`) acting as a proxy for the growth logic.

*   **API Compliance:** Updated for Vintage Story 1.21, utilizing `ShouldReceiveServerGameTicks` and `OnServerGameTick` for optimized server-side performance.
*   **Dynamic Growth:** The growth probability is calculated at runtime in `OnLoaded` using the world's `SpeedOfTime`, ensuring consistent growth rates regardless of server calendar configuration.
*   **Safety Checks:**
    *   **Liquid Integrity:** Growth checks strictly for `LiquidCode == "water"` AND `BlockMaterial == Liquid`.
    *   **Strict Code Matching:** Explicitly verifies vanilla `seaweed-kelp-top` and `seaweed-kelp-section` asset codes before modifying the world.

## 🏗️ Building from Source

**Requirements:**
* .NET 8.0 SDK
* Vintage Story v1.21+

**Steps:**
1.  Clone the repository.
2.  **Configuration:** Ensure `VINTAGE_STORY_PATH` is set in `SeaweedFarming.csproj` to match your game installation (Default: `%appdata%\Vintagestory`).
3.  Run the build command:
    ```powershell
    dotnet build
    ```
4.  The build script will automatically package the mod into `SeaweedFarming.zip` in the project root.

## 📄 License

MIT License - Feel free to include this in modpacks or modify for your own server needs.