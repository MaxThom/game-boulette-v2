# game-boulette-v2

## To run using HotReload
1. Add *"hotReloadProfile": "blazorwasm"* in profiles in *launchSettings.json*
2. Use *dotnet watch* inside client project folder in terminal
3. If change type is not supported, it will restart the app. Check terminal and set to always.

## Docker
### To build
1. In solution folder
2. Run ```docker build -t maxthom/game-boulette -f .\GameBoulette.Server\Dockerfile .```

### To run
1. Run ```docker run -d -p 49116:80 -p 49115:443 --name GameBoulette maxthom/game-boulette```
2. Navigate to ```http://localhost:49116```