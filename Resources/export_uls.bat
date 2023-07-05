@echo off
for %%a in (*.LanguageSource;*.LanguageSourceAsset;) do Parser_ULS.exe -e "%%a" -o $0c -dauto -ndesc
