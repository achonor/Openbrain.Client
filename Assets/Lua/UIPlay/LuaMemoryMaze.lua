require("Base.class")
require("Protocol.cmd_pb")
local LuaPlayConfig = require("Configs.LuaPlayConfig")

local LuaMemoryMaze = class("LuaMemoryMaze").New()

function LuaMemoryMaze:Awake()
	print("LuaMemoryMaze:Awake ")
	self.playInterFace = self.transform:GetComponent("LuaPlayInterface")

	
    --当前是第几个问题
	self.problemIdx = -1
	--本次问题的结果
	self.problemResult = {}
	--遮罩
	self.mMask = self.transform:Find("Mask").gameObject
	--色块的父节点
	self.mContent = self.transform:Find("Content")
	--所有色块
	self.blockList = {}
	for idx = 0, self.mContent.childCount - 1, 1 do
		local tmpBlock = self.mContent:Find(tostring(idx))
		tmpBlock.gameObject:SetActive(false)
		self.blockList[idx] = tmpBlock
		--注册回调
		EventTrigger.Get(tmpBlock.gameObject).onClick = function()
			self:ClickBlock(tmpBlock, idx)
		end
	end

	--获取配置
	self.playData = LuaPlayConfig:GetDataByID(self:GetPlayID())
	print("LuaMemoryMaze self.playData.name = "..self.playData.name)
end

function LuaMemoryMaze:Start()
	print("LuaMemoryMaze:Start")
end


function LuaMemoryMaze:OnOpen()
	self.problemIdx = -1
end

function LuaMemoryMaze:CreateProblem()
	print("LuaMemoryMaze:CreateProblem")
	--问题下标加一
	self.problemIdx = math.clamp(self.problemIdx + 1, 0, #self.playData.param1 - 1)
	--问题难度
	local problemLevel = self.playData.param1[self.problemIdx + 1]
	--随机几个数字
	self.problemResult = Function.RandInRange(0, 8, problemLevel)
	--打开遮罩防止点击
	self.mMask:SetActive(true)
	--色块的状态
	print(#self.blockList)
	for idx = 0, #self.blockList, 1 do
		
	end
end

function LuaMemoryMaze:GetPlayID()
	return 1
end

return LuaMemoryMaze