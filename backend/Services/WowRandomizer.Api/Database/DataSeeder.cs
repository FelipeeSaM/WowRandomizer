using Microsoft.EntityFrameworkCore;
using WowRandomizer.Api.Entities;

namespace WowRandomizer.Api.Database;

public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (await context.Factions.AnyAsync()) return;

        var factions = new Dictionary<string, Faction>
        {
            ["Alliance"] = new() { Name = "Alliance" },
            ["Horde"]    = new() { Name = "Horde" }
        };

        var classes = new Dictionary<string, GameClass>
        {
            ["Warrior"]       = new() { Name = "Warrior" },
            ["Paladin"]       = new() { Name = "Paladin" },
            ["Hunter"]        = new() { Name = "Hunter" },
            ["Rogue"]         = new() { Name = "Rogue" },
            ["Priest"]        = new() { Name = "Priest" },
            ["Death Knight"]  = new() { Name = "Death Knight" },
            ["Shaman"]        = new() { Name = "Shaman" },
            ["Mage"]          = new() { Name = "Mage" },
            ["Warlock"]       = new() { Name = "Warlock" },
            ["Monk"]          = new() { Name = "Monk" },
            ["Druid"]         = new() { Name = "Druid" },
            ["Demon Hunter"]  = new() { Name = "Demon Hunter" },
            ["Evoker"]        = new() { Name = "Evoker" }
        };

        var races = new Dictionary<string, Race>
        {
            ["Human"]               = new() { Name = "Human",               IsNeutral = false },
            ["Dwarf"]               = new() { Name = "Dwarf",               IsNeutral = false },
            ["Dark Iron Dwarf"]     = new() { Name = "Dark Iron Dwarf",     IsNeutral = false },
            ["Gnome"]               = new() { Name = "Gnome",               IsNeutral = false },
            ["Night Elf"]           = new() { Name = "Night Elf",           IsNeutral = false },
            ["Draenei"]             = new() { Name = "Draenei",             IsNeutral = false },
            ["Lightforged Draenei"] = new() { Name = "Lightforged Draenei", IsNeutral = false },
            ["Worgen"]              = new() { Name = "Worgen",              IsNeutral = false },
            ["Void Elf"]            = new() { Name = "Void Elf",            IsNeutral = false },
            ["Kul Tiran"]           = new() { Name = "Kul Tiran",           IsNeutral = false },
            ["Mechagnome"]          = new() { Name = "Mechagnome",          IsNeutral = false },
            ["Orc"]                 = new() { Name = "Orc",                 IsNeutral = false },
            ["Mag'har Orc"]         = new() { Name = "Mag'har Orc",         IsNeutral = false },
            ["Undead"]              = new() { Name = "Undead",              IsNeutral = false },
            ["Tauren"]              = new() { Name = "Tauren",              IsNeutral = false },
            ["Highmountain Tauren"] = new() { Name = "Highmountain Tauren", IsNeutral = false },
            ["Troll"]               = new() { Name = "Troll",               IsNeutral = false },
            ["Zandalari Troll"]     = new() { Name = "Zandalari Troll",     IsNeutral = false },
            ["Blood Elf"]           = new() { Name = "Blood Elf",           IsNeutral = false },
            ["Goblin"]              = new() { Name = "Goblin",              IsNeutral = false },
            ["Vulpera"]             = new() { Name = "Vulpera",             IsNeutral = false },
            ["Nightborne"]          = new() { Name = "Nightborne",          IsNeutral = false },
            ["Pandaren"]            = new() { Name = "Pandaren",            IsNeutral = true  },
            ["Dracthyr"]            = new() { Name = "Dracthyr",            IsNeutral = true  },
            ["Earthen"]             = new() { Name = "Earthen",             IsNeutral = true  }
        };

        var professions = new List<Profession>
        {
            new() { Name = "Alchemy",        IsPrimary = true  },
            new() { Name = "Blacksmithing",  IsPrimary = true  },
            new() { Name = "Leatherworking", IsPrimary = true  },
            new() { Name = "Tailoring",      IsPrimary = true  },
            new() { Name = "Engineering",    IsPrimary = true  },
            new() { Name = "Enchanting",     IsPrimary = true  },
            new() { Name = "Inscription",    IsPrimary = true  },
            new() { Name = "Jewelcrafting",  IsPrimary = true  },
            new() { Name = "Herbalism",      IsPrimary = true  },
            new() { Name = "Mining",         IsPrimary = true  },
            new() { Name = "Skinning",       IsPrimary = true  },
            new() { Name = "Cooking",        IsPrimary = false },
            new() { Name = "Fishing",        IsPrimary = false },
            new() { Name = "Archaeology",    IsPrimary = false }
        };

        var factionRaceMappings = new Dictionary<string, string[]>
        {
            ["Alliance"] = ["Human", "Dwarf", "Dark Iron Dwarf", "Gnome", "Night Elf", "Draenei",
                            "Lightforged Draenei", "Worgen", "Void Elf", "Kul Tiran", "Mechagnome",
                            "Pandaren", "Dracthyr", "Earthen"],
            ["Horde"]    = ["Orc", "Mag'har Orc", "Undead", "Tauren", "Highmountain Tauren", "Troll",
                            "Zandalari Troll", "Blood Elf", "Goblin", "Vulpera", "Nightborne",
                            "Pandaren", "Dracthyr", "Earthen"]
        };

        var raceClassMappings = new Dictionary<string, string[]>
        {
            ["Human"]               = ["Warrior", "Paladin", "Hunter", "Rogue", "Priest", "Mage", "Warlock", "Monk", "Death Knight"],
            ["Dwarf"]               = ["Warrior", "Paladin", "Hunter", "Rogue", "Priest", "Shaman", "Monk", "Death Knight"],
            ["Dark Iron Dwarf"]     = ["Warrior", "Paladin", "Hunter", "Rogue", "Priest", "Mage", "Warlock", "Monk", "Death Knight"],
            ["Gnome"]               = ["Warrior", "Hunter", "Rogue", "Priest", "Mage", "Warlock", "Monk", "Death Knight"],
            ["Night Elf"]           = ["Warrior", "Hunter", "Rogue", "Priest", "Druid", "Monk", "Demon Hunter", "Mage", "Death Knight"],
            ["Draenei"]             = ["Warrior", "Paladin", "Hunter", "Priest", "Shaman", "Mage", "Monk", "Death Knight"],
            ["Lightforged Draenei"] = ["Warrior", "Paladin", "Hunter", "Priest", "Mage", "Monk", "Death Knight"],
            ["Worgen"]              = ["Warrior", "Hunter", "Rogue", "Priest", "Druid", "Mage", "Warlock", "Monk", "Death Knight"],
            ["Void Elf"]            = ["Warrior", "Hunter", "Rogue", "Priest", "Mage", "Warlock", "Monk", "Death Knight"],
            ["Kul Tiran"]           = ["Warrior", "Hunter", "Rogue", "Priest", "Druid", "Monk", "Shaman", "Death Knight"],
            ["Mechagnome"]          = ["Warrior", "Hunter", "Rogue", "Priest", "Mage", "Warlock", "Monk", "Death Knight"],
            ["Orc"]                 = ["Warrior", "Hunter", "Rogue", "Shaman", "Warlock", "Monk", "Death Knight", "Mage"],
            ["Mag'har Orc"]         = ["Warrior", "Hunter", "Rogue", "Shaman", "Monk", "Death Knight"],
            ["Undead"]              = ["Warrior", "Hunter", "Rogue", "Priest", "Mage", "Warlock", "Monk", "Death Knight"],
            ["Tauren"]              = ["Warrior", "Paladin", "Hunter", "Druid", "Shaman", "Monk", "Priest", "Death Knight"],
            ["Highmountain Tauren"] = ["Warrior", "Hunter", "Druid", "Shaman", "Monk", "Death Knight"],
            ["Troll"]               = ["Warrior", "Hunter", "Rogue", "Priest", "Mage", "Warlock", "Shaman", "Monk", "Druid", "Death Knight"],
            ["Zandalari Troll"]     = ["Warrior", "Paladin", "Hunter", "Rogue", "Priest", "Mage", "Shaman", "Druid", "Monk", "Death Knight"],
            ["Blood Elf"]           = ["Warrior", "Paladin", "Hunter", "Rogue", "Priest", "Mage", "Warlock", "Monk", "Death Knight", "Demon Hunter"],
            ["Goblin"]              = ["Warrior", "Hunter", "Rogue", "Priest", "Mage", "Warlock", "Shaman", "Monk", "Death Knight"],
            ["Vulpera"]             = ["Warrior", "Hunter", "Rogue", "Priest", "Mage", "Warlock", "Shaman", "Monk", "Death Knight"],
            ["Nightborne"]          = ["Warrior", "Hunter", "Rogue", "Mage", "Priest", "Warlock", "Monk", "Death Knight"],
            ["Pandaren"]            = ["Warrior", "Hunter", "Rogue", "Priest", "Shaman", "Mage", "Monk", "Death Knight"],
            ["Dracthyr"]            = ["Warrior", "Hunter", "Mage", "Rogue", "Priest", "Warlock", "Evoker"],
            ["Earthen"]             = ["Warrior", "Paladin", "Hunter", "Rogue", "Priest", "Shaman", "Mage", "Warlock", "Monk"]
        };

        await context.Factions.AddRangeAsync(factions.Values);
        await context.Classes.AddRangeAsync(classes.Values);
        await context.Races.AddRangeAsync(races.Values);
        await context.Professions.AddRangeAsync(professions);
        await context.SaveChangesAsync();

        var factionRaces = factionRaceMappings
            .SelectMany(fr => fr.Value.Select(raceName => new FactionRace
            {
                FactionId = factions[fr.Key].Id,
                RaceId    = races[raceName].Id
            }));

        var raceClasses = raceClassMappings
            .SelectMany(rc => rc.Value.Select(className => new RaceClass
            {
                RaceId  = races[rc.Key].Id,
                ClassId = classes[className].Id
            }));

        await context.FactionRaces.AddRangeAsync(factionRaces);
        await context.RaceClasses.AddRangeAsync(raceClasses);
        await context.SaveChangesAsync();
    }
}
