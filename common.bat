if not defined VisualStudioVersion (
	if defined VS100COMNTOOLS set VisualStudioVersion=10.0
	if defined VS110COMNTOOLS set VisualStudioVersion=11.0
)

if "%VisualStudioVersion%"=="10.0" set _VSxCOMNTOOLS=%VS100COMNTOOLS%
if "%VisualStudioVersion%"=="11.0" set _VSxCOMNTOOLS=%VS110COMNTOOLS%

@echo VisualStudioVersion=%VisualStudioVersion%
@echo VisualStudioCommonTools=%_VSxCOMNTOOLS%

call "%_VSxCOMNTOOLS%\vsvars32.bat"
