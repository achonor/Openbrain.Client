require("Base.class")
local LuaTest = class("LuaTest").New()

function LuaTest:Awake()
	print("LuaTest:Awake "..self.gameObject.name)
end

function LuaTest:Start()
	Scheduler.Instance:CreateScheduler("LuaTest.Start", 0, 0, 1.0, function(param)
		print("LuaTest:Start.Scheduler ................10086")
	end, nil);
end

return LuaTest