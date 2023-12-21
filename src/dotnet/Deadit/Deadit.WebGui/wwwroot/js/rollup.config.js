/************************************************************************** 

This is the Rollup.JS configuration file.

***************************************************************************/

class RollupConfig
{
    constructor(input, output) {
        this.input = input;

        this.output = {
            format: 'iife',
            compact: true,
            sourcemap: true,
            file: output,
        }
    }
}





const configs = [
    new RollupConfig('ts/out/pages/home/index.js', 'dist/home.bundle.js'),
    // new RollupConfig('custom/pages/home/index.js', 'dist/home.bundle.js'),
    // new RollupConfig('custom/pages/account/index.js', 'dist/account.bundle.js'),
    // new RollupConfig('custom/pages/labels/index.js', 'dist/labels.bundle.js'),
    // new RollupConfig('custom/pages/checklists/index.js', 'dist/checklists.bundle.js'),
    // new RollupConfig('custom/pages/checklist-settings/general/index.js', 'dist/checklist-settings.general.bundle.js'),
];



// rollup.config.js
export default configs;