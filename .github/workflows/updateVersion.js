const path = require("path");
const fs = require("fs");

let packageDir = __dirname;

// Get package.json directory by trying the directory tree
while (!fs.existsSync(path.resolve(packageDir, "package.json"))) {
    packageDir = path.resolve(packageDir, "..");
    if (packageDir == "/") {
        throw new Error("No package.json found in directory tree.");
    }
}

let packageJson = JSON.parse(fs.readFileSync(path.resolve(packageDir, "package.json")));

packageJson.version = require("git-tag-version")({
    uniqueSnapshot: true
});


console.log(`Updating ${path.resolve(packageDir, "package.json")} version to ${packageJson.version}.`);
fs.writeFileSync(path.join(packageDir, "package.json"), JSON.stringify(packageJson, 4));
