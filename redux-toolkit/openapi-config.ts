import type { ConfigFile } from "@rtk-query/codegen-openapi";

const config: ConfigFile = {
    schemaFile: "../openapi.yaml",
    apiFile: "../build/ts/emptyApi.ts",
    apiImport: "emptySplitApi",
    outputFile: "../build/ts/redux.ts",
    exportName: "ridesaverAPI",
    hooks: true,
};

export default config;
