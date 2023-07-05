@echo off
for %%a in (*.csv) do unPacker_CSV.exe -pc 4 "%%a"
