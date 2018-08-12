--主入口函数。从这里开始lua逻辑
function Main()					
	print("logic start")
	ReconnectionLuaDebug()
end

--场景切换通知
function OnLevelWasLoaded(level)
	collectgarbage("collect")
	Time.timeSinceLevelLoad = 0
end

function ReconnectionLuaDebug()
	local  breakInfoFun, xpcallFun = require("LuaDebugjit")("localhost",7003)  
end

function OnApplicationQuit()
end