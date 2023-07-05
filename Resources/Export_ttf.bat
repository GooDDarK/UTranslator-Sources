@echo off
for %%a in (sharedassets0.assets;) do UPacker.exe export "%%a" -mb_new -t ttf
