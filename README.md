# Seaweed Farming (Cultivated Seaweed)

A lightweight, performance-focused mod for **Vintage Story** that adds renewable, farmable seaweed.

I originally built this to help with the sushi recipes in [Expanded Foods](https://mods.vintagestory.at/expandedfoods), creating a simple way to keep the kitchen stocked without needing to constantly forage the ocean floor.

## 🚀 Features

* **Cultivated Visuals:** **Cultivated Seaweed** grows in perfectly straight, aligned columns without random offsets. This is intentional, providing a uniform, "farmed" appearance that is distinct from wild seaweed.
* **Whole Stack Breaking:** Breaking any part of the seaweed stalk will break the entire column above and below it, simplifying the harvest process.
* **Performance Focused:** Utilizes a lightweight BlockEntity. Only the cultivated roots you plant are active, meaning no unnecessary overhead for the rest of the ocean. The tick rate for growth checks is fully configurable.
* **Renewable Resource:** Turn a single piece of seaweed into an infinite source of food and crafting material. It requires compost to craft new roots.
* **Safe for Existing Worlds:** Does not alter terrain generation or biomes. It is strictly a gameplay addition that is safe to add to established saves.

## 🛠️ The Gameplay Loop

Farming seaweed is a simple, rewarding cycle:

1.  **Craft:** Combine **1x Seaweed** + **1x Compost** to create a **Cultivated Seaweed Root**.
2.  **Plant:** Place the root underwater (must be submerged in **Salt Water**).
3.  **Grow:** Wait for it to grow! It will slowly reach up to the surface (max 10 blocks).
4.  **Harvest:** Break any part of the plant. The entire column will collapse, dropping vanilla **Seaweed**.
    * *Note: Breaking the plant destroys the root.*
5.  **Repeat:** Use the harvested seaweed to craft more roots (with compost), or use it for food and crafting!

## ⚙️ Configuration

The mod generates a `SeaweedFarming.json` config file in your `ModConfig` folder upon first launch.

| Option | Default | Description |
| :--- | :--- | :--- |
| `MaxHeight` | `10` | Maximum height (in blocks) the cultivated seaweed can grow. |
| `BaseGrowthHours` | `36.0` | Average in-game hours between growth stages. |
| `GrowthVariance` | `0.25` | Random variance percentage for growth to keep farms feeling organic (0.25 = ±25%). |
| `TickIntervalMinutes` | `1.0` | How often the server checks for growth (in real-world minutes). |

## 📦 Installation

1.  Download the latest release (or build from source).
2.  Place the `SeaweedFarming.zip` file into your Vintage Story `Mods` folder:
    * **Windows:** `%appdata%/Vintagestory/Mods`
    * **Linux:** `~/.config/Vintagestory/Mods`
3.  Launch the game.

## 💻 Technical Details (For Developers)

This mod uses a **Custom Block Class** (`BlockCultivatedSeaweed`) paired with a **BlockEntity** (`BlockEntityCultivatedSeaweed`) to handle growth logic.

* **Scheduled Growth:** Instead of random ticks, each root schedules its next growth time using `RegisterGameTickListener`. This ensures reliable growth rates (~2.25 days avg) without hammering the server with constant random tick checks.
* **Safety Checks:**
    * **Liquid Integrity:** Growth checks strictly for `LiquidCode == "saltwater"` AND `BlockMaterial == Liquid`.
    * **Strict Code Matching:** Explicitly verifies vanilla `seaweed-kelp-top` and `seaweed-kelp-section` asset codes before modifying the world.

## 🏗️ Building from Source

**Requirements:**
* .NET 8.0 SDK
* Vintage Story v1.21.6+

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

**Maintenance Note:** If I am ever unavailable to update this mod, you have my explicit permission to create and publish 'continued' versions. I only ask that you kindly link back to this original version in case I return to the project.