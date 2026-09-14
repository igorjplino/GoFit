/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./projects/admin/src/**/*.{html,ts}",
    "./projects/athlete/src/**/*.{html,ts}",
    "./src/shared/**/*.{html,ts}",
  ],
  theme: {
    extend: {
      colors: {
        primary: '#7d00fa',
      },
      screens: {
        'xs': '320px',
      },
    },
  },
  plugins: [],
}