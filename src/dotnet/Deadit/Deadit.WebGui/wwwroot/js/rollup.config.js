/************************************************************************** 

This is the Rollup.JS configuration file.

To get the typescript plugin going:

    cd into the wwwroot/js directory where this file is:

    npm install tslib --save-dev
    npm install typescript --save-dev
    npm install @rollup/plugin-typescript --save-dev
    npm install @types/bootstrap --save-dev 
    npm install marked --save-dev

***************************************************************************/

import typescript from '@rollup/plugin-typescript';

class RollupConfig
{
    constructor(input, output) {
        this.input = input;

        this.external = ['bootstrap', 'marked'];

        this.output = {
            // format: 'es',
            compact: true,
            sourcemap: true,
            file: output,
            inlineDynamicImports: true,
            interop: "auto",
            format: 'iife',

            globals: {
                bootstrap: 'bootstrap',
                marked: 'marked',
            },
        }

        this.plugins = [typescript()];
    }
}



const configs = [    
    new RollupConfig('ts/custom/pages/home/index.ts', 'dist/home.bundle.js'),
    new RollupConfig('ts/custom/pages/login/index.ts', 'dist/login.bundle.js'),
    new RollupConfig('ts/custom/pages/community/community-page/index.ts', 'dist/community-page.bundle.js'),
    new RollupConfig('ts/custom/pages/community/create-post/index.ts', 'dist/create-post-page.bundle.js'),
    new RollupConfig('ts/custom/pages/community/post-page/index.ts', 'dist/post-page.bundle.js'),
    new RollupConfig('ts/custom/pages/communities/communities-page/index.ts', 'dist/communities-page.bundle.js'),
    new RollupConfig('ts/custom/pages/communities/create-community/index.ts', 'dist/create-community.bundle.js'),


    new RollupConfig('ts/custom/pages/community/settings/general/index.ts', 'dist/community-settings-general.bundle.js'),
    new RollupConfig('ts/custom/pages/community/settings/content/index.ts', 'dist/community-settings-content.bundle.js'),
    new RollupConfig('ts/custom/pages/community/settings/members/index.ts', 'dist/community-settings-members.bundle.js'),
];



// rollup.config.js
export default configs;