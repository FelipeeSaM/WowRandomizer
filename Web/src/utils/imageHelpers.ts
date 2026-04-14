/**
 * Returns the race emoji
 * @param raceName - Race name
 * @returns Corresponding emoji
 */
export function getRaceEmoji(raceName: string): string {
  const normalized = raceName.toLowerCase()
  const raceEmojis: Record<string, string> = {
    'human': '🧑',
    'orc': '👹',
    'dwarf': '🧔',
    'night elf': '🌙',
    'undead': '🧟',
    'tauren': '🐮',
    'gnome': '🤖',
    'troll': '🧞',
    'goblin': '🤑',
    'blood elf': '🧝',
    'draenei': '👽',
    'worgen': '🐺',
    'pandaren': '🐼',
    'void elf': '🌀',
    'nightborne': '🌌',
    'highmountain tauren': '🦌',
    'lightforged draenei': '💡',
    'zandalari troll': '🦎',
    'kul tiran': '⚓',
    'dark iron dwarf': '🔥',
    'maghar orc': '🪓',
    'mechagnome': '🦾',
    'vulpera': '🦊',
    'dracthyr': '🐲',
  }
  // Normalizes compound names to facilitate matching
  const key = Object.keys(raceEmojis).find(k => normalized === k || normalized.replace(/[- ]/g, '') === k.replace(/[- ]/g, ''))
  return key ? raceEmojis[key] : '🎭'
}
/**
 * Helper functions to get faction and class image URLs
 */

/**
 * Returns the faction icon URL
 * @param faction - Faction name (e.g., "Alliance", "Horde")
 * @returns SVG icon URL
 */
export function getFactionIcon(faction: string): string {
  const normalized = faction.toLowerCase().replace(/\s+/g, '-')
  return `/images/factions/${normalized}.png`
}

/**
 * Returns the profession icon URL
 * @param professionName - Profession name (e.g., "Mining", "Blacksmithing")
 * @returns PNG/SVG icon URL
 */
export function getProfessionIcon(professionName: string): string {
  const normalized = professionName.toLowerCase().replace(/\s+/g, '-')
  return `/images/professions/${normalized}.png`
}

/**
 * Returns the class icon URL
 * @param className - Class name (e.g., "Warrior", "Death Knight")
 * @returns SVG icon URL
 */
export function getClassIcon(className: string): string {
  const normalized = className.toLowerCase().replace(/\s+/g, '-')
  return `/images/classes/${normalized}.png`
}

/**
 * Returns emoji fallback in case the image fails to load
 * @param faction - Faction name
 * @returns Corresponding emoji
 */
export function getFactionEmoji(faction: string): string {
  const normalized = faction.toLowerCase()
  if (normalized.includes('alliance')) return '🦁'
  if (normalized.includes('horde')) return '🐺'
  return '⚔️'
}

/**
 * Returns the class emoji
 * @param className - Class name
 * @returns Corresponding emoji
 */
export function getClassEmoji(className: string): string {
  const normalized = className.toLowerCase()
  const classEmojis: Record<string, string> = {
    'warrior': '⚔️',
    'paladin': '🛡️',
    'hunter': '🏹',
    'rogue': '🗡️',
    'priest': '✨',
    'shaman': '⚡',
    'mage': '🔮',
    'warlock': '🔥',
    'monk': '🥋',
    'druid': '🌿',
    'demon-hunter': '😈',
    'death-knight': '💀',
  }
  return classEmojis[normalized] || '⚔️'
}

/**
 * Returns the profession emoji
 * @param professionName - Profession name
 * @returns Corresponding emoji
 */
export function getProfessionEmoji(professionName: string): string {
  const normalized = professionName.toLowerCase()
  const professionEmojis: Record<string, string> = {
    'mining': '⛏️',
    'blacksmithing': '⚒️',
    'alchemy': '⚗️',
    'herbalism': '🌿',
    'skinning': '🔪',
    'leatherworking': '👞',
    'enchanting': '✨',
    'engineering': '🔧',
    'inscription': '🖋️',
    'jewelcrafting': '💎',
    'tailoring': '🧵',
    'fishing': '🎣',
    'cooking': '🍳',
    'first-aid': '⛑️',
  }
  return professionEmojis[normalized] || '🎯'
}