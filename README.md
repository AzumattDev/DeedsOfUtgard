# Deeds of Utgard

A super simple currency that was created for a server (now died out) by me. I am just uploading if someone else finds a
use. It was used in their marketplace to buy plots of land.

`The prefabs are not craftable, but you can buy them from the market (if you have one) or spawn them in.`

`If the mod is installed on the server, it version checks with itself. Kicking players that don't have it.`

## How they used the prefabs

They chose to make "deeds" that would be sold by an NPC. These deeds all had a different cost and "size" for the plot of
land they would buy. They used WardIsLove's custom radius to configure a protection for their plot of land depending on
the size they bought. The admin would change the radius of the ward.

I guess, if you come up with an implementation for your server, feel free to use
this. `This mod is provided as is and will only recieve updates if requested. (This includes if an update to the game breaks the mod. I will only update if someone requests it.)`

## Information

#### Stack size = 50

#### Weight = 0.1

### Prefab Names

- Normal Deeds <img src="https://i.imgur.com/OU0KYu2.png" align="center" width="64" height="64">
    * Deed10m
    * Deed20m
    * Deed50m
- Utgard Deeds (Red outline) <img src="https://i.imgur.com/robxZ5Y.png" align="center" width="64" height="64">
    * DeedUtgard10m
    * DeedUtgard20m
    * DeedUtgard50m
      <br>
      <br>

### Internal Token Names

- ##### Normal Deeds
    * $item_deed_10m
    * $item_deed_20m
    * $item_deed_50m
    * $item_deed_10m_description
    * $item_deed_20m_description
    * $item_deed_50m_description
- ##### Utgard Deeds (Red outline)
    * $item_deed_utgard_10m
    * $item_deed_utgard_20m
    * $item_deed_utgard_50m
    * $item_deed_utgard_10m_description
    * $item_deed_utgard_20m_description
    * $item_deed_utgard_50m_description
      <br>
      <br>

### Localization Example

Create a file with the following naming convention: `DeedsOfUtgard.{Language}.yml`

Example file name would be: `DeedsOfUtgard.Spanish.yml`. You can place this file anywhere inside of the BepInEx folder
to localize the mod. Keep in mind you can only have one localization file per language. If you have multiple, the mod
will only load the first one it finds (and will print warnings to the console telling you where the duplicate was found
and that you should remove it).

```yaml
item_deed_10m: "Deed 10m"
item_deed_20m: "Deed 20m"
item_deed_50m: "Deed 50m"
item_deed_utgard_10m: "Deed 10m"
item_deed_utgard_20m: "Deed 20m"
item_deed_utgard_50m: "Deed 50m"
item_deed_10m_description: "A deed for a 10m plot of land."
item_deed_20m_description: "A deed for a 20m plot of land."
item_deed_50m_description: "A deed for a 50m plot of land."
item_deed_utgard_10m_description: "A deed for a 10m plot of land."
item_deed_utgard_20m_description: "A deed for a 20m plot of land."
item_deed_utgard_50m_description: "A deed for a 50m plot of land."
```

# Author Information

### Azumatt

`DISCORD:` Azumatt#2625

`STEAM:` https://steamcommunity.com/id/azumatt/

For Questions or Comments, find me in the Odin Plus Team Discord or in mine:

[![https://i.imgur.com/XXP6HCU.png](https://i.imgur.com/XXP6HCU.png)](https://discord.gg/Pb6bVMnFb2)
<a href="https://discord.gg/pdHgy6Bsng"><img src="https://i.imgur.com/Xlcbmm9.png" href="https://discord.gg/pdHgy6Bsng" width="175" height="175"></a>