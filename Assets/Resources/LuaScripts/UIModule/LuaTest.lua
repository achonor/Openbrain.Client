require("Base.class") 

local LuaTest = class("LuaTest").New()

function LuaTest:Awake()
	print("LuaTest:Awake "..self.gameObject.name)
end

return LuaTest