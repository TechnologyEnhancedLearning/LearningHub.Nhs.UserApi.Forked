console.log("Running webpack build...")

const path = require('path');

module.exports = {
    entry: {
        nhsuk: './Scripts/nhsuk.ts',
    },
    output: {
        path: path.resolve(__dirname, './wwwroot/js/bundle/'),
        publicPath: '/wwwroot/js/bundle/',
        filename: '[name].js'
    },
    resolve: {
        extensions: ['.ts', '.js', '.cjs', '.json'],
        alias: {
            '@': path.join(__dirname)
        }
    },
    module: {
        rules: [
            {
                test: /\.ts$/,
                loader: 'ts-loader',
                exclude: /node_modules/,
            },
            {
                test: /\.(js|cjs)$/,
                loader: 'babel-loader',
                exclude: function (path) {
                    if (path.indexOf("node_modules") > -1) {
                        return true;
                    }
                    console.log(path);
                    return false;
                },
                include: [
                    path.resolve('node_modules/nhsuk-frontend'),
                ],
            },
            {
                test: /\.scss$/,
                use: [
                    {
                        loader: 'style-loader'
                    },
                    {
                        loader: 'css-loader',
                        options: {
                            url: false,
                        },
                    },
                    {
                        loader: 'sass-loader',
                        options: {
                            implementation: require("sass"),
                        },
                    },
                ],
            },
            {
                test: /\.css$/,
                use: [
                    {
                        loader: 'style-loader'
                    },
                    {
                        loader: 'css-loader'
                    },
                ],
            },
            {
                test: /\.(eot|svg|ttf|woff|woff2)(\?\S*)?$/,
                loader: 'file-loader'
            },
            {
                test: /\.(png|jpe?g|gif|svg)(\?\S*)?$/,
                loader: 'file-loader',
                options: {
                    name: '[name].[ext]?[hash]'
                }
            }
        ]
    },
    devServer: {
        proxy: {
            '*': {
                target: 'http://localhost:53306',
                changeOrigin: true
            }
        }
    },
};

if (process.env.NODE_ENV === 'production') {
    module.exports.mode = 'production';
} else {
    module.exports.mode = 'development';
    module.exports.devtool = 'source-map';
}