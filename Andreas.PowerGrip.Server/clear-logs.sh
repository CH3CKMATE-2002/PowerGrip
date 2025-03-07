#!/bin/bash
# Author: Andreas Hanna

# Fetches the script location:
DIR="$(dirname "$(readlink -f "${BASH_SOURCE[0]}")")";

rm -rf "$DIR/logs";