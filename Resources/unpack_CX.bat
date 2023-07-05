@echo off
for %%a in (*.csv) do unPacker_CSV.exe -uc 4 "%%a"
