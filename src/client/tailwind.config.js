/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{html,ts}",
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