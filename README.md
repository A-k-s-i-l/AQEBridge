# AQEBridge
---
This program is for communication between BonedMod and intiface. Currently are supported only vibrators.

**This tool/mod works only in "campaign". So it cannont be used in gallery.**

Since original BonedMod is using old .NET, I wasn't able to fit this bridge into it (due to technical reasons). So that is the reason for this program being separate.



---
# How to install
1. If not installed, install BonedMod and bepinex.
2. Replace BonedMod in `BepInEx\plugins` with modified BonedMod in releases. *(I only added communication to this bridge, otherwise it's same as v6b)*
3. Done
---
# How to use
1. Start intiface
2. Start AQE(game) and load save
3. Start AQEBridge

If your game is not active or if you are not in game(loaded save) for some time, intiface will disconnect from AQEBridge because Bridge wasn't sending any data and so it closed their connection. In this case, simply restart bridge and it will reconnect.
Enjoy...


---

### **THIS IS NOT OFFICIAL AND BEFORE USING BACKUP YOUR GAME!**
 *It shouldn't break your game or something, but just in case.*
