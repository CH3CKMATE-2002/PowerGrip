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
echo " - Copy $SERVICE_NAME to $SERVICE_PATH"
echo " - Reload systemd daemons"
echo " - Create a new group $APP_GROUP_NAME"
echo " - Add you ($USER) to the $APP_GROUP_NAME group"

echo "Copying $SERVICE_NAME file..."
sudo cp "$SERVICE_NAME" "${SERVICE_PATH}/${SERVICE_NAME}"

echo "Reloading daemons..."
sudo systemctl daemon-reload

echo "Creating a new group ($APP_GROUP_NAME)..."
sudo groupadd "$APP_GROUP_NAME"

echo "Adding yourself ($USER) to the newly created group..."
sudo usermod -aG "$APP_GROUP_NAME" "$USER"

echo "Just finished! Enjoy PowerGrip!"
echo "You may need to restart the system to make"
echo "the changes take effect"