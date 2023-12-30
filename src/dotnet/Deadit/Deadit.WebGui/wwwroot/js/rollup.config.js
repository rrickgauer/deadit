/************************************************************************** 

This is the Rollup.JS configuration file.

To get the typescript plugin going:

    cd into the wwwroot/js directory where this file is:

    npm install tslib --save-dev
    npm install typescript --save-dev
    npm install @rollup/plugin-typescript --save-dev

***************************************************************************/

import typescript from '@rollup/plugin-typescript';

class RollupConfig
{
    constructor(input, output) {
        this.input = input;

        this.output = {
            format: 'es',
            compact: true,
            sourcemap: true,
            file: output,
        }

        this.plugins = [typescript()];
    }
}



const configs = [    
    new RollupConfig('ts/custom/pages/home/index.ts', 'dist/home.bundle.js'),
    new RollupConfig('ts/custom/pages/communities/communities-page/index.ts', 'dist/communities-page.bundle.js'),
    new RollupConfig('ts/custom/pages/communities/create-community/index.ts', 'dist/create-community.bundle.js'),
];



// rollup.config.js
export default configs;