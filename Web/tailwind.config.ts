/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{vue,js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      colors: {
        // Cores de World of Warcraft
        alliance: {
          DEFAULT: '#0078FF',
          light: '#4D9FFF',
          dark: '#003E82',
        },
        horde: {
          DEFAULT: '#B30000',
          light: '#E03E3E',
          dark: '#7A0000',
        },
      },
      fontFamily: {
        sans: ['system-ui', 'Segoe UI', 'Roboto', 'sans-serif'],
        heading: ['system-ui', 'Segoe UI', 'Roboto', 'sans-serif'],
      },
    },
  },
}
