REM
REM    Kerbal Mod Enabler handles quick and easy installation of mods.
REM    Copyright (C) 2019  CYBUTEK
REM
REM    This program is free software: you can redistribute it and/or modify
REM    it under the terms of the GNU General Public License as published by
REM    the Free Software Foundation, either version 3 of the License, or
REM    (at your option) any later version.
REM
REM    This program is distributed in the hope that it will be useful,
REM    but WITHOUT ANY WARRANTY; without even the implied warranty of
REM    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
REM    GNU General Public License for more details.
REM
REM    You should have received a copy of the GNU General Public License
REM    along with this program.  If not, see <https://www.gnu.org/licenses/>.
REM

@ECHO OFF

SET self=%0
SET source=%~1
SET target=%~2

:LOOP
TASKLIST | FIND /I "KerbalModEnabler" >NUL 2>&1
IF ERRORLEVEL 1 (
	GOTO CONTINUE
) ELSE (
	ECHO Waiting for Kerbal Mod Enabler to exit...
	TIMEOUT /T 4 /NOBREAK
	CLS
	GOTO LOOP
)

:CONTINUE
MOVE %source% %target%
START %target%
DEL %self%