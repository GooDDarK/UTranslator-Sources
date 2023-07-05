@echo off
for %%a in (*.LanguageSource;*.LanguageSourceAsset;) do Parser_ULS.exe -i "%%a" -o $0c -dauto
