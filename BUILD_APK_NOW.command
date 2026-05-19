#!/bin/bash
# Double-click this file in Finder to build the APK.
# IMPORTANT: Close Unity Editor completely before running.

set -euo pipefail
PROJECT="$(cd "$(dirname "$0")" && pwd)"
UNITY="/Applications/Unity/Hub/Editor/6000.4.7f1/Unity.app/Contents/MacOS/Unity"
APK="$PROJECT/Builds/Android/VirtualEyeClinic.apk"
LOG="$PROJECT/Builds/android-build.log"

if [[ ! -x "$UNITY" ]]; then
  osascript -e 'display alert "Unity not found" message "Install Unity 6000.4.7f1 with Android Build Support."'
  exit 1
fi

osascript -e 'display notification "Closing Unity if open..." with title "Virtual Eye Clinic"'
osascript -e 'tell application "Unity" to quit' 2>/dev/null || true
sleep 4
killall Unity 2>/dev/null || true
sleep 2
rm -f "$PROJECT/Temp/UnityLockfile" 2>/dev/null || true

mkdir -p "$PROJECT/Builds"

echo "Building APK... (5-15 minutes first time)"
"$UNITY" \
  -batchmode \
  -nographics \
  -quit \
  -projectPath "$PROJECT" \
  -executeMethod VirtualEyeClinic.Editor.BuildCi.BuildAndroid \
  -logFile "$LOG"

if [[ -f "$APK" ]]; then
  open -R "$APK"
  osascript -e "display alert \"APK ready!\" message \"Saved to:\n$APK\""
else
  open "$LOG"
  osascript -e 'display alert "Build failed" message "Log file opened. Send the last red errors for help."'
  exit 1
fi
