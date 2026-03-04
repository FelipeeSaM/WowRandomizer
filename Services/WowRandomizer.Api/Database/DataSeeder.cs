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
            ["Alliance"] = new() { Id = 1, Name = "Alliance" },
            ["Horde"]    = new() { Id = 2, Name = "Horde" }
        };

        var classes = new Dictionary<string, GameClass>
        {
            ["Warrior"]       = new() { Id = 1,  Name = "Warrior" },
            ["Paladin"]       = new() { Id = 2,  Name = "Paladin" },
            ["Hunter"]        = new() { Id = 3,  Name = "Hunter" },
            ["Rogue"]         = new() { Id = 4,  Name = "Rogue" },
            ["Priest"]        = new() { Id = 5,  Name = "Priest" },
            ["Death Knight"]  = new() { Id = 6,  Name = "Death Knight" },
            ["Shaman"]        = new() { Id = 7,  Name = "Shaman" },
            ["Mage"]          = new() { Id = 8,  Name = "Mage" },
            ["Warlock"]       = new() { Id = 9,  Name = "Warlock" },
            ["Monk"]          = new() { Id = 10, Name = "Monk" },
            ["Druid"]         = new() { Id = 11, Name = "Druid" },
            ["Demon Hunter"]  = new() { Id = 12, Name = "Demon Hunter" },
            ["Evoker"]        = new() { Id = 13, Name = "Evoker" }
        };

        var races = new Dictionary<string, Race>
        {
            ["Human"]               = new() { Id = 1,  Name = "Human",               IsNeutral = false },
            ["Dwarf"]               = new() { Id = 2,  Name = "Dwarf",               IsNeutral = false },
            ["Dark Iron Dwarf"]     = new() { Id = 3,  Name = "Dark Iron Dwarf",     IsNeutral = false },
            ["Gnome"]               = new() { Id = 4,  Name = "Gnome",               IsNeutral = false },
            ["Night Elf"]           = new() { Id = 5,  Name = "Night Elf",           IsNeutral = false },
            ["Draenei"]             = new() { Id = 6,  Name = "Draenei",             IsNeutral = false },
            ["Lightforged Draenei"] = new() { Id = 7,  Name = "Lightforged Draenei", IsNeutral = false },
            ["Worgen"]              = new() { Id = 8,  Name = "Worgen",              IsNeutral = false },
            ["Void Elf"]            = new() { Id = 9,  Name = "Void Elf",            IsNeutral = false },
            ["Kul Tiran"]           = new() { Id = 10, Name = "Kul Tiran",           IsNeutral = false },
            ["Mechagnome"]          = new() { Id = 11, Name = "Mechagnome",          IsNeutral = false },
            ["Orc"]                 = new() { Id = 12, Name = "Orc",                 IsNeutral = false },
            ["Mag'har Orc"]         = new() { Id = 13, Name = "Mag'har Orc",         IsNeutral = false },
            ["Undead"]              = new() { Id = 14, Name = "Undead",              IsNeutral = false },
            ["Tauren"]              = new() { Id = 15, Name = "Tauren",              IsNeutral = false },
            ["Highmountain Tauren"] = new() { Id = 16, Name = "Highmountain Tauren", IsNeutral = false },
            ["Troll"]               = new() { Id = 17, Name = "Troll",               IsNeutral = false },
            ["Zandalari Troll"]     = new() { Id = 18, Name = "Zandalari Troll",     IsNeutral = false },
            ["Blood Elf"]           = new() { Id = 19, Name = "Blood Elf",           IsNeutral = false },
            ["Goblin"]              = new() { Id = 20, Name = "Goblin",              IsNeutral = false },
            ["Vulpera"]             = new() { Id = 21, Name = "Vulpera",             IsNeutral = false },
            ["Nightborne"]          = new() { Id = 22, Name = "Nightborne",          IsNeutral = false },
            ["Pandaren"]            = new() { Id = 23, Name = "Pandaren",            IsNeutral = true  },
            ["Dracthyr"]            = new() { Id = 24, Name = "Dracthyr",            IsNeutral = true  },
            ["Earthen"]             = new() { Id = 25, Name = "Earthen",             IsNeutral = true  }
        };

        var professions = new List<Profession>
        {
            new() { Id = 1,  Name = "Alchemy",        IsPrimary = true  },
            new() { Id = 2,  Name = "Blacksmithing",  IsPrimary = true  },
            new() { Id = 3,  Name = "Leatherworking", IsPrimary = true  },
            new() { Id = 4,  Name = "Tailoring",      IsPrimary = true  },
            new() { Id = 5,  Name = "Engineering",    IsPrimary = true  },
            new() { Id = 6,  Name = "Enchanting",     IsPrimary = true  },
            new() { Id = 7,  Name = "Inscription",    IsPrimary = true  },
            new() { Id = 8,  Name = "Jewelcrafting",  IsPrimary = true  },
            new() { Id = 9,  Name = "Herbalism",      IsPrimary = true  },
            new() { Id = 10, Name = "Mining",         IsPrimary = true  },
            new() { Id = 11, Name = "Skinning",       IsPrimary = true  },
            new() { Id = 12, Name = "Cooking",        IsPrimary = false },
            new() { Id = 13, Name = "Fishing",        IsPrimary = false },
            new() { Id = 14, Name = "Archaeology",    IsPrimary = false }
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
