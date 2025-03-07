#!/bin/bash
# Author: Andreas Hanna

# Fetches the script location:
DIR="$(dirname "$(readlink -f "${BASH_SOURCE[0]}")")";

"$DIR/bin/Debug/net8.0/power-grip";