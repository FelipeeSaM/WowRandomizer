/**
 * Retorna emoji da raça
 * @param raceName - Nome da raça
 * @returns Emoji correspondente
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
  // Normaliza nomes compostos para facilitar o match
  const key = Object.keys(raceEmojis).find(k => normalized === k || normalized.replace(/[- ]/g, '') === k.replace(/[- ]/g, ''))
  return key ? raceEmojis[key] : '🎭'
}
/**
 * Helper functions para obter URLs de imagens de facções e classes
 */

/**
 * Retorna a URL do ícone da facção
 * @param faction - Nome da facção (ex: "Alliance", "Horde")
 * @returns URL do ícone SVG
 */
export function getFactionIcon(faction: string): string {
  const normalized = faction.toLowerCase().replace(/\s+/g, '-')
  return `/images/factions/${normalized}.png`
}

/**
 * Retorna a URL do ícone da profissão
 * @param professionName - Nome da profissão (ex: "Mining", "Blacksmithing")
 * @returns URL do ícone PNG/SVG
 */
export function getProfessionIcon(professionName: string): string {
  const normalized = professionName.toLowerCase().replace(/\s+/g, '-')
  return `/images/professions/${normalized}.png`
}

/**
 * Retorna a URL do ícone da classe
 * @param className - Nome da classe (ex: "Warrior", "Death Knight")
 * @returns URL do ícone SVG
 */
export function getClassIcon(className: string): string {
  const normalized = className.toLowerCase().replace(/\s+/g, '-')
  return `/images/classes/${normalized}.png`
}

/**
 * Retorna fallback de emoji caso a imagem não carregue
 * @param faction - Nome da facção
 * @returns Emoji correspondente
 */
export function getFactionEmoji(faction: string): string {
  const normalized = faction.toLowerCase()
  if (normalized.includes('alliance')) return '🦁'
  if (normalized.includes('horde')) return '🐺'
  return '⚔️'
}

/**
 * Retorna emoji da classe
 * @param className - Nome da classe
 * @returns Emoji correspondente
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
 * Retorna emoji da profissão
 * @param professionName - Nome da profissão
 * @returns Emoji correspondente
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