const path = require('path')
const HtmlWebpackPlugin = require('html-webpack-plugin')

var config = {
  entry: {
    app: [
      path.resolve(__dirname, 'src/main.ts')
    ],
  },
  output: {
    pathinfo: true,
    path: path.resolve(__dirname, 'dist'),
    publicPath: './',
    filename: '[name].js'
  },
  module: {
    rules: [
      {
        test: /\.ts$/,
        use: 'ts-loader'
      }
    ]
  },
  plugins: [
    new HtmlWebpackPlugin({
      template: './src/index.html',
    }),
  ]
}
module.exports = (env, argv) => {
  console.log(argv.mode)
  if (argv.mode === 'development') {
    config.devServer = {
      contentBase: path.join(__dirname, 'dist'),
      compress: true,
      port: 3000
    }
  }
  return config
}
