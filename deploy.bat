@echo off
git fetch . master:deployment
git push origin deployment
