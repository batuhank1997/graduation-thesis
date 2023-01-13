#!/bin/bash

longhorn_registry_login() {
    npm config set registry https://registry.npmjs.org

    npm install -g npm-cli-adduser
    npm-cli-adduser --registry http://registry.longhorn.games --username longhorn --password longhorn --email hello@longhorn.games
}

longhorn_registry_login