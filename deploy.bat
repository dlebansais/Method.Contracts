@echo off
if '%1' == '' goto error
echo Deploying %1 ...
git commit --allow-empty -m "%1"
git fetch . master:deployment
git push origin deployment
goto end

:error
echo Parameter: version tag. Ex: deploy v1.0.0
goto end

:end