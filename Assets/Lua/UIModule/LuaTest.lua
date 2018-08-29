require("Base.class")
require("Protocol.cmd_pb")

local LuaTest = class("LuaTest").New()

function LuaTest:Awake()
	print("LuaTest:Awake "..self.gameObject.name)
end

function LuaTest:Start()
	print("LuaTest:Start 1")
	UserEventManager.RegisterEvent("rep_message_login_game", function(param)
		print("LuaTest:Start rep_message_login_game 1")
		--require("Configs.LuaEmojiConfig")
		require("Configs.LuaPlayConfig")

		local paramStr = tolua.tolstring(param)
		print(#paramStr)
		print("lua_rep_message_login_game"..paramStr)
		local repMsg = cmd_pb.rep_message_login_game()
		repMsg:ParseFromString(paramStr)
		print("=================================="..tostring(repMsg))
	end)
	print("LuaTest:Start 2")
end

return LuaTest