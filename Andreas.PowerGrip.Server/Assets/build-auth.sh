#!/usr/bin/env bash
# ╔═════════════════╦════════════════════════════════════════════════════════════╗
# ║ Author          ║ CH3CKMATE-2002 (Andreas Hanna)                             ║
# ╠═════════════════╬════════════════════════════════════════════════════════════╣
# ║ Contributors    ║ Nobody                                                     ║
# ╚═════════════════╩════════════════════════════════════════════════════════════╝

# ══════════════════════════════════════╗
# ║ Script Metadata                     ║
# ══════════════════════════════════════╝
SCRIPT_PATH="$(readlink -f "$0")"                             # Full path of the script.
PARENT_DIR="$(dirname "${SCRIPT_PATH}")"                      # Parent directory of the script.
SCRIPT_NAME="$(basename "${SCRIPT_PATH}" | cut -d '.' -f 1)"  # Name of the script (without extension, regardless what it is).
SCRIPT_AUTHOR='CH3CKMATE-2002 (Andreas Hanna)'

TARGET_NAME='pam_helper'

# Tests if a command exists or not
function command_exists() {
    command -v "$@" &> '/dev/null'
}

cd "$PARENT_DIR" || { echo "Failure at changing directory to $PARENT_DIR"; exit 1; }


echo "$SCRIPT_NAME by $SCRIPT_AUTHOR"

if command_exists 'gcc'; then
    gcc -o "$TARGET_NAME" "${TARGET_NAME}.c" -lpam
    # Change ownership to root and set the setuid bit to
    # run as root without needing a password!
    sudo chown root:root "$TARGET_NAME"
    sudo chmod u+s "$TARGET_NAME"
else
    echo "You must install gcc to compile the pam_auth"
    exit 1
fi