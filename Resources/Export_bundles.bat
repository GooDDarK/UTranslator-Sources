@echo off
color a
for %%a in (*.unity3D;*.bundle) do UPacker.exe exportbundle "%%a"