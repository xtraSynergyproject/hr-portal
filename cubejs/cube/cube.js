// Cube.js configuration options: https://cube.dev/docs/config
const fetch = require('node-fetch');
module.exports = {
    schemaVersion: async ({ securityContext }) => {
        const schemaVersions = await (
            await fetch('https://webapidev.aitalkx.com/common/query/GetLastUpdatedSynergySchemaDate')
        ).json();

        return schemaVersions;
    },
};