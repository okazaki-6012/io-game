# io game template
GameEngine: phaser
Langage: typescript
build:
- webpack(webpack-cli): bundler
- typescript(ts-loader): trans compiler (typescript > javascript)
- webpack-dev-server: development server
- html-webpack-plugin: html build