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
		self.blockList[idx] = tmpBlock
		--注册回调
		EventTrigger.Get(tmpBlock.gameObject).onClick = function()
			self:ClickBlock(tmpBlock, idx)
		end
	end
	--遮罩的点击事件
	EventTrigger.Get(self.mMask).onClick = function()
		--开始旋转
	end
	--获取配置
	self.playData = LuaPlayConfig:GetDataByID(self:GetPlayID())
	print("LuaMemoryMaze self.playData.name = "..self.playData.name)

	--方块颜色
	self.blockColor = {
		[1] = GameConst.BlueBlock,
		[2] = GameConst.LightBlue
	}
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
	self.problemResult = {}
	local tmpArray = Function.RandInRange(0, 8, problemLevel)
	for idx = 0, problemLevel - 1, 1 do
		self.problemResult[#self.problemResult + 1] = tmpArray[idx]
	end
	--打开遮罩防止点击
	self.mMask:SetActive(true)
	--色块的状态
	print(#self.blockList)
	for idx = 0, #self.blockList, 1 do
		--修改色块颜色
		local tmpImage = self.blockList[idx]:GetComponent("Image")
		tmpImage.color = self.blockColor[1]
	end
	--选中的色块的颜色
	print(#self.problemResult)
	for idx, val in pairs(self.problemResult) do
		local tmpImage = self.blockList[val]:GetComponent("Image")
		tmpImage.color = self.blockColor[2]
	end
end

--设置色块状态
function LuaMemoryMaze:RotationBlock(objBlock, endColor)
	local rotation = objBlock:GetComponent("Rotation")
	
end


function LuaMemoryMaze:GetPlayID()
	return 1
end

return LuaMemoryMaze