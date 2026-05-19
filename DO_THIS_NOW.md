# Build APK — 2 options (pick one)

Your scripts are already inside this project:  
`Desktop/VirtualEyeClinic_Source/VirtualEyeClinic_Source`

---

## Option A — Easiest (double-click)

1. **Close Unity completely** (Unity → Quit, or Cmd+Q)
2. In Finder, open folder `VirtualEyeClinic_Source` (the inner one)
3. **Double-click** `BUILD_APK_NOW.command`
4. If macOS blocks it: Right-click → **Open** → **Open**
5. Wait 5–15 minutes
6. APK appears at: `Builds/Android/VirtualEyeClinic.apk`

---

## Option B — Inside Unity (if batch build fails)

1. Open this project in Unity Hub
2. Top menu: **Virtual Eye Clinic → 1. Create Scenes And Build Settings**
3. **File → Build Profiles** → **Android** → **Switch Platform**
4. **Virtual Eye Clinic → 3. Build Android APK**

---

## Before building (Android module)

Unity Hub → **Installs** → Unity 6000.4.7f1 → gear → **Add modules** → enable:
- Android Build Support
- Android SDK & NDK Tools
- OpenJDK

---

## Test in Editor first

1. **Virtual Eye Clinic → 1. Create Scenes And Build Settings**
2. Open `Assets/Scenes/ClinicScene`
3. Press **Play**
