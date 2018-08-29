require("Base.class")

local LuaPlayConfig = class("LuaPlayConfig", "LuaDataReader", "Configs.LuaDataReader").New()


function LuaPlayConfig:GetDataConfigName()
    return "play_data_v2.bin"
end

function LuaPlayConfig:GetProtoInstance()
    require("Protocol.xls2proto.play_data_v2_pb")
    return xls2proto.play_data_v2_pb.play_data_ARRAY()
end


function LuaPlayConfig:OnLoad()
    self.super.OnLoad(self)
end

function LuaPlayConfig:GetDataByID(id)
    return self.dataDict[id]
end


LuaPlayConfig:OnLoad()

return LuaPlayConfig