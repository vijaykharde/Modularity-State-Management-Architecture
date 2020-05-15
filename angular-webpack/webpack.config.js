var path = require('path');
var UglifyJsPlugin = require('uglifyjs-webpack-plugin');
var CopyWebpackPlugin = require('copy-webpack-plugin');
var CleanWebpackPlugin = require('clean-webpack-plugin');

console.log('@@@@@@@@@ USING DEVELOPMENT @@@@@@@@@@@@@@@');

module.exports = {
  /*devServer: {
    contentBase: path.join(__dirname, 'dist'),
    compress: true,
    port: 9000
  },*/
  mode: 'development',
  devtool: 'source-map',
  performance: {
    hints: false
  },
  // this is where we add new Angular applications that can be rooted in an MVC view.
  // there needs to be a corresponding typescript file in the root of /ClientApp,
  // and then a corresponding folder in /ClientApp for that root module.  (see Access below)
  // Also, change the webpack.config.prod.js to use the -aot.ts version.  (Ahead-of-time Angular)
  // ALSO!!! Further below your new root module needs added (look for "access" further below)
  /*entry: {
      'wwwroot/dist/polyfills': './ClientApp/ngCompile/polyfills.ts',
      'wwwroot/dist/vendor': './ClientApp/ngCompile/vendor.ts',
      'wwwroot/dist/login': './ClientApp/ngCompile/Login.ts',
      'Protected/js/main': './ClientApp/ngCompile/Main.ts'
  },*/
  entry: {
    'wwwroot/dist/polyfills': './src/polyfills.ts',
    //'vendor': './ClientApp/ngCompile/vendor.ts',
    //'login': './ClientApp/ngCompile/Login.ts',
    'wwwroot/dist/main': './src/Main.ts'
  },
  /*
  entry: {
      'polyfills': './ClientApp/ngCompile/polyfills.ts',
      'vendor': './ClientApp/ngCompile/vendor.ts',
      'login': './ClientApp/ngCompile/Login.ts',
      'main': './ClientApp/ngCompile/Main.ts'
  },
  
  output: {
      path: __dirname + '/wwwroot/',
      filename: 'dist/[name].bundle.js',
      chunkFilename: 'dist/[name].[id].chunk.js',
      publicPath: '/'
  }, */
  output: {
    path: __dirname,
    filename: '[name].bundle.js',
    chunkFilename: '[id].chunk.js',
    publicPath: '/'
  },

  /*output: {
      path: __dirname,
      filename: '[name].bundle.js',
      chunkFilename: '[id].chunk.js',
      publicPath: '/'
  },*/

  optimization: {
    minimizer: [
      // we specify a custom UglifyJsPlugin here to get source maps in production
      new UglifyJsPlugin({
        cache: true,
        parallel: true,
        uglifyOptions: {
          compress: true,
          ecma: 5,
          mangle: false
        },
        sourceMap: true,
        extractComments: true
      })
    ],
    splitChunks: {
      chunks: "async",
      minSize: 30000,
      minChunks: 1,
      maxAsyncRequests: 5,
      maxInitialRequests: 3,
      automaticNameDelimiter: '~',
      name: true,
      cacheGroups: {
        vendors: {
          test: /[\\/]node_modules[\\/]/,
          priority: -10
        },
        default: {
          minChunks: 2,
          priority: -20,
          reuseExistingChunk: true
        }
      }
    }
  },

  resolve: {
    extensions: ['.ts', '.js', '.json', '.css', '.scss', '.html']/*,
        alias: {
            // Force all modules to use the same jquery version.
            'jquery': path.join(__dirname, 'node_modules/jquery/src/jquery'),
            'jqueryui': path.join(__dirname, 'node_modules/jqueryui'),  
            '@tecommon': path.join(__dirname, 'ClientApp/@tecommon')
        }*/
  },

  devServer: {
    historyApiFallback: true,
    contentBase: path.join(__dirname, '/wwwroot/dist/'),
    watchOptions: {
      aggregateTimeout: 300,
      poll: 1000
    }
  },

  module: {
    rules: [
      {
        test: /\.ts$/,
        loaders: [
          'awesome-typescript-loader',
          'angular-router-loader',
          'angular2-template-loader',
          'source-map-loader'
          //'ngtemplate-loader'
        ]
      },
      {
        test: /\.(png|jpg|gif|woff|woff2|ttf|svg|eot)$/,
        loader: 'file-loader?name=wwwroot/dist/assets/[name]-[hash:6].[ext]'   // watch for this when changing paths!
      },
      {
        test: /favicon.ico$/,
        loader: 'file-loader?name=wwwroot/[name].[ext]'           // also this one ^
      },
      {
        test: /\.css$/,
        loader: 'style-loader!css-loader!to-string-loader'
      },
      {
        test: /\.scss$/,
        exclude: /node_modules/,
        loaders: ['style-loader', 'css-loader', 'sass-loader', 'to-string-loader']
      },
      /*{
        test: /\.html$/i,
        use: ['file-loader?name=[name].[ext]', 'extract-loader', 'html-loader'],
      },*/
      {
        test: /\.html$/,
        loader: 'html-loader'
      }
    ],
    exprContextCritical: false
  },
  plugins: [
    // new webpack.optimize.CommonsChunkPlugin(
    //     {
    //         names: ['vendor', 'polyfills', 'manifest']
    //     }),
    /*
            new CleanWebpackPlugin([
                    './dist/*',
                    './Protected/*'
                ]
            ),
    
            new CopyWebpackPlugin([
                { from: './ClientApp/resources/*.*', to: './wwwroot/dist/assets/', flatten: true },
                { from: './ClientApp/resources/images/*.*', to: './wwwroot/dist/assets/', flatten: true },
                { from: './ClientApp/resources/help/FAQ/*.*', to: './Protected/FAQ', flatten: true },
                { from: './ClientApp/resources/help/MDE/*.*', to: './Protected/MDE', flatten: true },
                { from: './ClientApp/resources/i18n/Login/*.*', to: './wwwroot/dist/assets/i18n', flatten: true },
                { from: './ClientApp/resources/i18n/Main/*.*', to: './Protected/i18n', flatten: true },
                { from: './node_modules/material-design-icons/iconfont/*.*', to: './wwwroot/dist/assets/fonts', flatten: true }
            ])*/
  ]

};

