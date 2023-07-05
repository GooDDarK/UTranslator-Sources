@echo off
for %%a in (resources.assets;) do UPacker.exe export "%%a" -mb_new -t LanguageSourceAsset
