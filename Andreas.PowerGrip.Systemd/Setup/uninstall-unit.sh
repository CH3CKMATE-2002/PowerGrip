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

# Tests if a command exists or not
function command_exists() {
    command -v "$@" &> '/dev/null'
}

echo "PowerGrip .service unit setup (${SCRIPT_NAME}) by $SCRIPT_AUTHOR"

cd "$PARENT_DIR" || { echo "Failed to change directory to: $PARENT_DIR" >&2 ; exit 1; }

SERVICE_PATH='/etc/systemd/system'
SERVICE_NAME='power-grip.service'

echo "Stopping $SERVICE_NAME"
sudo systemctl stop "$SERVICE_NAME"

echo "Removing $SERVICE_NAME file"
sudo rm "${SERVICE_PATH}/${SERVICE_NAME}"

sudo systemctl daemon-reload

# ══════════════════════════════════════╗
# ║ Script Variables                    ║
# ══════════════════════════════════════╝

SERVICE_PATH='/etc/systemd/system'
SERVICE_NAME='power-grip.service'
APP_GROUP_NAME='powergrip'

# ══════════════════════════════════════╗
# ║ Script Logic                        ║
# ══════════════════════════════════════╝

echo "This script will ask you for your"
echo "sudo password to do the following operations:"
echo " - Delete $SERVICE_NAME from $SERVICE_PATH"
echo " - Reload systemd daemons"
echo " - Remove you ($USER) from the $APP_GROUP_NAME group"
echo " - Deletes the $APP_GROUP_NAME group"

echo "Deleting $SERVICE_NAME file..."
sudo rm -f "${SERVICE_PATH}/${SERVICE_NAME}"

echo "Reloading daemons..."
sudo systemctl daemon-reload

echo "Removing yourself ($USER) from $APP_GROUP_NAME group..."
sudo usermod -aG "$APP_GROUP_NAME" "$USER"

echo "Creating a new group ($APP_GROUP_NAME)..."
sudo groupdel "$APP_GROUP_NAME"

echo "Just finished! Goodbye :'("
echo "You may need to restart the system to make"
echo "the changes take effect"