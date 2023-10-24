@echo off
color a
for %%a in (resources.assets;) do UPacker_Advanced.exe export "%%a" -mb_new -t .*font*,*sdf*.tex,*font*.tex,*atlas*.tex,*.TMP_FontAsset
pause