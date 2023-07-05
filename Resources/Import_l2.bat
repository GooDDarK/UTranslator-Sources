@echo off
for %%a in (resources.assets;) do UPacker.exe import "%%a" -mb_new -t 114
